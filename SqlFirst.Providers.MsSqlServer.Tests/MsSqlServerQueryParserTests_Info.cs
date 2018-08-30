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
		public void GetQueryBaseInfoTest_1()
		{
			string query = QuerySelect.SelectNotUniqueFieldsWithRightJoin;
			var queryParser = new MsSqlServerQueryParser();
			IQueryBaseInfo result = queryParser.GetQueryBaseInfo(query);

			IQuerySection[] sections = result.Sections.ToArray();

			sections.Length.ShouldBe(1);

			IQuerySection bodySection = sections[0];
			bodySection.Type.ShouldBe(QuerySectionType.Body);
			bodySection.Name.ShouldBeNull();

			result.Type.ShouldBe(QueryType.Read);
		}

		[Fact]
		public void GetQueryBaseInfoTest_2()
		{
			string query = QueryDelete.DeleteByGuid;
			var queryParser = new MsSqlServerQueryParser();
			IQueryBaseInfo result = queryParser.GetQueryBaseInfo(query);

			IQuerySection[] sections = result.Sections.ToArray();

			sections.Length.ShouldBe(2);

			IQuerySection variablesSection = sections[0];
			variablesSection.Type.ShouldBe(QuerySectionType.Declarations);
			variablesSection.Name.ShouldBe(QuerySectionName.Declarations);

			IQuerySection bodySection = sections[1];
			bodySection.Type.ShouldBe(QuerySectionType.Body);
			bodySection.Name.ShouldBeNull();

			result.Type.ShouldBe(QueryType.Delete);
		}

		[Fact]
		public void GetQueryBaseInfoTest_3()
		{
			string query = QueryInsert.InsertStringGuidDate;
			var queryParser = new MsSqlServerQueryParser();
			IQueryBaseInfo result = queryParser.GetQueryBaseInfo(query);

			IQuerySection[] sections = result.Sections.ToArray();

			sections.Length.ShouldBe(2);

			IQuerySection variablesSection = sections[0];
			variablesSection.Type.ShouldBe(QuerySectionType.Declarations);
			variablesSection.Name.ShouldBe(QuerySectionName.Declarations);

			IQuerySection bodySection = sections[1];
			bodySection.Type.ShouldBe(QuerySectionType.Body);
			bodySection.Name.ShouldBeNull();

			result.Type.ShouldBe(QueryType.Create);
		}

		[Fact]
		public void GetQueryBaseInfoTest_4()
		{
			string query = QueryUpdate.UpdateDateByGuid;
			var queryParser = new MsSqlServerQueryParser();
			IQueryBaseInfo result = queryParser.GetQueryBaseInfo(query);

			IQuerySection[] sections = result.Sections.ToArray();

			sections.Length.ShouldBe(2);

			IQuerySection variablesSection = sections[0];
			variablesSection.Type.ShouldBe(QuerySectionType.Declarations);
			variablesSection.Name.ShouldBe(QuerySectionName.Declarations);

			IQuerySection bodySection = sections[1];
			bodySection.Type.ShouldBe(QuerySectionType.Body);
			bodySection.Name.ShouldBeNull();

			result.Type.ShouldBe(QueryType.Update);
		}

		[Fact]
		public void GetQueryBaseInfoTest_5()
		{
			string query = QuerySelect.SelectGuidAndDateWithMiltipleSections;
			var queryParser = new MsSqlServerQueryParser();
			IQueryBaseInfo result = queryParser.GetQueryBaseInfo(query);

			IQuerySection[] sections = result.Sections.ToArray();

			sections.Length.ShouldBe(4);

			IQuerySection optionsSection = sections[0];
			optionsSection.Type.ShouldBe(QuerySectionType.Options);
			optionsSection.Name.ShouldBe(QuerySectionName.Options);

			IQuerySection variablesSection1 = sections[1];
			variablesSection1.Type.ShouldBe(QuerySectionType.Declarations);
			variablesSection1.Name.ShouldBe(QuerySectionName.Declarations);

			IQuerySection variablesSection2 = sections[2];
			variablesSection2.Type.ShouldBe(QuerySectionType.Declarations);
			variablesSection2.Name.ShouldBe(QuerySectionName.Declarations);

			IQuerySection bodySection = sections[3];
			bodySection.Type.ShouldBe(QuerySectionType.Body);
			bodySection.Name.ShouldBeNull();

			result.Type.ShouldBe(QueryType.Read);
		}

		[Fact]
		public void GetQueryBaseInfoTest_6()
		{
			string query = QuerySelect.SelectGuidAndDateWithUnknownSections;
			var queryParser = new MsSqlServerQueryParser();
			IQueryBaseInfo result = queryParser.GetQueryBaseInfo(query);

			IQuerySection[] sections = result.Sections.ToArray();

			sections.Length.ShouldBe(4);

			IQuerySection optionsSection = sections[0];
			optionsSection.Type.ShouldBe(QuerySectionType.Options);
			optionsSection.Name.ShouldBe(QuerySectionName.Options);
			optionsSection.Content.ShouldBe("-- enable Async");

			IQuerySection unknownSection = sections[1];
			unknownSection.Type.ShouldBe(QuerySectionType.Unknown);
			unknownSection.Name.ShouldBeNull();
			unknownSection.Content.ShouldBe(@"/*
 Некоторое количество
 произвольного 
		текста
*/

");

			IQuerySection variablesSection = sections[2];
			variablesSection.Type.ShouldBe(QuerySectionType.Declarations);
			variablesSection.Name.ShouldBe(QuerySectionName.Declarations);
			variablesSection.Content.ShouldBe(@"declare @userKey varchar(MAX) ='test'; 
declare @take int = 42;");

			IQuerySection bodySection = sections[3];
			bodySection.Type.ShouldBe(QuerySectionType.Body);
			bodySection.Name.ShouldBeNull();

			result.Type.ShouldBe(QueryType.Read);
		}

		[Fact]
		public void GetQueryBaseInfoTest_7()
		{
			string query = QuerySelect.SelectGuidAndDateWithUnknownAndCustomSections;
			var queryParser = new MsSqlServerQueryParser();
			IQueryBaseInfo result = queryParser.GetQueryBaseInfo(query);

			IQuerySection[] sections = result.Sections.ToArray();

			sections.Length.ShouldBe(6);

			IQuerySection unknownSection = sections[0];
			unknownSection.Type.ShouldBe(QuerySectionType.Unknown);
			unknownSection.Name.ShouldBeNull();
			unknownSection.Content.ShouldBe(@"/*
 Некоторое количество
 произвольного 
		текста
*/

");

			IQuerySection optionsSection = sections[1];
			optionsSection.Type.ShouldBe(QuerySectionType.Options);
			optionsSection.Name.ShouldBe(QuerySectionName.Options);
			optionsSection.Content.ShouldBe(@"-- disable all
-- enable Async");

			IQuerySection variablesSection1 = sections[2];
			variablesSection1.Type.ShouldBe(QuerySectionType.Declarations);
			variablesSection1.Name.ShouldBe(QuerySectionName.Declarations);
			variablesSection1.Content.ShouldBe(@"declare @userKey varchar(MAX) ='test';");

			IQuerySection customSection = sections[3];
			customSection.Type.ShouldBe(QuerySectionType.Custom);
			customSection.Name.ShouldBe("mySpecialSection");
			customSection.Content.ShouldBe("--simple test");

			IQuerySection variablesSection2 = sections[4];
			variablesSection2.Type.ShouldBe(QuerySectionType.Declarations);
			variablesSection2.Name.ShouldBe(QuerySectionName.Declarations);
			variablesSection2.Content.ShouldBe("declare @take int = 42;");

			IQuerySection bodySection = sections[5];
			bodySection.Type.ShouldBe(QuerySectionType.Body);
			bodySection.Name.ShouldBeNull();

			result.Type.ShouldBe(QueryType.Read);
		}

		[Fact]
		public void GetQueryBaseInfoTest_8()
		{
			string query = QuerySelect.SelectGuidAndDateWithUnknownUnnamedAndCustomSections;
			var queryParser = new MsSqlServerQueryParser();
			IQueryBaseInfo result = queryParser.GetQueryBaseInfo(query);

			IQuerySection[] sections = result.Sections.ToArray();

			sections.Length.ShouldBe(6);

			IQuerySection unknownSection = sections[0];
			unknownSection.Type.ShouldBe(QuerySectionType.Unknown);
			unknownSection.Name.ShouldBeNull();
			unknownSection.Content.ShouldBe(@"/*
 Некоторое количество
 произвольного 
		текста
*/

");

			IQuerySection optionsSection = sections[1];
			optionsSection.Type.ShouldBe(QuerySectionType.Unknown);
			optionsSection.Name.ShouldBe(string.Empty);
			optionsSection.Content.ShouldBe(@"-- disable all
-- enable Async");

			IQuerySection variablesSection1 = sections[2];
			variablesSection1.Type.ShouldBe(QuerySectionType.Declarations);
			variablesSection1.Name.ShouldBe(QuerySectionName.Declarations);
			variablesSection1.Content.ShouldBe(@"declare @userKey varchar(MAX) ='test';");

			IQuerySection customSection = sections[3];
			customSection.Type.ShouldBe(QuerySectionType.Custom);
			customSection.Name.ShouldBe("mySpecialSection");
			customSection.Content.ShouldBe("--simple test");

			IQuerySection variablesSection2 = sections[4];
			variablesSection2.Type.ShouldBe(QuerySectionType.Declarations);
			variablesSection2.Name.ShouldBe(QuerySectionName.Declarations);
			variablesSection2.Content.ShouldBe("declare @take int = 42;");

			IQuerySection bodySection = sections[5];
			bodySection.Type.ShouldBe(QuerySectionType.Body);
			bodySection.Name.ShouldBeNull();

			result.Type.ShouldBe(QueryType.Read);
		}

		[Fact]
		public void GetQueryInfoTest_1()
		{
			string query = QuerySelect.SelectGuidAndDateWithPagingAndPartOfParameters;
			var queryParser = new MsSqlServerQueryParser();
			IQueryInfo result = queryParser.GetQueryInfo(query, ConnectionString);

			result.Type.ShouldBe(QueryType.Read);

			result.Parameters.ShouldNotBeNull();

			IQueryParamInfo[] parameters = result.Parameters.ToArray();

			parameters.Length.ShouldBe(3);

			IQueryParamInfo userKey = parameters[0];
			userKey.ShouldNotBeNull();
			userKey.DbName.ShouldBe("userKey");
			userKey.DbType.ShouldBe(MsSqlDbType.VarChar);
			userKey.Length.ShouldBe("MAX");
			userKey.DefaultValue.ShouldBe("test");

			IQueryParamInfo skip = parameters[1];
			skip.ShouldNotBeNull();
			skip.DbName.ShouldBe("skip");
			skip.DbType.ShouldBe(MsSqlDbType.Int);
			skip.Length.ShouldBeNull();
			skip.DefaultValue.ShouldBe(42);

			IQueryParamInfo take = parameters[2]; // undeclared
			take.ShouldNotBeNull();
			take.DbName.ShouldBe("take");
			take.DbType.ShouldBe(MsSqlDbType.Bigint);
			skip.Length.ShouldBeNull();
			take.DefaultValue.ShouldBeNull();

			result.Results.ShouldNotBeNull();
			IFieldDetails[] queryResults = result.Results.ToArray();

			queryResults.Length.ShouldBe(2);

			IFieldDetails caseId = queryResults[0];
			caseId.ColumnName.ShouldBe("CaseId");
			caseId.AllowDbNull.ShouldBeFalse();
			caseId.DbType.ShouldBe(MsSqlDbType.UniqueIdentifier);
			caseId.ColumnOrdinal.ShouldBe(0);

			IFieldDetails createDateUtc = queryResults[1];
			createDateUtc.ColumnName.ShouldBe("CreateDateUtc");
			createDateUtc.AllowDbNull.ShouldBeFalse();
			createDateUtc.DbType.ShouldBe(MsSqlDbType.DateTime);
			createDateUtc.ColumnOrdinal.ShouldBe(1);
		}

		[Fact]
		public void GetQueryInfoTest_2()
		{
			string query = QuerySelect.SelectGuidAndDateWithUnknownSections;
			var queryParser = new MsSqlServerQueryParser();
			IQueryInfo result = queryParser.GetQueryInfo(query, ConnectionString);

			result.Type.ShouldBe(QueryType.Read);

			result.Parameters.ShouldNotBeNull();

			IQueryParamInfo[] parameters = result.Parameters.ToArray();

			parameters.Length.ShouldBe(3);

			IQueryParamInfo userKey = parameters[0];
			userKey.ShouldNotBeNull();
			userKey.DbName.ShouldBe("userKey");
			userKey.DbType.ShouldBe(MsSqlDbType.VarChar);
			userKey.Length.ShouldBe("MAX");
			userKey.DefaultValue.ShouldBe("test");

			IQueryParamInfo take = parameters[1];
			take.ShouldNotBeNull();
			take.DbName.ShouldBe("take");
			take.DbType.ShouldBe(MsSqlDbType.Int);
			take.Length.ShouldBeNull();
			take.DefaultValue.ShouldBe(42);

			IQueryParamInfo skip = parameters[2]; // undeclared
			skip.ShouldNotBeNull();
			skip.DbName.ShouldBe("skip");
			skip.DbType.ShouldBe(MsSqlDbType.Bigint);
			skip.Length.ShouldBeNull();
			skip.DefaultValue.ShouldBeNull();

			result.Results.ShouldNotBeNull();
			IFieldDetails[] queryResults = result.Results.ToArray();

			queryResults.Length.ShouldBe(2);

			IFieldDetails caseId = queryResults[0];
			caseId.ColumnName.ShouldBe("CaseId");
			caseId.AllowDbNull.ShouldBeFalse();
			caseId.DbType.ShouldBe(MsSqlDbType.UniqueIdentifier);
			caseId.ColumnOrdinal.ShouldBe(0);

			IFieldDetails createDateUtc = queryResults[1];
			createDateUtc.ColumnName.ShouldBe("CreateDateUtc");
			createDateUtc.AllowDbNull.ShouldBeFalse();
			createDateUtc.DbType.ShouldBe(MsSqlDbType.DateTime);
			createDateUtc.ColumnOrdinal.ShouldBe(1);
		}

		[Fact]
		public void GetQueryInfoTest_3()
		{
			string query = QuerySelect.SelectGuidAndDateWithUnknownAndCustomSections;
			var queryParser = new MsSqlServerQueryParser();
			IQueryInfo result = queryParser.GetQueryInfo(query, ConnectionString);

			result.Type.ShouldBe(QueryType.Read);

			result.Parameters.ShouldNotBeNull();

			IQueryParamInfo[] parameters = result.Parameters.ToArray();

			parameters.Length.ShouldBe(3);

			IQueryParamInfo userKey = parameters[0];
			userKey.ShouldNotBeNull();
			userKey.DbName.ShouldBe("userKey");
			userKey.DbType.ShouldBe(MsSqlDbType.VarChar);
			userKey.Length.ShouldBe("MAX");
			userKey.DefaultValue.ShouldBe("test");

			IQueryParamInfo take = parameters[1];
			take.ShouldNotBeNull();
			take.DbName.ShouldBe("take");
			take.DbType.ShouldBe(MsSqlDbType.Int);
			take.Length.ShouldBeNull();
			take.DefaultValue.ShouldBe(42);

			IQueryParamInfo skip = parameters[2]; // undeclared
			skip.ShouldNotBeNull();
			skip.DbName.ShouldBe("skip");
			skip.DbType.ShouldBe(MsSqlDbType.Bigint);
			skip.Length.ShouldBeNull();
			skip.DefaultValue.ShouldBeNull();

			result.Results.ShouldNotBeNull();
			IFieldDetails[] queryResults = result.Results.ToArray();

			queryResults.Length.ShouldBe(2);

			IFieldDetails caseId = queryResults[0];
			caseId.ColumnName.ShouldBe("CaseId");
			caseId.AllowDbNull.ShouldBeFalse();
			caseId.DbType.ShouldBe(MsSqlDbType.UniqueIdentifier);
			caseId.ColumnOrdinal.ShouldBe(0);

			IFieldDetails createDateUtc = queryResults[1];
			createDateUtc.ColumnName.ShouldBe("CreateDateUtc");
			createDateUtc.AllowDbNull.ShouldBeFalse();
			createDateUtc.DbType.ShouldBe(MsSqlDbType.DateTime);
			createDateUtc.ColumnOrdinal.ShouldBe(1);
		}
	}
}