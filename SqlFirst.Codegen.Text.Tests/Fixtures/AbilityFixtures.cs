using System.Diagnostics.CodeAnalysis;
using SqlFirst.Codegen.Text.Tests.Fixtures.Abilities.Common;
using SqlFirst.Codegen.Text.Tests.Fixtures.Abilities.Delete;
using SqlFirst.Codegen.Text.Tests.Fixtures.Abilities.Insert;
using SqlFirst.Codegen.Text.Tests.Fixtures.Abilities.Select;
using SqlFirst.Codegen.Text.Tests.Fixtures.Abilities.Update;

namespace SqlFirst.Codegen.Text.Tests.Fixtures
{
	[SuppressMessage("ReSharper", "InconsistentNaming")]
	internal static class AbilityFixtures
	{
		public static CommonFixtures Common { get; } = new CommonFixtures();

		public static DeleteFixtures Delete { get; } = new DeleteFixtures();

		public static InsertFixtures Insert { get; } = new InsertFixtures();

		public static SelectFixtures Select { get; } = new SelectFixtures();

		public static UpdateFixtures Update { get; } = new UpdateFixtures();

	}
}
