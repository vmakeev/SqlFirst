using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.CompilerServices;
using SqlFirst.Codegen.Text.Tests.Fixtures.Abilities.Common;

namespace SqlFirst.Codegen.Text.Tests.Fixtures.Abilities.GetDataTable
{
	[SuppressMessage("ReSharper", "InconsistentNaming")]
	internal class GetDataTableFixtures
	{
		public string GetDataTableAbility_IsTableType_NonNullable_SingleIntColumn_NonNullable_Test => GetFixtureText();

		public string GetDataTableAbility_IsTableType_Nullable_SingleIntColumn_NonNullable_Test => GetFixtureText();

		public string GetDataTableAbility_IsTableType_Nullable_SingleIntColumn_Nullable_Test => GetFixtureText();

		public string GetDataTableAbility_IsTableType_Nullable_SingleStringColumn_Nullable_Test => GetFixtureText();

		public string GetDataTableAbility_IsTableType_Nullable_SingleStringColumn_NonNullable_Test => GetFixtureText();

		public string GetDataTableAbility_IsTableType_Nullable_TwoColumns_BothNonNullable_Test => GetFixtureText();

		public string GetDataTableAbility_IsTableType_Nullable_TwoColumns_BothNullable_Test => GetFixtureText();

		private static string GetFixtureText([CallerMemberName] string name = null)
		{
			Stream stream = typeof(CommonFixtures).Assembly.GetManifestResourceStream($"SqlFirst.Codegen.Text.Tests.Fixtures.Abilities.GetDataTable.{name}.txt");
			string queryText = new StreamReader(stream ?? throw new InvalidOperationException()).ReadToEnd();
			return queryText;
		}
	}
}
