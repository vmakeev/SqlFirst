using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace SqlFirst.Providers.Postgres
{
	[SuppressMessage("ReSharper", "InconsistentNaming")]
	internal static class PostgresDbType
	{
		public const string Int8 = "int8";
		public const string Bigint = "bigint";
		public const string Bigserial = "bigserial";
		public const string Serial8 = "serial8";
		public const string Bit = "bit";
		public const string BitVarying = "bit varying";
		public const string Varbit = "varbit";
		public const string Boolean = "boolean";
		public const string Bool = "bool";
		public const string Box = "box";
		public const string Bytea = "bytea";
		public const string Character = "character";
		public const string Char = "char";
		public const string CharacterVarying = "character varying";
		public const string Varchar = "varchar";
		public const string Cidr = "cidr";
		public const string Circle = "circle";
		public const string Date = "date";
		public const string DoublePrecision = "double precision";
		public const string Float8 = "float8";
		public const string Integer = "integer";
		public const string Int = "int";
		public const string Int4 = "int4";
		public const string Interval = "interval";
		public const string Json = "json";
		public const string Jsonb = "jsonb";
		public const string Line = "line";
		public const string Lseg = "lseg";
		public const string Macaddr = "macaddr";
		public const string Macaddr8 = "macaddr8";
		public const string Money = "money";
		public const string Numeric = "numeric";
		public const string Decimal = "decimal";
		public const string Path = "path";
		public const string Pg_lsn = "pg_lsn";
		public const string Point = "point";
		public const string Polygon = "polygon";
		public const string Real = "real";
		public const string Float4 = "float4";
		public const string Smallint = "smallint";
		public const string Int2 = "int2";
		public const string Smallserial = "smallserial";
		public const string Serial2 = "serial2";
		public const string Serial = "serial";
		public const string Serial4 = "serial4";
		public const string Text = "text";
		public const string Time = "time";
		public const string TimeWithoutTimeZone = "time without time zone";
		public const string TimeWithTimeZone = "time with time zone";
		public const string TimeTZ = "timetz";
		public const string Timestamp = "timestamp";
		public const string TimestampWithoutTimeZone = "timestamp without time zone";
		public const string TimestampWithTimeZone = "timestamp with time zone";
		public const string TimestampTZ = "timestamptz";
		public const string Tsquery = "tsquery";
		public const string Tsvector = "tsvector";
		public const string Txid_snapshot = "txid_snapshot";
		public const string Guid = "uuid";
		public const string Xml = "xml";

		private static readonly Regex _sizeRegex = new Regex("\\((?<size>[^\\)]+)\\)", RegexOptions.Compiled);

		public static string Normalize(string type)
		{
			string result = type?.ToLowerInvariant();
			if (string.IsNullOrEmpty(result))
			{
				return result;
			}

			result = _sizeRegex.Replace(result, string.Empty);

			while (result.Contains("  "))
			{
				result = result.Replace("  ", " ");
			}

			return result.Trim();
		}

		public static string GetLength(string type)
		{
			if (string.IsNullOrEmpty(type))
			{
				return null;
			}

			Match match = _sizeRegex.Match(type);

			return match.Success
				? match.Groups["size"]?.Value?.Trim()
				: null;
		}
	}
}