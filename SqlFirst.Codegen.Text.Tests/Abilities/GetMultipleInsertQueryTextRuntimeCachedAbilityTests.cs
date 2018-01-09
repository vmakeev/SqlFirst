using System;
using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using Shouldly;
using SqlFirst.Codegen.Text.QueryObject.Abilities;
using SqlFirst.Codegen.Text.QueryObject.Abilities.Insert;
using SqlFirst.Codegen.Text.QueryObject.Data;
using SqlFirst.Core;
using Xunit;

namespace SqlFirst.Codegen.Text.Tests.Abilities
{
	public class GetMultipleInsertQueryTextRuntimeCachedAbilityTests
	{
		[Fact]
		public void GetMultipleInsertQueryTextRuntimeCachedAbility_Test()
		{
			ICodeGenerationContext context = GetDefaultCodeGenerationContext();

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
			result.Nested.ShouldContain(@"protected struct NumberedParameterInfo
{
	/// <summary>Initializes a new instance of the <see cref=""T:System.Object"" /> class.</summary>
	public NumberedParameterInfo(string dbName, string semanticName)
	{
		DbName = dbName;
		SemanticName = semanticName;
	}

	public string DbName { get; }
	public string SemanticName { get; }
}");
			result.Fields.ShouldNotBeNull();
			result.Fields.Count().ShouldBe(5);
			result.Fields.ShouldContain(@"protected static readonly Regex _numberedValueRegex = new Regex(NumberedValueRegexPattern, NumberedValueRegexOptions);");
			result.Fields.ShouldContain(@"protected static readonly Regex _balancedParenthesisRegex = new Regex(BalancedParenthesisRegexPattern, BalancedParenthesisRegexOptions);");
			result.Fields.ShouldContain(@"protected static readonly object _queryTemplatesLocker = new object();");
			result.Fields.ShouldContain(@"protected static string _cachedValuesTemplate;");
			result.Fields.ShouldContain(@"protected static string _cachedQueryTemplate;");

			result.Properties.ShouldBeEmpty();

			ability.GetDependencies().ShouldNotBeNull();
			ability.GetDependencies().Count().ShouldBe(1);
			ability.GetDependencies().ShouldContain(KnownAbilityName.GetQueryText);

			result.Usings.ShouldNotBeNull();
			result.Usings.Count().ShouldBe(4);
			result.Usings.ShouldContain("System");
			result.Usings.ShouldContain("System.Linq");
			result.Usings.ShouldContain("System.Collections.Generic");
			result.Usings.ShouldContain("System.Text.RegularExpressions");

			result.Methods.ShouldNotBeNull();
			result.Methods.Count().ShouldBe(4);
			result.Methods.ShouldContain(@"/// <summary>
/// Возвращает текст запроса для вставки <paramref name=""rowsCount"" /> записей
/// </summary>
/// <param name=""rowsCount"">Количество записей</param>
/// <returns>Текст запроса для вставки <paramref name=""rowsCount"" /> записей</returns>
private string GetQueryText(int rowsCount)
{
	GetQueryTemplates(GetQueryText(), out string queryTemplate, out string valuesTemplate);

	if (rowsCount <= 0)
	{
		throw new ArgumentOutOfRangeException(nameof(rowsCount), rowsCount, $""{nameof(rowsCount)} must be greater than zero."");
	}

	IEnumerable<string> rowTemplates = Enumerable.Range(0, rowsCount).Select(index => string.Format(valuesTemplate, index));
	string rowTemplatesString = string.Join($"",{Environment.NewLine}"", rowTemplates);

	string queryText = string.Format(queryTemplate, rowTemplatesString);
	return queryText;
}");
			result.Methods.ShouldContain(@"protected void GetQueryTemplates(string queryText, out string queryTemplate, out string valuesTemplate)
{
	if (string.IsNullOrEmpty(_cachedQueryTemplate) || string.IsNullOrEmpty(_cachedValuesTemplate))
	{
		lock (_queryTemplatesLocker)
		{
			string valuesSection = GetInsertedValuesSection(queryText);
			if (string.IsNullOrEmpty(valuesSection))
			{
				throw new Exception(""Unable to find inserted values in query text."");
			}

			string queryTemplateLocal = queryText.Replace(valuesSection, ""{0}"");
			string valuesTemplateLocal = valuesSection;

			IEnumerable<NumberedParameterInfo> numberedParameters = GetNumberedParameters(queryText);
			foreach (NumberedParameterInfo numberedParameter in numberedParameters)
			{
				valuesTemplateLocal = valuesTemplateLocal.Replace(numberedParameter.DbName, numberedParameter.SemanticName + ""_{0}"");
			}

			_cachedQueryTemplate = queryTemplateLocal;
			_cachedValuesTemplate = valuesTemplateLocal;
		}
	}

	queryTemplate = _cachedQueryTemplate;
	valuesTemplate = _cachedValuesTemplate;
}");
			result.Methods.ShouldContain(@"protected virtual IEnumerable<NumberedParameterInfo> GetNumberedParameters(string insertedValuesSection)
{
	MatchCollection matches = _numberedValueRegex.Matches(insertedValuesSection);
	if (matches.Count == 0)
	{
		yield break;
	}

	foreach (Match match in matches)
	{
		if (match.Success)
		{
			yield return new NumberedParameterInfo(match.Groups[""dbName""].Value, match.Groups[""semanticName""].Value);
		}
	}
}");
			result.Methods.ShouldContain(@"/// <summary>
/// Возвращает текст запроса для вставки <paramref name=""rowsCount"" /> записей
/// </summary>
/// <param name=""rowsCount"">Количество записей</param>
/// <returns>Текст запроса для вставки <paramref name=""rowsCount"" /> записей</returns>
private string GetQueryText(int rowsCount)
{
	GetQueryTemplates(GetQueryText(), out string queryTemplate, out string valuesTemplate);

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

		private static ICodeGenerationContext GetDefaultCodeGenerationContext()
		{
			var mapper = A.Fake<IDatabaseTypeMapper>(p => p.Strict());
			A.CallTo(() => mapper.MapToClrType("uniqueidentifier", true)).Returns(typeof(Guid?));
			A.CallTo(() => mapper.MapToClrType("int", true)).Returns(typeof(int?));
			A.CallTo(() => mapper.MapToClrType("int", false)).Returns(typeof(int));

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