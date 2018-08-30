using System.Linq;
using Shouldly;
using SqlFirst.Core;
using SqlFirst.Providers.Postgres.Tests.Queries;
using Xunit;

namespace SqlFirst.Providers.Postgres.Tests
{
	public partial class PostgresQueryParserTests
	{
		[Fact]
		public void GetDeclaredParametersTest_Select_2()
		{
			string query = QuerySelect.SelectDateWithNamedOrdinal;
			var queryParser = new PostgresQueryParser();
			IQueryParamInfo[] declaredParameters = queryParser.GetDeclaredParameters(query).ToArray();

			declaredParameters.ShouldNotBeNull();
			declaredParameters.ShouldBeEmpty();
		}

		[Fact]
		public void GetUndeclaredParametersTest_Select_1()
		{
			string query = QuerySelect.SelectGuidAndDateWithPaging;
			var queryParser = new PostgresQueryParser();
			IQueryParamInfo[] undeclaredParameters = queryParser.GetUndeclaredParameters(query, ConnectionString).ToArray();

			undeclaredParameters.ShouldNotBeNull();
			undeclaredParameters.Length.ShouldBe(3);

			IQueryParamInfo userKey = undeclaredParameters[0];
			userKey.ShouldNotBeNull();
			userKey.DbName.ShouldBe("userkey");
			userKey.SemanticName.ShouldBe("userkey");
			userKey.IsNumbered.ShouldBe(false);
			userKey.DbType.ShouldBe(PostgresDbType.Text);
			userKey.Length.ShouldBeNull();
			userKey.DefaultValue.ShouldBeNull();

			IQueryParamInfo take = undeclaredParameters[1];
			take.ShouldNotBeNull();
			take.DbName.ShouldBe("take");
			take.SemanticName.ShouldBe("take");
			take.IsNumbered.ShouldBe(false);
			take.DbType.ShouldBe(PostgresDbType.Bigint);
			take.Length.ShouldBeNull();
			take.DefaultValue.ShouldBeNull();

			IQueryParamInfo skip = undeclaredParameters[2];
			skip.ShouldNotBeNull();
			skip.DbName.ShouldBe("skip");
			skip.SemanticName.ShouldBe("skip");
			skip.IsNumbered.ShouldBe(false);
			skip.DbType.ShouldBe(PostgresDbType.Bigint);
			skip.Length.ShouldBeNull();
			skip.DefaultValue.ShouldBeNull();
		}

		[Fact]
		public void GetUndeclaredParametersTest_Select_2()
		{
			string query = QuerySelect.SelectGuidAndDateWithPagingNoParameters;
			var queryParser = new PostgresQueryParser();
			IQueryParamInfo[] undeclaredParameters = queryParser.GetUndeclaredParameters(query, ConnectionString).ToArray();

			undeclaredParameters.ShouldNotBeNull();
			undeclaredParameters.Length.ShouldBe(3);

			IQueryParamInfo userKey = undeclaredParameters[0];
			userKey.ShouldNotBeNull();
			userKey.DbName.ShouldBe("userKey");
			userKey.DbType.ShouldBe(PostgresDbType.Text);
			userKey.Length.ShouldBe(null);
			userKey.DefaultValue.ShouldBeNull();

			IQueryParamInfo skip = undeclaredParameters[1];
			skip.ShouldNotBeNull();
			skip.DbName.ShouldBe("skip");
			skip.DbType.ShouldBe(PostgresDbType.Bigint);
			skip.Length.ShouldBeNull();
			skip.DefaultValue.ShouldBeNull();

			IQueryParamInfo take = undeclaredParameters[2];
			take.ShouldNotBeNull();
			take.DbName.ShouldBe("take");
			take.DbType.ShouldBe(PostgresDbType.Bigint);
			take.Length.ShouldBeNull();
			take.DefaultValue.ShouldBeNull();
		}

