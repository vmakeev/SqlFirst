using System;
using System.Collections.Generic;
using SqlFirst.Core;
using SqlFirst.Core.Impl;
using Xunit;
using Xunit.Should;

namespace SqlFirst.Providers.MsSqlServer.Tests
{
	public class MsSqlServerCodeEmitterTests
	{
		[Fact]
		public void EmitQueryTest_1()
		{
			var section1 = new QuerySection(QuerySectionType.Declarations, "test declarations");
			var section2 = new QuerySection(QuerySectionType.Body, "test body");

			var sections = new List<IQuerySection> { section1, section2 };

			var emitter = new MsSqlServerCodeEmitter();

			string query = emitter.EmitQuery(sections);

			query.ShouldBe(
@"-- begin variables

test declarations

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
				new QuerySection(QuerySectionType.Declarations, "test declarations_1"),
				new QuerySection(QuerySectionType.Custom, "myCustom1", "myCustom1_1"),
				new QuerySection(QuerySectionType.Options, "test options_1"),
				new QuerySection(QuerySectionType.Custom, "myCustom2", "myCustom2_1"),
				new QuerySection(QuerySectionType.Unknown, "some useless text 2"),
				new QuerySection(QuerySectionType.Custom, "myCustom2", "myCustom2_2"),
				new QuerySection(QuerySectionType.Declarations, "test declarations_2"),
				new QuerySection(QuerySectionType.Body, "test body_2"),
				new QuerySection(QuerySectionType.Custom, "myCusTom1", "myCustom1_2"),
				new QuerySection(QuerySectionType.Options, "test options_2"),

			};

			var emitter = new MsSqlServerCodeEmitter();

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

-- begin variables

test declarations_1
test declarations_2

-- end

test body_1
test body_2");
		}

