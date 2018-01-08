using System.Linq;
using SqlFirst.Core;
using SqlFirst.Providers.Postgres.Tests.Queries;
using Xunit;
using Xunit.Should;

namespace SqlFirst.Providers.Postgres.Tests
{
	public partial class PostgresQueryParserTests
	{
		private const string ConnectionString = "Server = 127.0.0.1; Port = 5432; Database = CasebookApi.Arbitrage.Tracking_dev; User Id = postgres;Password = postgres;";

		[Fact]
		public void GetResultsTest_1()
		{
			string query = QuerySelect.SelectGuidAndDateWithPaging;
			var queryParser = new PostgresQueryParser();
			IFieldDetails[] results = queryParser.GetResultDetails(query, ConnectionString).ToArray();

			results.ShouldNotBeEmpty();
			results.Length.ShouldBe(2);

			IFieldDetails caseId = results[0];
			caseId.ColumnName.ShouldBe("caseid");
			caseId.AllowDbNull.ShouldBeFalse();
			caseId.DbType.ShouldBe(PostgresDbType.Guid);
			caseId.ColumnOrdinal.ShouldBe(0);

			IFieldDetails createDateUtc = results[1];
			createDateUtc.ColumnName.ShouldBe("createdateutc");
			createDateUtc.AllowDbNull.ShouldBeFalse();
			createDateUtc.DbType.ShouldBe(PostgresDbType.Timestamp);
			createDateUtc.ColumnOrdinal.ShouldBe(1);
		}

		[Fact]
		public void GetResultsTest_2()
		{
			string query = QuerySelect.SelectCount;
			var queryParser = new PostgresQueryParser();
			IFieldDetails[] results = queryParser.GetResultDetails(query, ConnectionString).ToArray();

			results.ShouldNotBeEmpty();
			results.Length.ShouldBe(1);

			IFieldDetails count = results[0];
			count.ColumnName.ShouldBe("count");
			count.AllowDbNull.ShouldBeTrue();
			count.DbType.ShouldBe(PostgresDbType.Int8);
			count.ColumnOrdinal.ShouldBe(0);
		}

		[Fact]
		public void GetResultsTest_3()
		{
			string query = QuerySelect.SelectTwoStringsWithLeftJoin;
			var queryParser = new PostgresQueryParser();
			IFieldDetails[] results = queryParser.GetResultDetails(query, ConnectionString).ToArray();

			results.ShouldNotBeEmpty();
			results.Length.ShouldBe(2);

			IFieldDetails userKey = results[0];
			userKey.ColumnName.ShouldBe("userkey");
			userKey.AllowDbNull.ShouldBeFalse();
			userKey.DbType.ShouldBe(PostgresDbType.Varchar);
			userKey.ColumnOrdinal.ShouldBe(0);

			IFieldDetails shardName = results[1];
			shardName.ColumnName.ShouldBe("shardname");
			shardName.AllowDbNull.ShouldBeFalse(); // left join
			shardName.DbType.ShouldBe(PostgresDbType.Varchar);
			shardName.ColumnOrdinal.ShouldBe(1);
		}

		[Fact]
		public void GetResultsTest_4()
		{
			string query = QuerySelect.SelectDateWithOrdinal;
			var queryParser = new PostgresQueryParser();
			IFieldDetails[] results = queryParser.GetResultDetails(query, ConnectionString).ToArray();

			results.ShouldNotBeEmpty();
			results.Length.ShouldBe(2);

			IFieldDetails createDateUtc = results[0];
			createDateUtc.ColumnName.ShouldBe("createdateutc");
			createDateUtc.AllowDbNull.ShouldBeFalse();
			createDateUtc.DbType.ShouldBe(PostgresDbType.Timestamp);
			createDateUtc.ColumnOrdinal.ShouldBe(0);

			IFieldDetails ordinal = results[1];
			ordinal.ColumnName.ShouldBe("column_1");
			ordinal.AllowDbNull.ShouldBeTrue();
			ordinal.DbType.ShouldBe(PostgresDbType.Int4);
			ordinal.ColumnOrdinal.ShouldBe(1);
		}

		[Fact]
		public void GetResultsTest_5()
		{
			string query = QuerySelect.SelectDateWithNamedOrdinal;
			var queryParser = new PostgresQueryParser();
			IFieldDetails[] results = queryParser.GetResultDetails(query, ConnectionString).ToArray();

			results.ShouldNotBeEmpty();
			results.Length.ShouldBe(2);

			IFieldDetails createDateUtc = results[0];
			createDateUtc.ColumnName.ShouldBe("createdateutc");
			createDateUtc.AllowDbNull.ShouldBeFalse();
			createDateUtc.DbType.ShouldBe(PostgresDbType.Timestamp);
			createDateUtc.ColumnOrdinal.ShouldBe(0);

			IFieldDetails ordinal = results[1];
			ordinal.ColumnName.ShouldBe("answer");
			ordinal.AllowDbNull.ShouldBeTrue();
			ordinal.DbType.ShouldBe(PostgresDbType.Int4);
			ordinal.ColumnOrdinal.ShouldBe(1);
		}

