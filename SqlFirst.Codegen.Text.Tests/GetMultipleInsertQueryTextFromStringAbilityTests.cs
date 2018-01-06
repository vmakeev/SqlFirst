using System;
using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using Shouldly;
using SqlFirst.Codegen.Text.QueryObject.Abilities.Insert;
using SqlFirst.Codegen.Text.QueryObject.Data;
using SqlFirst.Core;
using Xunit;

namespace SqlFirst.Codegen.Text.Tests
{
	public class GetMultipleInsertQueryTextFromStringAbilityTests
	{
		private static ICodeGenerationContext GetDefaultCodeGenerationContext()
		{
			var mapper = A.Fake<IDatabaseTypeMapper>(p => p.Strict());
			A.CallTo(() => mapper.Map("uniqueidentifier", true)).Returns(typeof(Guid?));
			A.CallTo(() => mapper.Map("int", true)).Returns(typeof(int?));
			A.CallTo(() => mapper.Map("int", false)).Returns(typeof(int));

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

		[Fact]
		public void GetMultipleInsertQueryTextFromStringAbility_Test()
		{
			ICodeGenerationContext context = GetDefaultCodeGenerationContext();

			var data = A.Dummy<IQueryObjectData>();

			var ability = new GetMultipleInsertQueryTextFromStringAbility();
			IQueryObjectData result = ability.Apply(context, data);

			ability.Name.ShouldBe("GetQueryTextMultipleInsert");

			result.Constants.ShouldBeEmpty();
			result.Nested.ShouldBeEmpty();
			result.Properties.ShouldBeEmpty();
			result.Fields.ShouldBeEmpty();

			ability.GetDependencies().ShouldNotBeNull();
			ability.GetDependencies().Count().ShouldBe(0);

			result.Usings.ShouldNotBeNull();
			result.Usings.Count().ShouldBe(3);
			result.Usings.ShouldContain("System");
			result.Usings.ShouldContain("System.Linq");
			result.Usings.ShouldContain("System.Collections.Generic");

			result.Methods.ShouldNotBeNull();
			result.Methods.Count().ShouldBe(1);
			result.Methods.ShouldContain(@"/// <summary>
/// Возвращает текст запроса для вставки <paramref name=""rowsCount""/> записей
/// </summary>
/// <param name=""rowsCount"">Количество записей</param>
/// <returns>Текст запроса для вставки <paramref name=""rowsCount""/> записей</returns>
private string GetQueryText(int rowsCount)
{
	const string queryTemplate = @""insert into sometable (id, number) values {0}"";
	const string valuesTemplate = @""(@FirstParam, @SECOND_Param_{0})"";

	if (rowsCount <= 0)
	{
		throw new ArgumentOutOfRangeException(nameof(rowsCount), rowsCount, $""{nameof(rowsCount)} must be greater than zero."");
	}

	IEnumerable<string> rowTemplates = Enumerable.Range(0, rowsCount).Select(index => string.Format(valuesTemplate, index));
	string rowTemplatesString = string.Join($"",{Environment.NewLine}"", rowTemplates);

	string queryText = string.Format(queryTemplate, rowTemplatesString);
	return queryText;
}");
		}
	}
}