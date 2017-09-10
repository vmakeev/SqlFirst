using System;
using System.Collections.Generic;
using System.Linq;
using SqlFirst.Core;
using SqlFirst.Providers.MsSqlServer.Tests.Queries;
using Xunit;
using Xunit.Should;

namespace SqlFirst.Providers.MsSqlServer.Tests
{
	public class MsSqlServerSchemaFetcherTests
	{
		private const string CONNECTION_STRING = @"Server=api-dev;Database=CasebookApi.Arbitrage.Tracking_dev;Integrated Security=SSPI;";

		[Fact]
		public void GetResultsTest_1()
		{
			string query = Query.SelectGuidAndDateWithPaging;
			ISchemaFetcher schemaFetcher = new MsSqlServerSchemaFetcher();
			List<FieldDetails> results = schemaFetcher.GetResults(CONNECTION_STRING, query);

			results.ShouldNotBeEmpty();
			results.Count.ShouldBe(2);

			FieldDetails caseId = results.First();
			caseId.ColumnName.ShouldBe("CaseId");
			caseId.BaseColumnName.ShouldBe("CaseId");
			caseId.AllowDbNull.ShouldBeFalse();
			caseId.ClrType.ShouldBe(typeof(Guid));
			caseId.DbType.ShouldBe(MsSqlDbType.UniqueIdentifier);
			caseId.IsUnique.ShouldBeFalse();

			FieldDetails createDateUtc = results.Last();
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
			ISchemaFetcher schemaFetcher = new MsSqlServerSchemaFetcher();
			List<FieldDetails> results = schemaFetcher.GetResults(CONNECTION_STRING, query);

			results.ShouldNotBeEmpty();
			results.Count.ShouldBe(1);

			FieldDetails count = results.Single();
			count.ColumnName.ShouldBe("column_0");
			count.BaseColumnName.ShouldBeNull();
			count.ClrType.ShouldBe(typeof(int));
			count.DbType.ShouldBe(MsSqlDbType.Int);
		}

		[Fact]
		public void GetResultsTest_3()
		{
			string query = Query.SelectTwoStringsWithJoin;
			ISchemaFetcher schemaFetcher = new MsSqlServerSchemaFetcher();
			List<FieldDetails> results = schemaFetcher.GetResults(CONNECTION_STRING, query);

			results.ShouldNotBeEmpty();
			results.Count.ShouldBe(2);

			FieldDetails userKey = results.First();
			userKey.ColumnName.ShouldBe("UserKey");
			userKey.BaseColumnName.ShouldBe("UserKey");
			userKey.ClrType.ShouldBe(typeof(string));
			userKey.DbType.ShouldBe(MsSqlDbType.NVarChar);

			FieldDetails shardName = results.Last();
			shardName.ColumnName.ShouldBe("ShardName");
			shardName.BaseColumnName.ShouldBe("ShardName");
			shardName.ClrType.ShouldBe(typeof(string));
			shardName.DbType.ShouldBe(MsSqlDbType.NVarChar);
		}

		[Fact]
		public void GetResultsTest_4()
		{
			string query = Query.SelectDateWithOrdinal;
			ISchemaFetcher schemaFetcher = new MsSqlServerSchemaFetcher();
			List<FieldDetails> results = schemaFetcher.GetResults(CONNECTION_STRING, query);

			results.ShouldNotBeEmpty();
			results.Count.ShouldBe(2);

			FieldDetails createDateUtc = results.First();
			createDateUtc.ColumnName.ShouldBe("CreateDateUtc");
			createDateUtc.BaseColumnName.ShouldBe("CreateDateUtc");
			createDateUtc.AllowDbNull.ShouldBeFalse();
			createDateUtc.ClrType.ShouldBe(typeof(DateTime));
			createDateUtc.DbType.ShouldBe(MsSqlDbType.DateTime);
			createDateUtc.IsUnique.ShouldBeFalse();

			FieldDetails ordinal = results.Last();
			ordinal.ColumnName.ShouldBe("column_1");
			ordinal.BaseColumnName.ShouldBeNull();
			ordinal.ClrType.ShouldBe(typeof(int));
			ordinal.DbType.ShouldBe(MsSqlDbType.Int);
		}

		[Fact]
		public void GetResultsTest_5()
		{
			string query = Query.SelectDateWithNamedOrdinal;
			ISchemaFetcher schemaFetcher = new MsSqlServerSchemaFetcher();
			List<FieldDetails> results = schemaFetcher.GetResults(CONNECTION_STRING, query);

			results.ShouldNotBeEmpty();
			results.Count.ShouldBe(2);

			FieldDetails createDateUtc = results.First();
			createDateUtc.ColumnName.ShouldBe("CreateDateUtc");
			createDateUtc.BaseColumnName.ShouldBe("CreateDateUtc");
			createDateUtc.AllowDbNull.ShouldBeFalse();
			createDateUtc.ClrType.ShouldBe(typeof(DateTime));
			createDateUtc.DbType.ShouldBe(MsSqlDbType.DateTime);
			createDateUtc.IsUnique.ShouldBeFalse();

			FieldDetails ordinal = results.Last();
			ordinal.ColumnName.ShouldBe("answer");
			ordinal.BaseColumnName.ShouldBe("answer");
			ordinal.ClrType.ShouldBe(typeof(int));
			ordinal.DbType.ShouldBe(MsSqlDbType.Int);
		}
	}

}