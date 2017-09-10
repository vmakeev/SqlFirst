using System.Linq;
using SqlFirst.Core.Parsing;
using SqlFirst.Providers.MsSqlServer.Tests.Queries;
using Xunit;
using Xunit.Should;

namespace SqlFirst.Providers.MsSqlServer.Tests
{
	public class MsSqlServerQueryParamsParserTests
	{
		private const string CONNECTION_STRING = @"Server=api-dev;Database=CasebookApi.Arbitrage.Tracking_dev;Integrated Security=SSPI;";

		[Fact]
		public void GetDeclaredParametersTest_1()
		{
			string query = Query.SelectGuidAndDateWithPagingAssignmentAndComments;
			var paramsParser = new MsSqlServerQueryParamsParser();
			IQueryParamInfo[] declaredParameters = paramsParser.GetDeclaredParameters(query).ToArray();

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
			var paramsParser = new MsSqlServerQueryParamsParser();
			IQueryParamInfo[] declaredParameters = paramsParser.GetDeclaredParameters(query).ToArray();

			declaredParameters.ShouldNotBeNull();
			declaredParameters.ShouldBeEmpty();
		}

		[Fact]
		public void GetDeclaredParametersTest_3()
		{
			string query = Query.SelectGuidAndDateWithPagingAndPartOfParameters;
			var paramsParser = new MsSqlServerQueryParamsParser();
			IQueryParamInfo[] declaredParameters = paramsParser.GetDeclaredParameters(query).ToArray();

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
			var paramsParser = new MsSqlServerQueryParamsParser();
			IQueryParamInfo[] undeclaredParameters = paramsParser.GetUndeclaredParameters(query, CONNECTION_STRING).ToArray();

			undeclaredParameters.ShouldNotBeNull();
			undeclaredParameters.ShouldBeEmpty();
		}

		[Fact]
		public void GetUndeclaredParametersTest_2()
		{
			string query = Query.SelectGuidAndDateWithPagingNoParameters;
			var paramsParser = new MsSqlServerQueryParamsParser();
			IQueryParamInfo[] undeclaredParameters = paramsParser.GetUndeclaredParameters(query, CONNECTION_STRING).ToArray();

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
			var paramsParser = new MsSqlServerQueryParamsParser();
			IQueryParamInfo[] undeclaredParameters = paramsParser.GetUndeclaredParameters(query, CONNECTION_STRING).ToArray();

			undeclaredParameters.ShouldNotBeNull();
			undeclaredParameters.Length.ShouldBe(1);
			
			IQueryParamInfo take = undeclaredParameters[0];
			take.ShouldNotBeNull();
			take.DbName.ShouldBe("take");
			take.DbType.ShouldBe(MsSqlDbType.Bigint);
			take.Length.ShouldBeNull();
			take.DefaultValue.ShouldBeNull();
		}
	}
}