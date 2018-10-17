using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.CompilerServices;
using SqlFirst.Codegen.Text.Tests.Fixtures.Abilities.Common;

namespace SqlFirst.Codegen.Text.Tests.Fixtures.Abilities.StoredProcedure
{
	[SuppressMessage("ReSharper", "InconsistentNaming")]
	internal class StoredProcedureFixtures
	{
		public string StoredProcedure_Parameters_Custom_Method_Exec => GetFixtureText();

		public string StoredProcedure_Parameters_Empty_Method_Exec => GetFixtureText();

		public string StoredProcedure_Parameters_Mixed_Method_Exec => GetFixtureText();

		public string StoredProcedure_Parameters_Simple_Method_Exec => GetFixtureText();


		public string StoredProcedureAsync_Parameters_Custom_Method_ExecAsync => GetFixtureText();

		public string StoredProcedureAsync_Parameters_Empty_Method_ExecAsync => GetFixtureText();

		public string StoredProcedureAsync_Parameters_Mixed_Method_ExecAsync => GetFixtureText();

		public string StoredProcedureAsync_Parameters_Simple_Method_ExecAsync => GetFixtureText();


		public string StoredProcedureWithResult_Parameters_Custom_Method_Exec => GetFixtureText();

		public string StoredProcedureWithResult_Parameters_Empty_Method_Exec => GetFixtureText();

		public string StoredProcedureWithResult_Parameters_Mixed_Method_Exec => GetFixtureText();

		public string StoredProcedureWithResult_Parameters_Simple_Method_Exec => GetFixtureText();


		public string StoredProcedureWithResultAsync_Parameters_Custom_Method_ExecAsync => GetFixtureText();

		public string StoredProcedureWithResultAsync_Parameters_Empty_Method_ExecAsync => GetFixtureText();

		public string StoredProcedureWithResultAsync_Parameters_Mixed_Method_ExecAsync => GetFixtureText();

		public string StoredProcedureWithResultAsync_Parameters_Simple_Method_ExecAsync => GetFixtureText();


		public string StoredProcedureWithScalarResult_Parameters_Custom_Method_Exec => GetFixtureText();

		public string StoredProcedureWithScalarResult_Parameters_Empty_Method_Exec => GetFixtureText();

		public string StoredProcedureWithScalarResult_Parameters_Mixed_Method_Exec => GetFixtureText();

		public string StoredProcedureWithScalarResult_Parameters_Simple_Method_Exec => GetFixtureText();


		public string StoredProcedureWithScalarResultAsync_Parameters_Custom_Method_ExecAsync => GetFixtureText();

		public string StoredProcedureWithScalarResultAsync_Parameters_Empty_Method_ExecAsync => GetFixtureText();

		public string StoredProcedureWithScalarResultAsync_Parameters_Mixed_Method_ExecAsync => GetFixtureText();

		public string StoredProcedureWithScalarResultAsync_Parameters_Simple_Method_ExecAsync => GetFixtureText();


		private static string GetFixtureText([CallerMemberName] string name = null)
		{
			Stream stream = typeof(CommonFixtures).Assembly.GetManifestResourceStream($"SqlFirst.Codegen.Text.Tests.Fixtures.Abilities.StoredProcedure.{name}.txt");
			string queryText = new StreamReader(stream ?? throw new InvalidOperationException()).ReadToEnd();
			return queryText;
		}
	}
}
