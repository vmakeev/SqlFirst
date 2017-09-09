using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using SqlFirst.Core;
using SqlFirst.Core.Codegen;

namespace SqlFirst.Providers.MsSqlServer
{
	public class MsSqlServerDatabaseProvider : IDatabaseProvider
	{
		private readonly string[] _typesWithLength =
		{
			MsSqlDbType.Char,
			MsSqlDbType.VarChar,
			MsSqlDbType.NChar,
			MsSqlDbType.NVarChar
		};

		public virtual IDbConnection GetConnection(string connectionString)
		{
			return new SqlConnection(connectionString);
		}

		public virtual List<IQueryParamInfo> GetDeclaredParameters(string queryText)
		{
			var queryParams = new List<IQueryParamInfo>();
			// get design time section
			string dt = Regex.Match(queryText, "-- designTime(?<designTime>.*)-- endDesignTime", RegexOptions.Singleline).Value;
			// extract declared parameters
			string pattern = "declare[^;]*";
			Match match = Regex.Match(dt, pattern, RegexOptions.IgnoreCase);
			while (match.Success)
			{
				string[] parts = match.Value.Split(' ');
				var queryParamInfo = new QueryParamInfo();
				FillParamInfo(queryParamInfo, parts[1].Substring(1), parts[2]);
				queryParams.Add(queryParamInfo);
				match = match.NextMatch();
			}

			return queryParams;
		}

		public virtual List<IQueryParamInfo> GetUndeclaredParameters(string queryText, string connectionString)
		{
			var myParams = new List<IQueryParamInfo>();
			// sp_describe_undeclared_parameters
			using (IDbConnection connection = GetConnection(connectionString))
			{
				IDbCommand cmd = connection.CreateCommand();
				cmd.CommandText = "sp_describe_undeclared_parameters @tsql";
				var tsql = new SqlParameter("@tsql", SqlDbType.NChar) { Value = queryText };
				cmd.Parameters.Add(tsql);

				connection.Open();
				IDataReader rdr = cmd.ExecuteReader();
				while (rdr.Read())
				{
					// ignore global variables
					if (rdr.GetString(1).Substring(0, 2) != "@@")
					{
						// build declaration.
						myParams.Add(new QueryParamInfo()
							{
								DbName = rdr.GetString(1),
								DbType = rdr.GetString(3),
								Length = rdr.GetInt16(4)
							}
						);
					}
				}
			}

			return myParams;
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

		private void FillParamInfo(IQueryParamInfo qp, string name, string sqlTypeAndLength)
		{
			string typeOnly;
			int length = 0;
			if (sqlTypeAndLength.IndexOf('(') > 0)
			{
				typeOnly = sqlTypeAndLength.Substring(0, sqlTypeAndLength.IndexOf('('));
				if (_typesWithLength.Any(p => p == typeOnly.ToLower()))
				{
					int.TryParse(Regex.Match(sqlTypeAndLength, "(?<=\\()\\s*(?'myInt'\\d*)").Groups["myInt"].Value, out length);
				}
			}
			else
			{
				typeOnly = sqlTypeAndLength;
			}

			Type clrType = MsSqlServerTypeMapper.Instance.Map(typeOnly, true);

			qp.ClrType = clrType;
			qp.DbType = MsSqlDbType.Normalize(typeOnly);
			qp.Length = length;
			qp.CsName = CSharpCodeHelper.GetValidVariableName(name);
			qp.DbName = '@' + name;
		}
	}
}