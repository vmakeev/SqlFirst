using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using SqlFirst.Core;

namespace SqlFirst.Providers.MsSqlServer
{
	public class MsSqlServerDatabaseProvider : IDatabaseProvider
	{
		private readonly string[] _typesWithLength =
		{
			"char",
			"varchar",
			"nchar",
			"nvarchar"
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

		public virtual string GetClrTypeName(string dbType, out string dbTypeNormalized, bool nullable = true)
		{
			switch (dbType.ToLower())
			{
				case "bigint":
					dbTypeNormalized = "BigInt";
					return nullable ? "long?" : "long";
				case "binary":
					dbTypeNormalized = "Binary";
					return "byte[]";
				case "image":
					dbTypeNormalized = "Image";
					return "byte[]";
				case "timestamp":
					dbTypeNormalized = "Timestamp";
					return "byte[]";
				case "varbinary":
					dbTypeNormalized = "Varbinary";
					return "byte[]";
				case "bit":
					dbTypeNormalized = "Bit";
					return nullable ? "bool?" : "bool";
				case "date":
					dbTypeNormalized = "Date";
					return nullable ? "DateTime?" : "DateTime";
				case "datetime":
					dbTypeNormalized = "DateTime";
					return nullable ? "DateTime?" : "DateTime";
				case "datetime2":
					dbTypeNormalized = "DateTime2";
					return nullable ? "DateTime?" : "DateTime";
				case "smalldatetime":
					dbTypeNormalized = "SmallDateTime";
					return nullable ? "DateTime?" : "DateTime";
				case "time":
					dbTypeNormalized = "Time";
					return nullable ? "DateTime?" : "DateTime";
				case "datetimeoffset":
					dbTypeNormalized = "DateTimeOffset";
					return nullable ? "DateTimeOffset?" : "DateTimeOffset";
				case "decimal":
					dbTypeNormalized = "Decimal";
					return nullable ? "decimal?" : "decimal";
				case "money":
					dbTypeNormalized = "Money";
					return nullable ? "decimal?" : "decimal";
				case "smallmoney":
					dbTypeNormalized = "SmallMoney";
					return nullable ? "decimal?" : "decimal";
				case "float":
					dbTypeNormalized = "Float";
					return nullable ? "double?" : "double";
				case "real":
					dbTypeNormalized = "Real";
					return nullable ? "float?" : "float";
				case "smallint":
					dbTypeNormalized = "SmallInt";
					return nullable ? "short?" : "short";
				case "tinyint":
					dbTypeNormalized = "TinyInt";
					return nullable ? "byte?" : "byte";
				case "int":
					dbTypeNormalized = "Int";
					return nullable ? "int?" : "int";
				case "char":
					dbTypeNormalized = "Char";
					return "string";
				case "nchar":
					dbTypeNormalized = "NChar";
					return "string";
				case "ntext":
					dbTypeNormalized = "NText";
					return "string";
				case "nvarchar":
					dbTypeNormalized = "NVarChar";
					return "string";
				case "varchar":
					dbTypeNormalized = "VarChar";
					return "string";
				case "text":
					dbTypeNormalized = "Text";
					return "string";
				case "xml":
					dbTypeNormalized = "Xml";
					return "string";
				case "sql_variant":
					dbTypeNormalized = "Variant";
					return "object";
				case "variant":
					dbTypeNormalized = "Variant";
					return "object";
				case "udt":
					dbTypeNormalized = "Udt";
					return "object";
				case "structured":
					dbTypeNormalized = "Structured";
					return "DataTable";
				case "uniqueidentifier":
					dbTypeNormalized = "UniqueIdentifier";
					return "Guid";
				default:
					throw new Exception("type not matched : " + dbType);
					// todo : keep going here. old method had a second switch on FieldDetails.DataType to catch a bunch of never seen types
			}
		}
		
		private void FillParamInfo(IQueryParamInfo qp, string name, string sqlTypeAndLength)
		{
			string typeOnly;
			int len = 0;
			if (sqlTypeAndLength.IndexOf('(') > 0)
			{
				typeOnly = sqlTypeAndLength.Substring(0, sqlTypeAndLength.IndexOf('('));
				if (_typesWithLength.Any(p => p == typeOnly.ToLower()))
				{
					int.TryParse(Regex.Match(sqlTypeAndLength, "(?<=\\()\\s*(?'myInt'\\d*)").Groups["myInt"].Value, out len);
				}
			}
			else
			{
				typeOnly = sqlTypeAndLength;
			}
			string csType = GetClrTypeName(typeOnly, out string normalizedType);

			qp.CsType = csType;
			qp.DbType = normalizedType;
			qp.Length = len;
			qp.CsName = name;
			qp.DbName = '@' + name;
		}
	}
}