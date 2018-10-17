using System.Linq;
using FakeItEasy;
using Shouldly;
using SqlFirst.Codegen.Text.QueryObject.Abilities;
using SqlFirst.Codegen.Text.QueryObject.Abilities.Common;
using SqlFirst.Codegen.Text.QueryObject.Data;
using SqlFirst.Codegen.Text.Tests.Fixtures;
using SqlFirst.Core;
using Xunit;

namespace SqlFirst.Codegen.Text.Tests.Abilities
{
	public class AddQueryCustomParameterAbilityTests
	{
		private class TestCommandParameterSpecificDbTypePropertyType
		{
		}

		private class TestCommandParameterType
		{
		}

		[Fact]
		public void AddQueryCustomParameterAbility_Success_Test()
		{
			var providerTypesInfo = A.Fake<IProviderTypesInfo>(p => p.Strict());
			A.CallTo(() => providerTypesInfo.CommandParameterSpecificDbTypePropertyType)
			.Returns(typeof(TestCommandParameterSpecificDbTypePropertyType));

			A.CallTo(() => providerTypesInfo.CommandParameterType)
			.Returns(typeof(TestCommandParameterType));

			A.CallTo(() => providerTypesInfo.CommandParameterSpecificCustomDbTypePropertyName)
			.Returns("TestCommandParameterSpecificCustomDbTypePropertyName");

			var databaseProvider = A.Fake<IDatabaseProvider>(p => p.Strict());
			A.CallTo(() => databaseProvider.ProviderTypesInfo).Returns(providerTypesInfo);

			var context = A.Fake<ICodeGenerationContext>(p => p.Strict());
			A.CallTo(() => context.DatabaseProvider).Returns(databaseProvider);

			var data = A.Dummy<IQueryObjectData>();

			var ability = new AddQueryCustomParameterAbility();
			IQueryObjectData result = ability.Apply(context, data);

			ability.Name.ShouldBe(KnownAbilityName.AddCustomParameter);
			ability.GetDependencies(context).ShouldBeEmpty();

			result.Constants.ShouldBeEmpty();
			result.Fields.ShouldBeEmpty();
			result.Nested.ShouldBeEmpty();
			result.Properties.ShouldBeEmpty();

			result.Usings.ShouldNotBeNull();
			result.Usings.Count().ShouldBe(3);
			result.Usings.ShouldContain("System");
			result.Usings.ShouldContain("System.Data");
			result.Usings.ShouldContain(typeof(TestCommandParameterType).Namespace);

			result.Methods.ShouldNotBeNull();
			result.Methods.Count().ShouldBe(1);

			result.Methods.ShouldContain(AbilityArtifacts.AddQueryCustomParameter.AddQueryCustomParameterAbility_Success);
		}
	}
}