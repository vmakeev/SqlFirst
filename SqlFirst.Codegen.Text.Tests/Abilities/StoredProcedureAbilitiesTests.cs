using System;
using System.Linq;
using FakeItEasy;
using MySpecificDatabaseTypes;
using Shouldly;
using SqlFirst.Codegen.Text.QueryObject.Abilities;
using SqlFirst.Codegen.Text.QueryObject.Abilities.StoredProcedure;
using SqlFirst.Codegen.Text.QueryObject.Data;
using SqlFirst.Codegen.Text.Tests.Fixtures;
using Xunit;

namespace SqlFirst.Codegen.Text.Tests.Abilities
{
	public class StoredProcedureAbilitiesTests
	{
		[Fact]
		public void StoredProcedure_Parameters_Simple_Test()
		{
			ICodeGenerationContext context = GetContextWithSimpleParameters();

			var data = A.Dummy<IQueryObjectData>();

			var ability = new StoredProcedureAbility();
			IQueryObjectData result = ability.Apply(context, data);

			ability.Name.ShouldBe("StoredProcedure");

			result.Constants.ShouldBeEmpty();
			result.Nested.ShouldBeEmpty();
			result.Properties.ShouldBeEmpty();
			result.Fields.ShouldBeEmpty();

			string[] dependencies = ability.GetDependencies(context)?.ToArray();
			dependencies.ShouldNotBeNull();
			dependencies.Length.ShouldBe(2);
			dependencies.ShouldContain(KnownAbilityName.GetQueryText);
			dependencies.ShouldContain(KnownAbilityName.AddParameter);

			result.Usings.ShouldNotBeNull();
			result.Usings.Count().ShouldBe(3);
			result.Usings.ShouldContain("System");
			result.Usings.ShouldContain("System.Data");
			result.Usings.ShouldContain("MySpecificDatabaseTypes");

			result.Methods.ShouldNotBeNull();
			result.Methods.Count().ShouldBe(1);
			result.Methods.ShouldContain(AbilityArtifacts.StoredProcedure.StoredProcedure_Parameters_Simple_Method_Exec);
		}

		[Fact]
		public void StoredProcedure_Parameters_Empty_Test()
		{
			ICodeGenerationContext context = GetContextWithNoParameters();

			var data = A.Dummy<IQueryObjectData>();

			var ability = new StoredProcedureAbility();
			IQueryObjectData result = ability.Apply(context, data);

			ability.Name.ShouldBe("StoredProcedure");

			result.Constants.ShouldBeEmpty();
			result.Nested.ShouldBeEmpty();
			result.Properties.ShouldBeEmpty();
			result.Fields.ShouldBeEmpty();

			string[] dependencies = ability.GetDependencies(context)?.ToArray();
			dependencies.ShouldNotBeNull();
			dependencies.Length.ShouldBe(1);
			dependencies.ShouldContain(KnownAbilityName.GetQueryText);

			result.Usings.ShouldNotBeNull();
			result.Usings.Count().ShouldBe(2);
			result.Usings.ShouldContain("System");
			result.Usings.ShouldContain("System.Data");

			result.Methods.ShouldNotBeNull();
			result.Methods.Count().ShouldBe(1);
			result.Methods.ShouldContain(AbilityArtifacts.StoredProcedure.StoredProcedure_Parameters_Empty_Method_Exec);
		}

		[Fact]
		public void StoredProcedure_Parameters_Mixed_Test()
		{
			ICodeGenerationContext context = GetContextWithMixedParameters();

			var data = A.Dummy<IQueryObjectData>();

			var ability = new StoredProcedureAbility();
			IQueryObjectData result = ability.Apply(context, data);

			ability.Name.ShouldBe("StoredProcedure");

			result.Constants.ShouldBeEmpty();
			result.Nested.ShouldBeEmpty();
			result.Properties.ShouldBeEmpty();
			result.Fields.ShouldBeEmpty();

			string[] dependencies = ability.GetDependencies(context)?.ToArray();
			dependencies.ShouldNotBeNull();
			dependencies.Length.ShouldBe(3);
			dependencies.ShouldContain(KnownAbilityName.AddCustomParameter);
			dependencies.ShouldContain(KnownAbilityName.GetQueryText);
			dependencies.ShouldContain(KnownAbilityName.AddParameter);

			result.Usings.ShouldNotBeNull();
			result.Usings.Count().ShouldBe(3);
			result.Usings.ShouldContain("System");
			result.Usings.ShouldContain("System.Data");
			result.Usings.ShouldContain("MySpecificDatabaseTypes");

			result.Methods.ShouldNotBeNull();
			result.Methods.Count().ShouldBe(1);
			result.Methods.ShouldContain(AbilityArtifacts.StoredProcedure.StoredProcedure_Parameters_Mixed_Method_Exec);
		}

