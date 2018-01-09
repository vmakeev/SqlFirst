using System;
using System.Data;

namespace SqlFirst.Providers.Postgres
{
	internal class PostgresAdoTypeMapper
	{
		/// <summary>
		/// Возвращает имя типа CLR, который может быть безопасно использован для представления указанного типа данных в БД
		/// </summary>
		/// <param name="dbType">Название типа данных в БД</param>
		/// <param name="nullable">Поддерживается ли значение null</param>
		/// <returns>Имя типа CLR</returns>
		public Type Map(DbType dbType, bool nullable)
		{
			Type baseType = GetBaseType(dbType);

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
	}
}