using System;
using System.Collections.Generic;
using System.Data;
using SqlFirst.Core;

namespace SqlFirst.Providers.MsSqlServer
{
	public class MsSqlServerTypeMapper : IDatabaseTypeMapper
	{
		/// <inheritdoc />
		public Type MapToClrType(string dbType, bool nullable, IDictionary<string, object> metadata = null)
		{
			Type baseType = GetBaseType(dbType, metadata);

			if (nullable && baseType.IsValueType)
			{
				return typeof(Nullable<>).MakeGenericType(baseType);
			}

			return baseType;
		}

		/// <inheritdoc />
		public DbType MapToDbType(string dbType, IDictionary<string, object> metadata = null)
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

		/// <inheritdoc />
		public IProviderSpecificType MapToProviderSpecificType(string dbType, IDictionary<string, object> metadata = null)
		{
			switch (MsSqlDbType.Normalize(dbType))
			{
				case MsSqlDbType.Char:
					return new MsSqlServerProviderSpecificType(SqlDbType.Char);

				case MsSqlDbType.NChar:
					return new MsSqlServerProviderSpecificType(SqlDbType.NChar);

				case MsSqlDbType.NText:
					return new MsSqlServerProviderSpecificType(SqlDbType.NText);

				case MsSqlDbType.NVarChar:
					return new MsSqlServerProviderSpecificType(SqlDbType.NVarChar);

				case MsSqlDbType.VarChar:
					return new MsSqlServerProviderSpecificType(SqlDbType.VarChar);

				case MsSqlDbType.Text:
					return new MsSqlServerProviderSpecificType(SqlDbType.Text);

				case MsSqlDbType.Xml:
					return new MsSqlServerProviderSpecificType(SqlDbType.Xml);

				case MsSqlDbType.DateTime:
					return new MsSqlServerProviderSpecificType(SqlDbType.DateTime);

				case MsSqlDbType.DateTime2:
					return new MsSqlServerProviderSpecificType(SqlDbType.DateTime2);

				case MsSqlDbType.SmallDateTime:
					return new MsSqlServerProviderSpecificType(SqlDbType.SmallDateTime);

				case MsSqlDbType.Time:
					return new MsSqlServerProviderSpecificType(SqlDbType.Time);

				case MsSqlDbType.Date:
					return new MsSqlServerProviderSpecificType(SqlDbType.Date);

				case MsSqlDbType.Binary:
					return new MsSqlServerProviderSpecificType(SqlDbType.Binary);

				case MsSqlDbType.Image:
					return new MsSqlServerProviderSpecificType(SqlDbType.Image);

				case MsSqlDbType.Timestamp:
					return new MsSqlServerProviderSpecificType(SqlDbType.Timestamp);

				case MsSqlDbType.VarBinary:
					return new MsSqlServerProviderSpecificType(SqlDbType.VarBinary);

				case MsSqlDbType.SmallMoney:
					return new MsSqlServerProviderSpecificType(SqlDbType.SmallMoney);

				case MsSqlDbType.Decimal:
					return new MsSqlServerProviderSpecificType(SqlDbType.Decimal);

				case MsSqlDbType.Money:
					return new MsSqlServerProviderSpecificType(SqlDbType.Money);

				case MsSqlDbType.Bit:
					return new MsSqlServerProviderSpecificType(SqlDbType.Bit);

				case MsSqlDbType.Bigint:
					return new MsSqlServerProviderSpecificType(SqlDbType.BigInt);

				case MsSqlDbType.DateTimeOffset:
					return new MsSqlServerProviderSpecificType(SqlDbType.DateTimeOffset);

				case MsSqlDbType.Real:
					return new MsSqlServerProviderSpecificType(SqlDbType.Real);

				case MsSqlDbType.Float:
					return new MsSqlServerProviderSpecificType(SqlDbType.Float);

				case MsSqlDbType.Smallint:
					return new MsSqlServerProviderSpecificType(SqlDbType.SmallInt);

				case MsSqlDbType.Tinyint:
					return new MsSqlServerProviderSpecificType(SqlDbType.TinyInt);

				case MsSqlDbType.Int:
					return new MsSqlServerProviderSpecificType(SqlDbType.Int);

				case MsSqlDbType.UniqueIdentifier:
					return new MsSqlServerProviderSpecificType(SqlDbType.UniqueIdentifier);

				case string _ when MsSqlServerTypeMetadata.FromData(metadata).IsTableType:
					return new MsSqlServerProviderSpecificType(SqlDbType.Structured);

				default:
					throw new ArgumentOutOfRangeException(nameof(dbType), dbType);
			}
		}

		private static Type GetBaseType(string dbType, IDictionary<string, object> metadata)
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

				case string _ when MsSqlServerTypeMetadata.FromData(metadata).IsTableType:
					return typeof(DataTable);

				default:
					throw new ArgumentOutOfRangeException(nameof(dbType), dbType);
			}
		}

	}
}