		[Fact]
		public void StoredProcedure_Parameters_Custom_Test()
		{
			ICodeGenerationContext context = GetContextWithTableParameterOnly();

			var data = A.Dummy<IQueryObjectData>();

			var ability = new StoredProcedureAbility();
			IQueryObjectData result = ability.Apply(context, data);

			ability.Name.ShouldBe("StoredProcedure");

			result.Constants.ShouldBeEmpty();
			result.Nested.ShouldBeEmpty();
			result.Properties.ShouldBeEmpty();
			result.Fields.ShouldBeEmpty();

			string[] dependencies = ability.GetDependencies(context)?.ToArray();
			dependencies.ShouldNotBeNull();
			dependencies.Length.ShouldBe(2);
			dependencies.ShouldContain(KnownAbilityName.AddCustomParameter);
			dependencies.ShouldContain(KnownAbilityName.GetQueryText);

			result.Usings.ShouldNotBeNull();
			result.Usings.Count().ShouldBe(2);
			result.Usings.ShouldContain("System");
			result.Usings.ShouldContain("System.Data");

			result.Methods.ShouldNotBeNull();
			result.Methods.Count().ShouldBe(1);
			result.Methods.ShouldContain(AbilityArtifacts.StoredProcedure.StoredProcedure_Parameters_Custom_Method_Exec);
		}

		[Fact]
		public void StoredProcedureAbility_Parameters_Result_Empty_NoThrow_Test()
		{
			ICodeGenerationContext context = GetContextWithNoParametersNoResults();

			var data = A.Dummy<IQueryObjectData>();

			var ability = new StoredProcedureAbility();

			ability.Apply(context, data);
		}


		[Fact]
		public void StoredProcedureAsyncAbility_Parameters_Simple_Test()
		{
			ICodeGenerationContext context = GetContextWithSimpleParameters();

			var data = A.Dummy<IQueryObjectData>();

			var ability = new StoredProcedureAsyncAbility();
			IQueryObjectData result = ability.Apply(context, data);

			ability.Name.ShouldBe("StoredProcedureAsync");

			result.Constants.ShouldBeEmpty();
			result.Nested.ShouldBeEmpty();
			result.Properties.ShouldBeEmpty();
			result.Fields.ShouldBeEmpty();

			string[] dependencies = ability.GetDependencies(context)?.ToArray();
			dependencies.ShouldNotBeNull();
			dependencies.Length.ShouldBe(2);
			dependencies.ShouldContain(KnownAbilityName.GetQueryText);
			dependencies.ShouldContain(KnownAbilityName.AddParameter);

			result.Usings.ShouldNotBeNull();
			result.Usings.Count().ShouldBe(6);
			result.Usings.ShouldContain("System");
			result.Usings.ShouldContain("System.Data");
			result.Usings.ShouldContain("MySpecificDatabaseTypes");
			result.Usings.ShouldContain("System.Data.Common");
			result.Usings.ShouldContain("System.Threading");
			result.Usings.ShouldContain("System.Threading.Tasks");

			result.Methods.ShouldNotBeNull();
			result.Methods.Count().ShouldBe(1);
			result.Methods.ShouldContain(AbilityArtifacts.StoredProcedure.StoredProcedureAsync_Parameters_Simple_Method_ExecAsync);
		}

		[Fact]
		public void StoredProcedureAsyncAbility_Parameters_Empty_Test()
		{
			ICodeGenerationContext context = GetContextWithNoParameters();

			var data = A.Dummy<IQueryObjectData>();

			var ability = new StoredProcedureAsyncAbility();
			IQueryObjectData result = ability.Apply(context, data);

			ability.Name.ShouldBe("StoredProcedureAsync");

			result.Constants.ShouldBeEmpty();
			result.Nested.ShouldBeEmpty();
			result.Properties.ShouldBeEmpty();
			result.Fields.ShouldBeEmpty();

			string[] dependencies = ability.GetDependencies(context)?.ToArray();
			dependencies.ShouldNotBeNull();
			dependencies.Length.ShouldBe(1);
			dependencies.ShouldContain(KnownAbilityName.GetQueryText);

			result.Usings.ShouldNotBeNull();
			result.Usings.Count().ShouldBe(5);
			result.Usings.ShouldContain("System");
			result.Usings.ShouldContain("System.Data");
			result.Usings.ShouldContain("System.Data.Common");
			result.Usings.ShouldContain("System.Threading");
			result.Usings.ShouldContain("System.Threading.Tasks");

			result.Methods.ShouldNotBeNull();
			result.Methods.Count().ShouldBe(1);
			result.Methods.ShouldContain(AbilityArtifacts.StoredProcedure.StoredProcedureAsync_Parameters_Empty_Method_ExecAsync);
		}

		[Fact]
		public void StoredProcedureAsyncAbility_Parameters_Mixed_Test()
		{
			ICodeGenerationContext context = GetContextWithMixedParameters();

			var data = A.Dummy<IQueryObjectData>();

			var ability = new StoredProcedureAsyncAbility();
			IQueryObjectData result = ability.Apply(context, data);

			ability.Name.ShouldBe("StoredProcedureAsync");

			result.Constants.ShouldBeEmpty();
			result.Nested.ShouldBeEmpty();
			result.Properties.ShouldBeEmpty();
			result.Fields.ShouldBeEmpty();

			string[] dependencies = ability.GetDependencies(context)?.ToArray();
			dependencies.ShouldNotBeNull();
			dependencies.Length.ShouldBe(3);
			dependencies.ShouldContain(KnownAbilityName.AddCustomParameter);
			dependencies.ShouldContain(KnownAbilityName.GetQueryText);
			dependencies.ShouldContain(KnownAbilityName.AddParameter);

			result.Usings.ShouldNotBeNull();
			result.Usings.Count().ShouldBe(6);
			result.Usings.ShouldContain("System");
			result.Usings.ShouldContain("System.Data");
			result.Usings.ShouldContain("MySpecificDatabaseTypes");
			result.Usings.ShouldContain("System.Data.Common");
			result.Usings.ShouldContain("System.Threading");
			result.Usings.ShouldContain("System.Threading.Tasks");

			result.Methods.ShouldNotBeNull();
			result.Methods.Count().ShouldBe(1);
			result.Methods.ShouldContain(AbilityArtifacts.StoredProcedure.StoredProcedureAsync_Parameters_Mixed_Method_ExecAsync);
		}

