using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.CompilerServices;
using SqlFirst.Codegen.Text.Tests.Fixtures.Abilities.Common;

namespace SqlFirst.Codegen.Text.Tests.Fixtures.Abilities.Update
{
	[SuppressMessage("ReSharper", "InconsistentNaming")]
	internal class UpdateFixtures
	{
		public string UpdateAbility_Method_Update => GetFixtureText();

		public string UpdateAsyncAbility_Method_UpdateAsync => GetFixtureText();

		public string UpdateWithResultAbility_Method_Update => GetFixtureText();

		public string UpdateWithResultAsyncAbility_Method_UpdateAsync => GetFixtureText();

		public string UpdateWithScalarResultAbility_Method_Update => GetFixtureText();

		public string UpdateWithScalarResultAsyncAbility_Method_UpdateAsync => GetFixtureText();

		private static string GetFixtureText([CallerMemberName] string name = null)
		{
			Stream stream = typeof(CommonFixtures).Assembly.GetManifestResourceStream($"SqlFirst.Codegen.Text.Tests.Fixtures.Abilities.Update.{name}.txt");
			string queryText = new StreamReader(stream ?? throw new InvalidOperationException()).ReadToEnd();
			return queryText;
		}
	}
}
