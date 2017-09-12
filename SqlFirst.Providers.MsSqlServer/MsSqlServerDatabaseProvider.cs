﻿using System.Data;
using System.Data.SqlClient;
using SqlFirst.Core;

namespace SqlFirst.Providers.MsSqlServer
{
	public class MsSqlServerDatabaseProvider : IDatabaseProvider
	{
		public virtual IDbConnection GetConnection(string connectionString)
		{
			return new SqlConnection(connectionString);
		}
	}
}