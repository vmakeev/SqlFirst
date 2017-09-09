using System;
using System.Data;
using SqlFirst.Core;

namespace SqlFirst.Providers.MsSqlServer
{
	internal static class MsSqlDbType
	{
		public static string Char => "char";

		public static string NChar => "nchar";

		public static string NText => "ntext";

		public static string NVarChar => "nvarchar";

		public static string VarChar => "varchar";

		public static string Text => "text";

		public static string Xml => "xml";

		public static string Date => "date";

		public static string DateTime => "datetime";

		public static string DateTime2 => "datetime2";

		public static string SmallDateTime => "smalldatetime";

		public static string Time => "time";

		public static string Binary => "binary";

		public static string Image => "image";

		public static string Timestamp => "timestamp";

		public static string VarBinary => "varbinary";

		public static string Decimal => "decimal";

		public static string Money => "money";

		public static string SmallMoney => "smallmoney";

		public static string SqlVariant => "sql_variant";

		public static string Variant => "variant";

		public static string Udt => "udt";

		public static string Bigint => "bigint";

		public static string DateTimeOffset => "datetimeoffset";

		public static string Float => "float";

		public static string Real => "real";

		public static string Smallint => "smallint";

		public static string Tinyint => "tinyint";

		public static string Int => "int";

		public static string Structured => "structured";

		public static string UniqueIdentifier => "uniqueidentifier";

		public static string Normalize(string type)
		{
			return type?.ToLowerInvariant();
		}
	}

	public class MsSqlServerTypeMapper : IDatabaseTypeMapper
	{
		internal static IDatabaseTypeMapper Instance { get; } = new MsSqlServerTypeMapper();

		/// <inheritdoc />
		public Type Map(string dbType, bool nullable)
		{
			Type baseType = GetBaseType(dbType);

			if (nullable && baseType.IsValueType)
			{
				return typeof(Nullable<>).MakeGenericType(baseType);
			}

			return baseType;
		}

		private static Type GetBaseType(string dbType)
		{
			switch (dbType.ToLower())
			{
				case "char":
				case "nchar":
				case "ntext":
				case "nvarchar":
				case "varchar":
				case "text":
				case "xml":
					return typeof(string);

				case "date":
				case "datetime":
				case "datetime2":
				case "smalldatetime":
				case "time":
					return typeof(DateTime);

				case "binary":
				case "image":
				case "timestamp":
				case "varbinary":
					return typeof(byte[]);

				case "decimal":
				case "money":
				case "smallmoney":
					return typeof(decimal);

				case "sql_variant":
				case "variant":
				case "udt":
					return typeof(object);

				case "bit":
					return typeof(bool);

				case "bigint":
					return typeof(long);

				case "datetimeoffset":
					return typeof(DateTimeOffset);

				case "float":
					return typeof(double);

				case "real":
					return typeof(float);

				case "smallint":
					return typeof(short);

				case "tinyint":
					return typeof(byte);

				case "int":
					return typeof(int);

				case "structured":
					return typeof(DataTable);

				case "uniqueidentifier":
					return typeof(Guid);

				default:
					throw new Exception("type not matched : " + dbType);
			}
		}
	}
}