		[Fact]
		public void StoredProcedureAsyncAbility_Parameters_Custom_Test()
		{
			ICodeGenerationContext context = GetContextWithTableParameterOnly();

			var data = A.Dummy<IQueryObjectData>();

			var ability = new StoredProcedureAsyncAbility();
			IQueryObjectData result = ability.Apply(context, data);

			ability.Name.ShouldBe("StoredProcedureAsync");

			result.Constants.ShouldBeEmpty();
			result.Nested.ShouldBeEmpty();
			result.Properties.ShouldBeEmpty();
			result.Fields.ShouldBeEmpty();

			string[] dependencies = ability.GetDependencies(context)?.ToArray();
			dependencies.ShouldNotBeNull();
			dependencies.Length.ShouldBe(2);
			dependencies.ShouldContain(KnownAbilityName.AddCustomParameter);
			dependencies.ShouldContain(KnownAbilityName.GetQueryText);

			result.Usings.ShouldNotBeNull();
			result.Usings.Count().ShouldBe(5);
			result.Usings.ShouldContain("System");
			result.Usings.ShouldContain("System.Data");
			result.Usings.ShouldContain("System.Data.Common");
			result.Usings.ShouldContain("System.Threading");
			result.Usings.ShouldContain("System.Threading.Tasks");

			result.Methods.ShouldNotBeNull();
			result.Methods.Count().ShouldBe(1);
			result.Methods.ShouldContain(AbilityArtifacts.StoredProcedure.StoredProcedureAsync_Parameters_Custom_Method_ExecAsync);
		}

		[Fact]
		public void StoredProcedureAsyncAbility_Parameters_Result_Empty_NoThrow_Test()
		{
			ICodeGenerationContext context = GetContextWithNoParametersNoResults();

			var data = A.Dummy<IQueryObjectData>();

			var ability = new StoredProcedureAsyncAbility();

			ability.Apply(context, data);
		}


		[Fact]
		public void StoredProcedureWithResultAbility_Parameters_Simple_Test()
		{
			ICodeGenerationContext context = GetContextWithSimpleParameters();

			var data = A.Dummy<IQueryObjectData>();

			var ability = new StoredProcedureWithResultAbility();
			IQueryObjectData result = ability.Apply(context, data);

			ability.Name.ShouldBe("StoredProcedure");

			result.Constants.ShouldBeEmpty();
			result.Nested.ShouldBeEmpty();
			result.Properties.ShouldBeEmpty();
			result.Fields.ShouldBeEmpty();

			string[] dependencies = ability.GetDependencies(context)?.ToArray();
			dependencies.ShouldNotBeNull();
			dependencies.Length.ShouldBe(3);
			dependencies.ShouldContain(KnownAbilityName.GetQueryText);
			dependencies.ShouldContain(KnownAbilityName.AddParameter);
			dependencies.ShouldContain(KnownAbilityName.GetItemFromRecord);

			result.Usings.ShouldNotBeNull();
			result.Usings.Count().ShouldBe(4);
			result.Usings.ShouldContain("System");
			result.Usings.ShouldContain("System.Data");
			result.Usings.ShouldContain("System.Collections.Generic");
			result.Usings.ShouldContain("MySpecificDatabaseTypes");

			result.Methods.ShouldNotBeNull();
			result.Methods.Count().ShouldBe(1);
			result.Methods.ShouldContain(AbilityArtifacts.StoredProcedure.StoredProcedureWithResult_Parameters_Simple_Method_Exec);
		}

		[Fact]
		public void StoredProcedureWithResultAbility_Parameters_Empty_Test()
		{
			ICodeGenerationContext context = GetContextWithNoParameters();

			var data = A.Dummy<IQueryObjectData>();

			var ability = new StoredProcedureWithResultAbility();
			IQueryObjectData result = ability.Apply(context, data);

			ability.Name.ShouldBe("StoredProcedure");

			result.Constants.ShouldBeEmpty();
			result.Nested.ShouldBeEmpty();
			result.Properties.ShouldBeEmpty();
			result.Fields.ShouldBeEmpty();

			string[] dependencies = ability.GetDependencies(context)?.ToArray();
			dependencies.ShouldNotBeNull();
			dependencies.Length.ShouldBe(2);
			dependencies.ShouldContain(KnownAbilityName.GetQueryText);
			dependencies.ShouldContain(KnownAbilityName.GetItemFromRecord);

			result.Usings.ShouldNotBeNull();
			result.Usings.Count().ShouldBe(3);
			result.Usings.ShouldContain("System");
			result.Usings.ShouldContain("System.Data");
			result.Usings.ShouldContain("System.Collections.Generic");

			result.Methods.ShouldNotBeNull();
			result.Methods.Count().ShouldBe(1);
			result.Methods.ShouldContain(AbilityArtifacts.StoredProcedure.StoredProcedureWithResult_Parameters_Empty_Method_Exec);
		}

