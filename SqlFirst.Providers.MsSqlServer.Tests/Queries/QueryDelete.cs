using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace SqlFirst.Providers.MsSqlServer.Tests.Queries
{
	internal static class QueryDelete
	{
		private static string GetQueryText([CallerMemberName] string name = null)
		{
			Stream stream = typeof(QuerySelect).Assembly.GetManifestResourceStream($"SqlFirst.Providers.MsSqlServer.Tests.Queries.Delete.{name}.sql");
			string queryText = new StreamReader(stream ?? throw new InvalidOperationException()).ReadToEnd();
			return queryText;
		}

		public static string DeleteByGuid => GetQueryText();
	}
}