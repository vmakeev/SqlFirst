using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Net.NetworkInformation;
using NpgsqlTypes;

namespace SqlFirst.Providers.Postgres
{
	internal class PostgresAdoTypeMapper
	{
		/// <summary>
		/// Возвращает имя типа CLR, который может быть безопасно использован для представления указанного типа данных в БД
		/// </summary>
		/// <param name="dbType">Название типа данных в БД</param>
		/// <param name="npgsqlDbType">Тип данные по версии npgsql</param>
		/// <param name="nullable">Поддерживается ли значение null</param>
		/// <returns>Имя типа CLR</returns>
		public Type Map(DbType dbType, NpgsqlDbType npgsqlDbType, bool nullable)
		{
			Type baseType = GetBaseType(dbType);

			if (baseType == null || baseType == typeof(object))
			{
				baseType = npgsqlDbType.TryGetArrayItemType(out var arrayItemType) 
					? GetBaseType(arrayItemType).MakeArrayType() 
					: GetBaseType(npgsqlDbType);
			}

			if (nullable && baseType.IsValueType)
			{
				return typeof(Nullable<>).MakeGenericType(baseType);
			}

			return baseType;
		}

		private static Type GetBaseType(DbType dbType)
		{
			Type clrType;
			switch (dbType)
			{
				case DbType.AnsiString:
				case DbType.AnsiStringFixedLength:
				case DbType.String:
				case DbType.StringFixedLength:
				case DbType.Xml:
					clrType = typeof(string);
					break;

				case DbType.Boolean:
					clrType = typeof(bool);
					break;

				case DbType.Byte:
					clrType = typeof(byte);
					break;

				case DbType.Decimal:
				case DbType.Currency:
				case DbType.VarNumeric:
					clrType = typeof(decimal);
					break;

				case DbType.Date:
				case DbType.DateTime:
				case DbType.DateTime2:
				case DbType.Time:
					clrType = typeof(DateTime);
					break;

				case DbType.DateTimeOffset:
					clrType = typeof(DateTimeOffset);
					break;

				case DbType.Double:
					clrType = typeof(double);
					break;

				case DbType.Guid:
					clrType = typeof(Guid);
					break;

				case DbType.Int16:
					clrType = typeof(short);
					break;

				case DbType.Int32:
					clrType = typeof(int);
					break;

				case DbType.Int64:
					clrType = typeof(long);
					break;

				case DbType.Object:
					clrType = typeof(object);
					break;

				case DbType.SByte:
					clrType = typeof(sbyte);
					break;

				case DbType.Single:
					clrType = typeof(float);
					break;

				case DbType.UInt16:
					clrType = typeof(ushort);
					break;
				case DbType.UInt32:
					clrType = typeof(uint);
					break;
				case DbType.UInt64:
					clrType = typeof(ulong);
					break;

				case DbType.Binary:
					clrType = typeof(byte[]); // todo: check it
					break;

				default:
					throw new ArgumentOutOfRangeException(nameof(dbType), dbType, $"Unsupported {typeof(DbType)}: {dbType}");
			}

			return clrType;
		}

