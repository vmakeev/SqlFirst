using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.CompilerServices;
using SqlFirst.Codegen.Text.Tests.Fixtures.Abilities.Common;

namespace SqlFirst.Codegen.Text.Tests.Fixtures.Abilities.Select
{
	[SuppressMessage("ReSharper", "InconsistentNaming")]
	internal class SelectFixtures
	{
		public string SelectFirstItemAbility_Method_GetFirst => GetFixtureText();

		public string SelectFirstItemAsyncAbility_Method_GetFirstAsync => GetFixtureText();

		public string SelectItemsIEnumerableAsyncNestedEnumerableAbility_Method_GetAsync => GetFixtureText();

		public string SelectItemsLazyAbility_Method_Get => GetFixtureText();

		public string SelectScalarAbility_Method_GetFirst => GetFixtureText();

		public string SelectScalarAsyncAbility_Method_GetFirstAsync => GetFixtureText();

		public string SelectScalarsAbility_Method_Get => GetFixtureText();

		public string SelectScalarsIEnumerableAsyncNestedEnumerableAbility_Method_GetAsync => GetFixtureText();

		private static string GetFixtureText([CallerMemberName] string name = null)
		{
			Stream stream = typeof(CommonFixtures).Assembly.GetManifestResourceStream($"SqlFirst.Codegen.Text.Tests.Fixtures.Abilities.Select.{name}.txt");
			string queryText = new StreamReader(stream ?? throw new InvalidOperationException()).ReadToEnd();
			return queryText;
		}
	}
}
