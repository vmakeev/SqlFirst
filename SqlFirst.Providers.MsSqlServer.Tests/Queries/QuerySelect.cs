﻿using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace SqlFirst.Providers.MsSqlServer.Tests.Queries
{
	internal static class QuerySelect
	{
		public static string SelectGuidAndDateWithUnknownUnnamedAndCustomSections => GetQueryText();

		public static string SelectGuidAndDateWithUnknownAndCustomSections => GetQueryText();

		public static string SelectGuidAndDateWithUnknownSections => GetQueryText();

		public static string SelectGuidAndDateWithMiltipleSections => GetQueryText();

		public static string SelectGuidAndDateWithPaging => GetQueryText();

		public static string SelectCount => GetQueryText();

		public static string SelectTwoStringsWithLeftJoin => GetQueryText();

		public static string SelectTwoStringsWithInnerJoin => GetQueryText();

		public static string SelectDateWithOrdinal => GetQueryText();

		public static string SelectDateWithNamedOrdinal => GetQueryText();

		public static string SelectGuidAndDateWithPagingAssignmentAndComments => GetQueryText();

		public static string SelectGuidAndDateWithPagingNoParameters => GetQueryText();

		public static string SelectGuidAndDateWithPagingAndPartOfParameters => GetQueryText();

		public static string SelectNotUniqueFieldsWithLeftJoin => GetQueryText();

		public static string SelectNotUniqueFieldsWithInnerJoin => GetQueryText();

		public static string SelectNotUniqueFieldsWithRightJoin => GetQueryText();

		public static string SelectNotUniqueNamedFieldsWithInnerJoin => GetQueryText();

		private static string GetQueryText([CallerMemberName] string name = null)
		{
			Stream stream = typeof(QuerySelect).Assembly.GetManifestResourceStream($"SqlFirst.Providers.MsSqlServer.Tests.Queries.Select.{name}.sql");
			string queryText = new StreamReader(stream ?? throw new InvalidOperationException()).ReadToEnd();
			return queryText;
		}
	}
}