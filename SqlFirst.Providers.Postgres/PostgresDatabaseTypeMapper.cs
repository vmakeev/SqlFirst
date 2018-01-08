using System;
using System.Collections;
using System.Data;
using NpgsqlTypes;
using SqlFirst.Core;

namespace SqlFirst.Providers.Postgres
{
	public class PostgresDatabaseTypeMapper : IDatabaseTypeMapper
	{
		/// <summary>
		/// Возвращает имя типа CLR, который может быть безопасно использован для представления указанного типа данных в БД
		/// </summary>
		/// <param name="dbType">Название типа данных в БД</param>
		/// <param name="nullable">Поддерживается ли значение null</param>
		/// <returns>Имя типа CLR</returns>
		public Type MapToClrType(string dbType, bool nullable)
		{
			Type baseType = GetBaseType(dbType);

			if (nullable && baseType.IsValueType)
			{
				return typeof(Nullable<>).MakeGenericType(baseType);
			}

			return baseType;
		}

		/// <summary>
		/// Возвращает <see cref="DbType"/>, который может быть безопасно использован для представления указанного типа данных в БД
		/// </summary>
		/// <param name="dbType">Название типа данных в БД</param>
		/// <returns><see cref="DbType"/></returns>
		public DbType MapToDbType(string dbType)
		{
			switch (PostgresDbType.Normalize(dbType))
			{
				case PostgresDbType.Int8:
				case PostgresDbType.Bigint:
				case PostgresDbType.Bigserial:
				case PostgresDbType.Serial8:
					return DbType.Int64;

				case PostgresDbType.Integer:
				case PostgresDbType.Int:
				case PostgresDbType.Int4:
				case PostgresDbType.Serial:
				case PostgresDbType.Serial4:
					return DbType.Int32;

				case PostgresDbType.Smallint:
				case PostgresDbType.Int2:
				case PostgresDbType.Smallserial:
				case PostgresDbType.Serial2:
					return DbType.Int16;

				case PostgresDbType.Money:
					return DbType.Currency;

				case PostgresDbType.Numeric:
				case PostgresDbType.Decimal:
					return DbType.Decimal;

				case PostgresDbType.Real:
				case PostgresDbType.Float4:
					return DbType.Single;

				case PostgresDbType.DoublePrecision:
				case PostgresDbType.Float8:
					return DbType.Double;

				case PostgresDbType.CharacterVarying:
				case PostgresDbType.Varchar:
				case PostgresDbType.Json:
				case PostgresDbType.Jsonb:
				case PostgresDbType.Text:
					return DbType.String;

				case PostgresDbType.Xml:
					return DbType.Xml;

				case PostgresDbType.Interval:
				case PostgresDbType.Timestamp:
				case PostgresDbType.TimestampWithoutTimeZone:
				case PostgresDbType.TimestampWithTimeZone:
				case PostgresDbType.TimestampTZ:
					return DbType.DateTime;

				case PostgresDbType.Time:
				case PostgresDbType.TimeWithoutTimeZone:
				case PostgresDbType.TimeWithTimeZone:
				case PostgresDbType.TimeTZ:
					return DbType.Time;

				case PostgresDbType.Date:
					return DbType.Date;

				case PostgresDbType.Guid:
					return DbType.Guid;

				case PostgresDbType.Bit:
				case PostgresDbType.BitVarying:
				case PostgresDbType.Varbit:
					return DbType.Binary;

				case PostgresDbType.Boolean:
				case PostgresDbType.Bool:
					return DbType.Boolean;

				case PostgresDbType.Bytea:
					return DbType.Binary;

				case PostgresDbType.Character:
				case PostgresDbType.Char:
					return DbType.String;

				case PostgresDbType.Box:
				case PostgresDbType.Cidr:
				case PostgresDbType.Line:
				case PostgresDbType.Lseg:
				case PostgresDbType.Point:
				case PostgresDbType.Macaddr:
				case PostgresDbType.Macaddr8:
					return DbType.String;

				case PostgresDbType.Path:
					return DbType.Binary;

				case PostgresDbType.Polygon:
				case PostgresDbType.Tsquery:
				case PostgresDbType.Tsvector:
					return DbType.Binary;

				default:
					throw new ArgumentOutOfRangeException(nameof(dbType), dbType, $"Unsupported {typeof(PostgresDbType)}: {dbType}");
			}
		}