		[Fact]
		public void StoredProcedureWithResultAbility_Parameters_Mixed_Test()
		{
			ICodeGenerationContext context = GetContextWithMixedParameters();

			var data = A.Dummy<IQueryObjectData>();

			var ability = new StoredProcedureWithResultAbility();
			IQueryObjectData result = ability.Apply(context, data);

			ability.Name.ShouldBe("StoredProcedure");

			result.Constants.ShouldBeEmpty();
			result.Nested.ShouldBeEmpty();
			result.Properties.ShouldBeEmpty();
			result.Fields.ShouldBeEmpty();

			string[] dependencies = ability.GetDependencies(context)?.ToArray();
			dependencies.ShouldNotBeNull();
			dependencies.Length.ShouldBe(4);
			dependencies.ShouldContain(KnownAbilityName.AddCustomParameter);
			dependencies.ShouldContain(KnownAbilityName.GetQueryText);
			dependencies.ShouldContain(KnownAbilityName.AddParameter);
			dependencies.ShouldContain(KnownAbilityName.GetItemFromRecord);

			result.Usings.ShouldNotBeNull();
			result.Usings.Count().ShouldBe(4);
			result.Usings.ShouldContain("System");
			result.Usings.ShouldContain("System.Data");
			result.Usings.ShouldContain("MySpecificDatabaseTypes");
			result.Usings.ShouldContain("System.Collections.Generic");

			result.Methods.ShouldNotBeNull();
			result.Methods.Count().ShouldBe(1);
			result.Methods.ShouldContain(AbilityArtifacts.StoredProcedure.StoredProcedureWithResult_Parameters_Mixed_Method_Exec);
		}

		[Fact]
		public void StoredProcedureWithResultAbility_Parameters_Custom_Test()
		{
			ICodeGenerationContext context = GetContextWithTableParameterOnly();

			var data = A.Dummy<IQueryObjectData>();

			var ability = new StoredProcedureWithResultAbility();
			IQueryObjectData result = ability.Apply(context, data);

			ability.Name.ShouldBe("StoredProcedure");

			result.Constants.ShouldBeEmpty();
			result.Nested.ShouldBeEmpty();
			result.Properties.ShouldBeEmpty();
			result.Fields.ShouldBeEmpty();

			string[] dependencies = ability.GetDependencies(context)?.ToArray();
			dependencies.ShouldNotBeNull();
			dependencies.Length.ShouldBe(3);
			dependencies.ShouldContain(KnownAbilityName.AddCustomParameter);
			dependencies.ShouldContain(KnownAbilityName.GetQueryText);
			dependencies.ShouldContain(KnownAbilityName.GetItemFromRecord);

			result.Usings.ShouldNotBeNull();
			result.Usings.Count().ShouldBe(3);
			result.Usings.ShouldContain("System");
			result.Usings.ShouldContain("System.Data");
			result.Usings.ShouldContain("System.Collections.Generic");

			result.Methods.ShouldNotBeNull();
			result.Methods.Count().ShouldBe(1);
			result.Methods.ShouldContain(AbilityArtifacts.StoredProcedure.StoredProcedureWithResult_Parameters_Custom_Method_Exec);
		}

		[Fact]
		public void StoredProcedureWithResultAbility_Parameters_Result_Empty_NoThrow_Test()
		{
			ICodeGenerationContext context = GetContextWithNoParametersNoResults();

			var data = A.Dummy<IQueryObjectData>();

			var ability = new StoredProcedureWithResultAbility();

			ability.Apply(context, data);
		}


		[Fact]
		public void StoredProcedureWithResultAsyncAbility_Parameters_Simple_Test()
		{
			ICodeGenerationContext context = GetContextWithSimpleParameters();

			var data = A.Dummy<IQueryObjectData>();

			var ability = new StoredProcedureWithResultAsyncAbility();
			IQueryObjectData result = ability.Apply(context, data);

			ability.Name.ShouldBe("StoredProcedureAsync");

			result.Constants.ShouldBeEmpty();
			result.Nested.ShouldBeEmpty();
			result.Properties.ShouldBeEmpty();
			result.Fields.ShouldBeEmpty();

			string[] dependencies = ability.GetDependencies(context)?.ToArray();
			dependencies.ShouldNotBeNull();
			dependencies.Length.ShouldBe(3);
			dependencies.ShouldContain(KnownAbilityName.GetQueryText);
			dependencies.ShouldContain(KnownAbilityName.AddParameter);
			dependencies.ShouldContain(KnownAbilityName.GetItemFromRecord);

			result.Usings.ShouldNotBeNull();
			result.Usings.Count().ShouldBe(7);
			result.Usings.ShouldContain("System");
			result.Usings.ShouldContain("System.Data");
			result.Usings.ShouldContain("System.Collections.Generic");
			result.Usings.ShouldContain("MySpecificDatabaseTypes");
			result.Usings.ShouldContain("System.Data.Common");
			result.Usings.ShouldContain("System.Threading");
			result.Usings.ShouldContain("System.Threading.Tasks");

			result.Methods.ShouldNotBeNull();
			result.Methods.Count().ShouldBe(1);
			result.Methods.ShouldContain(AbilityArtifacts.StoredProcedure.StoredProcedureWithResultAsync_Parameters_Simple_Method_ExecAsync);
		}

