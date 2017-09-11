using System;
using System.Linq;
using SqlFirst.Core.Parsing;
using SqlFirst.Providers.MsSqlServer.Tests.Queries;
using Xunit;
using Xunit.Should;

namespace SqlFirst.Providers.MsSqlServer.Tests
{
	public class MsSqlServerQueryParserTests
	{
		private const string CONNECTION_STRING = @"Server=api-dev;Database=CasebookApi.Arbitrage.Tracking_dev;Integrated Security=SSPI;";

		[Fact]
		public void GetDeclaredParametersTest_1()
		{
			string query = Query.SelectGuidAndDateWithPagingAssignmentAndComments;
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
			string query = Query.SelectDateWithNamedOrdinal;
			var queryParser = new MsSqlServerQueryParser();
			IQueryParamInfo[] declaredParameters = queryParser.GetDeclaredParameters(query).ToArray();

			declaredParameters.ShouldNotBeNull();
			declaredParameters.ShouldBeEmpty();
		}

		[Fact]
		public void GetDeclaredParametersTest_3()
		{
			string query = Query.SelectGuidAndDateWithPagingAndPartOfParameters;
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
			string query = Query.SelectGuidAndDateWithPaging;
			var queryParser = new MsSqlServerQueryParser();
			IQueryParamInfo[] undeclaredParameters = queryParser.GetUndeclaredParameters(query, CONNECTION_STRING).ToArray();

			undeclaredParameters.ShouldNotBeNull();
			undeclaredParameters.ShouldBeEmpty();
		}

		[Fact]
		public void GetUndeclaredParametersTest_2()
		{
			string query = Query.SelectGuidAndDateWithPagingNoParameters;
			var queryParser = new MsSqlServerQueryParser();
			IQueryParamInfo[] undeclaredParameters = queryParser.GetUndeclaredParameters(query, CONNECTION_STRING).ToArray();

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
			string query = Query.SelectGuidAndDateWithPagingAndPartOfParameters;
			var queryParser = new MsSqlServerQueryParser();
			IQueryParamInfo[] undeclaredParameters = queryParser.GetUndeclaredParameters(query, CONNECTION_STRING).ToArray();

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
			string query = Query.SelectGuidAndDateWithPaging;
			var queryParser = new MsSqlServerQueryParser();
			IFieldDetails[] results = queryParser.GetResultDetails(query, CONNECTION_STRING).ToArray();

			results.ShouldNotBeEmpty();
			results.Length.ShouldBe(2);

			IFieldDetails caseId = results[0];
			caseId.ColumnName.ShouldBe("CaseId");
			caseId.BaseColumnName.ShouldBe("CaseId");
			caseId.AllowDbNull.ShouldBeFalse();
			caseId.ClrType.ShouldBe(typeof(Guid));
			caseId.DbType.ShouldBe(MsSqlDbType.UniqueIdentifier);
			caseId.IsUnique.ShouldBeFalse();

			IFieldDetails createDateUtc = results.Last();
			createDateUtc.ColumnName.ShouldBe("CreateDateUtc");
			createDateUtc.BaseColumnName.ShouldBe("CreateDateUtc");
			createDateUtc.AllowDbNull.ShouldBeFalse();
			createDateUtc.ClrType.ShouldBe(typeof(DateTime));
			createDateUtc.DbType.ShouldBe(MsSqlDbType.DateTime);
			createDateUtc.IsUnique.ShouldBeFalse();
		}

		[Fact]
		public void GetResultsTest_2()
		{
			string query = Query.SelectCount;
			var queryParser = new MsSqlServerQueryParser();
			IFieldDetails[] results = queryParser.GetResultDetails(query, CONNECTION_STRING).ToArray();

			results.ShouldNotBeEmpty();
			results.Length.ShouldBe(1);

			IFieldDetails count = results[0];
			count.ColumnName.ShouldBe("column_0");
			count.BaseColumnName.ShouldBeNull();
			count.ClrType.ShouldBe(typeof(int));
			count.DbType.ShouldBe(MsSqlDbType.Int);
		}

		[Fact]
		public void GetResultsTest_3()
		{
			string query = Query.SelectTwoStringsWithJoin;
			var queryParser = new MsSqlServerQueryParser();
			IFieldDetails[] results = queryParser.GetResultDetails(query, CONNECTION_STRING).ToArray();

			results.ShouldNotBeEmpty();
			results.Length.ShouldBe(2);

			IFieldDetails userKey = results[0];
			userKey.ColumnName.ShouldBe("UserKey");
			userKey.BaseColumnName.ShouldBe("UserKey");
			userKey.ClrType.ShouldBe(typeof(string));
			userKey.DbType.ShouldBe(MsSqlDbType.NVarChar);

			IFieldDetails shardName = results[1];
			shardName.ColumnName.ShouldBe("ShardName");
			shardName.BaseColumnName.ShouldBe("ShardName");
			shardName.ClrType.ShouldBe(typeof(string));
			shardName.DbType.ShouldBe(MsSqlDbType.NVarChar);
		}

