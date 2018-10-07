using System.Collections.Generic;
using SqlFirst.Core;

namespace SqlFirst.Codegen.Text.ParameterItem
{
	internal sealed class FieldDetailsWithoutMetadataEqualityComparer : IEqualityComparer<IFieldDetails>
	{
		public bool Equals(IFieldDetails x, IFieldDetails y)
		{
			if (ReferenceEquals(x, y))
			{
				return true;
			}

			if (ReferenceEquals(x, null))
			{
				return false;
			}

			if (ReferenceEquals(y, null))
			{
				return false;
			}

			return string.Equals(x.ColumnName, y.ColumnName) && x.ColumnOrdinal == y.ColumnOrdinal && x.AllowDbNull == y.AllowDbNull && string.Equals(x.DbType, y.DbType);
		}

		public int GetHashCode(IFieldDetails obj)
		{
			unchecked
			{
				int hashCode = obj.ColumnOrdinal;
				hashCode = (hashCode * 397) ^ (obj.ColumnName != null ? obj.ColumnName.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ obj.AllowDbNull.GetHashCode();
				hashCode = (hashCode * 397) ^ (obj.DbType != null ? obj.DbType.GetHashCode() : 0);
				return hashCode;
			}
		}
	}
}