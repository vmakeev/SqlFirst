using System.Collections.Generic;
using SqlFirst.Core;
using SqlFirst.Core.Impl;
using Xunit;
using Xunit.Should;

namespace SqlFirst.Providers.Postgres.Tests
{
	public class PostgresCodeEmitterTests
	{
		[Fact]
		public void EmitQueryTest_1()
		{
			var section1 = new QuerySection(QuerySectionType.Options, "test options");
			var section2 = new QuerySection(QuerySectionType.Body, "test body");

			var sections = new List<IQuerySection> { section1, section2 };

			var emitter = new PostgresCodeEmitter();

			string query = emitter.EmitQuery(sections);

			query.ShouldBe(
				@"-- begin sqlFirstOptions

test options

-- end

test body");
		}

		[Fact]
		public void EmitQueryTest_2()
		{
			var sections = new List<IQuerySection>
			{
				new QuerySection(QuerySectionType.Unknown, "some useless text 1"),
				new QuerySection(QuerySectionType.Body, "test body_1"),
				new QuerySection(QuerySectionType.Custom, "myCustom1", "myCustom1_1"),
				new QuerySection(QuerySectionType.Options, "test options_1"),
				new QuerySection(QuerySectionType.Custom, "myCustom2", "myCustom2_1"),
				new QuerySection(QuerySectionType.Unknown, "some useless text 2"),
				new QuerySection(QuerySectionType.Custom, "myCustom2", "myCustom2_2"),
				new QuerySection(QuerySectionType.Body, "test body_2"),
				new QuerySection(QuerySectionType.Custom, "myCusTom1", "myCustom1_2"),
				new QuerySection(QuerySectionType.Options, "test options_2"),
			};

			var emitter = new PostgresCodeEmitter();

			string query = emitter.EmitQuery(sections);

			// fuck my eyes...
			query.ShouldBe(
				@"some useless text 1

-- begin sqlFirstOptions

test options_1
test options_2

-- end

-- begin myCustom1

myCustom1_1
myCustom1_2

-- end

-- begin myCustom2

myCustom2_1
myCustom2_2

-- end

some useless text 2

test body_1
test body_2");
		}

		[Fact]
		public void EmitQueryTest_3()
		{
			var sections = new List<IQuerySection>
			{
				new QuerySection(QuerySectionType.Body, "test body"),
				new QuerySection(QuerySectionType.Options, "test options_1"),
				new QuerySection(QuerySectionType.Unknown, "some useless text"),
				new QuerySection(QuerySectionType.Options, "test options_2"),
			};

			var emitter = new PostgresCodeEmitter();

			string query = emitter.EmitQuery(sections);

			query.ShouldBe(
				@"-- begin sqlFirstOptions

test options_1
test options_2

-- end

some useless text

test body");
		}

		[Fact]
		public void EmitQueryTest_4()
		{
			var sections = new List<IQuerySection>
			{
				new QuerySection(QuerySectionType.Body, "test body"),
				new QuerySection(QuerySectionType.Options, "test options_1"),
				new QuerySection(QuerySectionType.Unknown, string.Empty),
				new QuerySection(QuerySectionType.Options, "test options_2"),
			};

			var emitter = new PostgresCodeEmitter();

			string query = emitter.EmitQuery(sections);

			query.ShouldBe(
				@"-- begin sqlFirstOptions

test options_1
test options_2

-- end

test body");
		}

		[Fact]
		public void EmitQueryTest_5()
		{
			var sections = new List<IQuerySection>
			{
				new QuerySection(QuerySectionType.Body, "test body"),
				new QuerySection(QuerySectionType.Options, string.Empty),
				new QuerySection(QuerySectionType.Unknown, string.Empty),
			};

			var emitter = new PostgresCodeEmitter();

			string query = emitter.EmitQuery(sections);

			query.ShouldBe(
				@"test body");
		}

		[Fact]
		public void EmitQueryTest_6()
		{
			var sections = new List<IQuerySection>
			{
				new QuerySection((QuerySectionType)42, "eat me"),
				new QuerySection(QuerySectionType.Body, "test body"),
				new QuerySection(QuerySectionType.Options, string.Empty),
				new QuerySection(QuerySectionType.Unknown, string.Empty),
				new QuerySection(QuerySectionType.Declarations, "test declarations_2"),
			};

			var emitter = new PostgresCodeEmitter();

			Assert.Throws<QueryEmitException>(() => emitter.EmitQuery(sections));
		}

		[Fact]
		public void EmitQueryTest_7()
		{
			var sections = new List<IQuerySection>
			{
				new QuerySection(QuerySectionType.Options, "some options"),
				new QuerySection(QuerySectionType.Declarations, "test declarations_1"),
			};

			var emitter = new PostgresCodeEmitter();

			// declarations not upported
			Assert.Throws<QueryEmitException>(() => emitter.EmitQuery(sections));
		}
	}
}