		[Fact]
		public void StoredProcedureWithResultAsyncAbility_Parameters_Empty_Test()
		{
			ICodeGenerationContext context = GetContextWithNoParameters();

			var data = A.Dummy<IQueryObjectData>();

			var ability = new StoredProcedureWithResultAsyncAbility();
			IQueryObjectData result = ability.Apply(context, data);

			ability.Name.ShouldBe("StoredProcedureAsync");

			result.Constants.ShouldBeEmpty();
			result.Nested.ShouldBeEmpty();
			result.Properties.ShouldBeEmpty();
			result.Fields.ShouldBeEmpty();

			string[] dependencies = ability.GetDependencies(context)?.ToArray();
			dependencies.ShouldNotBeNull();
			dependencies.Length.ShouldBe(2);
			dependencies.ShouldContain(KnownAbilityName.GetQueryText);
			dependencies.ShouldContain(KnownAbilityName.GetItemFromRecord);

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
			result.Methods.ShouldContain(AbilityArtifacts.StoredProcedure.StoredProcedureWithResultAsync_Parameters_Empty_Method_ExecAsync);
		}

		[Fact]
		public void StoredProcedureWithResultAsyncAbility_Parameters_Mixed_Test()
		{
			ICodeGenerationContext context = GetContextWithMixedParameters();

			var data = A.Dummy<IQueryObjectData>();

			var ability = new StoredProcedureWithResultAsyncAbility();
			IQueryObjectData result = ability.Apply(context, data);

			ability.Name.ShouldBe("StoredProcedureAsync");

			result.Constants.ShouldBeEmpty();
			result.Nested.ShouldBeEmpty();
			result.Properties.ShouldBeEmpty();
			result.Fields.ShouldBeEmpty();

			string[] dependencies = ability.GetDependencies(context)?.ToArray();
			dependencies.ShouldNotBeNull();
			dependencies.Length.ShouldBe(4);
			dependencies.ShouldContain(KnownAbilityName.AddCustomParameter);
			dependencies.ShouldContain(KnownAbilityName.GetQueryText);
			dependencies.ShouldContain(KnownAbilityName.AddParameter);
			dependencies.ShouldContain(KnownAbilityName.GetItemFromRecord);

			result.Usings.ShouldNotBeNull();
			result.Usings.Count().ShouldBe(7);
			result.Usings.ShouldContain("System");
			result.Usings.ShouldContain("System.Data");
			result.Usings.ShouldContain("MySpecificDatabaseTypes");
			result.Usings.ShouldContain("System.Collections.Generic");
			result.Usings.ShouldContain("System.Data.Common");
			result.Usings.ShouldContain("System.Threading");
			result.Usings.ShouldContain("System.Threading.Tasks");

			result.Methods.ShouldNotBeNull();
			result.Methods.Count().ShouldBe(1);
			result.Methods.ShouldContain(AbilityArtifacts.StoredProcedure.StoredProcedureWithResultAsync_Parameters_Mixed_Method_ExecAsync);
		}

		[Fact]
		public void StoredProcedureWithResultAsyncAbility_Parameters_Custom_Test()
		{
			ICodeGenerationContext context = GetContextWithTableParameterOnly();

			var data = A.Dummy<IQueryObjectData>();

			var ability = new StoredProcedureWithResultAsyncAbility();
			IQueryObjectData result = ability.Apply(context, data);

			ability.Name.ShouldBe("StoredProcedureAsync");

			result.Constants.ShouldBeEmpty();
			result.Nested.ShouldBeEmpty();
			result.Properties.ShouldBeEmpty();
			result.Fields.ShouldBeEmpty();

			string[] dependencies = ability.GetDependencies(context)?.ToArray();
			dependencies.ShouldNotBeNull();
			dependencies.Length.ShouldBe(3);
			dependencies.ShouldContain(KnownAbilityName.AddCustomParameter);
			dependencies.ShouldContain(KnownAbilityName.GetQueryText);
			dependencies.ShouldContain(KnownAbilityName.GetItemFromRecord);

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
			result.Methods.ShouldContain(AbilityArtifacts.StoredProcedure.StoredProcedureWithResultAsync_Parameters_Custom_Method_ExecAsync);
		}

		[Fact]
		public void StoredProcedureWithResultAsyncAbility_Parameters_Result_Empty_NoThrow_Test()
		{
			ICodeGenerationContext context = GetContextWithNoParametersNoResults();

			var data = A.Dummy<IQueryObjectData>();

			var ability = new StoredProcedureWithResultAsyncAbility();

			ability.Apply(context, data);
		}


		[Fact]
		public void StoredProcedureWithScalarResultAbility_Parameters_Simple_Test()
		{
			ICodeGenerationContext context = GetContextWithSimpleParameters();

			var data = A.Dummy<IQueryObjectData>();

			var ability = new StoredProcedureWithScalarResultAbility();
			IQueryObjectData result = ability.Apply(context, data);

			ability.Name.ShouldBe("StoredProcedure");

			result.Constants.ShouldBeEmpty();
			result.Nested.ShouldBeEmpty();
			result.Properties.ShouldBeEmpty();
			result.Fields.ShouldBeEmpty();

			string[] dependencies = ability.GetDependencies(context)?.ToArray();
			dependencies.ShouldNotBeNull();
			dependencies.Length.ShouldBe(3);
			dependencies.ShouldContain(KnownAbilityName.GetQueryText);
			dependencies.ShouldContain(KnownAbilityName.AddParameter);
			dependencies.ShouldContain(KnownAbilityName.GetScalarFromRecord);

			result.Usings.ShouldNotBeNull();
			result.Usings.Count().ShouldBe(4);
			result.Usings.ShouldContain("System");
			result.Usings.ShouldContain("System.Data");
			result.Usings.ShouldContain("System.Collections.Generic");
			result.Usings.ShouldContain("MySpecificDatabaseTypes");

			result.Methods.ShouldNotBeNull();
			result.Methods.Count().ShouldBe(1);
			result.Methods.ShouldContain(AbilityArtifacts.StoredProcedure.StoredProcedureWithScalarResult_Parameters_Simple_Method_Exec);
		}

