using System.Linq;
using SqlFirst.Core.Parsing;
using SqlFirst.Providers.MsSqlServer.Tests.Queries;
using Xunit;
using Xunit.Should;

namespace SqlFirst.Providers.MsSqlServer.Tests
{
	public class MsSqlServerQueryParserTests
	{
		private const string ConnectionString = @"Server=api-dev;Database=CasebookApi.Arbitrage.Tracking_dev;Integrated Security=SSPI;";

		[Fact]
		public void GetDeclaredParametersTest_1()
		{
			string query = QuerySelect.SelectGuidAndDateWithPagingAssignmentAndComments;
			var queryParser = new MsSqlServerQueryParser();
			IQueryParamInfo[] declaredParameters = queryParser.GetDeclaredParameters(query).ToArray();

			declaredParameters.ShouldNotBeNull();
			declaredParameters.Length.ShouldBe(3);

			IQueryParamInfo userKey = declaredParameters[0];
			userKey.ShouldNotBeNull();
			userKey.DbName.ShouldBe("userKey");
			userKey.DbType.ShouldBe(MsSqlDbType.VarChar);
			userKey.Length.ShouldBe("255");
			userKey.DefaultValue.ShouldBe("test");

			IQueryParamInfo skip = declaredParameters[1];
			skip.ShouldNotBeNull();
			skip.DbName.ShouldBe("skip");
			skip.DbType.ShouldBe(MsSqlDbType.Int);
			skip.Length.ShouldBeNull();
			skip.DefaultValue.ShouldBe(42);

			IQueryParamInfo take = declaredParameters[2];
			take.ShouldNotBeNull();
			take.DbName.ShouldBe("take");
			take.DbType.ShouldBe(MsSqlDbType.Int);
			skip.Length.ShouldBeNull();
			take.DefaultValue.ShouldBeNull();
		}

		[Fact]
		public void GetDeclaredParametersTest_2()
		{
			string query = QuerySelect.SelectDateWithNamedOrdinal;
			var queryParser = new MsSqlServerQueryParser();
			IQueryParamInfo[] declaredParameters = queryParser.GetDeclaredParameters(query).ToArray();

			declaredParameters.ShouldNotBeNull();
			declaredParameters.ShouldBeEmpty();
		}

		[Fact]
		public void GetDeclaredParametersTest_3()
		{
			string query = QuerySelect.SelectGuidAndDateWithPagingAndPartOfParameters;
			var queryParser = new MsSqlServerQueryParser();
			IQueryParamInfo[] declaredParameters = queryParser.GetDeclaredParameters(query).ToArray();

			declaredParameters.ShouldNotBeNull();
			declaredParameters.Length.ShouldBe(2);

			IQueryParamInfo userKey = declaredParameters[0];
			userKey.ShouldNotBeNull();
			userKey.DbName.ShouldBe("userKey");
			userKey.DbType.ShouldBe(MsSqlDbType.VarChar);
			userKey.Length.ShouldBe("MAX");
			userKey.DefaultValue.ShouldBe("test");

			IQueryParamInfo skip = declaredParameters[1];
			skip.ShouldNotBeNull();
			skip.DbName.ShouldBe("skip");
			skip.DbType.ShouldBe(MsSqlDbType.Int);
			skip.Length.ShouldBeNull();
			skip.DefaultValue.ShouldBe(42);
		}

		[Fact]
		public void GetUndeclaredParametersTest_1()
		{
			string query = QuerySelect.SelectGuidAndDateWithPaging;
			var queryParser = new MsSqlServerQueryParser();
			IQueryParamInfo[] undeclaredParameters = queryParser.GetUndeclaredParameters(query, ConnectionString).ToArray();

			undeclaredParameters.ShouldNotBeNull();
			undeclaredParameters.ShouldBeEmpty();
		}

