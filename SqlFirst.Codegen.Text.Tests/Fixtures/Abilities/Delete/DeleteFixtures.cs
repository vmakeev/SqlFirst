using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.CompilerServices;
using SqlFirst.Codegen.Text.Tests.Fixtures.Abilities.Common;

namespace SqlFirst.Codegen.Text.Tests.Fixtures.Abilities.Delete
{
	[SuppressMessage("ReSharper", "InconsistentNaming")]
	internal class DeleteFixtures
	{
		public string DeleteAbility_Method_Delete => GetFixtureText();

		public string DeleteAsyncAbility_Method_DeleteAsync => GetFixtureText();

		public string DeleteWithResultAbility_Method_Delete => GetFixtureText();

		public string DeleteWithResultAsyncAbility_Method_DeleteAsync => GetFixtureText();

		public string DeleteWithScalarResultAbility_Method_Delete => GetFixtureText();

		public string DeleteWithScalarResultAsyncAbility_Method_DeleteAsync => GetFixtureText();

		private static string GetFixtureText([CallerMemberName] string name = null)
		{
			Stream stream = typeof(CommonFixtures).Assembly.GetManifestResourceStream($"SqlFirst.Codegen.Text.Tests.Fixtures.Abilities.Delete.{name}.txt");
			string queryText = new StreamReader(stream ?? throw new InvalidOperationException()).ReadToEnd();
			return queryText;
		}
	}
}
