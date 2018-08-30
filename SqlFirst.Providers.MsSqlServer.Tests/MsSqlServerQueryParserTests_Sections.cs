using System;
using System.Linq;
using Shouldly;
using SqlFirst.Core;
using SqlFirst.Core.Impl;
using SqlFirst.Providers.MsSqlServer.Tests.Queries;
using Xunit;

namespace SqlFirst.Providers.MsSqlServer.Tests
{
	public partial class MsSqlServerQueryParserTests
	{
		[Fact]
		public void GetQuerySectionsTest_1()
		{
			string query = QuerySelect.SelectNotUniqueFieldsWithRightJoin;
			var queryParser = new MsSqlServerQueryParser();

			IQuerySection[] sections = queryParser.GetQuerySections(query).ToArray();

			sections.Length.ShouldBe(1);

			IQuerySection bodySection = sections[0];
			bodySection.Type.ShouldBe(QuerySectionType.Body);
			bodySection.Name.ShouldBeNull();
		}

		[Fact]
		public void GetQuerySectionsTest_2()
		{
			string query = QueryDelete.DeleteByGuid;
			var queryParser = new MsSqlServerQueryParser();

			IQuerySection[] sections = queryParser.GetQuerySections(query).ToArray();

			sections.Length.ShouldBe(2);

			IQuerySection variablesSection = sections[0];
			variablesSection.Type.ShouldBe(QuerySectionType.Declarations);
			variablesSection.Name.ShouldBe(QuerySectionName.Declarations);

			IQuerySection bodySection = sections[1];
			bodySection.Type.ShouldBe(QuerySectionType.Body);
			bodySection.Name.ShouldBeNull();
		}

		[Fact]
		public void GetQuerySectionsTest_3()
		{
			string query = QuerySelect.SelectNotUniqueFieldsWithRightJoin;
			var queryParser = new MsSqlServerQueryParser();

			IQuerySection[] sections = queryParser.GetQuerySections(query, null).ToArray();

			sections.Length.ShouldBe(1);

			IQuerySection bodySection = sections[0];
			bodySection.Type.ShouldBe(QuerySectionType.Body);
			bodySection.Name.ShouldBeNull();
		}

		[Fact]
		public void GetQuerySectionsTest_4()
		{
			string query = QueryDelete.DeleteByGuid;
			var queryParser = new MsSqlServerQueryParser();

			IQuerySection[] sections = queryParser.GetQuerySections(query, QuerySectionName.Declarations).ToArray();

			sections.Length.ShouldBe(1);

			IQuerySection variablesSection = sections[0];
			variablesSection.Type.ShouldBe(QuerySectionType.Declarations);
			variablesSection.Name.ShouldBe(QuerySectionName.Declarations);
		}

		[Fact]
		public void GetQuerySectionsTest_5()
		{
			string query = QuerySelect.SelectNotUniqueFieldsWithRightJoin;
			var queryParser = new MsSqlServerQueryParser();

			IQuerySection[] sections = queryParser.GetQuerySections(query, QuerySectionType.Body).ToArray();

			sections.Length.ShouldBe(1);

			IQuerySection bodySection = sections[0];
			bodySection.Type.ShouldBe(QuerySectionType.Body);
			bodySection.Name.ShouldBeNull();
		}

		[Fact]
		public void GetQuerySectionsTest_6()
		{
			string query = QueryDelete.DeleteByGuid;
			var queryParser = new MsSqlServerQueryParser();

			IQuerySection[] sections = queryParser.GetQuerySections(query, QuerySectionType.Declarations).ToArray();

			sections.Length.ShouldBe(1);

			IQuerySection bodySection = sections[0];
			bodySection.Type.ShouldBe(QuerySectionType.Declarations);
			bodySection.Name.ShouldBe(QuerySectionName.Declarations);
		}

		[Fact]
		public void GetQuerySectionsTest_7()
		{
			var queryParser = new MsSqlServerQueryParser();
			Assert.Throws<ArgumentNullException>(() => queryParser.GetQuerySections(null).ToArray());
			Assert.Throws<ArgumentNullException>(() => queryParser.GetQuerySections(null, QuerySectionType.Body).ToArray());
			Assert.Throws<ArgumentNullException>(() => queryParser.GetQuerySections(null, QuerySectionName.Options).ToArray());
			Assert.Throws<ArgumentNullException>(() => queryParser.GetQuerySections(null, null).ToArray());
		}
	}
}