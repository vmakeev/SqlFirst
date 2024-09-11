using NpgsqlTypes;

namespace SqlFirst.Providers.Postgres;

public static class ArrayTypesExtensions
{
	public static bool IsArrayType(this NpgsqlDbType type)
	{
		return type < 0;
	}
		
	public static bool IsDbTypeArrayType(this string dbType)
	{
		if (!int.TryParse(dbType, out var intValue))
		{
			return false;
		}

		return IsArrayType((NpgsqlDbType)intValue);
	}
		
	public static bool TryGetIntDbTypeNpgsqlDbTypeUnsafe(this string dbType, out NpgsqlDbType npgsqlType)
	{
		if (!int.TryParse(dbType, out var intValue))
		{
			npgsqlType = NpgsqlDbType.Array;
			return false;
		}

		npgsqlType = (NpgsqlDbType)intValue;
		return true;
	}
		
	public static bool TryGetDbTypeArrayItemType(this string dbType, out NpgsqlDbType arrayItemType)
	{
		if (!int.TryParse(dbType, out var intValue))
		{
			arrayItemType = NpgsqlDbType.Array;
			return false;
		}

		return TryGetArrayItemType((NpgsqlDbType)intValue, out arrayItemType);
	}
		
	public static bool TryGetArrayItemType(this NpgsqlDbType type, out NpgsqlDbType arrayItemType)
	{
		if (type.IsArrayType())
		{
			arrayItemType = type ^ NpgsqlDbType.Array;
			return true;
		}

		arrayItemType = NpgsqlDbType.Array;
		return false;
	}
}