		[Fact]
		public void GetUndeclaredParametersTest_2()
		{
			string query = QuerySelect.SelectGuidAndDateWithPagingNoParameters;
			var queryParser = new MsSqlServerQueryParser();
			IQueryParamInfo[] undeclaredParameters = queryParser.GetUndeclaredParameters(query, ConnectionString).ToArray();

			undeclaredParameters.ShouldNotBeNull();
			undeclaredParameters.Length.ShouldBe(3);

			IQueryParamInfo userKey = undeclaredParameters[0];
			userKey.ShouldNotBeNull();
			userKey.DbName.ShouldBe("userKey");
			userKey.DbType.ShouldBe(MsSqlDbType.NVarChar);
			userKey.Length.ShouldBe("255");
			userKey.DefaultValue.ShouldBeNull();

			IQueryParamInfo skip = undeclaredParameters[1];
			skip.ShouldNotBeNull();
			skip.DbName.ShouldBe("skip");
			skip.DbType.ShouldBe(MsSqlDbType.Bigint);
			skip.Length.ShouldBeNull();
			skip.DefaultValue.ShouldBeNull();

			IQueryParamInfo take = undeclaredParameters[2];
			take.ShouldNotBeNull();
			take.DbName.ShouldBe("take");
			take.DbType.ShouldBe(MsSqlDbType.Bigint);
			take.Length.ShouldBeNull();
			take.DefaultValue.ShouldBeNull();
		}

		[Fact]
		public void GetUndeclaredParametersTest_3()
		{
			string query = QuerySelect.SelectGuidAndDateWithPagingAndPartOfParameters;
			var queryParser = new MsSqlServerQueryParser();
			IQueryParamInfo[] undeclaredParameters = queryParser.GetUndeclaredParameters(query, ConnectionString).ToArray();

			undeclaredParameters.ShouldNotBeNull();
			undeclaredParameters.Length.ShouldBe(1);

			IQueryParamInfo take = undeclaredParameters[0];
			take.ShouldNotBeNull();
			take.DbName.ShouldBe("take");
			take.DbType.ShouldBe(MsSqlDbType.Bigint);
			take.Length.ShouldBeNull();
			take.DefaultValue.ShouldBeNull();
		}

		[Fact]
		public void GetResultsTest_1()
		{
			string query = QuerySelect.SelectGuidAndDateWithPaging;
			var queryParser = new MsSqlServerQueryParser();
			IFieldDetails[] results = queryParser.GetResultDetails(query, ConnectionString).ToArray();

			results.ShouldNotBeEmpty();
			results.Length.ShouldBe(2);

			IFieldDetails caseId = results[0];
			caseId.ColumnName.ShouldBe("CaseId");
			caseId.AllowDbNull.ShouldBeFalse();
			caseId.DbType.ShouldBe(MsSqlDbType.UniqueIdentifier);
			caseId.ColumnOrdinal.ShouldBe(0);

			IFieldDetails createDateUtc = results[1];
			createDateUtc.ColumnName.ShouldBe("CreateDateUtc");
			createDateUtc.AllowDbNull.ShouldBeFalse();
			createDateUtc.DbType.ShouldBe(MsSqlDbType.DateTime);
			createDateUtc.ColumnOrdinal.ShouldBe(1);
		}

		[Fact]
		public void GetResultsTest_2()
		{
			string query = QuerySelect.SelectCount;
			var queryParser = new MsSqlServerQueryParser();
			IFieldDetails[] results = queryParser.GetResultDetails(query, ConnectionString).ToArray();

			results.ShouldNotBeEmpty();
			results.Length.ShouldBe(1);

			IFieldDetails count = results[0];
			count.ColumnName.ShouldBe("column_0");
			count.AllowDbNull.ShouldBeTrue();
			count.DbType.ShouldBe(MsSqlDbType.Int);
			count.ColumnOrdinal.ShouldBe(0);
		}

