using System.Linq;
using SqlFirst.Core.Parsing;
using SqlFirst.Providers.MsSqlServer.Tests.Queries;
using Xunit;
using Xunit.Should;

namespace SqlFirst.Providers.MsSqlServer.Tests
{
	public class MsSqlServerQueryParamsParserTests
	{
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
			userKey.Length.ShouldBe(255);
			userKey.DefaultValue.ShouldBe("test");

			IQueryParamInfo skip = declaredParameters[1];
			skip.ShouldNotBeNull();
			skip.DbName.ShouldBe("skip");
			skip.DbType.ShouldBe(MsSqlDbType.Int);
			skip.Length.ShouldBe(0);
			skip.DefaultValue.ShouldBe(42);

			IQueryParamInfo take = declaredParameters[2];
			take.ShouldNotBeNull();
			take.DbName.ShouldBe("take");
			take.DbType.ShouldBe(MsSqlDbType.Int);
			take.Length.ShouldBe(0);
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
	}
}