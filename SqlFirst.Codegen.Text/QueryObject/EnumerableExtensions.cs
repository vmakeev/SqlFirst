using System.Collections.Generic;
using System.Linq;

namespace SqlFirst.Codegen.Text.QueryObject
{
	internal static class EnumerableExtensions
	{
		public static IEnumerable<T> Append<T>(this IEnumerable<T> source, params T[] values)
		{
			return source.Concat(values);
		}
	}
}