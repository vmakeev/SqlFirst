using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.CompilerServices;

namespace SqlFirst.Codegen.Text.Tests.Fixtures.Abilities.Common
{
	[SuppressMessage("ReSharper", "InconsistentNaming")]
	internal class CommonFixtures
	{
		public string AddSqlConnectionParameterAbility_Method_AddParameter => GetFixtureText();

		public string GetQueryTextFromResourceCacheableAbility_Method_CalculateChecksum => GetFixtureText();

		public string GetQueryTextFromResourceCacheableAbility_Method_GetQueryText => GetFixtureText();

		public string GetQueryTextFromStringAbility_Method_GetQueryText => GetFixtureText();

		public string MapDataRecordToItemAbility_Method_GetItemFromRecord => GetFixtureText();

		public string MapDataRecordToScalarAbility_Method_GetScalarFromRecord => GetFixtureText();

		public string MapValueToScalarAbility_Method_GetScalarFromValue => GetFixtureText();

		private static string GetFixtureText([CallerMemberName] string name = null)
		{
			Stream stream = typeof(CommonFixtures).Assembly.GetManifestResourceStream($"SqlFirst.Codegen.Text.Tests.Fixtures.Abilities.Common.{name}.txt");
			string queryText = new StreamReader(stream ?? throw new InvalidOperationException()).ReadToEnd();
			return queryText;
		}
	}
}
