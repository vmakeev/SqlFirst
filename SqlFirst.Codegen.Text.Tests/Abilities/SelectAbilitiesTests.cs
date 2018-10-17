using System;
using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using MySpecificDatabaseTypes;
using Shouldly;
using SqlFirst.Codegen.Text.QueryObject.Abilities;
using SqlFirst.Codegen.Text.QueryObject.Abilities.Select;
using SqlFirst.Codegen.Text.QueryObject.Data;
using SqlFirst.Codegen.Text.Tests.Fixtures;
using SqlFirst.Core;
using Xunit;

namespace SqlFirst.Codegen.Text.Tests.Abilities
{
	// todo: add 'AddParameter' check for custom types. Also check dependencies when no input parameters present
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

			ability.GetDependencies(context).ShouldNotBeNull();
			ability.GetDependencies(context).Count().ShouldBe(3);
			ability.GetDependencies(context).ShouldContain(KnownAbilityName.GetQueryText);
			ability.GetDependencies(context).ShouldContain(KnownAbilityName.AddParameter);
			ability.GetDependencies(context).ShouldContain(KnownAbilityName.GetItemFromRecord);

			result.Usings.ShouldNotBeNull();
			result.Usings.Count().ShouldBe(3);
			result.Usings.ShouldContain("System");
			result.Usings.ShouldContain("System.Data");
			result.Usings.ShouldContain("MySpecificDatabaseTypes");

			result.Methods.ShouldNotBeNull();
			result.Methods.Count().ShouldBe(1);
			result.Methods.ShouldContain(AbilityArtifacts.Select.SelectFirstItemAbility_Method_GetFirst);
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

			ability.GetDependencies(context).ShouldNotBeNull();
			ability.GetDependencies(context).Count().ShouldBe(3);
			ability.GetDependencies(context).ShouldContain(KnownAbilityName.GetQueryText);
			ability.GetDependencies(context).ShouldContain(KnownAbilityName.AddParameter);
			ability.GetDependencies(context).ShouldContain(KnownAbilityName.GetItemFromRecord);

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
			result.Methods.ShouldContain(AbilityArtifacts.Select.SelectFirstItemAsyncAbility_Method_GetFirstAsync);
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

			ability.GetDependencies(context).ShouldNotBeNull();
			ability.GetDependencies(context).Count().ShouldBe(4);
			ability.GetDependencies(context).ShouldContain(KnownAbilityName.GetQueryText);
			ability.GetDependencies(context).ShouldContain(KnownAbilityName.AddParameter);
			ability.GetDependencies(context).ShouldContain(KnownAbilityName.GetItemFromRecord);
			ability.GetDependencies(context).ShouldContain(KnownAbilityName.AsyncEnumerable);

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
			result.Methods.ShouldContain(AbilityArtifacts.Select.SelectItemsIEnumerableAsyncNestedEnumerableAbility_Method_GetAsync);
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
			result.Methods.ShouldContain(AbilityArtifacts.Select.SelectItemsLazyAbility_Method_Get);
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

			ability.GetDependencies(context).ShouldNotBeNull();
			ability.GetDependencies(context).Count().ShouldBe(3);
			ability.GetDependencies(context).ShouldContain(KnownAbilityName.GetQueryText);
			ability.GetDependencies(context).ShouldContain(KnownAbilityName.AddParameter);
			ability.GetDependencies(context).ShouldContain(KnownAbilityName.GetScalarFromValue);

			result.Usings.ShouldNotBeNull();
			result.Usings.Count().ShouldBe(3);
			result.Usings.ShouldContain("System");
			result.Usings.ShouldContain("System.Data");
			result.Usings.ShouldContain("MySpecificDatabaseTypes");

			result.Methods.ShouldNotBeNull();
			result.Methods.Count().ShouldBe(1);
			result.Methods.ShouldContain(AbilityArtifacts.Select.SelectScalarAbility_Method_GetFirst);
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

