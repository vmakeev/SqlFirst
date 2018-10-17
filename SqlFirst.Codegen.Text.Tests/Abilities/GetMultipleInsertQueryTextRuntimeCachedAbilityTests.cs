using System;
using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using Shouldly;
using SqlFirst.Codegen.Text.QueryObject.Abilities;
using SqlFirst.Codegen.Text.QueryObject.Abilities.Insert;
using SqlFirst.Codegen.Text.QueryObject.Data;
using SqlFirst.Codegen.Text.Tests.Fixtures;
using SqlFirst.Core;
using Xunit;

namespace SqlFirst.Codegen.Text.Tests.Abilities
{
	public class GetMultipleInsertQueryTextRuntimeCachedAbilityTests
	{
		private IQueryParamInfo GetQueryParamInfo(bool isComplexType)
		{
			var result = A.Fake<IQueryParamInfo>(p => p.Strict());
			A.CallTo(() => result.IsComplexType).Returns(isComplexType);
			return result;
		}

		[Fact]
		public void GetMultipleInsertQueryTextRuntimeCachedAbility_Test()
		{
			ICodeGenerationContext context = GetDefaultCodeGenerationContext();
			A.CallTo(() => context.IncomingParameters).Returns(new[] { GetQueryParamInfo(true), GetQueryParamInfo(false) });

			var data = A.Dummy<IQueryObjectData>();

			var ability = new GetMultipleInsertQueryTextRuntimeCachedAbility();
			IQueryObjectData result = ability.Apply(context, data);

			ability.Name.ShouldBe("GetQueryTextMultipleInsert");

			result.Constants.ShouldNotBeNull();
			result.Constants.Count().ShouldBe(4);
			result.Constants.ShouldContain(@"private const RegexOptions NumberedValueRegexOptions = RegexOptions.CultureInvariant | RegexOptions.Compiled | RegexOptions.IgnoreCase;");
			result.Constants.ShouldContain(@"private const string NumberedValueRegexPattern = @""\@(?<dbName>(?<semanticName>[a-zA-Z0-9_]+)_N)"";");
			result.Constants.ShouldContain(@"private const RegexOptions BalancedParenthesisRegexOptions = RegexOptions.CultureInvariant | RegexOptions.Compiled | RegexOptions.IgnoreCase;");
			result.Constants.ShouldContain(
				@"private const string BalancedParenthesisRegexPattern = @""values[^\(]*(?<valueTemplate>\([^\(\)]*(((?<Open>\()[^\(\)]*)+((?<Close-Open>\))[^\(\)]*)+)*(?(Open)(?!))\))"";");

			result.Nested.ShouldNotBeNull();
			result.Nested.Count().ShouldBe(1);
			result.Nested.ShouldContain(AbilityArtifacts.Insert.GetMultipleInsertQueryTextRuntimeCachedAbility_Type_NumberedParameterInfo);
			result.Fields.ShouldNotBeNull();
			result.Fields.Count().ShouldBe(5);
			result.Fields.ShouldContain(@"protected static readonly Regex _numberedValueRegex = new Regex(NumberedValueRegexPattern, NumberedValueRegexOptions);");
			result.Fields.ShouldContain(@"protected static readonly Regex _balancedParenthesisRegex = new Regex(BalancedParenthesisRegexPattern, BalancedParenthesisRegexOptions);");
			result.Fields.ShouldContain(@"protected static readonly object _queryTemplatesLocker = new object();");
			result.Fields.ShouldContain(@"protected static string _cachedValuesTemplate;");
			result.Fields.ShouldContain(@"protected static string _cachedQueryTemplate;");

			result.Properties.ShouldBeEmpty();

			ability.GetDependencies(context).ShouldNotBeNull();
			ability.GetDependencies(context).Count().ShouldBe(1);
			ability.GetDependencies(context).ShouldContain(KnownAbilityName.GetQueryText);

			result.Usings.ShouldNotBeNull();
			result.Usings.Count().ShouldBe(4);
			result.Usings.ShouldContain("System");
			result.Usings.ShouldContain("System.Linq");
			result.Usings.ShouldContain("System.Collections.Generic");
			result.Usings.ShouldContain("System.Text.RegularExpressions");

			result.Methods.ShouldNotBeNull();
			result.Methods.Count().ShouldBe(4);
			result.Methods.ShouldContain(AbilityArtifacts.Insert.GetMultipleInsertQueryTextRuntimeCachedAbility_Method_GetQueryText_MultipleRows);
			result.Methods.ShouldContain(AbilityArtifacts.Insert.GetMultipleInsertQueryTextRuntimeCachedAbility_Method_GetQueryTemplates);
			result.Methods.ShouldContain(AbilityArtifacts.Insert.GetMultipleInsertQueryTextRuntimeCachedAbility_Method_GetNumberedParameters);
			result.Methods.ShouldContain(AbilityArtifacts.Insert.GetMultipleInsertQueryTextRuntimeCachedAbility_Method_GetQueryText);
		}

		private static ICodeGenerationContext GetDefaultCodeGenerationContext()
		{
			var mapper = A.Fake<IDatabaseTypeMapper>(p => p.Strict());
			A.CallTo(() => mapper.MapToClrType("uniqueidentifier", true, A<IDictionary<string, object>>._)).Returns(typeof(Guid?));
			A.CallTo(() => mapper.MapToClrType("int", true, A<IDictionary<string, object>>._)).Returns(typeof(int?));
			A.CallTo(() => mapper.MapToClrType("int", false, A<IDictionary<string, object>>._)).Returns(typeof(int));

			var firstParameter = A.Fake<IQueryParamInfo>(p => p.Strict());
			A.CallTo(() => firstParameter.DbName).Returns("FirstParam");
			A.CallTo(() => firstParameter.IsNumbered).Returns(false);
			A.CallTo(() => firstParameter.SemanticName).Returns("FirstParam");
			A.CallTo(() => firstParameter.DbType).Returns("uniqueidentifier");

			var secondParameter = A.Fake<IQueryParamInfo>(p => p.Strict());
			A.CallTo(() => secondParameter.DbName).Returns("SECOND_Param_N");
			A.CallTo(() => secondParameter.IsNumbered).Returns(true);
			A.CallTo(() => secondParameter.SemanticName).Returns("SECOND_Param");
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
			A.CallTo(() => options.TryGetValue("QueryText", out _))
			.Returns(true)
			.AssignsOutAndRefParametersLazily((string a, object b) => new object[] { "insert into sometable (id, number) values (@FirstParam, @SECOND_Param_N)" });

			A.CallTo(() => context.Options).Returns(options);

			return context;
		}
	}
}