		[Fact]
		public void GetResultsTest_3()
		{
			string query = QuerySelect.SelectTwoStringsWithLeftJoin;
			var queryParser = new MsSqlServerQueryParser();
			IFieldDetails[] results = queryParser.GetResultDetails(query, ConnectionString).ToArray();

			results.ShouldNotBeEmpty();
			results.Length.ShouldBe(2);

			IFieldDetails userKey = results[0];
			userKey.ColumnName.ShouldBe("UserKey");
			userKey.AllowDbNull.ShouldBeFalse();
			userKey.DbType.ShouldBe(MsSqlDbType.NVarChar);
			userKey.ColumnOrdinal.ShouldBe(0);

			IFieldDetails shardName = results[1];
			shardName.ColumnName.ShouldBe("ShardName");
			shardName.AllowDbNull.ShouldBeTrue(); // left join
			shardName.DbType.ShouldBe(MsSqlDbType.NVarChar);
			shardName.ColumnOrdinal.ShouldBe(1);
		}

		[Fact]
		public void GetResultsTest_4()
		{
			string query = QuerySelect.SelectDateWithOrdinal;
			var queryParser = new MsSqlServerQueryParser();
			IFieldDetails[] results = queryParser.GetResultDetails(query, ConnectionString).ToArray();

			results.ShouldNotBeEmpty();
			results.Length.ShouldBe(2);

			IFieldDetails createDateUtc = results[0];
			createDateUtc.ColumnName.ShouldBe("CreateDateUtc");
			createDateUtc.AllowDbNull.ShouldBeFalse();
			createDateUtc.DbType.ShouldBe(MsSqlDbType.DateTime);
			createDateUtc.ColumnOrdinal.ShouldBe(0);

			IFieldDetails ordinal = results[1];
			ordinal.ColumnName.ShouldBe("column_1");
			ordinal.AllowDbNull.ShouldBeFalse();
			ordinal.DbType.ShouldBe(MsSqlDbType.Int);
			ordinal.ColumnOrdinal.ShouldBe(1);
		}

		[Fact]
		public void GetResultsTest_5()
		{
			string query = QuerySelect.SelectDateWithNamedOrdinal;
			var queryParser = new MsSqlServerQueryParser();
			IFieldDetails[] results = queryParser.GetResultDetails(query, ConnectionString).ToArray();

			results.ShouldNotBeEmpty();
			results.Length.ShouldBe(2);

			IFieldDetails createDateUtc = results[0];
			createDateUtc.ColumnName.ShouldBe("CreateDateUtc");
			createDateUtc.AllowDbNull.ShouldBeFalse();
			createDateUtc.DbType.ShouldBe(MsSqlDbType.DateTime);
			createDateUtc.ColumnOrdinal.ShouldBe(0);

			IFieldDetails ordinal = results[1];
			ordinal.ColumnName.ShouldBe("answer");
			ordinal.AllowDbNull.ShouldBeFalse();
			ordinal.DbType.ShouldBe(MsSqlDbType.Int);
			ordinal.ColumnOrdinal.ShouldBe(1);
		}

		[Fact]
		public void GetResultsTest_6()
		{
			string query = QuerySelect.SelectNotUniqueFieldsWithLeftJoin;
			var queryParser = new MsSqlServerQueryParser();
			IFieldDetails[] results = queryParser.GetResultDetails(query, ConnectionString).ToArray();

			results.ShouldNotBeEmpty();
			results.Length.ShouldBe(2);

			IFieldDetails id1 = results[0];
			id1.ColumnName.ShouldBe("Id");
			id1.AllowDbNull.ShouldBeFalse();
			id1.DbType.ShouldBe(MsSqlDbType.Int);
			id1.ColumnOrdinal.ShouldBe(0);

			IFieldDetails id2 = results[1];
			id2.ColumnName.ShouldBe("Id");
			id2.AllowDbNull.ShouldBeTrue();
			id2.DbType.ShouldBe(MsSqlDbType.Int);
			id2.ColumnOrdinal.ShouldBe(1);
		}

