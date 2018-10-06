using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using Antlr4.Runtime;
using SqlFirst.Core;
using SqlFirst.Core.Impl;
using SqlFirst.Providers.MsSqlServer.VariableDeclarations;
using SqlFirst.Providers.MsSqlServer.VariableDeclarations.Generated;

namespace SqlFirst.Providers.MsSqlServer
{
	public class MsSqlServerQueryParser : QueryParserBase
	{
		private readonly Lazy<IDatabaseProvider> _databaseProvider = new Lazy<IDatabaseProvider>(() => new MsSqlServerDatabaseProvider());
		private readonly Lazy<IFieldInfoProvider> _fieldInfoProvider = new Lazy<IFieldInfoProvider>(() => new MsSqlServerFieldInfoProvider());
		private readonly Lazy<MsSqlServerCodeEmitter> _sqlCodeEmitter = new Lazy<MsSqlServerCodeEmitter>(() => new MsSqlServerCodeEmitter());

		/// <inheritdoc />
		public override IEnumerable<IFieldDetails> GetResultDetails(string queryText, string connectionString)
		{
			if (string.IsNullOrEmpty(connectionString))
			{
				throw new QueryParsingException("Connection string must be specified.");
			}

			DataTable schemaTable;

			try
			{
				schemaTable = GetQuerySchema(queryText, connectionString);
			}
			catch (Exception ex)
			{
				throw new QueryParsingException("Unable to determine query results", ex);
			}

			if (schemaTable == null)
			{
				yield break;
			}

			for (int i = 0; i <= schemaTable.Rows.Count - 1; i++)
			{
				DataRow fieldMetadata = schemaTable.Rows[i];
				IFieldDetails fieldDetails = _fieldInfoProvider.Value.GetFieldDetails(fieldMetadata);

				yield return fieldDetails;
			}
		}

		/// <inheritdoc />
		public override IQueryBaseInfo GetQueryBaseInfo(string queryText)
		{
			List<IQuerySection> sections = GetQuerySections(queryText).ToList();

			IQuerySection bodySection = sections.SingleOrDefault(querySection => querySection.Type == QuerySectionType.Body);

			var queryType = QueryType.Unknown;

			if (bodySection != null)
			{
				string queryBody = bodySection.Content.Trim().ToLowerInvariant();

				if (queryBody.StartsWith("select"))
				{
					queryType = QueryType.Read;
				}
				else if (queryBody.StartsWith("update"))
				{
					queryType = QueryType.Update;
				}
				else if (queryBody.StartsWith("insert"))
				{
					queryType = QueryType.Create;
				}
				else if (queryBody.StartsWith("delete"))
				{
					queryType = QueryType.Delete;
				}
				else if (queryBody.StartsWith("merge"))
				{
					queryType = QueryType.Merge;
				}
				else if (queryBody.StartsWith("exec"))
				{
					queryType = QueryType.StoredProcedure;
				}
			}

			return new MsSqlServerQueryBaseInfo
			{
				Type = queryType,
				Sections = sections,
				SqlFirstOptions = GetOptions(queryText)
			};
		}

		/// <inheritdoc />
		public override IQueryInfo GetQueryInfo(string queryText, string connectionString)
		{
			if (string.IsNullOrEmpty(connectionString))
			{
				throw new QueryParsingException("Connection string must be specified.");
			}

			IQueryBaseInfo baseInfo = GetQueryBaseInfo(queryText);

			IEnumerable<IQueryParamInfo> declaredParameters = GetDeclaredParameters(queryText);
			IEnumerable<IQueryParamInfo> undeclaredParameters = GetUndeclaredParameters(queryText, connectionString);
			IEnumerable<IQueryParamInfo> parameters = declaredParameters.Concat(undeclaredParameters);

			IEnumerable<IFieldDetails> results = GetResultDetails(queryText, connectionString);

			IQueryInfo queryInfo = new MsSqlServerQueryInfo
			{
				Type = baseInfo.Type,
				Parameters = parameters,
				Results = results,
				Sections = baseInfo.Sections,
				SqlFirstOptions = baseInfo.SqlFirstOptions
			};

			return queryInfo;
		}

		/// <inheritdoc />
		protected override IEnumerable<IQueryParamInfo> GetDeclaredParametersInternal(string parametersDeclaration)
		{
			try
			{
				using (TextReader textReader = new StringReader(parametersDeclaration))
				{
					var stream = new AntlrInputStream(textReader);
					var lexer = new SqlVariableDeclarationsLexer(stream);
					var tokens = new CommonTokenStream(lexer);
					var parser = new SqlVariableDeclarationsParser(tokens)
					{
						ErrorHandler = new BailErrorStrategy()
					};

					return new QueryParamInfoVisitor().Visit(parser.root());
				}
			}
			catch (Exception ex)
			{
				throw new QueryParsingException("Unable to parse variable declarations section", ex);
			}
		}

