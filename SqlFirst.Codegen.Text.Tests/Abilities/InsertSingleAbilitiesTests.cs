using System;
using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using MySpecificDatabaseTypes;
using Shouldly;
using SqlFirst.Codegen.Text.QueryObject.Abilities;
using SqlFirst.Codegen.Text.QueryObject.Abilities.Insert;
using SqlFirst.Codegen.Text.QueryObject.Data;
using SqlFirst.Codegen.Text.Tests.Fixtures;
using SqlFirst.Core;
using Xunit;

namespace SqlFirst.Codegen.Text.Tests.Abilities
{
	public class InsertSingleAbilitiesTests
	{
		[Fact]
		public void InsertSingleValuePlainAbility_Test()
		{
			ICodeGenerationContext context = GetDefaultCodeGenerationContext();

			var data = A.Dummy<IQueryObjectData>();

			var ability = new InsertSingleValuePlainAbility();
			IQueryObjectData result = ability.Apply(context, data);

			ability.Name.ShouldBe("InsertSingleValuePlain");

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
			result.Methods.ShouldContain(AbilityArtifacts.Insert.InsertSingleValuePlainAbility_Method_Add);
		}

		[Fact]
		public void InsertSingleValuePlainAsyncAbility_Test()
		{
			ICodeGenerationContext context = GetDefaultCodeGenerationContext();

			var data = A.Dummy<IQueryObjectData>();

			var ability = new InsertSingleValuePlainAsyncAbility();
			IQueryObjectData result = ability.Apply(context, data);

			ability.Name.ShouldBe("InsertSingleValuePlainAsync");

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
			result.Methods.ShouldContain(AbilityArtifacts.Insert.InsertSingleValuePlainAsyncAbility_Method_AddAsync);
		}

		[Fact]
		public void InsertSingleValuePlainWithResultAbility_Test()
		{
			ICodeGenerationContext context = GetDefaultCodeGenerationContext();

			var data = A.Dummy<IQueryObjectData>();

			var ability = new InsertSingleValuePlainWithResultAbility();
			IQueryObjectData result = ability.Apply(context, data);

			ability.Name.ShouldBe("InsertSingleValuePlain");

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
			result.Methods.ShouldContain(AbilityArtifacts.Insert.InsertSingleValuePlainWithResultAbility_Method_Add);
		}

		[Fact]
		public void InsertSingleValuePlainWithResultAsyncAbility_Test()
		{
			ICodeGenerationContext context = GetDefaultCodeGenerationContext();

			var data = A.Dummy<IQueryObjectData>();

			var ability = new InsertSingleValuePlainWithResultAsyncAbility();
			IQueryObjectData result = ability.Apply(context, data);

			ability.Name.ShouldBe("InsertSingleValuePlainAsync");

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
			result.Methods.ShouldContain(AbilityArtifacts.Insert.InsertSingleValuePlainWithResultAsyncAbility_Method_AddAsync);
		}

		[Fact]
		public void InsertSingleValuePlainWithScalarResultAbility_Test()
		{
			ICodeGenerationContext context = GetDefaultCodeGenerationContext();

			var data = A.Dummy<IQueryObjectData>();

			var ability = new InsertSingleValuePlainWithScalarResultAbility();
			IQueryObjectData result = ability.Apply(context, data);

			ability.Name.ShouldBe("InsertSingleValuePlain");

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
			result.Methods.ShouldContain(AbilityArtifacts.Insert.InsertSingleValuePlainWithScalarResultAbility_Method_Add);
		}

		[Fact]
		public void InsertSingleValuePlainWithScalarResultAsyncAbility_Test()
		{
			ICodeGenerationContext context = GetDefaultCodeGenerationContext();

			var data = A.Dummy<IQueryObjectData>();

			var ability = new InsertSingleValuePlainWithScalarResultAsyncAbility();
			IQueryObjectData result = ability.Apply(context, data);

			ability.Name.ShouldBe("InsertSingleValuePlainAsync");

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
			result.Methods.ShouldContain(AbilityArtifacts.Insert.InsertSingleValuePlainWithScalarResultAsyncAbility_Method_AddAsync);
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