		[Fact]
		public void GetResultsTest_6()
		{
			string query = QuerySelect.SelectNotUniqueFieldsWithLeftJoin;
			var queryParser = new PostgresQueryParser();
			IFieldDetails[] results = queryParser.GetResultDetails(query, ConnectionString).ToArray();

			results.ShouldNotBeEmpty();
			results.Length.ShouldBe(2);

			IFieldDetails id1 = results[0];
			id1.ColumnName.ShouldBe("id");
			id1.AllowDbNull.ShouldBeFalse();
			id1.DbType.ShouldBe(PostgresDbType.Int4);
			id1.ColumnOrdinal.ShouldBe(0);

			IFieldDetails id2 = results[1];
			id2.ColumnName.ShouldBe("id");
			id2.AllowDbNull.ShouldBeFalse(); // much smarter than mssql
			id2.DbType.ShouldBe(PostgresDbType.Int4);
			id2.ColumnOrdinal.ShouldBe(1);
		}


		[Fact]
		public void GetResultsTest_7()
		{
			string query = QuerySelect.SelectNotUniqueNamedFieldsWithInnerJoin;
			var queryParser = new PostgresQueryParser();
			IFieldDetails[] results = queryParser.GetResultDetails(query, ConnectionString).ToArray();

			results.ShouldNotBeEmpty();
			results.Length.ShouldBe(2);

			IFieldDetails subscriptionId = results[0];
			subscriptionId.ColumnName.ShouldBe("subscriptionid");
			subscriptionId.AllowDbNull.ShouldBeFalse();
			subscriptionId.DbType.ShouldBe(PostgresDbType.Int4);
			subscriptionId.ColumnOrdinal.ShouldBe(0);

			IFieldDetails eventId = results[1];
			eventId.ColumnName.ShouldBe("eventid");
			eventId.AllowDbNull.ShouldBeFalse();
			eventId.DbType.ShouldBe(PostgresDbType.Int4);
			eventId.ColumnOrdinal.ShouldBe(1);
		}

		[Fact]
		public void GetResultsTest_8()
		{
			string query = QuerySelect.SelectTwoStringsWithInnerJoin;
			var queryParser = new PostgresQueryParser();
			IFieldDetails[] results = queryParser.GetResultDetails(query, ConnectionString).ToArray();

			results.ShouldNotBeEmpty();
			results.Length.ShouldBe(2);

			IFieldDetails userKey = results[0];
			userKey.ColumnName.ShouldBe("userkey");
			userKey.AllowDbNull.ShouldBeFalse();
			userKey.DbType.ShouldBe(PostgresDbType.Varchar);
			userKey.ColumnOrdinal.ShouldBe(0);

			IFieldDetails shardName = results[1];
			shardName.ColumnName.ShouldBe("shardname");
			shardName.AllowDbNull.ShouldBeFalse(); // inner join
			shardName.DbType.ShouldBe(PostgresDbType.Varchar);
			shardName.ColumnOrdinal.ShouldBe(1);
		}

		[Fact]
		public void GetResultsTest_9()
		{
			string query = QuerySelect.SelectNotUniqueFieldsWithInnerJoin;
			var queryParser = new PostgresQueryParser();
			IFieldDetails[] results = queryParser.GetResultDetails(query, ConnectionString).ToArray();

			results.ShouldNotBeEmpty();
			results.Length.ShouldBe(2);

			IFieldDetails id1 = results[0];
			id1.ColumnName.ShouldBe("id");
			id1.AllowDbNull.ShouldBeFalse();
			id1.DbType.ShouldBe(PostgresDbType.Int4);
			id1.ColumnOrdinal.ShouldBe(0);

			IFieldDetails id2 = results[1];
			id2.ColumnName.ShouldBe("id");
			id2.AllowDbNull.ShouldBeFalse();
			id2.DbType.ShouldBe(PostgresDbType.Int4);
			id2.ColumnOrdinal.ShouldBe(1);
		}

		[Fact]
		public void GetResultsTest_10()
		{
			string query = QuerySelect.SelectNotUniqueFieldsWithRightJoin;
			var queryParser = new PostgresQueryParser();
			IFieldDetails[] results = queryParser.GetResultDetails(query, ConnectionString).ToArray();

			results.ShouldNotBeEmpty();
			results.Length.ShouldBe(2);

			IFieldDetails id1 = results[0];
			id1.ColumnName.ShouldBe("id");
			id1.AllowDbNull.ShouldBeFalse();
			id1.DbType.ShouldBe(PostgresDbType.Int4);
			id1.ColumnOrdinal.ShouldBe(0);

			IFieldDetails id2 = results[1];
			id2.ColumnName.ShouldBe("id");
			id2.AllowDbNull.ShouldBeFalse();
			id2.DbType.ShouldBe(PostgresDbType.Int4);
			id2.ColumnOrdinal.ShouldBe(1);
		}

		[Fact]
		public void GetResultsTest_12()
		{
			string query = QuerySelect.SelectGuidAndDateWithPagingNoParameters;
			var queryParser = new PostgresQueryParser();
			IFieldDetails[] results = queryParser.GetResultDetails(query, ConnectionString).ToArray();

			results.ShouldNotBeEmpty();
			results.Length.ShouldBe(2);

			IFieldDetails caseId = results[0];
			caseId.ColumnName.ShouldBe("caseid");
			caseId.AllowDbNull.ShouldBeFalse();
			caseId.DbType.ShouldBe(PostgresDbType.Guid);
			caseId.ColumnOrdinal.ShouldBe(0);

			IFieldDetails createDateUtc = results[1];
			createDateUtc.ColumnName.ShouldBe("createdateutc");
			createDateUtc.AllowDbNull.ShouldBeFalse();
			createDateUtc.DbType.ShouldBe(PostgresDbType.Timestamp);
			createDateUtc.ColumnOrdinal.ShouldBe(1);
		}
	}
}