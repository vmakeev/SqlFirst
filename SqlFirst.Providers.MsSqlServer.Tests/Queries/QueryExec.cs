using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace SqlFirst.Providers.MsSqlServer.Tests.Queries
{
	internal static class QueryExec
	{
		public static string ExecDeclaredStringParameter => GetQueryText();

		public static string ExecDeclaredTableParameterOneColumn => GetQueryText();

		public static string ExecNoParameters => GetQueryText();

		public static string ExecUndeclaredStringParameter => GetQueryText();

		public static string ExecUndeclaredTableParameterOneColumn => GetQueryText();

		public static string ExecDeclaredTableParameterTwoColumns => GetQueryText();

		public static string ExecUndeclaredTableParameterTwoColumns => GetQueryText();

		public static string ExecDeclaredTableParameterInvalidType => GetQueryText();

		private static string GetQueryText([CallerMemberName] string name = null)
		{
			Stream stream = typeof(QuerySelect).Assembly.GetManifestResourceStream($"SqlFirst.Providers.MsSqlServer.Tests.Queries.Exec.{name}.sql");
			string queryText = new StreamReader(stream ?? throw new InvalidOperationException()).ReadToEnd();
			return queryText;
		}
	}
}