			ability.GetDependencies(context).ShouldNotBeNull();
			ability.GetDependencies(context).Count().ShouldBe(3);
			ability.GetDependencies(context).ShouldContain(KnownAbilityName.GetQueryText);
			ability.GetDependencies(context).ShouldContain(KnownAbilityName.AddParameter);
			ability.GetDependencies(context).ShouldContain(KnownAbilityName.GetScalarFromValue);

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
			result.Methods.ShouldContain(AbilityArtifacts.Select.SelectScalarAsyncAbility_Method_GetFirstAsync);
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

			ability.GetDependencies(context).ShouldNotBeNull();
			ability.GetDependencies(context).Count().ShouldBe(3);
			ability.GetDependencies(context).ShouldContain(KnownAbilityName.GetQueryText);
			ability.GetDependencies(context).ShouldContain(KnownAbilityName.AddParameter);
			ability.GetDependencies(context).ShouldContain(KnownAbilityName.GetScalarFromRecord);

			result.Usings.ShouldNotBeNull();
			result.Usings.Count().ShouldBe(4);
			result.Usings.ShouldContain("System");
			result.Usings.ShouldContain("System.Collections.Generic");
			result.Usings.ShouldContain("System.Data");
			result.Usings.ShouldContain("MySpecificDatabaseTypes");

			result.Methods.ShouldNotBeNull();
			result.Methods.Count().ShouldBe(1);
			result.Methods.ShouldContain(AbilityArtifacts.Select.SelectScalarsAbility_Method_Get);
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

			ability.GetDependencies(context).ShouldNotBeNull();
			ability.GetDependencies(context).Count().ShouldBe(4);
			ability.GetDependencies(context).ShouldContain(KnownAbilityName.GetQueryText);
			ability.GetDependencies(context).ShouldContain(KnownAbilityName.AddParameter);
			ability.GetDependencies(context).ShouldContain(KnownAbilityName.GetScalarFromRecord);
			ability.GetDependencies(context).ShouldContain(KnownAbilityName.AsyncEnumerable);

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
			result.Methods.ShouldContain(AbilityArtifacts.Select.SelectScalarsIEnumerableAsyncNestedEnumerableAbility_Method_GetAsync);
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
			A.CallTo(() => mapper.MapToClrType("int", false, A<IDictionary<string, object>>._)).Returns(typeof(int));
			A.CallTo(() => mapper.MapToClrType("date", false, A<IDictionary<string, object>>._)).Returns(typeof(DateTime));
			A.CallTo(() => mapper.MapToProviderSpecificType("uniqueidentifier", A<IDictionary<string, object>>._)).Returns(GetProviderSpecificType("MySpecificGuidType"));
			A.CallTo(() => mapper.MapToProviderSpecificType("int", A<IDictionary<string, object>>._)).Returns(GetProviderSpecificType("MySpecificIntType"));
			A.CallTo(() => mapper.MapToProviderSpecificType("date", A<IDictionary<string, object>>._)).Returns(GetProviderSpecificType("MySpecificDateType"));

			var firstParameter = A.Fake<IQueryParamInfo>(p => p.Strict());
			A.CallTo(() => firstParameter.DbName).Returns("FirstParam");
			A.CallTo(() => firstParameter.DbType).Returns("uniqueidentifier");
			A.CallTo(() => firstParameter.DbTypeMetadata).Returns(null);
			A.CallTo(() => firstParameter.IsComplexType).Returns(false);

			var secondParameter = A.Fake<IQueryParamInfo>(p => p.Strict());
			A.CallTo(() => secondParameter.DbName).Returns("SECOND_Param");
			A.CallTo(() => secondParameter.DbType).Returns("int");
			A.CallTo(() => secondParameter.DbTypeMetadata).Returns(null);
			A.CallTo(() => secondParameter.IsComplexType).Returns(false);

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