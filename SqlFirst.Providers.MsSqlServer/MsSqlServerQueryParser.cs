using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using Antlr4.Runtime;
using SqlFirst.Core;
using SqlFirst.Core.Parsing;
using SqlFirst.Providers.MsSqlServer.VariableDeclarations;
using SqlFirst.Providers.MsSqlServer.VariableDeclarations.Generated;

namespace SqlFirst.Providers.MsSqlServer
{
	public class MsSqlServerQueryParser : QueryParser
	{
		private readonly Lazy<IDatabaseProvider> _databaseProvider = new Lazy<IDatabaseProvider>(() => new MsSqlServerDatabaseProvider());
		private readonly Lazy<IFieldInfoProvider> _fieldInfoProvider = new Lazy<IFieldInfoProvider>(() => new MsSqlServerFieldInfoProvider());

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