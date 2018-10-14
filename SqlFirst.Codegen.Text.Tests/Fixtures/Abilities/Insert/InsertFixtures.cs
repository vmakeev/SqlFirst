using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.CompilerServices;
using SqlFirst.Codegen.Text.Tests.Fixtures.Abilities.Common;

namespace SqlFirst.Codegen.Text.Tests.Fixtures.Abilities.Insert
{
	[SuppressMessage("ReSharper", "InconsistentNaming")]
	internal class InsertFixtures
	{
		public string GetMultipleInsertQueryTextPrecompiledAbility_Method_GetQueryText => GetFixtureText();

		public string GetMultipleInsertQueryTextRuntimeCachedAbility_Type_NumberedParameterInfo => GetFixtureText();

		public string GetMultipleInsertQueryTextRuntimeCachedAbility_Method_GetQueryText_MultipleRows => GetFixtureText();

		public string GetMultipleInsertQueryTextRuntimeCachedAbility_Method_GetQueryText => GetFixtureText();

		public string GetMultipleInsertQueryTextRuntimeCachedAbility_Method_GetQueryTemplates => GetFixtureText();

		public string GetMultipleInsertQueryTextRuntimeCachedAbility_Method_GetNumberedParameters => GetFixtureText();

		public string InsertMultipleValuesAbility_Method_Add => GetFixtureText();

		public string InsertMultipleValuesAsyncAbility_Method_AddAsync => GetFixtureText();

		public string InsertMultipleValuesWithResultAbility_Method_Add => GetFixtureText();

		public string InsertMultipleValuesWithResultAsyncAbility_Method_AddAsync => GetFixtureText();

		public string InsertMultipleValuesWithScalarResultAbility_Method_Add => GetFixtureText();

		public string InsertMultipleValuesWithScalarResultAsyncAbility_Method_AddAsync => GetFixtureText();

		public string InsertSingleValuePlainAbility_Method_Add => GetFixtureText();

		public string InsertSingleValuePlainAsyncAbility_Method_AddAsync => GetFixtureText();

		public string InsertSingleValuePlainWithResultAbility_Method_Add => GetFixtureText();

		public string InsertSingleValuePlainWithResultAsyncAbility_Method_AddAsync => GetFixtureText();

		public string InsertSingleValuePlainWithScalarResultAbility_Method_Add => GetFixtureText();

		public string InsertSingleValuePlainWithScalarResultAsyncAbility_Method_AddAsync => GetFixtureText();

		private static string GetFixtureText([CallerMemberName] string name = null)
		{
			Stream stream = typeof(CommonFixtures).Assembly.GetManifestResourceStream($"SqlFirst.Codegen.Text.Tests.Fixtures.Abilities.Insert.{name}.txt");
			string queryText = new StreamReader(stream ?? throw new InvalidOperationException()).ReadToEnd();
			return queryText;
		}
	}
}
