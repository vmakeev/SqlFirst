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

			IEnumerable<IQueryParamInfo> declaredParameters = GetDeclaredParameters(queryText, connectionString);
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
		protected override IEnumerable<IQueryParamInfo> GetDeclaredParametersInternal(string parametersDeclaration, string connectionString)
		{
			if (string.IsNullOrEmpty(connectionString))
			{
				throw new QueryParsingException("Connection string must be specified.");
			}

			DeclaredQueryParamInfo[] declaredQueryParamInfos;

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

					declaredQueryParamInfos = new QueryParamInfoVisitor()
												   .Visit(parser.root())
												   .ToArray();
				}
			}
			catch (Exception ex)
			{
				throw new QueryParsingException("Unable to parse variable declarations section", ex);
			}

			bool NeedInspection(string typeName)
			{
				string declaredType = MsSqlDbType.Normalize(typeName);
				return declaredType == MsSqlDbType.Udt ||
						declaredType == MsSqlDbType.Structured ||
						!MsSqlDbType.IsKnownType(declaredType);
			}

			foreach (DeclaredQueryParamInfo info in declaredQueryParamInfos)
			{
				var result = new QueryParamInfo
				{
					IsComplexType = false,
					DbTypeMetadata = null,
					ComplexTypeData = null,
					DbType = info.DbType,
					IsNumbered = info.IsNumbered,
					DbName = info.DbName,
					DefaultValue = info.DefaultValue,
					Length = info.Length,
					SemanticName = info.SemanticName
				};

				if (NeedInspection(result.DbType))
				{
					bool isComplexType;
					IComplexTypeData complexTypeData;
					MsSqlServerTypeMetadata metadata;

					(isComplexType, complexTypeData, metadata) = GetTypeDetails(result.DbType, connectionString);

					result.IsComplexType = isComplexType;
					result.ComplexTypeData = complexTypeData;
					result.DbTypeMetadata = metadata;
				}

				yield return result;
			}
		}

		private (bool isComplexType, IComplexTypeData complexTypeData, MsSqlServerTypeMetadata metadata) GetTypeDetails(string dbType, string connectionString)
		{
			if (string.IsNullOrEmpty(connectionString))
			{
				throw new QueryParsingException("Connection string must be specified.");
			}

			bool isComplexType = false;
			IComplexTypeData complexTypeData = null;
			MsSqlServerTypeMetadata metadata = null;

			using (IDbConnection connection = _databaseProvider.Value.GetConnection(connectionString))
			{
				connection.Open();
				MsSqlServerTypeDescription typeDescription = DescribeUserTypeByNameQuery.GetFirst(connection, dbType);

				if (typeDescription == null)
				{
					throw new QueryParsingException($"Type [{dbType}] is not a valid database type.");
				}

				if (typeDescription.IsTableType)
				{
					string queryText = $"declare @target {typeDescription.Name}; select * from @target";
					// todo: add recursive types support
					IFieldDetails[] fieldDetails = GetResultDetails(queryText, connectionString).ToArray();

					isComplexType = true;

					metadata = new MsSqlServerTypeMetadata
					{
						IsTableType = true,
						TableTypeColumns = fieldDetails
					};

					string complexTypeDisplayedName = typeDescription.Name;
					string complexTypeItemName = typeDescription.Name + "Item";

					complexTypeData = metadata.TableTypeColumns == null
						? null
						: new ComplexTypeData(
							name: complexTypeItemName, 
							dbTypeDisplayedName: complexTypeDisplayedName,
							isTableType: true, 
							allowNull: typeDescription.IsNullable ?? false,
							fields: metadata.TableTypeColumns);
				}
			}

			return (isComplexType, complexTypeData, metadata);
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
							string complexTypeItemName = null;

							if (string.IsNullOrEmpty(dbType) && description.SuggestedUserTypeId != null)
							{
								(dbType, complexTypeItemName, typeMetadata) = DescribeUserType(description, connectionString);
							}

							(bool isNumbered, string semanticName) = QueryParamInfoNameHelper.GetNameSemantic(dbName);

							bool isTableType = typeMetadata?.IsTableType ?? false;
							bool isComplexType = isTableType;

							ComplexTypeData complexTypeData = typeMetadata?.TableTypeColumns == null
								? null
								: new ComplexTypeData(
									name: complexTypeItemName, 
									dbTypeDisplayedName: dbType,
									isTableType: isTableType,
									allowNull: typeMetadata.IsNullable ?? false,
									fields: typeMetadata.TableTypeColumns);

							var info = new QueryParamInfo
							{
								DbName = dbName,
								DbType = MsSqlDbType.Normalize(dbType),
								Length = MsSqlDbType.GetLength(dbType),
								IsNumbered = isNumbered,
								SemanticName = semanticName,
								DbTypeMetadata = typeMetadata,
								IsComplexType = isComplexType,
								ComplexTypeData = complexTypeData
							};

							yield return info;
						}
					}
				}
			}
		}

		private (string typeName, string itemName, MsSqlServerTypeMetadata metadata) DescribeUserType(UndeclaredParameterDescription parameterDescription, string connectionString)
		{
			if (parameterDescription.SuggestedUserTypeId == null)
			{
				throw new QueryParsingException("Unable to describe user type: suggested user type identifier is missing.");
			}

			string typeName = parameterDescription.SuggestedUserTypeName;
			string itemName = null;
			var metadata = new MsSqlServerTypeMetadata();

			MsSqlServerTypeDescription systemTypeDescription = GetMsSqlUserTypeDescription(parameterDescription, connectionString);
			if (systemTypeDescription.IsTableType)
			{
				typeName = systemTypeDescription.Name;
				itemName = systemTypeDescription.Name + "Item";
				string queryText = $"declare @target {systemTypeDescription.Name}; select * from @target";
				// todo: add recursive types support
				IFieldDetails[] fieldDetails = GetResultDetails(queryText, connectionString).ToArray();

				metadata = new MsSqlServerTypeMetadata
				{
					IsTableType = true,
					TableTypeColumns = fieldDetails,
				};
			}

			metadata.IsNullable = systemTypeDescription.IsNullable;

			return (typeName, itemName ?? typeName, metadata);
		}

		private MsSqlServerTypeDescription GetMsSqlUserTypeDescription(UndeclaredParameterDescription parameterDescription, string connectionString)
		{
			using (IDbConnection connection = _databaseProvider.Value.GetConnection(connectionString))
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