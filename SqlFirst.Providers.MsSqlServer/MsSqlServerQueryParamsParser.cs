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
	public class MsSqlServerQueryParamsParser : QueryParamsParser
	{
		private readonly Lazy<IDatabaseProvider> _databaseProvider = new Lazy<IDatabaseProvider>(() => new MsSqlServerDatabaseProvider());

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
							var info = new QueryParamInfo
							{
								DbName = dataReader.GetString(1),
								DbType = dataReader.GetString(3),
								Length = dataReader.GetInt16(4)
							};

							yield return info;
						}
					}
				}
			}
		}
	}
}