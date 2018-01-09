using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using SqlFirst.Codegen.Text.QueryObject.Data;
using SqlFirst.Codegen.Text.Snippets;
using SqlFirst.Core;
using SqlFirst.Core.Impl;

namespace SqlFirst.Codegen.Text.QueryObject.Abilities.Insert
{
	internal class GetMultipleInsertQueryTextPrecompiledAbility : IQueryObjectAbility
	{
		private const string BalancedParenthesisRegexPattern = @"values[^\(]*(?<valueTemplate>\([^\(\)]*(((?<Open>\()[^\(\)]*)+((?<Close-Open>\))[^\(\)]*)+)*(?(Open)(?!))\))";
		private const RegexOptions BalancedParenthesisRegexOptions = RegexOptions.CultureInvariant | RegexOptions.Compiled | RegexOptions.IgnoreCase;
		private static readonly Regex _balancedParenthesisRegex = new Regex(BalancedParenthesisRegexPattern, BalancedParenthesisRegexOptions);

		private string GetInsertedValuesSection(string query)
		{
			MatchCollection matches = _balancedParenthesisRegex.Matches(query);
			if (matches.Count == 0)
			{
				return null;
			}

			Match last = matches[matches.Count - 1];
			string result = last.Groups["valueTemplate"].Value;
			return result;
		}

		private (string queryTemplate, string valuesTemplate) GetQueryTemplates(ICodeGenerationContext context, string queryText)
		{
			string valuesSection = GetInsertedValuesSection(queryText);
			if (string.IsNullOrEmpty(valuesSection))
			{
				throw new CodeGenerationException("Unable to find inserted values in query text.");
			}

			string queryTemplate = queryText.Replace(valuesSection, "{0}");
			string valuesTemplate = valuesSection;

			IQueryParamInfo[] numberedParameters = context.IncomingParameters.Where(p => p.IsNumbered).ToArray();
			foreach (IQueryParamInfo numberedParameter in numberedParameters)
			{
				valuesTemplate = valuesTemplate.Replace(numberedParameter.DbName, QueryParamInfoNameHelper.GetNumberedNameTemplate(numberedParameter.SemanticName));
			}

			return (queryTemplate, valuesTemplate);
		}

		/// <inheritdoc />
		public IQueryObjectData Apply(ICodeGenerationContext context, IQueryObjectData data)
		{
			string queryText = context.GetQueryText();
			if (string.IsNullOrEmpty(queryText))
			{
				throw new CodeGenerationException("Can not find query text at current code generation context.");
			}

			(string queryTemplate, string valuesTemplate) = GetQueryTemplates(context, queryText);

			string method = Snippet.Query.Methods.Common.GetQueryFromStringMultipleInsert.Render(new
			{
				QueryTemplate = queryTemplate,
				ValuesTemplate = valuesTemplate
			});

			QueryObjectData result = QueryObjectData.CreateFrom(data);
			result.Methods = result.Methods.AppendItems(method);
			result.Usings = result.Usings.AppendItems(
				"System",
				"System.Collections.Generic",
				"System.Linq");
			return result;
		}

		/// <inheritdoc />
		public IEnumerable<string> GetDependencies() => Enumerable.Empty<string>();

		/// <inheritdoc />
		public string Name { get; } = KnownAbilityName.GetQueryTextMultipleInsert;
	}
}