		[Fact]
		public void GetUndeclaredParametersTest_Insert_1()
		{
			string query = QueryInsert.InsertStringGuidDate;
			var queryParser = new PostgresQueryParser();
			IQueryParamInfo[] undeclaredParameters = queryParser.GetUndeclaredParameters(query, ConnectionString).ToArray();

			undeclaredParameters.ShouldNotBeNull();
			undeclaredParameters.Length.ShouldBe(3);

			IQueryParamInfo userKey = undeclaredParameters[0];
			userKey.ShouldNotBeNull();
			userKey.DbName.ShouldBe("userKey");
			userKey.SemanticName.ShouldBe("userKey");
			userKey.IsNumbered.ShouldBe(false);
			userKey.DbType.ShouldBe(PostgresDbType.Varchar);
			userKey.Length.ShouldBeNull();
			userKey.DefaultValue.ShouldBeNull();

			IQueryParamInfo caseId = undeclaredParameters[1];
			caseId.ShouldNotBeNull();
			caseId.DbName.ShouldBe("caseId");
			caseId.SemanticName.ShouldBe("caseId");
			caseId.IsNumbered.ShouldBe(false);
			caseId.DbType.ShouldBe(PostgresDbType.Guid);
			caseId.Length.ShouldBeNull();
			caseId.DefaultValue.ShouldBeNull();

			IQueryParamInfo createDateUtc = undeclaredParameters[2];
			createDateUtc.ShouldNotBeNull();
			createDateUtc.DbName.ShouldBe("createDateUtc");
			createDateUtc.SemanticName.ShouldBe("createDateUtc");
			createDateUtc.IsNumbered.ShouldBe(false);
			createDateUtc.DbType.ShouldBe(PostgresDbType.Timestamp);
			createDateUtc.Length.ShouldBeNull();
			createDateUtc.DefaultValue.ShouldBeNull();
		}

		[Fact]
		public void GetUndeclaredParametersTest_Insert_2()
		{
			string query = QueryInsert.InsertStringGuidDateWithOption;
			var queryParser = new PostgresQueryParser();
			IQueryParamInfo[] undeclaredParameters = queryParser.GetUndeclaredParameters(query, ConnectionString).ToArray();

			undeclaredParameters.ShouldNotBeNull();
			undeclaredParameters.Length.ShouldBe(3);

			IQueryParamInfo userKey = undeclaredParameters[0];
			userKey.ShouldNotBeNull();
			userKey.DbName.ShouldBe("userKey");
			userKey.SemanticName.ShouldBe("userKey");
			userKey.IsNumbered.ShouldBe(false);
			userKey.DbType.ShouldBe(PostgresDbType.Varchar);
			userKey.Length.ShouldBeNull();
			userKey.DefaultValue.ShouldBeNull();

			IQueryParamInfo caseId = undeclaredParameters[1];
			caseId.ShouldNotBeNull();
			caseId.DbName.ShouldBe("caseId");
			caseId.SemanticName.ShouldBe("caseId");
			caseId.IsNumbered.ShouldBe(false);
			caseId.DbType.ShouldBe(PostgresDbType.Guid);
			caseId.Length.ShouldBeNull();
			caseId.DefaultValue.ShouldBeNull();

			IQueryParamInfo createDateUtc = undeclaredParameters[2];
			createDateUtc.ShouldNotBeNull();
			createDateUtc.DbName.ShouldBe("createDateUtc");
			createDateUtc.SemanticName.ShouldBe("createDateUtc");
			createDateUtc.IsNumbered.ShouldBe(false);
			createDateUtc.DbType.ShouldBe(PostgresDbType.Timestamp);
			createDateUtc.Length.ShouldBeNull();
			createDateUtc.DefaultValue.ShouldBeNull();
		}

		[Fact]
		public void GetUndeclaredParametersTest_Insert_3()
		{
			string query = QueryInsert.InsertStringGuidDateWithOptions;
			var queryParser = new PostgresQueryParser();
			IQueryParamInfo[] undeclaredParameters = queryParser.GetUndeclaredParameters(query, ConnectionString).ToArray();

			undeclaredParameters.ShouldNotBeNull();
			undeclaredParameters.Length.ShouldBe(3);

			IQueryParamInfo userKey = undeclaredParameters[0];
			userKey.ShouldNotBeNull();
			userKey.DbName.ShouldBe("userKey");
			userKey.SemanticName.ShouldBe("userKey");
			userKey.IsNumbered.ShouldBe(false);
			userKey.DbType.ShouldBe(PostgresDbType.Varchar);
			userKey.Length.ShouldBeNull();
			userKey.DefaultValue.ShouldBeNull();

			IQueryParamInfo caseId = undeclaredParameters[1];
			caseId.ShouldNotBeNull();
			caseId.DbName.ShouldBe("caseId");
			caseId.SemanticName.ShouldBe("caseId");
			caseId.IsNumbered.ShouldBe(false);
			caseId.DbType.ShouldBe(PostgresDbType.Guid);
			caseId.Length.ShouldBeNull();
			caseId.DefaultValue.ShouldBeNull();

			IQueryParamInfo createDateUtc = undeclaredParameters[2];
			createDateUtc.ShouldNotBeNull();
			createDateUtc.DbName.ShouldBe("createDateUtc");
			createDateUtc.SemanticName.ShouldBe("createDateUtc");
			createDateUtc.IsNumbered.ShouldBe(false);
			createDateUtc.DbType.ShouldBe(PostgresDbType.Timestamp);
			createDateUtc.Length.ShouldBeNull();
			createDateUtc.DefaultValue.ShouldBeNull();
		}
	}
}