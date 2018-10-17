using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.CompilerServices;
using SqlFirst.Codegen.Text.Tests.Fixtures.Abilities.Common;

namespace SqlFirst.Codegen.Text.Tests.Fixtures.Abilities.AddQueryCustomParameter
{
	[SuppressMessage("ReSharper", "InconsistentNaming")]
	internal class AddQueryCustomParameterFixtures
	{
		public string AddQueryCustomParameterAbility_Success => GetFixtureText();

		private static string GetFixtureText([CallerMemberName] string name = null)
		{
			Stream stream = typeof(CommonFixtures).Assembly.GetManifestResourceStream($"SqlFirst.Codegen.Text.Tests.Fixtures.Abilities.AddQueryCustomParameter.{name}.txt");
			string queryText = new StreamReader(stream ?? throw new InvalidOperationException()).ReadToEnd();
			return queryText;
		}
	}
}
