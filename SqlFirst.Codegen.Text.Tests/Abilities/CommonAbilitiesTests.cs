using System;
using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using MySpecificDatabaseTypes;
using Shouldly;
using SqlFirst.Codegen.Text.QueryObject.Abilities;
using SqlFirst.Codegen.Text.QueryObject.Abilities.Common;
using SqlFirst.Codegen.Text.QueryObject.Data;
using SqlFirst.Codegen.Text.Tests.Fixtures;
using SqlFirst.Core;
using Xunit;

namespace SqlFirst.Codegen.Text.Tests.Abilities
{
	public class CommonAbilitiesTests
	{
		private IQueryParamInfo GetQueryParamInfo(bool isComplexType)
		{
			var result = A.Fake<IQueryParamInfo>(p => p.Strict());
			A.CallTo(() => result.IsComplexType).Returns(isComplexType);
			return result;
		}

		[Fact]
		public void AddSqlConnectionParameterAbility_Test()
		{
			var context = A.Fake<ICodeGenerationContext>(p => p.Strict());

			var providerTypesInfo = A.Fake<IProviderTypesInfo>(p => p.Strict());
			A.CallTo(() => providerTypesInfo.CommandParameterSpecificDbTypePropertyType).Returns(typeof(MySpecificDbType));
			A.CallTo(() => providerTypesInfo.CommandParameterType).Returns(typeof(MySpecificParameterType));
			A.CallTo(() => providerTypesInfo.CommandParameterSpecificDbTypePropertyName).Returns("MySpecificDbTypePropertyName");

			var provider = A.Fake<IDatabaseProvider>(p => p.Strict());
			A.CallTo(() => provider.ProviderTypesInfo).Returns(providerTypesInfo);

			A.CallTo(() => context.DatabaseProvider).Returns(provider);
			A.CallTo(() => context.IncomingParameters).Returns(new[] { GetQueryParamInfo(true), GetQueryParamInfo(false) });

			var data = A.Dummy<IQueryObjectData>();

			var ability = new AddQueryParameterAbility();
			IQueryObjectData result = ability.Apply(context, data);

			ability.Name.ShouldBe(KnownAbilityName.AddParameter);
			ability.GetDependencies(context).ShouldBeEmpty();

			result.Constants.ShouldBeEmpty();
			result.Fields.ShouldBeEmpty();
			result.Nested.ShouldBeEmpty();
			result.Properties.ShouldBeEmpty();

			result.Usings.ShouldNotBeNull();
			result.Usings.Count().ShouldBe(3);
			result.Usings.ShouldContain("System");
			result.Usings.ShouldContain("System.Data");
			result.Usings.ShouldContain("MySpecificDatabaseTypes");

			result.Methods.ShouldNotBeNull();
			result.Methods.Count().ShouldBe(1);
			result.Methods.ShouldContain(AbilityArtifacts.Common.AddSqlConnectionParameterAbility_Method_AddParameter);
		}

		[Fact]
		public void GetQueryTextFromResourceCacheableAbility_Test()
		{
			var context = A.Fake<ICodeGenerationContext>(p => p.Strict());

			var options = A.Fake<IReadOnlyDictionary<string, object>>(p => p.Strict());

			object _;
			A.CallTo(() => options.TryGetValue("QueryName", out _))
			.Returns(true)
			.AssignsOutAndRefParametersLazily((string a, object b) => new object[] { "TestQueryName" });

			A.CallTo(() => options.TryGetValue("QueryText", out _))
			.Returns(true)
			.AssignsOutAndRefParametersLazily((string a, object b) => new object[] { "SomeQueryText" });

			A.CallTo(() => options.TryGetValue("QueryTextRaw", out _))
			.Returns(true)
			.AssignsOutAndRefParametersLazily((string a, object b) => new object[] { "SomeQueryTextRaw" });

			A.CallTo(() => options.TryGetValue("ResourcePath", out _))
			.Returns(true)
			.AssignsOutAndRefParametersLazily((string a, object b) => new object[] { "TestQueryResourcePath" });

			A.CallTo(() => context.Options).Returns(options);
			A.CallTo(() => context.IncomingParameters).Returns(new[] { GetQueryParamInfo(true), GetQueryParamInfo(false) });

			var data = A.Dummy<IQueryObjectData>();

			var ability = new GetQueryTextFromResourceCacheableAbility();
			IQueryObjectData result = ability.Apply(context, data);

			ability.Name.ShouldBe(KnownAbilityName.GetQueryText);
			ability.GetDependencies(context).ShouldBeEmpty();

			result.Constants.ShouldBeEmpty();
			result.Nested.ShouldBeEmpty();
			result.Properties.ShouldBeEmpty();

			result.Fields.ShouldNotBeNull();
			result.Fields.Count().ShouldBe(3);
			result.Fields.ShouldContain("private const int QueryTextChecksum = 22918;");
			result.Fields.ShouldContain("private string _cachedSql;");
			result.Fields.ShouldContain("private readonly object _cachedSqlLocker = new object();");

			result.Usings.ShouldNotBeNull();
			result.Usings.Count().ShouldBe(4);
			result.Usings.ShouldContain("System");
			result.Usings.ShouldContain("System.IO");
			result.Usings.ShouldContain("System.Text");
			result.Usings.ShouldContain("System.Text.RegularExpressions");

			result.Methods.ShouldNotBeNull();
			result.Methods.Count().ShouldBe(2);
			result.Methods.ShouldContain(AbilityArtifacts.Common.GetQueryTextFromResourceCacheableAbility_Method_CalculateChecksum);
			result.Methods.ShouldContain(AbilityArtifacts.Common.GetQueryTextFromResourceCacheableAbility_Method_GetQueryText);
		}