		internal static Type GetBaseType(NpgsqlDbType npgsqlDbType)
		{
			switch (npgsqlDbType)
			{
				case NpgsqlDbType.Bigint:
					return typeof(long);

				case NpgsqlDbType.Double:
					return typeof(double);

				case NpgsqlDbType.Integer:
					return typeof(int);

				case NpgsqlDbType.Money:
				case NpgsqlDbType.Numeric:
					return typeof(decimal);

				case NpgsqlDbType.Real:
					return typeof(float);

				case NpgsqlDbType.Smallint:
					return typeof(short);

				case NpgsqlDbType.Boolean:
				case NpgsqlDbType.Bit:
					return typeof(bool);

				case NpgsqlDbType.Box:
					return typeof(NpgsqlBox);

				case NpgsqlDbType.Line:
					return typeof(NpgsqlLine);

				case NpgsqlDbType.LSeg:
					return typeof(NpgsqlLSeg);

				case NpgsqlDbType.Path:
					return typeof(NpgsqlPath);

				case NpgsqlDbType.Point:
					return typeof(NpgsqlPoint);

				case NpgsqlDbType.Polygon:
					return typeof(NpgsqlPolygon);

				case NpgsqlDbType.Char:
				case NpgsqlDbType.InternalChar:
					return typeof(char);

				case NpgsqlDbType.Text:
				case NpgsqlDbType.Varchar:
				case NpgsqlDbType.Name:
				case NpgsqlDbType.Citext:
				case NpgsqlDbType.Xml:
				case NpgsqlDbType.Json:
				case NpgsqlDbType.Jsonb:
					return typeof(string);

				case NpgsqlDbType.Bytea:
				case NpgsqlDbType.Varbit:
					return typeof(byte[]);

				case NpgsqlDbType.Date:
				case NpgsqlDbType.Timestamp:
				case NpgsqlDbType.TimestampTz:
				case NpgsqlDbType.Abstime:
					return typeof(DateTime);

				case NpgsqlDbType.Time:
				case NpgsqlDbType.Interval:
					return typeof(TimeSpan);

				case NpgsqlDbType.TimeTz:
					return typeof(DateTimeOffset);

				case NpgsqlDbType.Inet:
					return typeof(IPAddress);

				case NpgsqlDbType.Cidr:
					return typeof((IPAddress, int));

				case NpgsqlDbType.MacAddr:
				case NpgsqlDbType.MacAddr8:
					return typeof(PhysicalAddress);

				case NpgsqlDbType.TsVector:
					return typeof(NpgsqlTsVector);

				case NpgsqlDbType.TsQuery:
					return typeof(NpgsqlTsQuery);

				case NpgsqlDbType.Uuid:
					return typeof(Guid);

				case NpgsqlDbType.Hstore:
					return typeof(Dictionary<string, string>);

				case NpgsqlDbType.Oidvector:
					return typeof(uint[]);

				case NpgsqlDbType.Xid:
				case NpgsqlDbType.Oid:
				case NpgsqlDbType.Cid:
					return typeof(uint);
				
				case NpgsqlDbType.Xid8:
					return typeof(ulong);

				case NpgsqlDbType.Regconfig:
				case NpgsqlDbType.JsonPath:
				case NpgsqlDbType.PgLsn:
				case NpgsqlDbType.Geometry:
				case NpgsqlDbType.Geography:
				case NpgsqlDbType.LTree:
				case NpgsqlDbType.LQuery:
				case NpgsqlDbType.LTxtQuery:
				case NpgsqlDbType.IntegerRange:
				case NpgsqlDbType.BigIntRange:
				case NpgsqlDbType.NumericRange:
				case NpgsqlDbType.TimestampRange:
				case NpgsqlDbType.TimestampTzRange:
				case NpgsqlDbType.DateRange:
				case NpgsqlDbType.IntegerMultirange:
				case NpgsqlDbType.BigIntMultirange:
				case NpgsqlDbType.NumericMultirange:
				case NpgsqlDbType.TimestampMultirange:
				case NpgsqlDbType.TimestampTzMultirange:
				case NpgsqlDbType.DateMultirange:
				case NpgsqlDbType.Multirange:
					throw new ArgumentOutOfRangeException(nameof(npgsqlDbType), npgsqlDbType, $"Unsupported {typeof(NpgsqlDbType)}: {npgsqlDbType}, need research");
				
				
				
				case NpgsqlDbType.Unknown:
				case NpgsqlDbType.Tid:
				case NpgsqlDbType.Regtype:
				case NpgsqlDbType.Int2Vector:
				case NpgsqlDbType.Array:
				case NpgsqlDbType.Range:
				case NpgsqlDbType.Refcursor:
				case NpgsqlDbType.Circle:
				default:
					throw new ArgumentOutOfRangeException(nameof(npgsqlDbType), npgsqlDbType, $"Unsupported {typeof(NpgsqlDbType)}: {npgsqlDbType}");
			}
		}
	}
}