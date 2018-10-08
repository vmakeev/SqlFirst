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

			IFieldDetails externalId = results[0];
			externalId.ColumnName.ShouldBe("ExternalId");
			externalId.AllowDbNull.ShouldBeFalse();
			externalId.DbType.ShouldBe(MsSqlDbType.UniqueIdentifier);
			externalId.ColumnOrdinal.ShouldBe(0);

			IFieldDetails dateOfBirth = results[1];
			dateOfBirth.ColumnName.ShouldBe("DateOfBirth");
			dateOfBirth.AllowDbNull.ShouldBeTrue();
			dateOfBirth.DbType.ShouldBe(MsSqlDbType.Date);
			dateOfBirth.ColumnOrdinal.ShouldBe(1);
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

			IFieldDetails email = results[0];
			email.ColumnName.ShouldBe("Email");
			email.AllowDbNull.ShouldBeFalse();
			email.DbType.ShouldBe(MsSqlDbType.NVarChar);
			email.ColumnOrdinal.ShouldBe(0);

			IFieldDetails name = results[1];
			name.ColumnName.ShouldBe("Name");
			name.AllowDbNull.ShouldBeTrue(); // left join
			name.DbType.ShouldBe(MsSqlDbType.NVarChar);
			name.ColumnOrdinal.ShouldBe(1);
		}

		[Fact]
		public void GetResultsTest_4()
		{
			string query = QuerySelect.SelectDateWithOrdinal;
			var queryParser = new MsSqlServerQueryParser();
			IFieldDetails[] results = queryParser.GetResultDetails(query, ConnectionString).ToArray();

			results.ShouldNotBeEmpty();
			results.Length.ShouldBe(2);

			IFieldDetails dateOfBirth = results[0];
			dateOfBirth.ColumnName.ShouldBe("DateOfBirth");
			dateOfBirth.AllowDbNull.ShouldBeTrue();
			dateOfBirth.DbType.ShouldBe(MsSqlDbType.Date);
			dateOfBirth.ColumnOrdinal.ShouldBe(0);

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

			IFieldDetails dateOfBirth = results[0];
			dateOfBirth.ColumnName.ShouldBe("DateOfBirth");
			dateOfBirth.AllowDbNull.ShouldBeTrue();
			dateOfBirth.DbType.ShouldBe(MsSqlDbType.Date);
			dateOfBirth.ColumnOrdinal.ShouldBe(0);

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
			id1.DbType.ShouldBe(MsSqlDbType.Bigint);
			id1.ColumnOrdinal.ShouldBe(0);

			IFieldDetails id2 = results[1];
			id2.ColumnName.ShouldBe("Id");
			id2.AllowDbNull.ShouldBeTrue();
			id2.DbType.ShouldBe(MsSqlDbType.Bigint);
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
			subscriptionId.ColumnName.ShouldBe("UserId");
			subscriptionId.AllowDbNull.ShouldBeFalse();
			subscriptionId.DbType.ShouldBe(MsSqlDbType.Bigint);
			subscriptionId.ColumnOrdinal.ShouldBe(0);

			IFieldDetails eventId = results[1];
			eventId.ColumnName.ShouldBe("RoleGroupId");
			eventId.AllowDbNull.ShouldBeFalse();
			eventId.DbType.ShouldBe(MsSqlDbType.Bigint);
			eventId.ColumnOrdinal.ShouldBe(1);
		}

		[Fact]
		public void GetResultsTest_8()
		{
			string query = QuerySelect.SelectTwoStringsWithInnerJoin;
			var queryParser = new MsSqlServerQueryParser();
			IFieldDetails[] results = queryParser.GetResultDetails(query, ConnectionString).ToArray();

			results.ShouldNotBeEmpty();
			results.Length.ShouldBe(2);

			IFieldDetails email = results[0];
			email.ColumnName.ShouldBe("Email");
			email.AllowDbNull.ShouldBeFalse();
			email.DbType.ShouldBe(MsSqlDbType.NVarChar);
			email.ColumnOrdinal.ShouldBe(0);

			IFieldDetails name = results[1];
			name.ColumnName.ShouldBe("Name");
			name.AllowDbNull.ShouldBeFalse(); // inner join
			name.DbType.ShouldBe(MsSqlDbType.NVarChar);
			name.ColumnOrdinal.ShouldBe(1);
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
			id1.DbType.ShouldBe(MsSqlDbType.Bigint);
			id1.ColumnOrdinal.ShouldBe(0);

			IFieldDetails id2 = results[1];
			id2.ColumnName.ShouldBe("Id");
			id2.AllowDbNull.ShouldBeFalse();
			id2.DbType.ShouldBe(MsSqlDbType.Bigint);
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
			id1.DbType.ShouldBe(MsSqlDbType.Bigint);
			id1.ColumnOrdinal.ShouldBe(0);

			IFieldDetails id2 = results[1];
			id2.ColumnName.ShouldBe("Id");
			id2.AllowDbNull.ShouldBeFalse();
			id2.DbType.ShouldBe(MsSqlDbType.Bigint);
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

			IFieldDetails externalId = results[0];
			externalId.ColumnName.ShouldBe("ExternalId");
			externalId.AllowDbNull.ShouldBeFalse();
			externalId.DbType.ShouldBe(MsSqlDbType.UniqueIdentifier);
			externalId.ColumnOrdinal.ShouldBe(0);

			IFieldDetails dateOfBirth = results[1];
			dateOfBirth.ColumnName.ShouldBe("DateOfBirth");
			dateOfBirth.AllowDbNull.ShouldBeTrue();
			dateOfBirth.DbType.ShouldBe(MsSqlDbType.Date);
			dateOfBirth.ColumnOrdinal.ShouldBe(1);
		}

		[Fact]
		public void GetResultsTest_12()
		{
			string query = QuerySelect.SelectGuidAndDateWithPagingNoParameters;
			var queryParser = new MsSqlServerQueryParser();
			IFieldDetails[] results = queryParser.GetResultDetails(query, ConnectionString).ToArray();

			results.ShouldNotBeEmpty();
			results.Length.ShouldBe(2);

			IFieldDetails externalId = results[0];
			externalId.ColumnName.ShouldBe("ExternalId");
			externalId.AllowDbNull.ShouldBeFalse();
			externalId.DbType.ShouldBe(MsSqlDbType.UniqueIdentifier);
			externalId.ColumnOrdinal.ShouldBe(0);

			IFieldDetails dateOfBirth = results[1];
			dateOfBirth.ColumnName.ShouldBe("DateOfBirth");
			dateOfBirth.AllowDbNull.ShouldBeTrue();
			dateOfBirth.DbType.ShouldBe(MsSqlDbType.Date);
			dateOfBirth.ColumnOrdinal.ShouldBe(1);
		}

		[Fact]
		public void GetResultsTest_13()
		{
			string query = QueryExec.ExecNoParameters;
			var queryParser = new MsSqlServerQueryParser();
			IFieldDetails[] results = queryParser.GetResultDetails(query, ConnectionString).ToArray();

			results.ShouldNotBeEmpty();
			results.Length.ShouldBe(3);

			IFieldDetails displayedName = results[0];
			displayedName.ColumnName.ShouldBe("UserName");
			displayedName.AllowDbNull.ShouldBeTrue();
			displayedName.DbType.ShouldBe(MsSqlDbType.NVarChar);
			displayedName.ColumnOrdinal.ShouldBe(0);

			IFieldDetails emailField = results[1];
			emailField.ColumnName.ShouldBe("UserEmail");
			emailField.AllowDbNull.ShouldBeFalse();
			emailField.DbType.ShouldBe(MsSqlDbType.NVarChar);
			emailField.ColumnOrdinal.ShouldBe(1);

			IFieldDetails roleName = results[2];
			roleName.ColumnName.ShouldBe("RoleName");
			roleName.AllowDbNull.ShouldBeTrue();
			roleName.DbType.ShouldBe(MsSqlDbType.NVarChar);
			roleName.ColumnOrdinal.ShouldBe(2);
		}
	}
}