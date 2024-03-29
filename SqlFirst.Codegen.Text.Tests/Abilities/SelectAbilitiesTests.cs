﻿using System;
using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using MySpecificDatabaseTypes;
using Shouldly;
using SqlFirst.Codegen.Text.QueryObject.Abilities;
using SqlFirst.Codegen.Text.QueryObject.Abilities.Select;
using SqlFirst.Codegen.Text.QueryObject.Data;
using SqlFirst.Core;
using Xunit;

namespace SqlFirst.Codegen.Text.Tests.Abilities
{
	public class SelectAbilitiesTests
	{
		[Fact]
		public void SelectFirstItemAbility_Test()
		{
			ICodeGenerationContext context = GetDefaultCodeGenerationContext();

			var data = A.Dummy<IQueryObjectData>();

			var ability = new SelectFirstItemAbility();
			IQueryObjectData result = ability.Apply(context, data);

			ability.Name.ShouldBe("GetFirstItem");

			result.Constants.ShouldBeEmpty();
			result.Nested.ShouldBeEmpty();
			result.Properties.ShouldBeEmpty();
			result.Fields.ShouldBeEmpty();

			ability.GetDependencies().ShouldNotBeNull();
			ability.GetDependencies().Count().ShouldBe(4);
			ability.GetDependencies().ShouldContain(KnownAbilityName.GetQueryText);
			ability.GetDependencies().ShouldContain(KnownAbilityName.AddParameter);
			ability.GetDependencies().ShouldContain(KnownAbilityName.GetItemFromRecord);
			ability.GetDependencies().ShouldContain(KnownAbilityName.PrepareCommand);

			result.Usings.ShouldNotBeNull();
			result.Usings.Count().ShouldBe(3);
			result.Usings.ShouldContain("System");
			result.Usings.ShouldContain("System.Data");
			result.Usings.ShouldContain("MySpecificDatabaseTypes");

			result.Methods.ShouldNotBeNull();
			result.Methods.Count().ShouldBe(1);
			result.Methods.ShouldContain(@"/// <summary>
/// Выполняет загрузку первого элемента типа <see cref=""QueryItemTestName""/>
/// </summary>
/// <param name=""connection"">Подключение к БД</param>
/// <param name=""firstParam"">FirstParam</param>
/// <param name=""secondParam"">SECOND_Param</param>
/// <returns>Первый элемент типа <see cref=""QueryItemTestName""/></returns>
public virtual QueryItemTestName GetFirst(IDbConnection connection, Guid? firstParam, int? secondParam)
{
	using(IDbCommand cmd = connection.CreateCommand())
	{
		cmd.CommandText = GetQueryText();
		AddParameter(cmd, MySpecificDbType.MySpecificGuidType, ""@FirstParam"", firstParam);
		AddParameter(cmd, MySpecificDbType.MySpecificIntType, ""@SECOND_Param"", secondParam);

		PrepareCommand(cmd);

		using (IDataReader reader = cmd.ExecuteReader())
		{
			if (!reader.Read())
			{
				return null;
			}

			return GetItemFromRecord(reader);
		}
	}
}");
		}

		[Fact]
		public void SelectFirstItemAsyncAbility_Test()
		{
			ICodeGenerationContext context = GetDefaultCodeGenerationContext();

			var data = A.Dummy<IQueryObjectData>();

			var ability = new SelectFirstItemAsyncAbility();
			IQueryObjectData result = ability.Apply(context, data);

			ability.Name.ShouldBe("GetFirstItemAsync");

			result.Constants.ShouldBeEmpty();
			result.Nested.ShouldBeEmpty();
			result.Properties.ShouldBeEmpty();
			result.Fields.ShouldBeEmpty();

			ability.GetDependencies().ShouldNotBeNull();
			ability.GetDependencies().Count().ShouldBe(4);
			ability.GetDependencies().ShouldContain(KnownAbilityName.GetQueryText);
			ability.GetDependencies().ShouldContain(KnownAbilityName.AddParameter);
			ability.GetDependencies().ShouldContain(KnownAbilityName.GetItemFromRecord);
			ability.GetDependencies().ShouldContain(KnownAbilityName.PrepareCommand);

			result.Usings.ShouldNotBeNull();
			result.Usings.Count().ShouldBe(6);
			result.Usings.ShouldContain("System");
			result.Usings.ShouldContain("System.Data");
			result.Usings.ShouldContain("System.Data.Common");
			result.Usings.ShouldContain("System.Threading");
			result.Usings.ShouldContain("System.Threading.Tasks");
			result.Usings.ShouldContain("MySpecificDatabaseTypes");

			result.Methods.ShouldNotBeNull();
			result.Methods.Count().ShouldBe(1);
			result.Methods.ShouldContain(@"/// <summary>
/// Выполняет загрузку первого элемента типа <see cref=""QueryItemTestName""/>
/// </summary>
/// <param name=""connection"">Подключение к БД</param>
/// <param name=""firstParam"">FirstParam</param>
/// <param name=""secondParam"">SECOND_Param</param>
/// <param name=""cancellationToken"">Токен отмены</param>
/// <returns>Первый элемент типа <see cref=""QueryItemTestName""/></returns>
public virtual async Task<QueryItemTestName> GetFirstAsync(DbConnection connection, Guid? firstParam, int? secondParam, CancellationToken cancellationToken)
{
	using(DbCommand cmd = connection.CreateCommand())
	{
		cmd.CommandText = GetQueryText();
		AddParameter(cmd, MySpecificDbType.MySpecificGuidType, ""@FirstParam"", firstParam);
		AddParameter(cmd, MySpecificDbType.MySpecificIntType, ""@SECOND_Param"", secondParam);

		PrepareCommand(cmd);

		using (DbDataReader reader = await cmd.ExecuteReaderAsync(cancellationToken))
		{
			if (await reader.ReadAsync(cancellationToken) != true)
			{
				return null;
			}

			return GetItemFromRecord(reader);
		}
	}
}");
		}

		[Fact]
		public void SelectItemsIEnumerableAsyncNestedEnumerableAbility_Test()
		{
			ICodeGenerationContext context = GetDefaultCodeGenerationContext();

			var data = A.Dummy<IQueryObjectData>();

			var ability = new SelectItemsIEnumerableAsyncNestedEnumerableAbility();
			IQueryObjectData result = ability.Apply(context, data);

			ability.Name.ShouldBe("GetItemsIEnumerableAsync");

			result.Constants.ShouldBeEmpty();
			result.Nested.ShouldBeEmpty();
			result.Properties.ShouldBeEmpty();
			result.Fields.ShouldBeEmpty();

			ability.GetDependencies().ShouldNotBeNull();
			ability.GetDependencies().Count().ShouldBe(5);
			ability.GetDependencies().ShouldContain(KnownAbilityName.GetQueryText);
			ability.GetDependencies().ShouldContain(KnownAbilityName.AddParameter);
			ability.GetDependencies().ShouldContain(KnownAbilityName.GetItemFromRecord);
			ability.GetDependencies().ShouldContain(KnownAbilityName.AsyncEnumerable);
			ability.GetDependencies().ShouldContain(KnownAbilityName.PrepareCommand);

			result.Usings.ShouldNotBeNull();
			result.Usings.Count().ShouldBe(7);
			result.Usings.ShouldContain("System");
			result.Usings.ShouldContain("System.Data");
			result.Usings.ShouldContain("System.Data.Common");
			result.Usings.ShouldContain("System.Threading");
			result.Usings.ShouldContain("System.Threading.Tasks");
			result.Usings.ShouldContain("System.Collections.Generic");
			result.Usings.ShouldContain("MySpecificDatabaseTypes");

			result.Methods.ShouldNotBeNull();
			result.Methods.Count().ShouldBe(1);
			result.Methods.ShouldContain(@"/// <summary>
/// Выполняет ленивую загрузку списка элементов типа <see cref=""QueryItemTestName""/>
/// </summary>
/// <param name=""connection"">Подключение к БД</param>
/// <param name=""firstParam"">FirstParam</param>
/// <param name=""secondParam"">SECOND_Param</param>
/// <param name=""cancellationToken"">Токен отмены</param>
/// <returns>Список элементов типа <see cref=""QueryItemTestName""/></returns>
public virtual Task<IEnumerable<QueryItemTestName>> GetAsync(DbConnection connection, Guid? firstParam, int? secondParam, CancellationToken cancellationToken)
{
	async Task<IEnumerator<QueryItemTestName>> CreateEnumerator()
	{	
		// Command will be disposed in DbAsyncEnumerator.Dispose() method
		DbCommand cmd = connection.CreateCommand();
		cmd.CommandText = GetQueryText();
		AddParameter(cmd, MySpecificDbType.MySpecificGuidType, ""@FirstParam"", firstParam);
		AddParameter(cmd, MySpecificDbType.MySpecificIntType, ""@SECOND_Param"", secondParam);

		PrepareCommand(cmd);

		DbDataReader reader = await cmd.ExecuteReaderAsync(cancellationToken);
		return new DbAsyncEnumerator<QueryItemTestName>(cmd, reader, GetItemFromRecord, cancellationToken);
	}

	IEnumerable<QueryItemTestName> enumerable = new Enumerable<QueryItemTestName>(async () => await CreateEnumerator());
	return Task.FromResult(enumerable);
}");
		}

		[Fact]
		public void SelectItemsLazyAbility_Test()
		{
			ICodeGenerationContext context = GetDefaultCodeGenerationContext();

			var data = A.Dummy<IQueryObjectData>();

			var ability = new SelectItemsLazyAbility();
			IQueryObjectData result = ability.Apply(context, data);

			ability.Name.ShouldBe("GetItemsLazy");

			result.Constants.ShouldBeEmpty();
			result.Nested.ShouldBeEmpty();
			result.Properties.ShouldBeEmpty();
			result.Fields.ShouldBeEmpty();

			ability.GetDependencies().ShouldNotBeNull();
			ability.GetDependencies().Count().ShouldBe(4);
			ability.GetDependencies().ShouldContain(KnownAbilityName.GetQueryText);
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
			result.Methods.ShouldContain(@"/// <summary>
/// Выполняет ленивую загрузку списка элементов типа <see cref=""QueryItemTestName""/>
/// </summary>
/// <param name=""connection"">Подключение к БД</param>
/// <param name=""firstParam"">FirstParam</param>
/// <param name=""secondParam"">SECOND_Param</param>
/// <returns>Список элементов типа <see cref=""QueryItemTestName""/></returns>
public virtual IEnumerable<QueryItemTestName> Get(IDbConnection connection, Guid? firstParam, int? secondParam)
{
	using (IDbCommand cmd = connection.CreateCommand())
	{
		cmd.CommandText = GetQueryText();
		AddParameter(cmd, MySpecificDbType.MySpecificGuidType, ""@FirstParam"", firstParam);
		AddParameter(cmd, MySpecificDbType.MySpecificIntType, ""@SECOND_Param"", secondParam);

		PrepareCommand(cmd);

		using (IDataReader reader = cmd.ExecuteReader())
		{
			while (reader.Read())
			{
				yield return GetItemFromRecord(reader);
			}
		}
	}
}");
		}

		[Fact]
		public void SelectScalarAbility_Test()
		{
			ICodeGenerationContext context = GetDefaultCodeGenerationContext();

			var data = A.Dummy<IQueryObjectData>();

			var ability = new SelectScalarAbility();
			IQueryObjectData result = ability.Apply(context, data);

			ability.Name.ShouldBe("GetScalar");

			result.Constants.ShouldBeEmpty();
			result.Nested.ShouldBeEmpty();
			result.Properties.ShouldBeEmpty();
			result.Fields.ShouldBeEmpty();

			ability.GetDependencies().ShouldNotBeNull();
			ability.GetDependencies().Count().ShouldBe(4);
			ability.GetDependencies().ShouldContain(KnownAbilityName.GetQueryText);
			ability.GetDependencies().ShouldContain(KnownAbilityName.AddParameter);
			ability.GetDependencies().ShouldContain(KnownAbilityName.GetScalarFromValue);
			ability.GetDependencies().ShouldContain(KnownAbilityName.PrepareCommand);

			result.Usings.ShouldNotBeNull();
			result.Usings.Count().ShouldBe(3);
			result.Usings.ShouldContain("System");
			result.Usings.ShouldContain("System.Data");
			result.Usings.ShouldContain("MySpecificDatabaseTypes");

			result.Methods.ShouldNotBeNull();
			result.Methods.Count().ShouldBe(1);
			result.Methods.ShouldContain(@"/// <summary>
/// Выполняет загрузку значения типа <see cref=""int""/>
/// </summary>
/// <param name=""connection"">Подключение к БД</param>
/// <param name=""firstParam"">FirstParam</param>
/// <param name=""secondParam"">SECOND_Param</param>
/// <returns>Значение типа  <see cref=""int""/></returns>
public virtual int GetFirst(IDbConnection connection, Guid? firstParam, int? secondParam)
{
	using(IDbCommand cmd = connection.CreateCommand())
	{
		cmd.CommandText = GetQueryText();
		AddParameter(cmd, MySpecificDbType.MySpecificGuidType, ""@FirstParam"", firstParam);
		AddParameter(cmd, MySpecificDbType.MySpecificIntType, ""@SECOND_Param"", secondParam);

		PrepareCommand(cmd);

		object value = cmd.ExecuteScalar();
		return GetScalarFromValue<int>(value);
	}
}");
		}

		[Fact]
		public void SelectScalarAsyncAbility_Test()
		{
			ICodeGenerationContext context = GetDefaultCodeGenerationContext();

			var data = A.Dummy<IQueryObjectData>();

			var ability = new SelectScalarAsyncAbility();
			IQueryObjectData result = ability.Apply(context, data);

			ability.Name.ShouldBe("GetScalarAsync");

			result.Constants.ShouldBeEmpty();
			result.Nested.ShouldBeEmpty();
			result.Properties.ShouldBeEmpty();
			result.Fields.ShouldBeEmpty();

			ability.GetDependencies().ShouldNotBeNull();
			ability.GetDependencies().Count().ShouldBe(4);
			ability.GetDependencies().ShouldContain(KnownAbilityName.GetQueryText);
			ability.GetDependencies().ShouldContain(KnownAbilityName.AddParameter);
			ability.GetDependencies().ShouldContain(KnownAbilityName.GetScalarFromValue);
			ability.GetDependencies().ShouldContain(KnownAbilityName.PrepareCommand);

			result.Usings.ShouldNotBeNull();
			result.Usings.Count().ShouldBe(6);
			result.Usings.ShouldContain("System");
			result.Usings.ShouldContain("System.Data");
			result.Usings.ShouldContain("System.Data.Common");
			result.Usings.ShouldContain("System.Threading");
			result.Usings.ShouldContain("System.Threading.Tasks");
			result.Usings.ShouldContain("MySpecificDatabaseTypes");

			result.Methods.ShouldNotBeNull();
			result.Methods.Count().ShouldBe(1);
			result.Methods.ShouldContain(@"/// <summary>
/// Выполняет загрузку значения типа <see cref=""int""/>
/// </summary>
/// <param name=""connection"">Подключение к БД</param>
/// <param name=""firstParam"">FirstParam</param>
/// <param name=""secondParam"">SECOND_Param</param>
/// <param name=""cancellationToken"">Токен отмены</param>
/// <returns>Значение типа  <see cref=""int""/></returns>
public virtual async Task<int> GetFirstAsync(DbConnection connection, Guid? firstParam, int? secondParam, CancellationToken cancellationToken)
{
	using(DbCommand cmd = connection.CreateCommand())
	{
		cmd.CommandText = GetQueryText();
		AddParameter(cmd, MySpecificDbType.MySpecificGuidType, ""@FirstParam"", firstParam);
		AddParameter(cmd, MySpecificDbType.MySpecificIntType, ""@SECOND_Param"", secondParam);

		PrepareCommand(cmd);

		object value = await cmd.ExecuteScalarAsync(cancellationToken);
		return GetScalarFromValue<int>(value);
	}
}");
		}

		[Fact]
		public void SelectScalarsAbility_Test()
		{
			ICodeGenerationContext context = GetDefaultCodeGenerationContext();

			var data = A.Dummy<IQueryObjectData>();

			var ability = new SelectScalarsAbility();
			IQueryObjectData result = ability.Apply(context, data);

			ability.Name.ShouldBe("GetScalarsIEnumerable");

			result.Constants.ShouldBeEmpty();
			result.Nested.ShouldBeEmpty();
			result.Properties.ShouldBeEmpty();
			result.Fields.ShouldBeEmpty();

			ability.GetDependencies().ShouldNotBeNull();
			ability.GetDependencies().Count().ShouldBe(4);
			ability.GetDependencies().ShouldContain(KnownAbilityName.GetQueryText);
			ability.GetDependencies().ShouldContain(KnownAbilityName.AddParameter);
			ability.GetDependencies().ShouldContain(KnownAbilityName.GetScalarFromRecord);
			ability.GetDependencies().ShouldContain(KnownAbilityName.PrepareCommand);

			result.Usings.ShouldNotBeNull();
			result.Usings.Count().ShouldBe(4);
			result.Usings.ShouldContain("System");
			result.Usings.ShouldContain("System.Collections.Generic");
			result.Usings.ShouldContain("System.Data");
			result.Usings.ShouldContain("MySpecificDatabaseTypes");

			result.Methods.ShouldNotBeNull();
			result.Methods.Count().ShouldBe(1);
			result.Methods.ShouldContain(@"/// <summary>
/// Выполняет загрузку значения типа <see cref=""int""/>
/// </summary>
/// <param name=""connection"">Подключение к БД</param>
/// <param name=""firstParam"">FirstParam</param>
/// <param name=""secondParam"">SECOND_Param</param>
/// <returns>Значение типа  <see cref=""int""/></returns>
public virtual IEnumerable<int> Get(IDbConnection connection, Guid? firstParam, int? secondParam)
{
	using (IDbCommand cmd = connection.CreateCommand())
	{
		cmd.CommandText = GetQueryText();
		AddParameter(cmd, MySpecificDbType.MySpecificGuidType, ""@FirstParam"", firstParam);
		AddParameter(cmd, MySpecificDbType.MySpecificIntType, ""@SECOND_Param"", secondParam);

		PrepareCommand(cmd);

		using (IDataReader reader = cmd.ExecuteReader())
		{
			while (reader.Read())
			{
				yield return GetScalarFromRecord<int>(reader);
			}
		}
	}
}");
		}

		[Fact]
		public void SelectScalarsIEnumerableAsyncNestedEnumerableAbility_Test()
		{
			ICodeGenerationContext context = GetDefaultCodeGenerationContext();

			var data = A.Dummy<IQueryObjectData>();

			var ability = new SelectScalarsIEnumerableAsyncNestedEnumerableAbility();
			IQueryObjectData result = ability.Apply(context, data);

			ability.Name.ShouldBe("GetScalarsIEnumerableAsync");

			result.Constants.ShouldBeEmpty();
			result.Nested.ShouldBeEmpty();
			result.Properties.ShouldBeEmpty();
			result.Fields.ShouldBeEmpty();

			ability.GetDependencies().ShouldNotBeNull();
			ability.GetDependencies().Count().ShouldBe(5);
			ability.GetDependencies().ShouldContain(KnownAbilityName.GetQueryText);
			ability.GetDependencies().ShouldContain(KnownAbilityName.AddParameter);
			ability.GetDependencies().ShouldContain(KnownAbilityName.GetScalarFromRecord);
			ability.GetDependencies().ShouldContain(KnownAbilityName.AsyncEnumerable);
			ability.GetDependencies().ShouldContain(KnownAbilityName.PrepareCommand);

			result.Usings.ShouldNotBeNull();
			result.Usings.Count().ShouldBe(7);
			result.Usings.ShouldContain("System");
			result.Usings.ShouldContain("System.Collections.Generic");
			result.Usings.ShouldContain("System.Data");
			result.Usings.ShouldContain("System.Data.Common");
			result.Usings.ShouldContain("System.Threading");
			result.Usings.ShouldContain("System.Threading.Tasks");
			result.Usings.ShouldContain("MySpecificDatabaseTypes");

			result.Methods.ShouldNotBeNull();
			result.Methods.Count().ShouldBe(1);
			result.Methods.ShouldContain(@"/// <summary>
/// Выполняет загрузку значения типа <see cref=""int""/>
/// </summary>
/// <param name=""connection"">Подключение к БД</param>
/// <param name=""firstParam"">FirstParam</param>
/// <param name=""secondParam"">SECOND_Param</param>
/// <param name=""cancellationToken"">Токен отмены</param>
/// <returns>Значение типа  <see cref=""int""/></returns>
public virtual Task<IEnumerable<int>> GetAsync(DbConnection connection, Guid? firstParam, int? secondParam, CancellationToken cancellationToken)
{
	async Task<IEnumerator<int>> CreateEnumerator()
	{	
		// Command will be disposed in DbAsyncEnumerator.Dispose() method
		DbCommand cmd = connection.CreateCommand();
		cmd.CommandText = GetQueryText();
		AddParameter(cmd, MySpecificDbType.MySpecificGuidType, ""@FirstParam"", firstParam);
		AddParameter(cmd, MySpecificDbType.MySpecificIntType, ""@SECOND_Param"", secondParam);

		PrepareCommand(cmd);

		DbDataReader reader = await cmd.ExecuteReaderAsync(cancellationToken);
		return new DbAsyncEnumerator<int>(cmd, reader, GetScalarFromRecord<int>, cancellationToken);
	}

	IEnumerable<int> enumerable = new Enumerable<int>(async () => await CreateEnumerator());
	return Task.FromResult(enumerable);
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
			A.CallTo(() => mapper.MapToClrType("int", false)).Returns(typeof(int));
			A.CallTo(() => mapper.MapToClrType("date", false)).Returns(typeof(DateTime));
			A.CallTo(() => mapper.MapToProviderSpecificType("uniqueidentifier")).Returns(GetProviderSpecificType("MySpecificGuidType"));
			A.CallTo(() => mapper.MapToProviderSpecificType("int")).Returns(GetProviderSpecificType("MySpecificIntType"));
			A.CallTo(() => mapper.MapToProviderSpecificType("date")).Returns(GetProviderSpecificType("MySpecificDateType"));

			var firstParameter = A.Fake<IQueryParamInfo>(p => p.Strict());
			A.CallTo(() => firstParameter.DbName).Returns("FirstParam");
			A.CallTo(() => firstParameter.DbType).Returns("uniqueidentifier");

			var secondParameter = A.Fake<IQueryParamInfo>(p => p.Strict());
			A.CallTo(() => secondParameter.DbName).Returns("SECOND_Param");
			A.CallTo(() => secondParameter.DbType).Returns("int");

			var firstResult = A.Fake<IFieldDetails>(p => p.Strict());
			A.CallTo(() => firstResult.ColumnName).Returns("Id");
			A.CallTo(() => firstResult.AllowDbNull).Returns(false);
			A.CallTo(() => firstResult.DbType).Returns("int");

			var context = A.Fake<ICodeGenerationContext>(p => p.Strict());
			A.CallTo(() => context.TypeMapper).Returns(mapper);
			A.CallTo(() => context.IncomingParameters).Returns(new[] { firstParameter, secondParameter });

			A.CallTo(() => context.OutgoingParameters).Returns(new[] { firstResult });

			var options = A.Fake<IReadOnlyDictionary<string, object>>(p => p.Strict());

			object _;
			A.CallTo(() => options.TryGetValue("QueryResultItemName", out _))
			.Returns(true)
			.AssignsOutAndRefParametersLazily((string a, object b) => new object[] { "QueryItemTestName" });

			A.CallTo(() => context.Options).Returns(options);

			return context;
		}
	}
}