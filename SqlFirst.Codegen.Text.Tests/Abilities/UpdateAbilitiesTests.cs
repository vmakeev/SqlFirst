using System;
using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using MySpecificDatabaseTypes;
using Shouldly;
using SqlFirst.Codegen.Text.QueryObject.Abilities;
using SqlFirst.Codegen.Text.QueryObject.Abilities.Update;
using SqlFirst.Codegen.Text.QueryObject.Data;
using SqlFirst.Core;
using Xunit;

namespace SqlFirst.Codegen.Text.Tests.Abilities
{
	// todo: add 'AddParameter' check for custom types. Also check dependencies when no input parameters present
	public class UpdateAbilitiesTests
	{
		[Fact]
		public void UpdateAbility_Test()
		{
			ICodeGenerationContext context = GetDefaultCodeGenerationContext();

			var data = A.Dummy<IQueryObjectData>();

			var ability = new UpdateAbility();
			IQueryObjectData result = ability.Apply(context, data);

			ability.Name.ShouldBe("Update");

			result.Constants.ShouldBeEmpty();
			result.Nested.ShouldBeEmpty();
			result.Properties.ShouldBeEmpty();
			result.Fields.ShouldBeEmpty();

			ability.GetDependencies(context).ShouldNotBeNull();
			ability.GetDependencies(context).Count().ShouldBe(2);
			ability.GetDependencies(context).ShouldContain(KnownAbilityName.GetQueryText);
			ability.GetDependencies(context).ShouldContain(KnownAbilityName.AddParameter);

			result.Usings.ShouldNotBeNull();
			result.Usings.Count().ShouldBe(3);
			result.Usings.ShouldContain("System");
			result.Usings.ShouldContain("System.Data");
			result.Usings.ShouldContain("MySpecificDatabaseTypes");

			result.Methods.ShouldNotBeNull();
			result.Methods.Count().ShouldBe(1);
			result.Methods.ShouldContain(@"/// <summary>
/// Выполняет обновление строк в таблице
/// </summary>
/// <param name=""connection"">Подключение к БД</param>
/// <param name=""firstParam"">FirstParam</param>
/// <param name=""secondParam"">SECOND_Param</param>
/// <returns>Количество измененных строк</returns>
public virtual int Update(IDbConnection connection, Guid? firstParam, int? secondParam)
{
	using(IDbCommand cmd = connection.CreateCommand())
	{
		cmd.CommandText = GetQueryText();
		AddParameter(cmd, MySpecificDbType.MySpecificGuidType, ""@FirstParam"", firstParam);
		AddParameter(cmd, MySpecificDbType.MySpecificIntType, ""@SECOND_Param"", secondParam);

		return cmd.ExecuteNonQuery();
	}
}");
		}

		[Fact]
		public void UpdateAsyncAbility_Test()
		{
			ICodeGenerationContext context = GetDefaultCodeGenerationContext();

			var data = A.Dummy<IQueryObjectData>();

			var ability = new UpdateAsyncAbility();
			IQueryObjectData result = ability.Apply(context, data);

			ability.Name.ShouldBe("UpdateAsync");

			result.Constants.ShouldBeEmpty();
			result.Nested.ShouldBeEmpty();
			result.Properties.ShouldBeEmpty();
			result.Fields.ShouldBeEmpty();

			ability.GetDependencies(context).ShouldNotBeNull();
			ability.GetDependencies(context).Count().ShouldBe(2);
			ability.GetDependencies(context).ShouldContain(KnownAbilityName.GetQueryText);
			ability.GetDependencies(context).ShouldContain(KnownAbilityName.AddParameter);

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
/// Выполняет обновление строк в таблице
/// </summary>
/// <param name=""connection"">Подключение к БД</param>
/// <param name=""firstParam"">FirstParam</param>
/// <param name=""secondParam"">SECOND_Param</param>
/// <param name=""cancellationToken"">Токен отмены</param>
/// <returns>Количество измененных строк</returns>
public virtual async Task<int> UpdateAsync(DbConnection connection, Guid? firstParam, int? secondParam, CancellationToken cancellationToken)
{
	using(DbCommand cmd = connection.CreateCommand())
	{
		cmd.CommandText = GetQueryText();
		AddParameter(cmd, MySpecificDbType.MySpecificGuidType, ""@FirstParam"", firstParam);
		AddParameter(cmd, MySpecificDbType.MySpecificIntType, ""@SECOND_Param"", secondParam);

		return await cmd.ExecuteNonQueryAsync(cancellationToken);
	}
}");
		}

		[Fact]
		public void UpdateWithResultAbility_Test()
		{
			ICodeGenerationContext context = GetDefaultCodeGenerationContext();

			var data = A.Dummy<IQueryObjectData>();

			var ability = new UpdateWithResultAbility();
			IQueryObjectData result = ability.Apply(context, data);

			ability.Name.ShouldBe("Update");

			result.Constants.ShouldBeEmpty();
			result.Nested.ShouldBeEmpty();
			result.Properties.ShouldBeEmpty();
			result.Fields.ShouldBeEmpty();

			ability.GetDependencies(context).ShouldNotBeNull();
			ability.GetDependencies(context).Count().ShouldBe(3);
			ability.GetDependencies(context).ShouldContain(KnownAbilityName.GetQueryText);
			ability.GetDependencies(context).ShouldContain(KnownAbilityName.AddParameter);
			ability.GetDependencies(context).ShouldContain(KnownAbilityName.GetItemFromRecord);

			result.Usings.ShouldNotBeNull();
			result.Usings.Count().ShouldBe(4);
			result.Usings.ShouldContain("System");
			result.Usings.ShouldContain("System.Data");
			result.Usings.ShouldContain("System.Collections.Generic");
			result.Usings.ShouldContain("MySpecificDatabaseTypes");

			result.Methods.ShouldNotBeNull();
			result.Methods.Count().ShouldBe(1);
			result.Methods.ShouldContain(@"/// <summary>
/// Выполняет обновление строк в таблице и возвращает дополнительные данные
/// </summary>
/// <param name=""connection"">Подключение к БД</param>
/// <param name=""firstParam"">FirstParam</param>
/// <param name=""secondParam"">SECOND_Param</param>
/// <returns>Результаты выполнения запроса</returns>
public virtual IEnumerable<QueryItemTestName> Update(IDbConnection connection, Guid? firstParam, int? secondParam)
{
	using(IDbCommand cmd = connection.CreateCommand())
	{
		cmd.CommandText = GetQueryText();
		AddParameter(cmd, MySpecificDbType.MySpecificGuidType, ""@FirstParam"", firstParam);
		AddParameter(cmd, MySpecificDbType.MySpecificIntType, ""@SECOND_Param"", secondParam);

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
		public void UpdateWithResultAsyncAbility_Test()
		{
			ICodeGenerationContext context = GetDefaultCodeGenerationContext();

			var data = A.Dummy<IQueryObjectData>();

			var ability = new UpdateWithResultAsyncAbility();
			IQueryObjectData result = ability.Apply(context, data);

			ability.Name.ShouldBe("UpdateAsync");

			result.Constants.ShouldBeEmpty();
			result.Nested.ShouldBeEmpty();
			result.Properties.ShouldBeEmpty();
			result.Fields.ShouldBeEmpty();

			ability.GetDependencies(context).ShouldNotBeNull();
			ability.GetDependencies(context).Count().ShouldBe(3);
			ability.GetDependencies(context).ShouldContain(KnownAbilityName.GetQueryText);
			ability.GetDependencies(context).ShouldContain(KnownAbilityName.AddParameter);
			ability.GetDependencies(context).ShouldContain(KnownAbilityName.GetItemFromRecord);

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
/// Выполняет обновление строк в таблице и возвращает дополнительные данные
/// </summary>
/// <param name=""connection"">Подключение к БД</param>
/// <param name=""firstParam"">FirstParam</param>
/// <param name=""secondParam"">SECOND_Param</param>
/// <param name=""cancellationToken"">Токен отмены</param>
/// <returns>Результаты выполнения запроса</returns>
public virtual async Task<IEnumerable<QueryItemTestName>> UpdateAsync(DbConnection connection, Guid? firstParam, int? secondParam, CancellationToken cancellationToken)
{
	using(DbCommand cmd = connection.CreateCommand())
	{
		cmd.CommandText = GetQueryText();
		AddParameter(cmd, MySpecificDbType.MySpecificGuidType, ""@FirstParam"", firstParam);
		AddParameter(cmd, MySpecificDbType.MySpecificIntType, ""@SECOND_Param"", secondParam);

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
		public void UpdateWithScalarResultAbility_Test()
		{
			ICodeGenerationContext context = GetDefaultCodeGenerationContext();

			var data = A.Dummy<IQueryObjectData>();

			var ability = new UpdateWithScalarResultAbility();
			IQueryObjectData result = ability.Apply(context, data);

			ability.Name.ShouldBe("Update");

			result.Constants.ShouldBeEmpty();
			result.Nested.ShouldBeEmpty();
			result.Properties.ShouldBeEmpty();
			result.Fields.ShouldBeEmpty();

			ability.GetDependencies(context).ShouldNotBeNull();
			ability.GetDependencies(context).Count().ShouldBe(3);
			ability.GetDependencies(context).ShouldContain(KnownAbilityName.GetQueryText);
			ability.GetDependencies(context).ShouldContain(KnownAbilityName.AddParameter);
			ability.GetDependencies(context).ShouldContain(KnownAbilityName.GetScalarFromRecord);

			result.Usings.ShouldNotBeNull();
			result.Usings.Count().ShouldBe(4);
			result.Usings.ShouldContain("System");
			result.Usings.ShouldContain("System.Data");
			result.Usings.ShouldContain("System.Collections.Generic");
			result.Usings.ShouldContain("MySpecificDatabaseTypes");

			result.Methods.ShouldNotBeNull();
			result.Methods.Count().ShouldBe(1);
			result.Methods.ShouldContain(@"/// <summary>
/// Выполняет обновление строк в таблице
/// </summary>
/// <param name=""connection"">Подключение к БД</param>
/// <param name=""firstParam"">FirstParam</param>
/// <param name=""secondParam"">SECOND_Param</param>
/// <returns>FirstResult</returns>
public virtual IEnumerable<DateTime> Update(IDbConnection connection, Guid? firstParam, int? secondParam)
{
	using(IDbCommand cmd = connection.CreateCommand())
	{
		cmd.CommandText = GetQueryText();
		AddParameter(cmd, MySpecificDbType.MySpecificGuidType, ""@FirstParam"", firstParam);
		AddParameter(cmd, MySpecificDbType.MySpecificIntType, ""@SECOND_Param"", secondParam);
		
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
		public void UpdateWithScalarResultAsyncAbility_Test()
		{
			ICodeGenerationContext context = GetDefaultCodeGenerationContext();

			var data = A.Dummy<IQueryObjectData>();

			var ability = new UpdateWithScalarResultAsyncAbility();
			IQueryObjectData result = ability.Apply(context, data);

			ability.Name.ShouldBe("UpdateAsync");

			result.Constants.ShouldBeEmpty();
			result.Nested.ShouldBeEmpty();
			result.Properties.ShouldBeEmpty();
			result.Fields.ShouldBeEmpty();

			ability.GetDependencies(context).ShouldNotBeNull();
			ability.GetDependencies(context).Count().ShouldBe(3);
			ability.GetDependencies(context).ShouldContain(KnownAbilityName.GetQueryText);
			ability.GetDependencies(context).ShouldContain(KnownAbilityName.AddParameter);
			ability.GetDependencies(context).ShouldContain(KnownAbilityName.GetScalarFromRecord);

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
/// Выполняет обновление строк в таблице и возвращает дополнительные данные
/// </summary>
/// <param name=""connection"">Подключение к БД</param>
/// <param name=""firstParam"">FirstParam</param>
/// <param name=""secondParam"">SECOND_Param</param>
/// <param name=""cancellationToken"">Токен отмены</param>
/// <returns>FirstResult</returns>
public virtual async Task<IEnumerable<DateTime>> UpdateAsync(DbConnection connection, Guid? firstParam, int? secondParam, CancellationToken cancellationToken)
{
	using(DbCommand cmd = connection.CreateCommand())
	{
		cmd.CommandText = GetQueryText();
		AddParameter(cmd, MySpecificDbType.MySpecificGuidType, ""@FirstParam"", firstParam);
		AddParameter(cmd, MySpecificDbType.MySpecificIntType, ""@SECOND_Param"", secondParam);
		
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
			A.CallTo(() => mapper.MapToClrType("uniqueidentifier", true, A<IDictionary<string, object>>._)).Returns(typeof(Guid?));
			A.CallTo(() => mapper.MapToClrType("int", true, A<IDictionary<string, object>>._)).Returns(typeof(int?));
			A.CallTo(() => mapper.MapToClrType("date", false, A<IDictionary<string, object>>._)).Returns(typeof(DateTime));
			A.CallTo(() => mapper.MapToProviderSpecificType("uniqueidentifier", A<IDictionary<string, object>>._)).Returns(GetProviderSpecificType("MySpecificGuidType"));
			A.CallTo(() => mapper.MapToProviderSpecificType("int", A<IDictionary<string, object>>._)).Returns(GetProviderSpecificType("MySpecificIntType"));
			A.CallTo(() => mapper.MapToProviderSpecificType("date", A<IDictionary<string, object>>._)).Returns(GetProviderSpecificType("MySpecificDateType"));

			var firstParameter = A.Fake<IQueryParamInfo>(p => p.Strict());
			A.CallTo(() => firstParameter.DbName).Returns("FirstParam");
			A.CallTo(() => firstParameter.SemanticName).Returns("FirstParam");
			A.CallTo(() => firstParameter.DbType).Returns("uniqueidentifier");
			A.CallTo(() => firstParameter.DbTypeMetadata).Returns(null);
			A.CallTo(() => firstParameter.IsComplexType).Returns(false);

			var secondParameter = A.Fake<IQueryParamInfo>(p => p.Strict());
			A.CallTo(() => secondParameter.DbName).Returns("SECOND_Param");
			A.CallTo(() => secondParameter.SemanticName).Returns("SECOND_Param");
			A.CallTo(() => secondParameter.DbType).Returns("int");
			A.CallTo(() => secondParameter.DbTypeMetadata).Returns(null);
			A.CallTo(() => secondParameter.IsComplexType).Returns(false);

			var firstResult = A.Fake<IFieldDetails>(p => p.Strict());
			A.CallTo(() => firstResult.ColumnName).Returns("FirstResult");
			A.CallTo(() => firstResult.DbType).Returns("date");
			A.CallTo(() => firstResult.AllowDbNull).Returns(false);

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