using System.Collections.Generic;
using System.Linq;
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
			result.Usings = result.Usings.AppendItems(
				"System",
				"System.Collections.Generic",
				"System.Linq",
				"System.Text.RegularExpressions");
			return result;
		}

		/// <inheritdoc />
		public IEnumerable<string> GetDependencies(ICodeGenerationContext context)
		{
			yield return KnownAbilityName.GetQueryText;
		}

		/// <inheritdoc />
		public string Name { get; } = KnownAbilityName.GetQueryTextMultipleInsert;

		private IEnumerable<string> GetFields()
		{
			string[] protectedModifiers = { Modifiers.Protected, Modifiers.Static };
			string[] protectedReadonlyModifiers = { Modifiers.Protected, Modifiers.Static, Modifiers.Readonly };

			string stringType = CSharpCodeHelper.GetTypeBuiltInName(typeof(string));
			string regexType = CSharpCodeHelper.GetTypeBuiltInName(typeof(Regex));
			string objectType = CSharpCodeHelper.GetTypeBuiltInName(typeof(object));

			yield return Snippet.Field.Field.Render(protectedModifiers, stringType, "_cachedQueryTemplate");
			yield return Snippet.Field.Field.Render(protectedModifiers, stringType, "_cachedValuesTemplate");

			yield return Snippet.Field.FieldWithValue.Render(protectedReadonlyModifiers, objectType, "_queryTemplatesLocker", "new object()");
			yield return Snippet.Field.FieldWithValue.Render(protectedReadonlyModifiers, regexType, "_balancedParenthesisRegex",
				"new Regex(BalancedParenthesisRegexPattern, BalancedParenthesisRegexOptions)");
			yield return Snippet.Field.FieldWithValue.Render(protectedReadonlyModifiers, regexType, "_numberedValueRegex",
				"new Regex(NumberedValueRegexPattern, NumberedValueRegexOptions)");
		}

		private IEnumerable<string> GetConstants()
		{
			string modifiers = Modifiers.Private;
			string stringType = CSharpCodeHelper.GetTypeBuiltInName(typeof(string));
			string regexOptionsType = CSharpCodeHelper.GetTypeBuiltInName(typeof(RegexOptions));

			yield return Snippet.Field.Const.Render(
				name: "BalancedParenthesisRegexPattern",
				modifiers: modifiers,
				type: stringType,
				value: "@\"values[^\\(]*(?<valueTemplate>\\([^\\(\\)]*(((?<Open>\\()[^\\(\\)]*)+((?<Close-Open>\\))[^\\(\\)]*)+)*(?(Open)(?!))\\))\"");

			yield return Snippet.Field.Const.Render(
				name: "BalancedParenthesisRegexOptions",
				modifiers: modifiers,
				type: regexOptionsType,
				value: "RegexOptions.CultureInvariant | RegexOptions.Compiled | RegexOptions.IgnoreCase");

			yield return Snippet.Field.Const.Render(
				name: "NumberedValueRegexPattern",
				modifiers: modifiers,
				type: stringType,
				value: "@\"\\@(?<dbName>(?<semanticName>[a-zA-Z0-9_]+)_N)\"");

			yield return Snippet.Field.Const.Render(
				name: "NumberedValueRegexOptions",
				modifiers: modifiers,
				type: regexOptionsType,
				value: "RegexOptions.CultureInvariant | RegexOptions.Compiled | RegexOptions.IgnoreCase");
		}

		private IEnumerable<string> GetNestedClasses()
		{
			yield return Snippet.Query.Methods.Common.Snippets.NumberedParameterInfo.Render();
		}

		private static IEnumerable<string> GetRequiredMethods()
		{
			yield return Snippet.Query.Methods.Common.Snippets.GetInsertedValuesSection.Render();
			yield return Snippet.Query.Methods.Common.Snippets.GetNumberedParameters.Render();
			yield return Snippet.Query.Methods.Common.Snippets.GetQueryTemplates.Render();
			yield return Snippet.Query.Methods.Common.GetQueryRuntimeGeneratedMultipleInsert.Render();
		}
	}
}