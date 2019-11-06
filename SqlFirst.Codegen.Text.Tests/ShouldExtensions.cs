using System.Collections.Generic;
using System.Linq;
using Shouldly;

namespace SqlFirst.Codegen.Text.Tests
{
	internal static class ShouldExtensions
	{
		private static string NormalizeCrlf(string source)
		{
			if (string.IsNullOrEmpty(source))
			{
				return source;
			}

			return source.Replace("\r\n", "\n");
		}

		public static void ShouldContain(this IEnumerable<string> actual, string expected)
		{
			string normalizedCrlf = NormalizeCrlf(expected);
			IEnumerable<string> normalizedActual = actual.Select(NormalizeCrlf);

			ShouldBeEnumerableTestExtensions.ShouldContain(normalizedActual, normalizedCrlf);
		}
	}
}