		[Fact]
		public void GetResultsTest_7()
		{
			string query = QuerySelect.SelectNotUniqueNamedFieldsWithInnerJoin;
			var queryParser = new MsSqlServerQueryParser();
			IFieldDetails[] results = queryParser.GetResultDetails(query, ConnectionString).ToArray();

			results.ShouldNotBeEmpty();
			results.Length.ShouldBe(2);

			IFieldDetails subscriptionId = results[0];
			subscriptionId.ColumnName.ShouldBe("SubscriptionId");
			subscriptionId.AllowDbNull.ShouldBeFalse();
			subscriptionId.DbType.ShouldBe(MsSqlDbType.Int);
			subscriptionId.ColumnOrdinal.ShouldBe(0);

			IFieldDetails eventId = results[1];
			eventId.ColumnName.ShouldBe("EventId");
			eventId.AllowDbNull.ShouldBeFalse();
			eventId.DbType.ShouldBe(MsSqlDbType.Int);
			eventId.ColumnOrdinal.ShouldBe(1);
			eventId.ColumnName.ShouldBe("EventId");
		}

		[Fact]
		public void GetResultsTest_8()
		{
			string query = QuerySelect.SelectTwoStringsWithInnerJoin;
			var queryParser = new MsSqlServerQueryParser();
			IFieldDetails[] results = queryParser.GetResultDetails(query, ConnectionString).ToArray();

			results.ShouldNotBeEmpty();
			results.Length.ShouldBe(2);

			IFieldDetails userKey = results[0];
			userKey.ColumnName.ShouldBe("UserKey");
			userKey.AllowDbNull.ShouldBeFalse();
			userKey.DbType.ShouldBe(MsSqlDbType.NVarChar);
			userKey.ColumnOrdinal.ShouldBe(0);

			IFieldDetails shardName = results[1];
			shardName.ColumnName.ShouldBe("ShardName");
			shardName.AllowDbNull.ShouldBeFalse(); // inner join
			shardName.DbType.ShouldBe(MsSqlDbType.NVarChar);
			shardName.ColumnOrdinal.ShouldBe(1);
		}

		[Fact]
		public void GetResultsTest_9()
		{
			string query = QuerySelect.SelectNotUniqueFieldsWithInnerJoin;
			var queryParser = new MsSqlServerQueryParser();
			IFieldDetails[] results = queryParser.GetResultDetails(query, ConnectionString).ToArray();

			results.ShouldNotBeEmpty();
			results.Length.ShouldBe(2);

			IFieldDetails id1 = results[0];
			id1.ColumnName.ShouldBe("Id");
			id1.AllowDbNull.ShouldBeFalse();
			id1.DbType.ShouldBe(MsSqlDbType.Int);
			id1.ColumnOrdinal.ShouldBe(0);

			IFieldDetails id2 = results[1];
			id2.ColumnName.ShouldBe("Id");
			id2.AllowDbNull.ShouldBeFalse();
			id2.DbType.ShouldBe(MsSqlDbType.Int);
			id2.ColumnOrdinal.ShouldBe(1);
		}

		[Fact]
		public void GetResultsTest_10()
		{
			string query = QuerySelect.SelectNotUniqueFieldsWithRightJoin;
			var queryParser = new MsSqlServerQueryParser();
			IFieldDetails[] results = queryParser.GetResultDetails(query, ConnectionString).ToArray();

			results.ShouldNotBeEmpty();
			results.Length.ShouldBe(2);

			IFieldDetails id1 = results[0];
			id1.ColumnName.ShouldBe("Id");
			id1.AllowDbNull.ShouldBeTrue();
			id1.DbType.ShouldBe(MsSqlDbType.Int);
			id1.ColumnOrdinal.ShouldBe(0);

			IFieldDetails id2 = results[1];
			id2.ColumnName.ShouldBe("Id");
			id2.AllowDbNull.ShouldBeFalse();
			id2.DbType.ShouldBe(MsSqlDbType.Int);
			id2.ColumnOrdinal.ShouldBe(1);
		}

