using System;
using System.Data;
using SqlFirst.Core;

namespace SqlFirst.Providers.MsSqlServer
{
	public class MsSqlServerTypeMapper : IDatabaseTypeMapper
	{
		/// <inheritdoc />
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
			switch (MsSqlDbType.Normalize(dbType))
			{
				case MsSqlDbType.Char:
				case MsSqlDbType.NChar:
				case MsSqlDbType.NText:
				case MsSqlDbType.NVarChar:
				case MsSqlDbType.VarChar:
				case MsSqlDbType.Text:
					return DbType.String;

				case MsSqlDbType.Xml:
					return DbType.Xml;

				case MsSqlDbType.DateTime:
				case MsSqlDbType.DateTime2:
				case MsSqlDbType.SmallDateTime:
					return DbType.DateTime;

				case MsSqlDbType.Time:
					return DbType.Time;

				case MsSqlDbType.Date:
					return DbType.Date;

				case MsSqlDbType.Binary:
				case MsSqlDbType.Image:
				case MsSqlDbType.Timestamp:
				case MsSqlDbType.VarBinary:
					return DbType.Binary;

				case MsSqlDbType.SmallMoney:
				case MsSqlDbType.Decimal:
					return DbType.Decimal;

				case MsSqlDbType.Money:
					return DbType.Currency;

				case MsSqlDbType.Bit:
					return DbType.Boolean;

				case MsSqlDbType.Bigint:
					return DbType.Int64;

				case MsSqlDbType.DateTimeOffset:
					return DbType.DateTimeOffset;

				case MsSqlDbType.Real:
				case MsSqlDbType.Float:
					return DbType.Single;

				case MsSqlDbType.Smallint:
					return DbType.Int16;

				case MsSqlDbType.Tinyint:
					return DbType.Byte;

				case MsSqlDbType.Int:
					return DbType.Int32;

				case MsSqlDbType.UniqueIdentifier:
					return DbType.Guid;

				default:
					throw new ArgumentOutOfRangeException(nameof(dbType), dbType);
			}
		}

		private static Type GetBaseType(string dbType)
		{
			switch (MsSqlDbType.Normalize(dbType))
			{
				case MsSqlDbType.Char:
				case MsSqlDbType.NChar:
				case MsSqlDbType.NText:
				case MsSqlDbType.NVarChar:
				case MsSqlDbType.VarChar:
				case MsSqlDbType.Text:
				case MsSqlDbType.Xml:
					return typeof(string);

				case MsSqlDbType.Date:
				case MsSqlDbType.DateTime:
				case MsSqlDbType.DateTime2:
				case MsSqlDbType.SmallDateTime:
				case MsSqlDbType.Time:
					return typeof(DateTime);

				case MsSqlDbType.Binary:
				case MsSqlDbType.Image:
				case MsSqlDbType.Timestamp:
				case MsSqlDbType.VarBinary:
					return typeof(byte[]);

				case MsSqlDbType.Decimal:
				case MsSqlDbType.Money:
				case MsSqlDbType.SmallMoney:
					return typeof(decimal);

				case MsSqlDbType.SqlVariant:
				case MsSqlDbType.Variant:
				case MsSqlDbType.Udt:
					return typeof(object);

				case MsSqlDbType.Bit:
					return typeof(bool);

				case MsSqlDbType.Bigint:
					return typeof(long);

				case MsSqlDbType.DateTimeOffset:
					return typeof(DateTimeOffset);

				case MsSqlDbType.Float:
					return typeof(double);

				case MsSqlDbType.Real:
					return typeof(float);

				case MsSqlDbType.Smallint:
					return typeof(short);

				case MsSqlDbType.Tinyint:
					return typeof(byte);

				case MsSqlDbType.Int:
					return typeof(int);

				case MsSqlDbType.Structured:
					return typeof(DataTable);

				case MsSqlDbType.UniqueIdentifier:
					return typeof(Guid);

				default:
					throw new ArgumentOutOfRangeException(nameof(dbType), dbType);
			}
		}
	}
}