		[Fact]
		public void StoredProcedureWithScalarResultAbility_Parameters_Empty_Test()
		{
			ICodeGenerationContext context = GetContextWithNoParameters();

			var data = A.Dummy<IQueryObjectData>();

			var ability = new StoredProcedureWithScalarResultAbility();
			IQueryObjectData result = ability.Apply(context, data);

			ability.Name.ShouldBe("StoredProcedure");

			result.Constants.ShouldBeEmpty();
			result.Nested.ShouldBeEmpty();
			result.Properties.ShouldBeEmpty();
			result.Fields.ShouldBeEmpty();

			string[] dependencies = ability.GetDependencies(context)?.ToArray();
			dependencies.ShouldNotBeNull();
			dependencies.Length.ShouldBe(2);
			dependencies.ShouldContain(KnownAbilityName.GetQueryText);
			dependencies.ShouldContain(KnownAbilityName.GetScalarFromRecord);

			result.Usings.ShouldNotBeNull();
			result.Usings.Count().ShouldBe(3);
			result.Usings.ShouldContain("System");
			result.Usings.ShouldContain("System.Data");
			result.Usings.ShouldContain("System.Collections.Generic");

			result.Methods.ShouldNotBeNull();
			result.Methods.Count().ShouldBe(1);
			result.Methods.ShouldContain(AbilityArtifacts.StoredProcedure.StoredProcedureWithScalarResult_Parameters_Empty_Method_Exec);
		}

		[Fact]
		public void StoredProcedureWithScalarResultAbility_Parameters_Mixed_Test()
		{
			ICodeGenerationContext context = GetContextWithMixedParameters();

			var data = A.Dummy<IQueryObjectData>();

			var ability = new StoredProcedureWithScalarResultAbility();
			IQueryObjectData result = ability.Apply(context, data);

			ability.Name.ShouldBe("StoredProcedure");

			result.Constants.ShouldBeEmpty();
			result.Nested.ShouldBeEmpty();
			result.Properties.ShouldBeEmpty();
			result.Fields.ShouldBeEmpty();

			string[] dependencies = ability.GetDependencies(context)?.ToArray();
			dependencies.ShouldNotBeNull();
			dependencies.Length.ShouldBe(4);
			dependencies.ShouldContain(KnownAbilityName.AddCustomParameter);
			dependencies.ShouldContain(KnownAbilityName.GetQueryText);
			dependencies.ShouldContain(KnownAbilityName.AddParameter);
			dependencies.ShouldContain(KnownAbilityName.GetScalarFromRecord);

			result.Usings.ShouldNotBeNull();
			result.Usings.Count().ShouldBe(4);
			result.Usings.ShouldContain("System");
			result.Usings.ShouldContain("System.Data");
			result.Usings.ShouldContain("MySpecificDatabaseTypes");
			result.Usings.ShouldContain("System.Collections.Generic");

			result.Methods.ShouldNotBeNull();
			result.Methods.Count().ShouldBe(1);
			result.Methods.ShouldContain(AbilityArtifacts.StoredProcedure.StoredProcedureWithScalarResult_Parameters_Mixed_Method_Exec);
		}

		[Fact]
		public void StoredProcedureWithScalarResultAbility_Parameters_Custom_Test()
		{
			ICodeGenerationContext context = GetContextWithTableParameterOnly();

			var data = A.Dummy<IQueryObjectData>();

			var ability = new StoredProcedureWithScalarResultAbility();
			IQueryObjectData result = ability.Apply(context, data);

			ability.Name.ShouldBe("StoredProcedure");

			result.Constants.ShouldBeEmpty();
			result.Nested.ShouldBeEmpty();
			result.Properties.ShouldBeEmpty();
			result.Fields.ShouldBeEmpty();

			string[] dependencies = ability.GetDependencies(context)?.ToArray();
			dependencies.ShouldNotBeNull();
			dependencies.Length.ShouldBe(3);
			dependencies.ShouldContain(KnownAbilityName.AddCustomParameter);
			dependencies.ShouldContain(KnownAbilityName.GetQueryText);
			dependencies.ShouldContain(KnownAbilityName.GetScalarFromRecord);

			result.Usings.ShouldNotBeNull();
			result.Usings.Count().ShouldBe(3);
			result.Usings.ShouldContain("System");
			result.Usings.ShouldContain("System.Data");
			result.Usings.ShouldContain("System.Collections.Generic");

			result.Methods.ShouldNotBeNull();
			result.Methods.Count().ShouldBe(1);
			result.Methods.ShouldContain(AbilityArtifacts.StoredProcedure.StoredProcedureWithScalarResult_Parameters_Custom_Method_Exec);
		}