		[Fact]
		public void GetQueryTextFromStringAbility_Test()
		{
			var context = A.Fake<ICodeGenerationContext>(p => p.Strict());

			var options = A.Fake<IReadOnlyDictionary<string, object>>(p => p.Strict());

			object _;
			A.CallTo(() => options.TryGetValue("QueryName", out _))
			.Returns(true)
			.AssignsOutAndRefParametersLazily((string a, object b) => new object[] { "TestQueryName" });

			A.CallTo(() => options.TryGetValue("QueryText", out _))
			.Returns(true)
			.AssignsOutAndRefParametersLazily((string a, object b) => new object[] { "TestQueryText" });

			A.CallTo(() => context.Options).Returns(options);
			A.CallTo(() => context.IncomingParameters).Returns(new[] { GetQueryParamInfo(true), GetQueryParamInfo(false) });

			var data = A.Dummy<IQueryObjectData>();

			var ability = new GetQueryTextFromStringAbility();
			IQueryObjectData result = ability.Apply(context, data);

			ability.Name.ShouldBe(KnownAbilityName.GetQueryText);
			ability.GetDependencies(context).ShouldBeEmpty();

			result.Constants.ShouldBeEmpty();
			result.Nested.ShouldBeEmpty();
			result.Properties.ShouldBeEmpty();

			result.Fields.ShouldNotBeNull();
			result.Fields.Count().ShouldBe(0);

			result.Usings.ShouldNotBeNull();
			result.Usings.Count().ShouldBe(0);

			result.Methods.ShouldNotBeNull();
			result.Methods.Count().ShouldBe(1);
			result.Methods.ShouldContain(AbilityArtifacts.Common.GetQueryTextFromStringAbility_Method_GetQueryText);
		}

		[Theory]
		[InlineData((string)null)]
		[InlineData("")]
		[InlineData(" ")]
		public void GetQueryTextFromStringAbility_EmptyString_Test(string queryText)
		{
			var context = A.Fake<ICodeGenerationContext>(p => p.Strict());

			var options = A.Fake<IReadOnlyDictionary<string, object>>(p => p.Strict());

			object _;
			A.CallTo(() => options.TryGetValue("QueryName", out _))
			.Returns(true)
			.AssignsOutAndRefParametersLazily((string a, object b) => new object[] { "TestQueryName" });

			A.CallTo(() => options.TryGetValue("QueryText", out _))
			.Returns(true)
			.AssignsOutAndRefParametersLazily((string a, object b) => new object[] { queryText });

			A.CallTo(() => context.Options).Returns(options);
			A.CallTo(() => context.IncomingParameters).Returns(Enumerable.Empty<IQueryParamInfo>());

			var data = A.Dummy<IQueryObjectData>();

			var ability = new GetQueryTextFromStringAbility();
			var exception = Assert.Throws<CodeGenerationException>(() => ability.Apply(context, data));
			exception.Message.ShouldBe("Can not find query text at current code generation context.");
		}

