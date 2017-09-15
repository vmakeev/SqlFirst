using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using Antlr4.Runtime;
using SqlFirst.Core;
using SqlFirst.Core.Parsing;
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
			DataTable schemaTable = GetQuerySchema(queryText, connectionString);

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
			QueryType queryType;
			string queryBody = queryText.GetQueryBody().Trim().ToLowerInvariant();

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
			else
			{
				queryType = QueryType.Unknown;
			}

			return new MsSqlServerQueryBaseInfo { QueryType = queryType };
		}

		/// <inheritdoc />
		public override IQueryInfo GetQueryInfo(string queryText, string connectionString)
		{
			IQueryBaseInfo baseInfo = GetQueryBaseInfo(queryText);

			IEnumerable<IQueryParamInfo> declaredParameters = GetDeclaredParameters(queryText);
			IEnumerable<IQueryParamInfo> undeclaredParameters = GetUndeclaredParameters(queryText, connectionString);
			IEnumerable<IQueryParamInfo> parameters = declaredParameters.Concat(undeclaredParameters);

			IEnumerable<IFieldDetails> results = GetResultDetails(queryText, connectionString);

			IQueryInfo queryInfo = new MsSqlServerQueryInfo
			{
				QueryType = baseInfo.QueryType,
				Parameters = parameters,
				Results = results
			};

			return queryInfo;
		}

		/// <inheritdoc />
		protected override IEnumerable<IQueryParamInfo> GetDeclaredParametersInternal(string parametersDeclaration)
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

		/// <inheritdoc />
		protected override IEnumerable<IQueryParamInfo> GetUndeclaredParametersInternal(IEnumerable<IQueryParamInfo> declared, string queryText, string connectionString)
		{
			using (IDbConnection connection = _databaseProvider.Value.GetConnection(connectionString))
			{
				IDbCommand command = connection.CreateCommand();
				command.CommandText = "sp_describe_undeclared_parameters @tsql";
				var parameter = new SqlParameter("@tsql", SqlDbType.NChar) { Value = queryText };
				command.Parameters.Add(parameter);

				connection.Open();
				using (IDataReader dataReader = command.ExecuteReader())
				{
					while (dataReader.Read())
					{
						// ignore global variables
						if (dataReader.GetString(1).Substring(0, 2) != "@@")
						{
							string dbName = dataReader.GetString(1);
							string dbType = dataReader.GetString(3);

							var info = new QueryParamInfo
							{
								DbName = dbName.TrimStart('@'),
								DbType = MsSqlDbType.Normalize(dbType),
								Length = MsSqlDbType.GetLength(dbType),
							};

							yield return info;
						}
					}
				}
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