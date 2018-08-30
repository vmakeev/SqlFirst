using System.Linq;
using Shouldly;
using SqlFirst.Core;
using SqlFirst.Providers.MsSqlServer.Tests.Queries;
using Xunit;

namespace SqlFirst.Providers.MsSqlServer.Tests
{
	public partial class MsSqlServerQueryParserTests
	{
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
	}
}