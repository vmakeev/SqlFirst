using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using SqlFirst.Core;
using SqlFirst.Core.Parsing;

namespace SqlFirst.Providers.MsSqlServer
{
	public class MsSqlServerDatabaseProvider : IDatabaseProvider
	{
		public virtual IDbConnection GetConnection(string connectionString)
		{
			return new SqlConnection(connectionString);
		}

		public virtual void PrepareParametersForSchemaFetching(IDbCommand command)
		{
			// nothing to do here.
		}

		public string BuildParameterDeclarations(List<IQueryParamInfo> parameters)
		{
			var stringBuilder = new StringBuilder();

			foreach (IQueryParamInfo queryParamInfo in parameters)
			{
				stringBuilder.Append("declare " + queryParamInfo.DbName + " " + queryParamInfo.DbType);
				stringBuilder.Append(";\n");
			}

			return stringBuilder.ToString();
		}
	}
}