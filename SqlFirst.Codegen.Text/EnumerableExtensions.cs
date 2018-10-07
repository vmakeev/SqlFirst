using System;
using System.Collections.Generic;
using System.Linq;

namespace SqlFirst.Codegen.Text
{
	internal static class EnumerableExtensions
	{
		private class DelegateComparer<T, TIdentity> : IEqualityComparer<T>
		{
			private readonly Func<T, TIdentity> _identitySelector;

			public DelegateComparer(Func<T, TIdentity> identitySelector)
			{
				_identitySelector = identitySelector;
			}

			public bool Equals(T x, T y)
			{
				return Equals(_identitySelector(x), _identitySelector(y));
			}

			public int GetHashCode(T obj)
			{
				return _identitySelector(obj).GetHashCode();
			}
		}

		public static IEnumerable<T> AppendItems<T>(this IEnumerable<T> source, params T[] values)
		{
			return source.Concat(values);
		}

		public static IEnumerable<T> DistinctBy<T, TIdentity>(this IEnumerable<T> source, Func<T, TIdentity> identitySelector)
		{
			return source.Distinct(By(identitySelector));
		}

		public static IEqualityComparer<TSource> By<TSource, TIdentity>(Func<TSource, TIdentity> identitySelector)
		{
			return new DelegateComparer<TSource, TIdentity>(identitySelector);
		}
	}
}