		[Fact]
		public void GetResultsTest_4()
		{
			string query = Query.SelectDateWithOrdinal;
			var queryParser = new MsSqlServerQueryParser();
			IFieldDetails[] results = queryParser.GetResultDetails(query, CONNECTION_STRING).ToArray();

			results.ShouldNotBeEmpty();
			results.Length.ShouldBe(2);

			IFieldDetails createDateUtc = results[0];
			createDateUtc.ColumnName.ShouldBe("CreateDateUtc");
			createDateUtc.BaseColumnName.ShouldBe("CreateDateUtc");
			createDateUtc.AllowDbNull.ShouldBeFalse();
			createDateUtc.ClrType.ShouldBe(typeof(DateTime));
			createDateUtc.DbType.ShouldBe(MsSqlDbType.DateTime);
			createDateUtc.IsUnique.ShouldBeFalse();

			IFieldDetails ordinal = results[1];
			ordinal.ColumnName.ShouldBe("column_1");
			ordinal.BaseColumnName.ShouldBeNull();
			ordinal.ClrType.ShouldBe(typeof(int));
			ordinal.DbType.ShouldBe(MsSqlDbType.Int);
		}

		[Fact]
		public void GetResultsTest_5()
		{
			string query = Query.SelectDateWithNamedOrdinal;
			var queryParser = new MsSqlServerQueryParser();
			IFieldDetails[] results = queryParser.GetResultDetails(query, CONNECTION_STRING).ToArray();

			results.ShouldNotBeEmpty();
			results.Length.ShouldBe(2);

			IFieldDetails createDateUtc = results[0];
			createDateUtc.ColumnName.ShouldBe("CreateDateUtc");
			createDateUtc.BaseColumnName.ShouldBe("CreateDateUtc");
			createDateUtc.AllowDbNull.ShouldBeFalse();
			createDateUtc.ClrType.ShouldBe(typeof(DateTime));
			createDateUtc.DbType.ShouldBe(MsSqlDbType.DateTime);
			createDateUtc.IsUnique.ShouldBeFalse();

			IFieldDetails ordinal = results[1];
			ordinal.ColumnName.ShouldBe("answer");
			ordinal.BaseColumnName.ShouldBe("answer");
			ordinal.ClrType.ShouldBe(typeof(int));
			ordinal.DbType.ShouldBe(MsSqlDbType.Int);
		}

		[Fact]
		public void GetResultsTest_6()
		{
			string query = Query.SelectNotUniqueFieldsWithJoin;
			var queryParser = new MsSqlServerQueryParser();
			IFieldDetails[] results = queryParser.GetResultDetails(query, CONNECTION_STRING).ToArray();

			results.ShouldNotBeEmpty();
			results.Length.ShouldBe(2);

			IFieldDetails id1 = results[0];
			id1.ColumnName.ShouldBe("Id");
			id1.BaseColumnName.ShouldBe("Id");
			id1.ClrType.ShouldBe(typeof(int));
			id1.DbType.ShouldBe(MsSqlDbType.Int);
			id1.IsIdentity.ShouldBeTrue();

			IFieldDetails id2 = results[1];
			id2.ColumnName.ShouldBe("Id");
			id2.BaseColumnName.ShouldBe("Id");
			id2.ClrType.ShouldBe(typeof(int));
			id2.DbType.ShouldBe(MsSqlDbType.Int);
			id2.IsIdentity.ShouldBeTrue();
		}

		[Fact]
		public void GetResultsTest_7()
		{
			string query = Query.SelectNotUniqueNamedFieldsWithJoin;
			var queryParser = new MsSqlServerQueryParser();
			IFieldDetails[] results = queryParser.GetResultDetails(query, CONNECTION_STRING).ToArray();

			results.ShouldNotBeEmpty();
			results.Length.ShouldBe(2);

			IFieldDetails subscriptionId = results[0];
			subscriptionId.ColumnName.ShouldBe("SubscriptionId");
			subscriptionId.BaseColumnName.ShouldBe("SubscriptionId"); //todo: WAT?
			subscriptionId.ClrType.ShouldBe(typeof(int));
			subscriptionId.DbType.ShouldBe(MsSqlDbType.Int);
			subscriptionId.IsIdentity.ShouldBeTrue();

			IFieldDetails eventId = results[1];
			eventId.ColumnName.ShouldBe("EventId");
			eventId.BaseColumnName.ShouldBe("EventId");
			eventId.ClrType.ShouldBe(typeof(int));
			eventId.DbType.ShouldBe(MsSqlDbType.Int);
			eventId.IsIdentity.ShouldBeTrue();
		}
	}
}