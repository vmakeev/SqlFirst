using System;
using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using Shouldly;
using SqlFirst.Codegen.Text.QueryObject.Abilities;
using SqlFirst.Codegen.Text.QueryObject.Abilities.Insert;
using SqlFirst.Codegen.Text.QueryObject.Data;
using SqlFirst.Core;
using Xunit;

namespace SqlFirst.Codegen.Text.Tests.Abilities
{
	public class InsertMultipleAbilitiesTests
	{
		private static ICodeGenerationContext GetDefaultCodeGenerationContext()
		{
			var mapper = A.Fake<IDatabaseTypeMapper>(p => p.Strict());
			A.CallTo(() => mapper.MapToClrType("uniqueidentifier", true)).Returns(typeof(Guid?));
			A.CallTo(() => mapper.MapToClrType("int", true)).Returns(typeof(int?));
			A.CallTo(() => mapper.MapToClrType("date", false)).Returns(typeof(DateTime));

			var firstParameter = A.Fake<IQueryParamInfo>(p => p.Strict());
			A.CallTo(() => firstParameter.DbName).Returns("FirstParam");
			A.CallTo(() => firstParameter.SemanticName).Returns("FirstParam");
			A.CallTo(() => firstParameter.IsNumbered).Returns(false);
			A.CallTo(() => firstParameter.DbType).Returns("uniqueidentifier");

			var secondParameter = A.Fake<IQueryParamInfo>(p => p.Strict());
			A.CallTo(() => secondParameter.DbName).Returns("SECOND_Param_N");
			A.CallTo(() => secondParameter.SemanticName).Returns("SECOND_Param");
			A.CallTo(() => secondParameter.IsNumbered).Returns(true);
			A.CallTo(() => secondParameter.DbType).Returns("int");

			var thirdParameter = A.Fake<IQueryParamInfo>(p => p.Strict());
			A.CallTo(() => thirdParameter.DbName).Returns("ThirdParam_N");
			A.CallTo(() => thirdParameter.SemanticName).Returns("ThirdParam");
			A.CallTo(() => thirdParameter.IsNumbered).Returns(true);
			A.CallTo(() => thirdParameter.DbType).Returns("date");

			var firstResult = A.Fake<IFieldDetails>(p => p.Strict());
			A.CallTo(() => firstResult.ColumnName).Returns("FirstResult");
			A.CallTo(() => firstResult.DbType).Returns("date");
			A.CallTo(() => firstResult.AllowDbNull).Returns(false);

			var context = A.Fake<ICodeGenerationContext>(p => p.Strict());
			A.CallTo(() => context.TypeMapper).Returns(mapper);
			A.CallTo(() => context.IncomingParameters).Returns(new[] { firstParameter, secondParameter, thirdParameter });
			A.CallTo(() => context.OutgoingParameters).Returns(new[] { firstResult });

			var options = A.Fake<IReadOnlyDictionary<string, object>>(p => p.Strict());

			object _;
			A.CallTo(() => options.TryGetValue("QueryResultItemName", out _))
			.Returns(true)
			.AssignsOutAndRefParametersLazily((string a, object b) => new object[] { "QueryItemTestName" });

			A.CallTo(() => options.TryGetValue("QueryParameterItemName", out _))
			.Returns(true)
			.AssignsOutAndRefParametersLazily((string a, object b) => new object[] { "QueryParamItemTestName" });

			A.CallTo(() => context.Options).Returns(options);
			return context;
		}