		[Fact]
		public void StoredProcedureWithScalarResultAbility_Parameters_Result_Empty_Throw_Test()
		{
			ICodeGenerationContext context = GetContextWithNoParametersNoResults();

			var data = A.Dummy<IQueryObjectData>();

			var ability = new StoredProcedureWithScalarResultAbility();

			Assert.Throws<CodeGenerationException>(() => ability.Apply(context, data));
		}


		[Fact]
		public void StoredProcedureWithScalarResultAsyncAbility_Parameters_Simple_Test()
		{
			ICodeGenerationContext context = GetContextWithSimpleParameters();

			var data = A.Dummy<IQueryObjectData>();

			var ability = new StoredProcedureWithScalarResultAsyncAbility();
			IQueryObjectData result = ability.Apply(context, data);

			ability.Name.ShouldBe("StoredProcedureAsync");

			result.Constants.ShouldBeEmpty();
			result.Nested.ShouldBeEmpty();
			result.Properties.ShouldBeEmpty();
			result.Fields.ShouldBeEmpty();

			string[] dependencies = ability.GetDependencies(context)?.ToArray();
			dependencies.ShouldNotBeNull();
			dependencies.Length.ShouldBe(3);
			dependencies.ShouldContain(KnownAbilityName.GetQueryText);
			dependencies.ShouldContain(KnownAbilityName.AddParameter);
			dependencies.ShouldContain(KnownAbilityName.GetScalarFromRecord);

			result.Usings.ShouldNotBeNull();
			result.Usings.Count().ShouldBe(7);
			result.Usings.ShouldContain("System");
			result.Usings.ShouldContain("System.Data");
			result.Usings.ShouldContain("System.Collections.Generic");
			result.Usings.ShouldContain("MySpecificDatabaseTypes");
			result.Usings.ShouldContain("System.Data.Common");
			result.Usings.ShouldContain("System.Threading");
			result.Usings.ShouldContain("System.Threading.Tasks");

			result.Methods.ShouldNotBeNull();
			result.Methods.Count().ShouldBe(1);
			result.Methods.ShouldContain(AbilityArtifacts.StoredProcedure.StoredProcedureWithScalarResultAsync_Parameters_Simple_Method_ExecAsync);
		}

		[Fact]
		public void StoredProcedureWithScalarResultAsyncAbility_Parameters_Empty_Test()
		{
			ICodeGenerationContext context = GetContextWithNoParameters();

			var data = A.Dummy<IQueryObjectData>();

			var ability = new StoredProcedureWithScalarResultAsyncAbility();
			IQueryObjectData result = ability.Apply(context, data);

			ability.Name.ShouldBe("StoredProcedureAsync");

			result.Constants.ShouldBeEmpty();
			result.Nested.ShouldBeEmpty();
			result.Properties.ShouldBeEmpty();
			result.Fields.ShouldBeEmpty();

			string[] dependencies = ability.GetDependencies(context)?.ToArray();
			dependencies.ShouldNotBeNull();
			dependencies.Length.ShouldBe(2);
			dependencies.ShouldContain(KnownAbilityName.GetQueryText);
			dependencies.ShouldContain(KnownAbilityName.GetScalarFromRecord);

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
			result.Methods.ShouldContain(AbilityArtifacts.StoredProcedure.StoredProcedureWithScalarResultAsync_Parameters_Empty_Method_ExecAsync);
		}

		[Fact]
		public void StoredProcedureWithScalarResultAsyncAbility_Parameters_Mixed_Test()
		{
			ICodeGenerationContext context = GetContextWithMixedParameters();

			var data = A.Dummy<IQueryObjectData>();

			var ability = new StoredProcedureWithScalarResultAsyncAbility();
			IQueryObjectData result = ability.Apply(context, data);

			ability.Name.ShouldBe("StoredProcedureAsync");

			result.Constants.ShouldBeEmpty();
			result.Nested.ShouldBeEmpty();
			result.Properties.ShouldBeEmpty();
			result.Fields.ShouldBeEmpty();

			string[] dependencies = ability.GetDependencies(context)?.ToArray();
			dependencies.ShouldNotBeNull();
			dependencies.Length.ShouldBe(4);
			dependencies.ShouldContain(KnownAbilityName.AddCustomParameter);
			dependencies.ShouldContain(KnownAbilityName.GetQueryText);
			dependencies.ShouldContain(KnownAbilityName.AddParameter);
			dependencies.ShouldContain(KnownAbilityName.GetScalarFromRecord);

			result.Usings.ShouldNotBeNull();
			result.Usings.Count().ShouldBe(7);
			result.Usings.ShouldContain("System");
			result.Usings.ShouldContain("System.Data");
			result.Usings.ShouldContain("MySpecificDatabaseTypes");
			result.Usings.ShouldContain("System.Collections.Generic");
			result.Usings.ShouldContain("System.Data.Common");
			result.Usings.ShouldContain("System.Threading");
			result.Usings.ShouldContain("System.Threading.Tasks");

			result.Methods.ShouldNotBeNull();
			result.Methods.Count().ShouldBe(1);
			result.Methods.ShouldContain(AbilityArtifacts.StoredProcedure.StoredProcedureWithScalarResultAsync_Parameters_Mixed_Method_ExecAsync);
		}

