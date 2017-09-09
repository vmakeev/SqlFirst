using System;
using System.Data;
using SqlFirst.Core;

namespace SqlFirst.Providers.MsSqlServer
{
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
					throw new Exception("type not matched : " + dbType);
			}
		}
	}
}