		private static Type GetBaseType(string dbType)
		{
			int.TryParse(PostgresDbType.GetLength(dbType), out int typeLength);

			switch (PostgresDbType.Normalize(dbType))
			{
				case PostgresDbType.Int8:
				case PostgresDbType.Bigint:
				case PostgresDbType.Bigserial:
				case PostgresDbType.Serial8:
					return typeof(long);

				case PostgresDbType.Integer:
				case PostgresDbType.Int:
				case PostgresDbType.Int4:
				case PostgresDbType.Serial:
				case PostgresDbType.Serial4:
					return typeof(int);

				case PostgresDbType.Smallint:
				case PostgresDbType.Int2:
				case PostgresDbType.Smallserial:
				case PostgresDbType.Serial2:
					return typeof(short);

				case PostgresDbType.Money:
				case PostgresDbType.Numeric:
				case PostgresDbType.Decimal:
					return typeof(decimal);

				case PostgresDbType.Real:
				case PostgresDbType.Float4:
					return typeof(float);

				case PostgresDbType.DoublePrecision:
				case PostgresDbType.Float8:
					return typeof(double);

				case PostgresDbType.CharacterVarying:
				case PostgresDbType.Varchar:
				case PostgresDbType.Json:
				case PostgresDbType.Jsonb:
				case PostgresDbType.Text:
				case PostgresDbType.Xml:
					return typeof(string);

				case PostgresDbType.Date:
				case PostgresDbType.Time:
				case PostgresDbType.TimeWithoutTimeZone:
				case PostgresDbType.TimeWithTimeZone:
				case PostgresDbType.TimeTZ:
				case PostgresDbType.Timestamp:
				case PostgresDbType.TimestampWithoutTimeZone:
				case PostgresDbType.TimestampWithTimeZone:
				case PostgresDbType.TimestampTZ:
					return typeof(DateTime);

				case PostgresDbType.Interval:
					return typeof(TimeSpan);

				case PostgresDbType.Guid:
					return typeof(Guid);

				case PostgresDbType.Bit:
					if (typeLength == 1)
					{
						return typeof(bool);
					}
					else
					{
						return typeof(BitArray);
					}

				case PostgresDbType.BitVarying:
				case PostgresDbType.Varbit:
					return typeof(BitArray);

				case PostgresDbType.Boolean:
				case PostgresDbType.Bool:
					return typeof(bool);

				case PostgresDbType.Bytea:
					return typeof(byte[]);

				case PostgresDbType.Character:
				case PostgresDbType.Char:
					return typeof(char);

				case PostgresDbType.Box:
					return typeof(string); // NpgsqlBox

				case PostgresDbType.Cidr:
					return typeof(string); // NpgsqlInet

				case PostgresDbType.Circle:
					return typeof(string); // NpgsqlCircle

				case PostgresDbType.Line:
					return typeof(string); // NpgsqlLine

				case PostgresDbType.Lseg:
					return typeof(string); // NpgsqlLSeg

				case PostgresDbType.Point:
					return typeof(string); // NpgsqlPoint

				case PostgresDbType.Macaddr:
				case PostgresDbType.Macaddr8:
					return typeof(string); // PhysicalAddress

				case PostgresDbType.Path:
					return typeof(NpgsqlPath);

				case PostgresDbType.Polygon:
					return typeof(NpgsqlPolygon);

				case PostgresDbType.Tsquery:
					return typeof(NpgsqlTsQuery);

				case PostgresDbType.Tsvector:
					return typeof(NpgsqlTsVector);

				default:
					throw new ArgumentOutOfRangeException(nameof(dbType), dbType, $"Unsupported {typeof(PostgresDbType)}: {dbType}");

			}
		}
	}
}