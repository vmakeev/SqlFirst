using System.Collections.Generic;

namespace SqlFirst.Codegen.Helpers
{
	internal static class EnumerableExtensions
	{
		public static IEnumerable<T> AsCacheable<T>(this IEnumerable<T> source)
		{
			if (source == null)
			{
				return null;
			}

			return new CacheableEnumerable<T>(source);
		}
	}
}