		[Fact]
		public void MapDataRecordToItemAbility_Test()
		{
			var mapper = A.Fake<IDatabaseTypeMapper>(p => p.Strict());
			A.CallTo(() => mapper.MapToClrType("dummyDbType1", false, A<IDictionary<string, object>>._)).Returns(typeof(Guid));
			A.CallTo(() => mapper.MapToClrType("dummyDbType2", true, A<IDictionary<string, object>>._)).Returns(typeof(int?));

			var firstParameter = A.Fake<IFieldDetails>(p => p.Strict());
			A.CallTo(() => firstParameter.ColumnName).Returns("FirstParam");
			A.CallTo(() => firstParameter.DbType).Returns("dummyDbType1");
			A.CallTo(() => firstParameter.AllowDbNull).Returns(false);

			var secondParameter = A.Fake<IFieldDetails>(p => p.Strict());
			A.CallTo(() => secondParameter.ColumnName).Returns("SECOND_Param");
			A.CallTo(() => secondParameter.DbType).Returns("dummyDbType2");
			A.CallTo(() => secondParameter.AllowDbNull).Returns(true);

			var context = A.Fake<ICodeGenerationContext>(p => p.Strict());
			A.CallTo(() => context.TypeMapper).Returns(mapper);
			A.CallTo(() => context.OutgoingParameters).Returns(new[] { firstParameter, secondParameter });

			var options = A.Fake<IReadOnlyDictionary<string, object>>(p => p.Strict());

			object _;
			A.CallTo(() => options.TryGetValue("QueryResultItemName", out _))
			.Returns(true)
			.AssignsOutAndRefParametersLazily((string a, object b) => new object[] { "QueryItemTestName" });

			A.CallTo(() => context.Options).Returns(options);
			A.CallTo(() => context.IncomingParameters).Returns(new[] { GetQueryParamInfo(true), GetQueryParamInfo(false) });

			var data = A.Dummy<IQueryObjectData>();

			var ability = new MapDataRecordToItemAbility();
			IQueryObjectData result = ability.Apply(context, data);

			ability.Name.ShouldBe(KnownAbilityName.GetItemFromRecord);
			ability.GetDependencies(context).ShouldBeEmpty();

			result.Constants.ShouldBeEmpty();
			result.Nested.ShouldBeEmpty();
			result.Properties.ShouldBeEmpty();
			result.Fields.ShouldBeEmpty();

			result.Usings.ShouldNotBeNull();
			result.Usings.Count().ShouldBe(2);
			result.Usings.ShouldContain("System");
			result.Usings.ShouldContain("System.Data");

			result.Methods.ShouldNotBeNull();
			result.Methods.Count().ShouldBe(1);
			result.Methods.ShouldContain(AbilityArtifacts.Common.MapDataRecordToItemAbility_Method_GetItemFromRecord);
		}

		[Fact]
		public void MapDataRecordToScalarAbility_Test()
		{
			var context = A.Fake<ICodeGenerationContext>(p => p.Strict());
			A.CallTo(() => context.IncomingParameters).Returns(new[] { GetQueryParamInfo(true), GetQueryParamInfo(false) });

			var data = A.Dummy<IQueryObjectData>();

			var ability = new MapDataRecordToScalarAbility();
			IQueryObjectData result = ability.Apply(context, data);

			ability.Name.ShouldBe(KnownAbilityName.GetScalarFromRecord);
			ability.GetDependencies(context).ShouldNotBeNull();
			ability.GetDependencies(context).Count().ShouldBe(1);
			ability.GetDependencies(context).ShouldContain(KnownAbilityName.GetScalarFromValue);

			result.Constants.ShouldBeEmpty();
			result.Nested.ShouldBeEmpty();
			result.Properties.ShouldBeEmpty();
			result.Fields.ShouldBeEmpty();

			result.Usings.ShouldNotBeNull();
			result.Usings.Count().ShouldBe(2);
			result.Usings.ShouldContain("System");
			result.Usings.ShouldContain("System.Data");

			result.Methods.ShouldNotBeNull();
			result.Methods.Count().ShouldBe(1);
			result.Methods.ShouldContain(AbilityArtifacts.Common.MapDataRecordToScalarAbility_Method_GetScalarFromRecord);
		}

		[Fact]
		public void MapValueToScalarAbility_Test()
		{
			var context = A.Fake<ICodeGenerationContext>(p => p.Strict());
			A.CallTo(() => context.IncomingParameters).Returns(new[] { GetQueryParamInfo(true), GetQueryParamInfo(false) });

			var data = A.Dummy<IQueryObjectData>();

			var ability = new MapValueToScalarAbility();
			IQueryObjectData result = ability.Apply(context, data);

			ability.Name.ShouldBe(KnownAbilityName.GetScalarFromValue);

			ability.GetDependencies(context).ShouldBeEmpty();
			result.Constants.ShouldBeEmpty();
			result.Nested.ShouldBeEmpty();
			result.Properties.ShouldBeEmpty();
			result.Fields.ShouldBeEmpty();

			result.Usings.ShouldNotBeNull();
			result.Usings.Count().ShouldBe(2);
			result.Usings.ShouldContain("System");
			result.Usings.ShouldContain("System.Data");

			result.Methods.ShouldNotBeNull();
			result.Methods.Count().ShouldBe(1);
			result.Methods.ShouldContain(AbilityArtifacts.Common.MapValueToScalarAbility_Method_GetScalarFromValue);
		}
	}
}