		[Fact]
		public void GetResultsTest_11()
		{
			string query = QuerySelect.SelectGuidAndDateWithPagingAndPartOfParameters;
			var queryParser = new MsSqlServerQueryParser();
			IFieldDetails[] results = queryParser.GetResultDetails(query, ConnectionString).ToArray();

			results.ShouldNotBeEmpty();
			results.Length.ShouldBe(2);

			IFieldDetails caseId = results[0];
			caseId.ColumnName.ShouldBe("CaseId");
			caseId.AllowDbNull.ShouldBeFalse();
			caseId.DbType.ShouldBe(MsSqlDbType.UniqueIdentifier);
			caseId.ColumnOrdinal.ShouldBe(0);

			IFieldDetails createDateUtc = results[1];
			createDateUtc.ColumnName.ShouldBe("CreateDateUtc");
			createDateUtc.AllowDbNull.ShouldBeFalse();
			createDateUtc.DbType.ShouldBe(MsSqlDbType.DateTime);
			createDateUtc.ColumnOrdinal.ShouldBe(1);
		}

		[Fact]
		public void GetResultsTest_12()
		{
			string query = QuerySelect.SelectGuidAndDateWithPagingNoParameters;
			var queryParser = new MsSqlServerQueryParser();
			IFieldDetails[] results = queryParser.GetResultDetails(query, ConnectionString).ToArray();

			results.ShouldNotBeEmpty();
			results.Length.ShouldBe(2);

			IFieldDetails caseId = results[0];
			caseId.ColumnName.ShouldBe("CaseId");
			caseId.AllowDbNull.ShouldBeFalse();
			caseId.DbType.ShouldBe(MsSqlDbType.UniqueIdentifier);
			caseId.ColumnOrdinal.ShouldBe(0);

			IFieldDetails createDateUtc = results[1];
			createDateUtc.ColumnName.ShouldBe("CreateDateUtc");
			createDateUtc.AllowDbNull.ShouldBeFalse();
			createDateUtc.DbType.ShouldBe(MsSqlDbType.DateTime);
			createDateUtc.ColumnOrdinal.ShouldBe(1);
		}

		[Fact]
		public void GetQueryBaseInfoTest_1()
		{
			string query = QuerySelect.SelectNotUniqueFieldsWithRightJoin;
			var queryParser = new MsSqlServerQueryParser();
			IQueryBaseInfo result = queryParser.GetQueryBaseInfo(query);

			result.QueryType.ShouldBe(QueryType.Read);
		}

		[Fact]
		public void GetQueryBaseInfoTest_2()
		{
			string query = QueryDelete.DeleteByGuid;
			var queryParser = new MsSqlServerQueryParser();
			IQueryBaseInfo result = queryParser.GetQueryBaseInfo(query);

			result.QueryType.ShouldBe(QueryType.Delete);
		}

		[Fact]
		public void GetQueryBaseInfoTest_3()
		{
			string query = QueryInsert.InsertStringGuidDate;
			var queryParser = new MsSqlServerQueryParser();
			IQueryBaseInfo result = queryParser.GetQueryBaseInfo(query);

			result.QueryType.ShouldBe(QueryType.Create);
		}

		[Fact]
		public void GetQueryBaseInfoTest_4()
		{
			string query = QueryUpdate.UpdateDateByGuid;
			var queryParser = new MsSqlServerQueryParser();
			IQueryBaseInfo result = queryParser.GetQueryBaseInfo(query);

			result.QueryType.ShouldBe(QueryType.Update);
		}


		[Fact]
		public void GetQueryInfoTest_1()
		{
			string query = QuerySelect.SelectGuidAndDateWithPagingAndPartOfParameters;
			var queryParser = new MsSqlServerQueryParser();
			IQueryInfo result = queryParser.GetQueryInfo(query, ConnectionString);

			result.QueryType.ShouldBe(QueryType.Read);

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
	}
}