		[Fact]
		public void StoredProcedureWithScalarResultAsyncAbility_Parameters_Custom_Test()
		{
			ICodeGenerationContext context = GetContextWithTableParameterOnly();

			var data = A.Dummy<IQueryObjectData>();

			var ability = new StoredProcedureWithScalarResultAsyncAbility();
			IQueryObjectData result = ability.Apply(context, data);

			ability.Name.ShouldBe("StoredProcedureAsync");

			result.Constants.ShouldBeEmpty();
			result.Nested.ShouldBeEmpty();
			result.Properties.ShouldBeEmpty();
			result.Fields.ShouldBeEmpty();

			string[] dependencies = ability.GetDependencies(context)?.ToArray();
			dependencies.ShouldNotBeNull();
			dependencies.Length.ShouldBe(3);
			dependencies.ShouldContain(KnownAbilityName.AddCustomParameter);
			dependencies.ShouldContain(KnownAbilityName.GetQueryText);
			dependencies.ShouldContain(KnownAbilityName.GetScalarFromRecord);

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
			result.Methods.ShouldContain(AbilityArtifacts.StoredProcedure.StoredProcedureWithScalarResultAsync_Parameters_Custom_Method_ExecAsync);
		}

		[Fact]
		public void StoredProcedureWithScalarResultAsyncAbility_Parameters_Result_Empty_Throw_Test()
		{
			ICodeGenerationContext context = GetContextWithNoParametersNoResults();

			var data = A.Dummy<IQueryObjectData>();

			var ability = new StoredProcedureWithScalarResultAsyncAbility();

			Assert.Throws<CodeGenerationException>(() => ability.Apply(context, data));
		}


		private static ICodeGenerationContext GetContextWithSimpleParameters()
		{
			ICodeGenerationContext result = CodeGenerationContextBuilder
											.Create(providerSpecificType: typeof(MySpecificDbType))
											.WithParameter(
												dbName: "FirstParam",
												semanticName: "FirstParam",
												dbType: "uniqueidentifier",
												clrType: typeof(Guid?),
												providerSpecificTypeElementName: "MySpecificGuidType")
											.WithParameter(
												dbName: "SECOND_Param",
												semanticName: "SECOND_Param",
												dbType: "int",
												clrType: typeof(int?),
												providerSpecificTypeElementName: "MySpecificIntType")
											.WithResultField(
												columnName: "FirstResult",
												dbType: "date",
												isNullable: false,
												clrType: typeof(DateTime))
											.Build();

			return result;
		}

		private static ICodeGenerationContext GetContextWithNoParameters()
		{
			ICodeGenerationContext result = CodeGenerationContextBuilder
											.Create(providerSpecificType: typeof(MySpecificDbType))
											.WithResultField(
												columnName: "FirstResult",
												dbType: "date",
												isNullable: false,
												clrType: typeof(DateTime))
											.Build();

			return result;
		}

		private static ICodeGenerationContext GetContextWithMixedParameters()
		{
			ICodeGenerationContext result = CodeGenerationContextBuilder
											.Create(providerSpecificType: typeof(MySpecificDbType))
											.WithParameter(
												dbName: "FirstParam",
												semanticName: "FirstParam",
												dbType: "uniqueidentifier",
												clrType: typeof(Guid?),
												providerSpecificTypeElementName: "MySpecificGuidType")
											.WithParameter(
												dbName: "SECOND_Param",
												semanticName: "SECOND_Param",
												dbType: "int",
												clrType: typeof(int?),
												providerSpecificTypeElementName: "MySpecificIntType")
											.WithTableParameter(
												dbName: "thirdParam",
												semanticName: "thirdParam",
												dbType: "sometabletype",
												setup: builder =>
													builder
														.WithBaseInfo(
															itemName: "SomeTableTypeItem",
															dbTypeDisplayedName: "SomeTableType",
															nullable: true)
														.WithField(
															columnName: "someColumnName",
															dbType: "int",
															clrType: typeof(int),
															isNullable: false,
															providerSpecificTypeElementName: "MySpecificIntType"))
											.WithResultField(
												columnName: "FirstResult",
												dbType: "date",
												isNullable: false,
												clrType: typeof(DateTime))
											.Build();

			return result;
		}

		private static ICodeGenerationContext GetContextWithTableParameterOnly()
		{
			ICodeGenerationContext result = CodeGenerationContextBuilder
											.Create(providerSpecificType: typeof(MySpecificDbType))
											.WithTableParameter(
												dbName: "FirstParam",
												semanticName: "FirstParam",
												dbType: "sometabletype",
												setup: builder =>
													builder
														.WithBaseInfo(
															itemName: "SomeTableTypeItem",
															dbTypeDisplayedName: "SomeTableType",
															nullable: true)
														.WithField(
															columnName: "someColumnName",
															dbType: "int",
															clrType: typeof(int),
															isNullable: false,
															providerSpecificTypeElementName: "MySpecificIntType"))
											.WithResultField(
												columnName: "FirstResult",
												dbType: "date",
												isNullable: false,
												clrType: typeof(DateTime))
											.Build();

			return result;
		}
		
		private static ICodeGenerationContext GetContextWithNoParametersNoResults()
		{
			ICodeGenerationContext result = CodeGenerationContextBuilder
											.Create(providerSpecificType: typeof(MySpecificDbType))
											.Build();

			return result;
		}
	}
}