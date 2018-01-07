using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using SqlFirst.Codegen.Helpers;
using SqlFirst.Codegen.Text.QueryObject.Data;
using SqlFirst.Codegen.Text.Snippets;

namespace SqlFirst.Codegen.Text.QueryObject.Abilities.Insert
{
	internal class GetMultipleInsertQueryTextRuntimeCachedAbility : IQueryObjectAbility
	{
		/// <inheritdoc />
		public IQueryObjectData Apply(ICodeGenerationContext context, IQueryObjectData data)
		{
			string queryText = context.GetQueryText();
			if (string.IsNullOrEmpty(queryText))
			{
				throw new CodeGenerationException("Can not find query text at current code generation context.");
			}

			IEnumerable<string> methods = GetRequiredMethods();
			IEnumerable<string> nested = GetNestedClasses();
			IEnumerable<string> fields = GetFields();
			IEnumerable<string> constants = GetConstants();

			QueryObjectData result = QueryObjectData.CreateFrom(data);
			result.Constants = result.Constants.Concat(constants);
			result.Fields = result.Fields.Concat(fields);
			result.Nested = result.Nested.Concat(nested);
			result.Methods = result.Methods.Concat(methods);
			result.Usings = result.Usings.Append(
				"System",
				"System.Collections.Generic",
				"System.Linq",
				"System.Text.RegularExpressions");
			return result;
		}

		private IEnumerable<string> GetFields()
		{
			string protectedModifiers = string.Join(" ", Modifiers.Protected, Modifiers.Static);
			string protectedReadonlyModifiers = string.Join(" ", Modifiers.Protected, Modifiers.Static, Modifiers.Readonly);
			string stringType = CSharpCodeHelper.GetTypeBuiltInName(typeof(string));
			string regexType = CSharpCodeHelper.GetTypeBuiltInName(typeof(Regex));
			string objectType = CSharpCodeHelper.GetTypeBuiltInName(typeof(object));

			var cachedQueryTemplate = new StringBuilder(FieldSnippet.Field);
			cachedQueryTemplate.Replace("$Modificators$", protectedModifiers);
			cachedQueryTemplate.Replace("$Type$", stringType);
			cachedQueryTemplate.Replace("$Name$", "_cachedQueryTemplate");
			yield return cachedQueryTemplate.ToString();

			var cachedValuesTemplate = new StringBuilder(FieldSnippet.Field);
			cachedValuesTemplate.Replace("$Modificators$", protectedModifiers);
			cachedValuesTemplate.Replace("$Type$", stringType);
			cachedValuesTemplate.Replace("$Name$", "_cachedValuesTemplate");
			yield return cachedValuesTemplate.ToString();

			var queryTemplatesLocker = new StringBuilder(FieldSnippet.FieldWithValue);
			queryTemplatesLocker.Replace("$Modificators$", protectedReadonlyModifiers);
			queryTemplatesLocker.Replace("$Type$", objectType);
			queryTemplatesLocker.Replace("$Name$", "_queryTemplatesLocker");
			queryTemplatesLocker.Replace("$Value$", "new object()");
			yield return queryTemplatesLocker.ToString();

			var balancedParenthesisRegex = new StringBuilder(FieldSnippet.FieldWithValue);
			balancedParenthesisRegex.Replace("$Modificators$", protectedReadonlyModifiers);
			balancedParenthesisRegex.Replace("$Type$", regexType);
			balancedParenthesisRegex.Replace("$Name$", "_balancedParenthesisRegex");
			balancedParenthesisRegex.Replace("$Value$", "new Regex(BalancedParenthesisRegexPattern, BalancedParenthesisRegexOptions)");
			yield return balancedParenthesisRegex.ToString();

			var numberedValueRegex = new StringBuilder(FieldSnippet.FieldWithValue);
			numberedValueRegex.Replace("$Modificators$", protectedReadonlyModifiers);
			numberedValueRegex.Replace("$Type$", regexType);
			numberedValueRegex.Replace("$Name$", "_numberedValueRegex");
			numberedValueRegex.Replace("$Value$", "new Regex(NumberedValueRegexPattern, NumberedValueRegexOptions)");
			yield return numberedValueRegex.ToString();
		}

