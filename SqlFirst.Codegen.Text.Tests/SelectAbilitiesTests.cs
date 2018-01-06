using System;
using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using Shouldly;
using SqlFirst.Codegen.Text.QueryObject.Abilities;
using SqlFirst.Codegen.Text.QueryObject.Abilities.Select;
using SqlFirst.Codegen.Text.QueryObject.Data;
using SqlFirst.Core;
using Xunit;

namespace SqlFirst.Codegen.Text.Tests
{
	public class SelectAbilitiesTests
	{
		[Fact]
		public void SelectFirstItemAbility_Test()
		{
			var mapper = A.Fake<IDatabaseTypeMapper>(p => p.Strict());
			A.CallTo(() => mapper.Map("uniqueidentifier", true)).Returns(typeof(Guid?));
			A.CallTo(() => mapper.Map("int", true)).Returns(typeof(int?));

			var firstParameter = A.Fake<IQueryParamInfo>(p => p.Strict());
			A.CallTo(() => firstParameter.DbName).Returns("FirstParam");
			A.CallTo(() => firstParameter.DbType).Returns("uniqueidentifier");

			var secondParameter = A.Fake<IQueryParamInfo>(p => p.Strict());
			A.CallTo(() => secondParameter.DbName).Returns("SECOND_Param");
			A.CallTo(() => secondParameter.DbType).Returns("int");

			var context = A.Fake<ICodeGenerationContext>(p => p.Strict());
			A.CallTo(() => context.TypeMapper).Returns(mapper);
			A.CallTo(() => context.IncomingParameters).Returns(new[] { firstParameter, secondParameter });

			var options = A.Fake<IReadOnlyDictionary<string, object>>(p => p.Strict());

			object _;
			A.CallTo(() => options.TryGetValue("QueryResultItemName", out _))
			.Returns(true)
			.AssignsOutAndRefParametersLazily((string a, object b) => new object[] { "QueryItemTestName" });

			A.CallTo(() => context.Options).Returns(options);

			var data = A.Dummy<IQueryObjectData>();

			var ability = new SelectFirstItemAbility();
			IQueryObjectData result = ability.Apply(context, data);

			ability.Name.ShouldBe("GetFirstItem");

			result.Constants.ShouldBeEmpty();
			result.Nested.ShouldBeEmpty();
			result.Properties.ShouldBeEmpty();
			result.Fields.ShouldBeEmpty();

			ability.GetDependencies().ShouldNotBeNull();
			ability.GetDependencies().Count().ShouldBe(3);
			ability.GetDependencies().ShouldContain(KnownAbilityName.GetQueryText);
			ability.GetDependencies().ShouldContain(KnownAbilityName.AddParameter);
			ability.GetDependencies().ShouldContain(KnownAbilityName.GetItemFromRecord);

			result.Usings.ShouldNotBeNull();
			result.Usings.Count().ShouldBe(2);
			result.Usings.ShouldContain("System");
			result.Usings.ShouldContain("System.Data");

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
		AddParameter(cmd, SqlDbType.UniqueIdentifier, ""@FirstParam"", firstParam);
		AddParameter(cmd, SqlDbType.Int, ""@SECOND_Param"", secondParam);

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
		public void SelectItemsIEnumerableAsyncNestedEnumerableAbility_Test()
		{
			var mapper = A.Fake<IDatabaseTypeMapper>(p => p.Strict());
			A.CallTo(() => mapper.Map("uniqueidentifier", true)).Returns(typeof(Guid?));
			A.CallTo(() => mapper.Map("int", true)).Returns(typeof(int?));

			var firstParameter = A.Fake<IQueryParamInfo>(p => p.Strict());
			A.CallTo(() => firstParameter.DbName).Returns("FirstParam");
			A.CallTo(() => firstParameter.DbType).Returns("uniqueidentifier");

			var secondParameter = A.Fake<IQueryParamInfo>(p => p.Strict());
			A.CallTo(() => secondParameter.DbName).Returns("SECOND_Param");
			A.CallTo(() => secondParameter.DbType).Returns("int");

			var context = A.Fake<ICodeGenerationContext>(p => p.Strict());
			A.CallTo(() => context.TypeMapper).Returns(mapper);
			A.CallTo(() => context.IncomingParameters).Returns(new[] { firstParameter, secondParameter });

			var options = A.Fake<IReadOnlyDictionary<string, object>>(p => p.Strict());

			object _;
			A.CallTo(() => options.TryGetValue("QueryResultItemName", out _))
			.Returns(true)
			.AssignsOutAndRefParametersLazily((string a, object b) => new object[] { "QueryItemTestName" });

			A.CallTo(() => context.Options).Returns(options);

			var data = A.Dummy<IQueryObjectData>();

			var ability = new SelectItemsIEnumerableAsyncNestedEnumerableAbility();
			IQueryObjectData result = ability.Apply(context, data);

			ability.Name.ShouldBe("GetItemsIEnumerableAsync");

			result.Constants.ShouldBeEmpty();
			result.Nested.ShouldBeEmpty();
			result.Properties.ShouldBeEmpty();
			result.Fields.ShouldBeEmpty();

			ability.GetDependencies().ShouldNotBeNull();
			ability.GetDependencies().Count().ShouldBe(4);
			ability.GetDependencies().ShouldContain(KnownAbilityName.GetQueryText);
			ability.GetDependencies().ShouldContain(KnownAbilityName.AddParameter);
			ability.GetDependencies().ShouldContain(KnownAbilityName.GetItemFromRecord);
			ability.GetDependencies().ShouldContain(KnownAbilityName.AsyncEnumerable);

			result.Usings.ShouldNotBeNull();
			result.Usings.Count().ShouldBe(6);
			result.Usings.ShouldContain("System");
			result.Usings.ShouldContain("System.Data");
			result.Usings.ShouldContain("System.Data.Common");
			result.Usings.ShouldContain("System.Threading");
			result.Usings.ShouldContain("System.Threading.Tasks");
			result.Usings.ShouldContain("System.Collections.Generic");

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
		AddParameter(cmd, SqlDbType.UniqueIdentifier, ""@FirstParam"", firstParam);
		AddParameter(cmd, SqlDbType.Int, ""@SECOND_Param"", secondParam);

		DbDataReader reader = await cmd.ExecuteReaderAsync(cancellationToken);
		return new DbAsyncEnumerator<QueryItemTestName>(cmd, reader, GetItemFromRecord, cancellationToken);
	}

	IEnumerable<QueryItemTestName> enumerable = new Enumerable<QueryItemTestName>(async () => await CreateEnumerator());
	return Task.FromResult(enumerable);
}");
		}
	}
}