		[Fact]
		public void EmitQueryTest_3()
		{
			var sections = new List<IQuerySection>
			{
				new QuerySection(QuerySectionType.Body, "test body"),
				new QuerySection(QuerySectionType.Declarations, "test declarations_1"),
				new QuerySection(QuerySectionType.Options, "test options_1"),
				new QuerySection(QuerySectionType.Unknown, "some useless text"),
				new QuerySection(QuerySectionType.Declarations, "test declarations_2"),
				new QuerySection(QuerySectionType.Options, "test options_2"),

			};

			var emitter = new MsSqlServerCodeEmitter();

			string query = emitter.EmitQuery(sections);

			query.ShouldBe(
@"-- begin sqlFirstOptions

test options_1
test options_2

-- end

some useless text

-- begin variables

test declarations_1
test declarations_2

-- end

test body");
		}

		[Fact]
		public void EmitQueryTest_4()
		{
			var sections = new List<IQuerySection>
			{
				new QuerySection(QuerySectionType.Body, "test body"),
				new QuerySection(QuerySectionType.Declarations, "test declarations_1"),
				new QuerySection(QuerySectionType.Options, "test options_1"),
				new QuerySection(QuerySectionType.Unknown, string.Empty),
				new QuerySection(QuerySectionType.Declarations, "test declarations_2"),
				new QuerySection(QuerySectionType.Options, "test options_2"),

			};

			var emitter = new MsSqlServerCodeEmitter();

			string query = emitter.EmitQuery(sections);

			query.ShouldBe(
@"-- begin sqlFirstOptions

test options_1
test options_2

-- end

-- begin variables

test declarations_1
test declarations_2

-- end

test body");
		}

		[Fact]
		public void EmitQueryTest_5()
		{
			var sections = new List<IQuerySection>
			{
				new QuerySection(QuerySectionType.Body, "test body"),
				new QuerySection(QuerySectionType.Declarations, "test declarations_1"),
				new QuerySection(QuerySectionType.Options, string.Empty),
				new QuerySection(QuerySectionType.Unknown, string.Empty),
				new QuerySection(QuerySectionType.Declarations, "test declarations_2"),

			};

			var emitter = new MsSqlServerCodeEmitter();

			string query = emitter.EmitQuery(sections);

			query.ShouldBe(
@"-- begin variables

test declarations_1
test declarations_2

-- end

test body");
		}

		[Fact]
		public void EmitQueryTest_6()
		{
			var sections = new List<IQuerySection>
			{
				new QuerySection((QuerySectionType)42, "eat me"),
				new QuerySection(QuerySectionType.Body, "test body"),
				new QuerySection(QuerySectionType.Declarations, "test declarations_1"),
				new QuerySection(QuerySectionType.Options, string.Empty),
				new QuerySection(QuerySectionType.Unknown, string.Empty),
				new QuerySection(QuerySectionType.Declarations, "test declarations_2"),
			};

			var emitter = new MsSqlServerCodeEmitter();

			Assert.Throws<QueryEmitException>(() => emitter.EmitQuery(sections));
		}

		[Fact]
		public void EmitQueryTest_7()
		{
			var sections = new List<IQuerySection>
			{
				new QuerySection(QuerySectionType.Declarations, "test declarations_1"),
				new QuerySection(QuerySectionType.Options, string.Empty),
				new QuerySection(QuerySectionType.Unknown, string.Empty),
				new QuerySection(QuerySectionType.Declarations, "test declarations_2"),

			};

			var emitter = new MsSqlServerCodeEmitter();

			string query = emitter.EmitQuery(sections);

			query.ShouldBe(
				@"-- begin variables

test declarations_1
test declarations_2

-- end");
		}

		[Theory]
		[InlineData(5, null, "5")]
		[InlineData(null, null, null)]
		[InlineData(5.4f, null, "5.4")]
		[InlineData("test", null, "'test'")]
		[InlineData("test", MsSqlDbType.VarChar, "'test'")]
		[InlineData("test", MsSqlDbType.Char, "'test'")]
		[InlineData("test", MsSqlDbType.Text, "'test'")]
		[InlineData("test", MsSqlDbType.NVarChar, "N'test'")]
		[InlineData("test", MsSqlDbType.NChar, "N'test'")]
		[InlineData("test", MsSqlDbType.NText, "N'test'")]
		[InlineData(null, MsSqlDbType.NText, null)]
		[InlineData(null, MsSqlDbType.Char, null)]
		public void EmitValueSuccessTest(object value, string dbType, string expected)
		{
			var emitter = new MsSqlServerCodeEmitter();
			string emittedValue = emitter.EmitValue(value, dbType);
			emittedValue.ShouldBe(expected);
		}

		[Theory]
		[InlineData(1L)]
		[InlineData(15.3d)]
		[InlineData('c')]
		public void EmitValueThrowsTest(object value)
		{
			var emitter = new MsSqlServerCodeEmitter();
			Assert.Throws<QueryEmitException>(() => emitter.EmitValue(value));
		}

		[Fact]
		public void EmitDeclarationTest_1()
		{
			IQueryParamInfo info = new QueryParamInfo
			{
				DbName = "test",
				DbType = MsSqlDbType.VarChar,
				DefaultValue = "123",
				Length = "max"
			};

			var emitter = new MsSqlServerCodeEmitter();
			string declaration = emitter.EmitDeclaration(info);
			declaration.ShouldBe("DECLARE @test varchar(MAX) = '123';");
		}

		[Fact]
		public void EmitDeclarationTest_2()
		{
			IQueryParamInfo info = new QueryParamInfo
			{
				DbName = "test",
				DbType = MsSqlDbType.NVarChar,
				DefaultValue = "123",
				Length = "255"
			};

			var emitter = new MsSqlServerCodeEmitter();
			string declaration = emitter.EmitDeclaration(info);
			declaration.ShouldBe("DECLARE @test nvarchar(255) = N'123';");
		}

		[Fact]
		public void EmitDeclarationTest_3()
		{
			IQueryParamInfo info = new QueryParamInfo
			{
				DbName = "test",
				DbType = MsSqlDbType.Int,
				DefaultValue = 123,
				Length = null
			};

			var emitter = new MsSqlServerCodeEmitter();
			string declaration = emitter.EmitDeclaration(info);
			declaration.ShouldBe("DECLARE @test int = 123;");
		}

		[Fact]
		public void EmitDeclarationTest_4()
		{
			IQueryParamInfo info = new QueryParamInfo
			{
				DbName = "test",
				DbType = MsSqlDbType.DateTime2,
				DefaultValue = null,
				Length = null
			};

			var emitter = new MsSqlServerCodeEmitter();
			string declaration = emitter.EmitDeclaration(info);
			declaration.ShouldBe("DECLARE @test datetime2;");
		}

		[Fact]
		public void EmitDeclarationTest_5()
		{
			IQueryParamInfo info = new QueryParamInfo
			{
				DbName = "test",
				DbType = MsSqlDbType.NVarChar,
				DefaultValue = "123",
				Length = null
			};

			var emitter = new MsSqlServerCodeEmitter();
			string declaration = emitter.EmitDeclaration(info);
			declaration.ShouldBe("DECLARE @test nvarchar = N'123';");
		}

		[Fact]
		public void EmitDeclarationTest_6()
		{
			IQueryParamInfo info = new QueryParamInfo
			{
				DbName = "test",
				DbType = MsSqlDbType.VarChar,
				DefaultValue = "123",
				Length = null
			};

			var emitter = new MsSqlServerCodeEmitter();
			string declaration = emitter.EmitDeclaration(info);
			declaration.ShouldBe("DECLARE @test varchar = '123';");
		}

		[Fact]
		public void EmitDeclarationTest_7()
		{
			IQueryParamInfo info = new QueryParamInfo
			{
				DbName = "test",
				DbType = MsSqlDbType.NText,
				DefaultValue = null,
				Length = null
			};

			var emitter = new MsSqlServerCodeEmitter();
			string declaration = emitter.EmitDeclaration(info);
			declaration.ShouldBe("DECLARE @test ntext;");
		}

		[Fact]
		public void EmitDeclarationTest_8()
		{
			IQueryParamInfo info = new QueryParamInfo
			{
				DbName = "test",
				DbType = MsSqlDbType.Text,
				DefaultValue = null,
				Length = "3"
			};

			var emitter = new MsSqlServerCodeEmitter();
			string declaration = emitter.EmitDeclaration(info);
			declaration.ShouldBe("DECLARE @test text(3);");
		}

		[Theory]
		[InlineData("		", MsSqlDbType.Bigint)]
		[InlineData(" ", MsSqlDbType.Bigint)]
		[InlineData("", MsSqlDbType.Bigint)]
		[InlineData(null, MsSqlDbType.Bigint)]

		[InlineData("		", "")]
		[InlineData(" ", "")]
		[InlineData("", "")]
		[InlineData(null, "")]
		[InlineData("test", "")]

		[InlineData("		", " ")]
		[InlineData(" ", " ")]
		[InlineData("", " ")]
		[InlineData(null, " ")]
		[InlineData("test", " ")]

		[InlineData("		", null)]
		[InlineData(" ", null)]
		[InlineData("", null)]
		[InlineData(null, null)]
		[InlineData("test", null)]
		public void EmitDeclarationTest_9(string dbName, string dbType)
		{
			IQueryParamInfo info = new QueryParamInfo
			{
				DbName = dbName,
				DbType = dbType,
			};

			var emitter = new MsSqlServerCodeEmitter();
			Assert.Throws<ArgumentException>(() => emitter.EmitDeclaration(info));
		}

		[Fact]
		public void EmitDeclarationTest_10()
		{
			var emitter = new MsSqlServerCodeEmitter();
			Assert.Throws<ArgumentNullException>(() => emitter.EmitDeclaration(null));
		}

		[Fact]
		public void EmitDeclarationsTest_1()
		{
			IQueryParamInfo info1 = new QueryParamInfo
			{
				DbName = "test1",
				DbType = MsSqlDbType.NVarChar,
				DefaultValue = "marco",
				Length = "Max"
			};

			IQueryParamInfo info2 = new QueryParamInfo
			{
				DbName = "test2",
				DbType = MsSqlDbType.Int,
				DefaultValue = 42,
				Length = null
			};

			var infos = new[] { info1, info2 };

			var emitter = new MsSqlServerCodeEmitter();
			string declaration = emitter.EmitDeclarations(infos);

			declaration.ShouldBe("DECLARE @test1 nvarchar(MAX) = N'marco';\r\nDECLARE @test2 int = 42;");
		}

		[Fact]
		public void EmitDeclarationsTest_2()
		{
			var emitter = new MsSqlServerCodeEmitter();
			Assert.Throws<ArgumentNullException>(() => emitter.EmitDeclarations(null));
		}
	}
}