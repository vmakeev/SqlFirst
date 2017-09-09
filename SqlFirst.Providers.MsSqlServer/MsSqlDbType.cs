using System.Diagnostics.CodeAnalysis;

namespace SqlFirst.Providers.MsSqlServer
{
	[SuppressMessage("ReSharper", "InconsistentNaming")]
	internal static class MsSqlDbType
	{
		public const string Char = "char";

		public const string NChar = "nchar";

		public const string NText = "ntext";

		public const string NVarChar = "nvarchar";

		public const string VarChar = "varchar";

		public const string Text = "text";

		public const string Xml = "xml";

		public const string Date = "date";

		public const string DateTime = "datetime";

		public const string DateTime2 = "datetime2";

		public const string SmallDateTime = "smalldatetime";

		public const string Time = "time";

		public const string Binary = "binary";

		public const string Image = "image";

		public const string Timestamp = "timestamp";

		public const string VarBinary = "varbinary";

		public const string Decimal = "decimal";

		public const string Money = "money";

		public const string SmallMoney = "smallmoney";

		public const string SqlVariant = "sql_variant";

		public const string Variant = "variant";

		public const string Udt = "udt";

		public const string Bit = "bit";

		public const string Bigint = "bigint";

		public const string DateTimeOffset = "datetimeoffset";

		public const string Float = "float";

		public const string Real = "real";

		public const string Smallint = "smallint";

		public const string Tinyint = "tinyint";

		public const string Int = "int";

		public const string Structured = "structured";

		public const string UniqueIdentifier = "uniqueidentifier";

		public static string Normalize(string type)
		{
			return type?.ToLowerInvariant();
		}
	}
}