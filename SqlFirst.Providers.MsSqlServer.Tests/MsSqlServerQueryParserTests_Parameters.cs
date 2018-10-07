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
		public void GetDeclaredParametersTest_Select_1()
		{
			string query = QuerySelect.SelectGuidAndDateWithPagingAssignmentAndComments;
			
			var queryParser = new MsSqlServerQueryParser();
			IQueryParamInfo[] declaredParameters = queryParser.GetDeclaredParameters(query, ConnectionString).ToArray();

			declaredParameters.ShouldNotBeNull();
			declaredParameters.Length.ShouldBe(3);

			IQueryParamInfo email = declaredParameters[0];
			email.ShouldNotBeNull();
			email.DbName.ShouldBe("email");
			email.DbType.ShouldBe(MsSqlDbType.VarChar);
			email.Length.ShouldBe("255");
			email.DefaultValue.ShouldBe("test@mail.com");

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
		public void GetDeclaredParametersTest_Select_2()
		{
			string query = QuerySelect.SelectDateWithNamedOrdinal;
			var queryParser = new MsSqlServerQueryParser();
			IQueryParamInfo[] declaredParameters = queryParser.GetDeclaredParameters(query, ConnectionString).ToArray();

			declaredParameters.ShouldNotBeNull();
			declaredParameters.ShouldBeEmpty();
		}

		[Fact]
		public void GetDeclaredParametersTest_Select_3()
		{
			string query = QuerySelect.SelectGuidAndDateWithPagingAndPartOfParameters;
			var queryParser = new MsSqlServerQueryParser();
			IQueryParamInfo[] declaredParameters = queryParser.GetDeclaredParameters(query, ConnectionString).ToArray();

			declaredParameters.ShouldNotBeNull();
			declaredParameters.Length.ShouldBe(2);

			IQueryParamInfo email = declaredParameters[0];
			email.ShouldNotBeNull();
			email.DbName.ShouldBe("email");
			email.DbType.ShouldBe(MsSqlDbType.VarChar);
			email.Length.ShouldBe("MAX");
			email.DefaultValue.ShouldBe("test@mail.com");

			IQueryParamInfo skip = declaredParameters[1];
			skip.ShouldNotBeNull();
			skip.DbName.ShouldBe("skip");
			skip.DbType.ShouldBe(MsSqlDbType.Int);
			skip.Length.ShouldBeNull();
			skip.DefaultValue.ShouldBe(42);
		}

		[Fact]
		public void GetDeclaredParametersTest_Select_4()
		{
			string query = QuerySelect.SelectGuidAndDateWithMultipleSections;
			var queryParser = new MsSqlServerQueryParser();
			IQueryParamInfo[] declaredParameters = queryParser.GetDeclaredParameters(query, ConnectionString).ToArray();

			declaredParameters.ShouldNotBeNull();
			declaredParameters.Length.ShouldBe(2);

			IQueryParamInfo email = declaredParameters[0];
			email.ShouldNotBeNull();
			email.DbName.ShouldBe("email");
			email.DbType.ShouldBe(MsSqlDbType.VarChar);
			email.Length.ShouldBe("MAX");
			email.DefaultValue.ShouldBe("test@mail.com");

			IQueryParamInfo skip = declaredParameters[1];
			skip.ShouldNotBeNull();
			skip.DbName.ShouldBe("take");
			skip.DbType.ShouldBe(MsSqlDbType.Int);
			skip.Length.ShouldBeNull();
			skip.DefaultValue.ShouldBe(42);
		}

		[Fact]
		public void GetUndeclaredParametersTest_Select_1()
		{
			string query = QuerySelect.SelectGuidAndDateWithPaging;
			var queryParser = new MsSqlServerQueryParser();
			IQueryParamInfo[] undeclaredParameters = queryParser.GetUndeclaredParameters(query, ConnectionString).ToArray();

			undeclaredParameters.ShouldNotBeNull();
			undeclaredParameters.ShouldBeEmpty();
		}

		[Fact]
		public void GetUndeclaredParametersTest_Select_2()
		{
			string query = QuerySelect.SelectGuidAndDateWithPagingNoParameters;
			var queryParser = new MsSqlServerQueryParser();
			IQueryParamInfo[] undeclaredParameters = queryParser.GetUndeclaredParameters(query, ConnectionString).ToArray();

			undeclaredParameters.ShouldNotBeNull();
			undeclaredParameters.Length.ShouldBe(3);

			IQueryParamInfo email = undeclaredParameters[0];
			email.ShouldNotBeNull();
			email.DbName.ShouldBe("email");
			email.DbType.ShouldBe(MsSqlDbType.NVarChar);
			email.Length.ShouldBe("50");
			email.DefaultValue.ShouldBeNull();

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
		public void GetUndeclaredParametersTest_Select_3()
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
		public void GetUndeclaredParametersTest_Select_4()
		{
			string query = QuerySelect.SelectGuidAndDateWithMultipleSections;
			var queryParser = new MsSqlServerQueryParser();
			IQueryParamInfo[] undeclaredParameters = queryParser.GetUndeclaredParameters(query, ConnectionString).ToArray();

			undeclaredParameters.ShouldNotBeNull();
			undeclaredParameters.Length.ShouldBe(1);

			IQueryParamInfo take = undeclaredParameters[0];
			take.ShouldNotBeNull();
			take.DbName.ShouldBe("skip");
			take.DbType.ShouldBe(MsSqlDbType.Bigint);
			take.Length.ShouldBeNull();
			take.DefaultValue.ShouldBeNull();
		}

		[Fact]
		public void GetUndeclaredParametersTest_Insert_1()
		{
			string query = QueryInsert.InsertStringGuidDate;
			var queryParser = new MsSqlServerQueryParser();
			IQueryParamInfo[] undeclaredParameters = queryParser.GetUndeclaredParameters(query, ConnectionString).ToArray();

			undeclaredParameters.ShouldNotBeNull();
			undeclaredParameters.ShouldBeEmpty();
		}

		[Fact]
		public void GetUndeclaredParametersTest_Insert_2()
		{
			string query = QueryInsert.InsertStringGuidDateUndeclared;
			var queryParser = new MsSqlServerQueryParser();
			IQueryParamInfo[] undeclaredParameters = queryParser.GetUndeclaredParameters(query, ConnectionString).ToArray();

			undeclaredParameters.ShouldNotBeNull();
			undeclaredParameters.Length.ShouldBe(3);
		}
	}
}