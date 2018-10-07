using System.Collections.Generic;
using System.Linq;
using SqlFirst.Core;

namespace SqlFirst.Codegen.Text.ParameterItem
{
	internal sealed class ComplexTypeParametersComparer : IEqualityComparer<IQueryParamInfo>
	{
		private static readonly IEqualityComparer<IFieldDetails> _fieldDetailsComparer = new FieldDetailsWithoutMetadataEqualityComparer();

		public bool Equals(IQueryParamInfo x, IQueryParamInfo y)
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

			return string.Equals(x.DbType, y.DbType) && x.IsComplexType == y.IsComplexType && ComplexTypeDataEquals(x.ComplexTypeData, y.ComplexTypeData);
		}

		private bool ComplexTypeDataEquals(IComplexTypeData x, IComplexTypeData y)
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

			if (x.Fields == null || y.Fields == null)
			{
				return x.Fields != null || y.Fields != null;
			}

			return x.Fields.SequenceEqual(y.Fields, _fieldDetailsComparer);
		}

		public int GetHashCode(IQueryParamInfo obj)
		{
			unchecked
			{
				int hashCode = (obj.DbType != null ? obj.DbType.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ obj.IsComplexType.GetHashCode();
				return hashCode;
			}
		}
	}
}