		/// <inheritdoc />
		protected override IEnumerable<IQueryParamInfo> GetUndeclaredParametersInternal(IEnumerable<IQueryParamInfo> declared, string queryText, string connectionString)
		{
			if (string.IsNullOrEmpty(connectionString))
			{
				throw new QueryParsingException("Connection string must be specified.");
			}

			using (IDbConnection connection = _databaseProvider.Value.GetConnection(connectionString))
			{
				IDbCommand command = connection.CreateCommand();
				command.CommandText = "sp_describe_undeclared_parameters @queryText";
				var parameter = new SqlParameter("@queryText", SqlDbType.NChar) { Value = queryText };
				command.Parameters.Add(parameter);

				connection.Open();
				using (IDataReader dataReader = ExecuteUndeclaredParametersReader(command)) // no other way to use catch + yield return
				{
					while (dataReader.Read())
					{
						UndeclaredParameterDescription description = UndeclaredParameterDescription.FromDataRecord(dataReader);
						MsSqlServerTypeMetadata typeMetadata = null;

						if (!description.Name.StartsWith("@@"))
						{
							string dbName = description.Name.TrimStart('@');
							string dbType = description.SuggestedSystemTypeName;
							if (string.IsNullOrEmpty(dbType) && description.SuggestedUserTypeId != null)
							{
								(dbType, typeMetadata) = DescribeUserType(description, connectionString);
							}

							(bool isNumbered, string semanticName) = QueryParamInfoNameHelper.GetNameSemantic(dbName);

							var info = new QueryParamInfo
							{
								DbName = dbName,
								DbType = MsSqlDbType.Normalize(dbType),
								Length = MsSqlDbType.GetLength(dbType),
								IsNumbered = isNumbered,
								SemanticName = semanticName,
								DbTypeMetadata = typeMetadata,
								IsComplexType = typeMetadata?.IsTableType ?? false
							};

							yield return info;
						}
					}
				}
			}
		}

		private (string typeName, MsSqlServerTypeMetadata metadata) DescribeUserType(UndeclaredParameterDescription parameterDescription, string connectionString)
		{
			if (parameterDescription.SuggestedUserTypeId == null)
			{
				throw new QueryParsingException("Unable to describe user type: identifier is missing.");
			}

			string typeName = parameterDescription.SuggestedUserTypeName;
			MsSqlServerTypeMetadata metadata = null;

			MsSqlServerTypeDescription systemTypeDescription = GetMsSqlUserTypeDescription(parameterDescription, connectionString);
			if (systemTypeDescription.IsTableType)
			{
				string queryText = $"declare @target {systemTypeDescription.Name}; select * from @target";
				// todo: add recursive types support
				IFieldDetails[] fieldDetails = GetResultDetails(queryText, connectionString).ToArray();

				metadata = new MsSqlServerTypeMetadata
				{
					IsTableType = true,
					TableTypeColumns = fieldDetails
				};
			}

			return (typeName, metadata);
		}

		private MsSqlServerTypeDescription GetMsSqlUserTypeDescription(UndeclaredParameterDescription parameterDescription, string connectionString)
		{
			using (var connection = new SqlConnection(connectionString))
			{
				connection.Open();
				return DescribeUserTypeByIdQuery.GetFirst(connection, parameterDescription.SuggestedUserTypeId);
			}
		}

		private static IDataReader ExecuteUndeclaredParametersReader(IDbCommand command)
		{
			try
			{
				return command.ExecuteReader();
			}
			catch (Exception ex)
			{
				throw new QueryParsingException("Unable to determine undeclared parameters", ex);
			}
		}

		/// <summary>
		/// Возвращает схему запроса
		/// </summary>
		/// <param name="queryText">Текст запроса</param>
		/// <param name="connectionString">Строка подключения к БД</param>
		/// <returns>Таблица со схемой запроса</returns>
		private DataTable GetQuerySchema(string queryText, string connectionString)
		{
			if (string.IsNullOrEmpty(connectionString))
			{
				throw new QueryParsingException("Connection string must be specified.");
			}

			IQueryParamInfo[] undeclaredParameters = GetUndeclaredParameters(queryText, connectionString).ToArray();
			if (undeclaredParameters.Any())
			{
				string declarations = _sqlCodeEmitter.Value.EmitDeclarations(undeclaredParameters);
				queryText = declarations + Environment.NewLine + queryText;
			}

			using (IDbConnection connection = _databaseProvider.Value.GetConnection(connectionString))
			{
				connection.Open();
				using (IDbCommand command = connection.CreateCommand())
				{
					command.CommandText = queryText;
					using (IDataReader dataReader = command.ExecuteReader(CommandBehavior.SchemaOnly))
					{
						DataTable dtSchema = dataReader.GetSchemaTable();
						return dtSchema;
					}
				}
			}
		}
	}
}