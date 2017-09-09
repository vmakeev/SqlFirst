using System;
using SqlFirst.Core;

namespace SqlFirst.Providers.MsSqlServer
{
	public class MsSqlServerTypeMapper : IDatabaseTypeMapper
	{
		/// <inheritdoc />
		public string Map(string dbType, out string dbTypeNormalized, bool nullable = true)
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
			}
		}
	}
}
