using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace SqlFirst.Demo.Wpf.HighlightSchemes
{
	internal static class Highlight
	{
		private static string GetText([CallerMemberName] string name = null)
		{
			Stream stream = typeof(Highlight).Assembly.GetManifestResourceStream($"SqlFirst.Demo.Wpf.HighlightSchemes.{name}.xml");
			string queryText = new StreamReader(stream ?? throw new InvalidOperationException()).ReadToEnd();
			return queryText;
		}

		public static string Sql => GetText();

		public static string CSharp => GetText();
	}
}
