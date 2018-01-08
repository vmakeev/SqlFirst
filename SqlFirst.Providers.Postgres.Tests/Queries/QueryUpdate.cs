using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace SqlFirst.Providers.Postgres.Tests.Queries
{
	internal static class QueryUpdate
	{
		private static string GetQueryText([CallerMemberName] string name = null)
		{
			Stream stream = typeof(QuerySelect).Assembly.GetManifestResourceStream($"SqlFirst.Providers.Postgres.Tests.Queries.Update.{name}.sql");
			string queryText = new StreamReader(stream ?? throw new InvalidOperationException()).ReadToEnd();
			return queryText;
		}

		public static string UpdateDateByGuid => GetQueryText();
	}
}
