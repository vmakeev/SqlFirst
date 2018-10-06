using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Shouldly;
using SqlFirst.Core;
using SqlFirst.Providers.Postgres.Tests.Queries;
using Xunit;

namespace SqlFirst.Providers.Postgres.Tests
{
	[SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
	public partial class PostgresQueryParserTests
	{
		[Fact]
		public void SingleOptionTest()
		{
			string query = QueryInsert.InsertStringGuidDateWithOption;
			var parser = new PostgresQueryParser();

			IEnumerable<ISqlFirstOption> options = parser.GetOptions(query);
			options.ShouldNotBeNull();

			ISqlFirstOption[] optionsArray = options.ToArray();
			optionsArray.Length.ShouldBe(1);

			ISqlFirstOption option = optionsArray.Single();
			option.ShouldNotBeNull();
			option.Name.ShouldBe("test");
			option.Parameters.ShouldNotBeNull();
			option.Parameters.Length.ShouldBe(2);
			option.Parameters[0].ShouldBe("my");
			option.Parameters[1].ShouldBe("option");
		}

		[Fact]
		public void SeveralOptionsTest()
		{
			string query = QueryInsert.InsertStringGuidDateWithOptions;
			var parser = new PostgresQueryParser();

			IEnumerable<ISqlFirstOption> options = parser.GetOptions(query);
			options.ShouldNotBeNull();

			ISqlFirstOption[] optionsArray = options.ToArray();
			optionsArray.Length.ShouldBe(2);

			ISqlFirstOption option1 = optionsArray[0];
			option1.ShouldNotBeNull();
			option1.Name.ShouldBe("test");
			option1.Parameters.ShouldNotBeNull();
			option1.Parameters.Length.ShouldBe(2);
			option1.Parameters[0].ShouldBe("my");
			option1.Parameters[1].ShouldBe("option");

			ISqlFirstOption option2 = optionsArray[1];
			option2.ShouldNotBeNull();
			option2.Name.ShouldBe("generate");
			option2.Parameters.ShouldNotBeNull();
			option2.Parameters.Length.ShouldBe(3);
			option2.Parameters[0].ShouldBe("item");
			option2.Parameters[1].ShouldBe("struct");
			option2.Parameters[2].ShouldBe("inpc");
		}
	}
}