		private IEnumerable<string> GetConstants()
		{
			string modifiers = Modifiers.Private;
			string stringType = CSharpCodeHelper.GetTypeBuiltInName(typeof(string));
			string regexOptionsType = CSharpCodeHelper.GetTypeBuiltInName(typeof(RegexOptions));

			var bpRegexTemplate = new StringBuilder(FieldSnippet.Const);
			bpRegexTemplate.Replace("$Modificators$", modifiers);
			bpRegexTemplate.Replace("$Type$", stringType);
			bpRegexTemplate.Replace("$Name$", "BalancedParenthesisRegexPattern");
			// @"values[^\(]*(?<valueTemplate>\([^\(\)]*(((?<Open>\()[^\(\)]*)+((?<Close-Open>\))[^\(\)]*)+)*(?(Open)(?!))\))"
			bpRegexTemplate.Replace("$Value$", "@\"values[^\\(]*(?<valueTemplate>\\([^\\(\\)]*(((?<Open>\\()[^\\(\\)]*)+((?<Close-Open>\\))[^\\(\\)]*)+)*(?(Open)(?!))\\))\"");
			yield return bpRegexTemplate.ToString();

			var bpRegexOptions = new StringBuilder(FieldSnippet.Const);
			bpRegexOptions.Replace("$Modificators$", modifiers);
			bpRegexOptions.Replace("$Type$", regexOptionsType);
			bpRegexOptions.Replace("$Name$", "BalancedParenthesisRegexOptions");
			bpRegexOptions.Replace("$Value$", "RegexOptions.CultureInvariant | RegexOptions.Compiled | RegexOptions.IgnoreCase");
			yield return bpRegexOptions.ToString();

			var nvRegexTemplate = new StringBuilder(FieldSnippet.Const);
			nvRegexTemplate.Replace("$Modificators$", modifiers);
			nvRegexTemplate.Replace("$Type$", stringType);
			nvRegexTemplate.Replace("$Name$", "NumberedValueRegexPattern");
			// @"\@(?<dbName>(?<semanticName>[a-zA-Z0-9_]+)_N)"
			nvRegexTemplate.Replace("$Value$", "@\"\\@(?<dbName>(?<semanticName>[a-zA-Z0-9_]+)_N)\"");
			yield return nvRegexTemplate.ToString();

			var nvRegexOptions = new StringBuilder(FieldSnippet.Const);
			nvRegexOptions.Replace("$Modificators$", modifiers);
			nvRegexOptions.Replace("$Type$", regexOptionsType);
			nvRegexOptions.Replace("$Name$", "NumberedValueRegexOptions");
			nvRegexOptions.Replace("$Value$", "RegexOptions.CultureInvariant | RegexOptions.Compiled | RegexOptions.IgnoreCase");
			yield return nvRegexOptions.ToString();
		}

		private IEnumerable<string> GetNestedClasses()
		{
			yield return QuerySnippet.Methods.Common.Snippets.NumberedParameterInfo;
		}

		private static IEnumerable<string> GetRequiredMethods()
		{
			yield return QuerySnippet.Methods.Common.Snippets.GetInsertedValuesSection;
			yield return QuerySnippet.Methods.Common.Snippets.GetNumberedParameters;
			yield return QuerySnippet.Methods.Common.Snippets.GetQueryTemplates;
			yield return QuerySnippet.Methods.Common.GetQueryRuntimeGeneratedMultipleInsert;
		}

		/// <inheritdoc />
		public IEnumerable<string> GetDependencies()
		{
			yield return KnownAbilityName.GetQueryText;
		}

		/// <inheritdoc />
		public string Name { get; } = KnownAbilityName.GetQueryTextMultipleInsert;
	}
}