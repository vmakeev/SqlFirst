using System;
using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using MySpecificDatabaseTypes;
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
			ability.GetDependencies().Count().ShouldBe(3);
			ability.GetDependencies().ShouldContain(KnownAbilityName.GetQueryTextMultipleInsert);
			ability.GetDependencies().ShouldContain(KnownAbilityName.AddParameter);
			ability.GetDependencies().ShouldContain(KnownAbilityName.PrepareCommand);

			result.Usings.ShouldNotBeNull();
			result.Usings.Count().ShouldBe(4);
			result.Usings.ShouldContain("System");
			result.Usings.ShouldContain("System.Data");
			result.Usings.ShouldContain("System.Collections.Generic");
			result.Usings.ShouldContain("MySpecificDatabaseTypes");

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
		AddParameter(cmd, MySpecificDbType.MySpecificGuidType, ""@FirstParam"", firstParam);

		int index = 0;
		foreach(QueryParamItemTestName item in items)
		{
			AddParameter(cmd, MySpecificDbType.MySpecificIntType, string.Format(""@SECOND_Param_{0}"", index), item.SecondParam);
			AddParameter(cmd, MySpecificDbType.MySpecificDateType, string.Format(""@ThirdParam_{0}"", index), item.ThirdParam);

			index++;
		}

		PrepareCommand(cmd);
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
			ability.GetDependencies().Count().ShouldBe(3);
			ability.GetDependencies().ShouldContain(KnownAbilityName.GetQueryTextMultipleInsert);
			ability.GetDependencies().ShouldContain(KnownAbilityName.AddParameter);
			ability.GetDependencies().ShouldContain(KnownAbilityName.PrepareCommand);

			result.Usings.ShouldNotBeNull();
			result.Usings.Count().ShouldBe(7);
			result.Usings.ShouldContain("System");
			result.Usings.ShouldContain("System.Data");
			result.Usings.ShouldContain("System.Collections.Generic");
			result.Usings.ShouldContain("System.Data.Common");
			result.Usings.ShouldContain("System.Threading");
			result.Usings.ShouldContain("System.Threading.Tasks");
			result.Usings.ShouldContain("MySpecificDatabaseTypes");

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
		AddParameter(cmd, MySpecificDbType.MySpecificGuidType, ""@FirstParam"", firstParam);

		int index = 0;
		foreach(QueryParamItemTestName item in items)
		{
			AddParameter(cmd, MySpecificDbType.MySpecificIntType, string.Format(""@SECOND_Param_{0}"", index), item.SecondParam);
			AddParameter(cmd, MySpecificDbType.MySpecificDateType, string.Format(""@ThirdParam_{0}"", index), item.ThirdParam);

			index++;
		}

		PrepareCommand(cmd);
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
			ability.GetDependencies().Count().ShouldBe(4);
			ability.GetDependencies().ShouldContain(KnownAbilityName.GetQueryTextMultipleInsert);
			ability.GetDependencies().ShouldContain(KnownAbilityName.AddParameter);
			ability.GetDependencies().ShouldContain(KnownAbilityName.GetItemFromRecord);
			ability.GetDependencies().ShouldContain(KnownAbilityName.PrepareCommand);

			result.Usings.ShouldNotBeNull();
			result.Usings.Count().ShouldBe(4);
			result.Usings.ShouldContain("System");
			result.Usings.ShouldContain("System.Data");
			result.Usings.ShouldContain("System.Collections.Generic");
			result.Usings.ShouldContain("MySpecificDatabaseTypes");

			result.Methods.ShouldNotBeNull();
			result.Methods.Count().ShouldBe(1);
			result.Methods.Single().ShouldBe(@"/// <summary>
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
		AddParameter(cmd, MySpecificDbType.MySpecificGuidType, ""@FirstParam"", firstParam);

		int index = 0;
		foreach(QueryParamItemTestName item in items)
		{
			AddParameter(cmd, MySpecificDbType.MySpecificIntType, string.Format(""@SECOND_Param_{0}"", index), item.SecondParam);
			AddParameter(cmd, MySpecificDbType.MySpecificDateType, string.Format(""@ThirdParam_{0}"", index), item.ThirdParam);

			index++;
		}

		PrepareCommand(cmd);

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
			ability.GetDependencies().Count().ShouldBe(4);
			ability.GetDependencies().ShouldContain(KnownAbilityName.GetQueryTextMultipleInsert);
			ability.GetDependencies().ShouldContain(KnownAbilityName.AddParameter);
			ability.GetDependencies().ShouldContain(KnownAbilityName.GetItemFromRecord);
			ability.GetDependencies().ShouldContain(KnownAbilityName.PrepareCommand);

			result.Usings.ShouldNotBeNull();
			result.Usings.Count().ShouldBe(7);
			result.Usings.ShouldContain("System");
			result.Usings.ShouldContain("System.Data");
			result.Usings.ShouldContain("System.Collections.Generic");
			result.Usings.ShouldContain("System.Data.Common");
			result.Usings.ShouldContain("System.Threading");
			result.Usings.ShouldContain("System.Threading.Tasks");
			result.Usings.ShouldContain("MySpecificDatabaseTypes");

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
		AddParameter(cmd, MySpecificDbType.MySpecificGuidType, ""@FirstParam"", firstParam);

		int index = 0;
		foreach(QueryParamItemTestName item in items)
		{
			AddParameter(cmd, MySpecificDbType.MySpecificIntType, string.Format(""@SECOND_Param_{0}"", index), item.SecondParam);
			AddParameter(cmd, MySpecificDbType.MySpecificDateType, string.Format(""@ThirdParam_{0}"", index), item.ThirdParam);

			index++;
		}

		PrepareCommand(cmd);

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
			ability.GetDependencies().Count().ShouldBe(4);
			ability.GetDependencies().ShouldContain(KnownAbilityName.GetQueryTextMultipleInsert);
			ability.GetDependencies().ShouldContain(KnownAbilityName.AddParameter);
			ability.GetDependencies().ShouldContain(KnownAbilityName.GetScalarFromRecord);
			ability.GetDependencies().ShouldContain(KnownAbilityName.PrepareCommand);

			result.Usings.ShouldNotBeNull();
			result.Usings.Count().ShouldBe(4);
			result.Usings.ShouldContain("System");
			result.Usings.ShouldContain("System.Data");
			result.Usings.ShouldContain("System.Collections.Generic");
			result.Usings.ShouldContain("MySpecificDatabaseTypes");

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
		AddParameter(cmd, MySpecificDbType.MySpecificGuidType, ""@FirstParam"", firstParam);

		int index = 0;
		foreach(QueryParamItemTestName item in items)
		{
			AddParameter(cmd, MySpecificDbType.MySpecificIntType, string.Format(""@SECOND_Param_{0}"", index), item.SecondParam);
			AddParameter(cmd, MySpecificDbType.MySpecificDateType, string.Format(""@ThirdParam_{0}"", index), item.ThirdParam);

			index++;
		}

		PrepareCommand(cmd);

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
			ability.GetDependencies().Count().ShouldBe(4);
			ability.GetDependencies().ShouldContain(KnownAbilityName.GetQueryTextMultipleInsert);
			ability.GetDependencies().ShouldContain(KnownAbilityName.AddParameter);
			ability.GetDependencies().ShouldContain(KnownAbilityName.GetScalarFromRecord);
			ability.GetDependencies().ShouldContain(KnownAbilityName.PrepareCommand);

			result.Usings.ShouldNotBeNull();
			result.Usings.Count().ShouldBe(7);
			result.Usings.ShouldContain("System");
			result.Usings.ShouldContain("System.Data");
			result.Usings.ShouldContain("System.Collections.Generic");
			result.Usings.ShouldContain("System.Data.Common");
			result.Usings.ShouldContain("System.Threading");
			result.Usings.ShouldContain("System.Threading.Tasks");
			result.Usings.ShouldContain("MySpecificDatabaseTypes");

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
		AddParameter(cmd, MySpecificDbType.MySpecificGuidType, ""@FirstParam"", firstParam);

		int index = 0;
		foreach(QueryParamItemTestName item in items)
		{
			AddParameter(cmd, MySpecificDbType.MySpecificIntType, string.Format(""@SECOND_Param_{0}"", index), item.SecondParam);
			AddParameter(cmd, MySpecificDbType.MySpecificDateType, string.Format(""@ThirdParam_{0}"", index), item.ThirdParam);

			index++;
		}

		PrepareCommand(cmd);

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

		private static IProviderSpecificType GetProviderSpecificType(string value)
		{
			var result = A.Fake<IProviderSpecificType>(p => p.Strict());
			A.CallTo(() => result.TypeName).Returns(typeof(MySpecificDbType).Name);
			A.CallTo(() => result.ValueName).Returns(value);
			A.CallTo(() => result.Usings).Returns(new[] { typeof(MySpecificDbType).Namespace });
			return result;
		}

		private static ICodeGenerationContext GetDefaultCodeGenerationContext()
		{
			var providerTypesInfo = A.Fake<IProviderTypesInfo>(p => p.Strict());
			A.CallTo(() => providerTypesInfo.CommandParameterSpecificDbTypePropertyType).Returns(typeof(MySpecificDbType));
			A.CallTo(() => providerTypesInfo.CommandParameterType).Returns(typeof(MySpecificParameterType));
			A.CallTo(() => providerTypesInfo.CommandParameterSpecificDbTypePropertyName).Returns("MySpecificDbTypePropertyName");

			var provider = A.Fake<IDatabaseProvider>(p => p.Strict());
			A.CallTo(() => provider.ProviderTypesInfo).Returns(providerTypesInfo);

			var mapper = A.Fake<IDatabaseTypeMapper>(p => p.Strict());
			A.CallTo(() => mapper.MapToClrType("uniqueidentifier", true)).Returns(typeof(Guid?));
			A.CallTo(() => mapper.MapToClrType("int", true)).Returns(typeof(int?));
			A.CallTo(() => mapper.MapToClrType("date", false)).Returns(typeof(DateTime));
			A.CallTo(() => mapper.MapToProviderSpecificType("uniqueidentifier")).Returns(GetProviderSpecificType("MySpecificGuidType"));
			A.CallTo(() => mapper.MapToProviderSpecificType("int")).Returns(GetProviderSpecificType("MySpecificIntType"));
			A.CallTo(() => mapper.MapToProviderSpecificType("date")).Returns(GetProviderSpecificType("MySpecificDateType"));

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
			A.CallTo(() => context.DatabaseProvider).Returns(provider);

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
	}
}