		[Fact]
		public void InsertMultipleValuesAbility_Test()
		{
			ICodeGenerationContext context = GetDefaultCodeGenerationContext();

			var data = A.Dummy<IQueryObjectData>();

			var ability = new InsertMultipleValuesAbility();
			IQueryObjectData result = ability.Apply(context, data);

			ability.Name.ShouldBe("InsertMultipleValues");

			result.Constants.ShouldBeEmpty();
			result.Nested.ShouldBeEmpty();
			result.Properties.ShouldBeEmpty();
			result.Fields.ShouldBeEmpty();

			ability.GetDependencies().ShouldNotBeNull();
			ability.GetDependencies().Count().ShouldBe(2);
			ability.GetDependencies().ShouldContain(KnownAbilityName.GetQueryTextMultipleInsert);
			ability.GetDependencies().ShouldContain(KnownAbilityName.AddParameter);

			result.Usings.ShouldNotBeNull();
			result.Usings.Count().ShouldBe(3);
			result.Usings.ShouldContain("System");
			result.Usings.ShouldContain("System.Data");
			result.Usings.ShouldContain("System.Collections.Generic");

			result.Methods.ShouldNotBeNull();
			result.Methods.Count().ShouldBe(1);
			result.Methods.ShouldContain(@"/// <summary>
/// Выполняет добавление строк в таблицу
/// </summary>
/// <param name=""connection"">Подключение к БД</param>
/// <param name=""firstParam"">FirstParam</param>
/// <param name=""items"">Добавляемые записи</param>
/// <returns>Количество добавленных строк</returns>
public virtual int Add(IDbConnection connection, Guid? firstParam, ICollection<QueryParamItemTestName> items)
{
	using(IDbCommand cmd = connection.CreateCommand())
	{
		cmd.CommandText = GetQueryText(items.Count);
		AddParameter(cmd, SqlDbType.UniqueIdentifier, ""@FirstParam"", firstParam);

		int index = 0;
		foreach(QueryParamItemTestName item in items)
		{
			AddParameter(cmd, SqlDbType.Int, string.Format(""@SECOND_Param_{0}"", index), item.SecondParam);
			AddParameter(cmd, SqlDbType.Date, string.Format(""@ThirdParam_{0}"", index), item.ThirdParam);

			index++;
		}

		return cmd.ExecuteNonQuery();
	}
}");
		}

		[Fact]
		public void InsertMultipleValuesAsyncAbility_Test()
		{
			ICodeGenerationContext context = GetDefaultCodeGenerationContext();

			var data = A.Dummy<IQueryObjectData>();

			var ability = new InsertMultipleValuesAsyncAbility();
			IQueryObjectData result = ability.Apply(context, data);

			ability.Name.ShouldBe("InsertMultipleValuesAsync");

			result.Constants.ShouldBeEmpty();
			result.Nested.ShouldBeEmpty();
			result.Properties.ShouldBeEmpty();
			result.Fields.ShouldBeEmpty();

			ability.GetDependencies().ShouldNotBeNull();
			ability.GetDependencies().Count().ShouldBe(2);
			ability.GetDependencies().ShouldContain(KnownAbilityName.GetQueryTextMultipleInsert);
			ability.GetDependencies().ShouldContain(KnownAbilityName.AddParameter);

			result.Usings.ShouldNotBeNull();
			result.Usings.Count().ShouldBe(6);
			result.Usings.ShouldContain("System");
			result.Usings.ShouldContain("System.Data");
			result.Usings.ShouldContain("System.Collections.Generic");
			result.Usings.ShouldContain("System.Data.Common");
			result.Usings.ShouldContain("System.Threading");
			result.Usings.ShouldContain("System.Threading.Tasks");

			result.Methods.ShouldNotBeNull();
			result.Methods.Count().ShouldBe(1);
			result.Methods.ShouldContain(@"/// <summary>
/// Выполняет добавление строк в таблицу
/// </summary>
/// <param name=""connection"">Подключение к БД</param>
/// <param name=""firstParam"">FirstParam</param>
/// <param name=""items"">Добавляемые записи</param>
/// <param name=""cancellationToken"">Токен отмены</param>
/// <returns>Количество добавленных строк</returns>
public virtual async Task<int> AddAsync(DbConnection connection, Guid? firstParam, ICollection<QueryParamItemTestName> items, CancellationToken cancellationToken)
{
	using(DbCommand cmd = connection.CreateCommand())
	{
		cmd.CommandText = GetQueryText(items.Count);
		AddParameter(cmd, SqlDbType.UniqueIdentifier, ""@FirstParam"", firstParam);

		int index = 0;
		foreach(QueryParamItemTestName item in items)
		{
			AddParameter(cmd, SqlDbType.Int, string.Format(""@SECOND_Param_{0}"", index), item.SecondParam);
			AddParameter(cmd, SqlDbType.Date, string.Format(""@ThirdParam_{0}"", index), item.ThirdParam);

			index++;
		}

		return await cmd.ExecuteNonQueryAsync(cancellationToken);
	}
}");
		}

		[Fact]
		public void InsertMultipleValuesWithResultAbility_Test()
		{
			ICodeGenerationContext context = GetDefaultCodeGenerationContext();

			var data = A.Dummy<IQueryObjectData>();

			var ability = new InsertMultipleValuesWithResultAbility();
			IQueryObjectData result = ability.Apply(context, data);

			ability.Name.ShouldBe("InsertMultipleValues");

			result.Constants.ShouldBeEmpty();
			result.Nested.ShouldBeEmpty();
			result.Properties.ShouldBeEmpty();
			result.Fields.ShouldBeEmpty();

			ability.GetDependencies().ShouldNotBeNull();
			ability.GetDependencies().Count().ShouldBe(3);
			ability.GetDependencies().ShouldContain(KnownAbilityName.GetQueryTextMultipleInsert);
			ability.GetDependencies().ShouldContain(KnownAbilityName.AddParameter);
			ability.GetDependencies().ShouldContain(KnownAbilityName.GetItemFromRecord);

			result.Usings.ShouldNotBeNull();
			result.Usings.Count().ShouldBe(3);
			result.Usings.ShouldContain("System");
			result.Usings.ShouldContain("System.Data");
			result.Usings.ShouldContain("System.Collections.Generic");

			result.Methods.ShouldNotBeNull();
			result.Methods.Count().ShouldBe(1);
			result.Methods.ShouldContain(@"/// <summary>
/// Выполняет добавление строк в таблицу и возвращает дополнительные данные
/// </summary>
/// <param name=""connection"">Подключение к БД</param>
/// <param name=""firstParam"">FirstParam</param>
/// <param name=""items"">Добавляемые записи</param>
/// <returns>Результаты выполнения запроса</returns>
public virtual IEnumerable<QueryItemTestName> Add(IDbConnection connection, Guid? firstParam, ICollection<QueryParamItemTestName> items)
{
	using(IDbCommand cmd = connection.CreateCommand())
	{
		cmd.CommandText = GetQueryText(items.Count);
		AddParameter(cmd, SqlDbType.UniqueIdentifier, ""@FirstParam"", firstParam);

		int index = 0;
		foreach(QueryParamItemTestName item in items)
		{
			AddParameter(cmd, SqlDbType.Int, string.Format(""@SECOND_Param_{0}"", index), item.SecondParam);
			AddParameter(cmd, SqlDbType.Date, string.Format(""@ThirdParam_{0}"", index), item.ThirdParam);

			index++;
		}

		var result = new List<QueryItemTestName>();
		using (IDataReader reader = cmd.ExecuteReader())
		{
			while (reader.Read())
			{
				QueryItemTestName resultItem = GetItemFromRecord(reader);
				result.Add(resultItem);
			}
		}

		return result;
	}
}");
		}

		[Fact]
		public void InsertMultipleValuesWithResultAsyncAbility_Test()
		{
			ICodeGenerationContext context = GetDefaultCodeGenerationContext();

			var data = A.Dummy<IQueryObjectData>();

			var ability = new InsertMultipleValuesWithResultAsyncAbility();
			IQueryObjectData result = ability.Apply(context, data);

			ability.Name.ShouldBe("InsertMultipleValuesAsync");

			result.Constants.ShouldBeEmpty();
			result.Nested.ShouldBeEmpty();
			result.Properties.ShouldBeEmpty();
			result.Fields.ShouldBeEmpty();

			ability.GetDependencies().ShouldNotBeNull();
			ability.GetDependencies().Count().ShouldBe(3);
			ability.GetDependencies().ShouldContain(KnownAbilityName.GetQueryTextMultipleInsert);
			ability.GetDependencies().ShouldContain(KnownAbilityName.AddParameter);
			ability.GetDependencies().ShouldContain(KnownAbilityName.GetItemFromRecord);

			result.Usings.ShouldNotBeNull();
			result.Usings.Count().ShouldBe(6);
			result.Usings.ShouldContain("System");
			result.Usings.ShouldContain("System.Data");
			result.Usings.ShouldContain("System.Collections.Generic");
			result.Usings.ShouldContain("System.Data.Common");
			result.Usings.ShouldContain("System.Threading");
			result.Usings.ShouldContain("System.Threading.Tasks");

			result.Methods.ShouldNotBeNull();
			result.Methods.Count().ShouldBe(1);
			result.Methods.ShouldContain(@"/// <summary>
/// Выполняет добавление строк в таблицу и возвращает дополнительные данные
/// </summary>
/// <param name=""connection"">Подключение к БД</param>
/// <param name=""firstParam"">FirstParam</param>
/// <param name=""items"">Добавляемые записи</param>
/// <param name=""cancellationToken"">Токен отмены</param>
/// <returns>Результаты выполнения запроса</returns>
public virtual async Task<IEnumerable<QueryItemTestName>> AddAsync(DbConnection connection, Guid? firstParam, ICollection<QueryParamItemTestName> items, CancellationToken cancellationToken)
{
	using(DbCommand cmd = connection.CreateCommand())
	{
		cmd.CommandText = GetQueryText(items.Count);
		AddParameter(cmd, SqlDbType.UniqueIdentifier, ""@FirstParam"", firstParam);

		int index = 0;
		foreach(QueryParamItemTestName item in items)
		{
			AddParameter(cmd, SqlDbType.Int, string.Format(""@SECOND_Param_{0}"", index), item.SecondParam);
			AddParameter(cmd, SqlDbType.Date, string.Format(""@ThirdParam_{0}"", index), item.ThirdParam);

			index++;
		}

		var result = new List<QueryItemTestName>();
		using (DbDataReader reader = await cmd.ExecuteReaderAsync(cancellationToken))
		{
			while (await reader.ReadAsync(cancellationToken))
			{
				QueryItemTestName resultItem = GetItemFromRecord(reader);
				result.Add(resultItem);
			}
		}

		return result;
	}	
}");
		}

		[Fact]
		public void InsertMultipleValuesWithScalarResultAbility_Test()
		{
			ICodeGenerationContext context = GetDefaultCodeGenerationContext();

			var data = A.Dummy<IQueryObjectData>();

			var ability = new InsertMultipleValuesWithScalarResultAbility();
			IQueryObjectData result = ability.Apply(context, data);

			ability.Name.ShouldBe("InsertMultipleValues");

			result.Constants.ShouldBeEmpty();
			result.Nested.ShouldBeEmpty();
			result.Properties.ShouldBeEmpty();
			result.Fields.ShouldBeEmpty();

			ability.GetDependencies().ShouldNotBeNull();
			ability.GetDependencies().Count().ShouldBe(3);
			ability.GetDependencies().ShouldContain(KnownAbilityName.GetQueryTextMultipleInsert);
			ability.GetDependencies().ShouldContain(KnownAbilityName.AddParameter);
			ability.GetDependencies().ShouldContain(KnownAbilityName.GetScalarFromRecord);

			result.Usings.ShouldNotBeNull();
			result.Usings.Count().ShouldBe(3);
			result.Usings.ShouldContain("System");
			result.Usings.ShouldContain("System.Data");
			result.Usings.ShouldContain("System.Collections.Generic");

			result.Methods.ShouldNotBeNull();
			result.Methods.Count().ShouldBe(1);
			result.Methods.ShouldContain(@"/// <summary>
/// Выполняет добавление строк в таблицу
/// </summary>
/// <param name=""connection"">Подключение к БД</param>
/// <param name=""firstParam"">FirstParam</param>
/// <param name=""items"">Добавляемые записи</param>
/// <returns>FirstResult</returns>
public virtual IEnumerable<DateTime> Add(IDbConnection connection, Guid? firstParam, ICollection<QueryParamItemTestName> items)
{
	using(IDbCommand cmd = connection.CreateCommand())
	{
		cmd.CommandText = GetQueryText(items.Count);
		AddParameter(cmd, SqlDbType.UniqueIdentifier, ""@FirstParam"", firstParam);

		int index = 0;
		foreach(QueryParamItemTestName item in items)
		{
			AddParameter(cmd, SqlDbType.Int, string.Format(""@SECOND_Param_{0}"", index), item.SecondParam);
			AddParameter(cmd, SqlDbType.Date, string.Format(""@ThirdParam_{0}"", index), item.ThirdParam);

			index++;
		}
		
		var result = new List<DateTime>();
		using (IDataReader reader = cmd.ExecuteReader())
		{
			while (reader.Read())
			{
				DateTime resultItem = GetScalarFromRecord<DateTime>(reader);
				result.Add(resultItem);
			}
		}

		return result;
	}
}");
		}

		[Fact]
		public void InsertMultipleValuesWithScalarResultAsyncAbility_Test()
		{
			ICodeGenerationContext context = GetDefaultCodeGenerationContext();

			var data = A.Dummy<IQueryObjectData>();

			var ability = new InsertMultipleValuesWithScalarResultAsyncAbility();
			IQueryObjectData result = ability.Apply(context, data);

			ability.Name.ShouldBe("InsertMultipleValuesAsync");

			result.Constants.ShouldBeEmpty();
			result.Nested.ShouldBeEmpty();
			result.Properties.ShouldBeEmpty();
			result.Fields.ShouldBeEmpty();

			ability.GetDependencies().ShouldNotBeNull();
			ability.GetDependencies().Count().ShouldBe(3);
			ability.GetDependencies().ShouldContain(KnownAbilityName.GetQueryTextMultipleInsert);
			ability.GetDependencies().ShouldContain(KnownAbilityName.AddParameter);
			ability.GetDependencies().ShouldContain(KnownAbilityName.GetScalarFromRecord);

			result.Usings.ShouldNotBeNull();
			result.Usings.Count().ShouldBe(6);
			result.Usings.ShouldContain("System");
			result.Usings.ShouldContain("System.Data");
			result.Usings.ShouldContain("System.Collections.Generic");
			result.Usings.ShouldContain("System.Data.Common");
			result.Usings.ShouldContain("System.Threading");
			result.Usings.ShouldContain("System.Threading.Tasks");

			result.Methods.ShouldNotBeNull();
			result.Methods.Count().ShouldBe(1);
			result.Methods.ShouldContain(@"/// <summary>
/// Выполняет добавление строк в таблицу и возвращает дополнительные данные
/// </summary>
/// <param name=""connection"">Подключение к БД</param>
/// <param name=""firstParam"">FirstParam</param>
/// <param name=""items"">Добавляемые записи</param>
/// <param name=""cancellationToken"">Токен отмены</param>
/// <returns>FirstResult</returns>
public virtual async Task<IEnumerable<DateTime>> AddAsync(DbConnection connection, Guid? firstParam, ICollection<QueryParamItemTestName> items, CancellationToken cancellationToken)
{
	using(DbCommand cmd = connection.CreateCommand())
	{
		cmd.CommandText = GetQueryText(items.Count);
		AddParameter(cmd, SqlDbType.UniqueIdentifier, ""@FirstParam"", firstParam);

		int index = 0;
		foreach(QueryParamItemTestName item in items)
		{
			AddParameter(cmd, SqlDbType.Int, string.Format(""@SECOND_Param_{0}"", index), item.SecondParam);
			AddParameter(cmd, SqlDbType.Date, string.Format(""@ThirdParam_{0}"", index), item.ThirdParam);

			index++;
		}
		
		var result = new List<DateTime>();
		using (DbDataReader reader = await cmd.ExecuteReaderAsync(cancellationToken))
		{
			while (await reader.ReadAsync(cancellationToken))
			{
				DateTime resultItem = GetScalarFromRecord<DateTime>(reader);
				result.Add(resultItem);
			}
		}

		return result;
	}
}");
		}
	}
}