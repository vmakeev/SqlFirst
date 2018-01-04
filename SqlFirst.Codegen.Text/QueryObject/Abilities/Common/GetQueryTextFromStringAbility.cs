using System.Collections.Generic;
using System.Linq;
using System.Text;
using SqlFirst.Codegen.Text.QueryObject.Data;
using SqlFirst.Codegen.Text.Snippets;

namespace SqlFirst.Codegen.Text.QueryObject.Abilities.Common
{
	internal class GetQueryTextFromStringAbility : IQueryObjectAbility
	{
		/// <inheritdoc />
		public IQueryObjectData Apply(ICodeGenerationContext context, IQueryObjectData data)
		{
			string queryText = context.GetQueryText();
			if (string.IsNullOrEmpty(queryText))
			{
				throw new CodeGenerationException("Can not find query text at current code generation context.");
			}

			string preparedQueryText = queryText
				.Replace("\"", "\"\"")
				.Replace(@"\", @"\\");

			var methodBuilder = new StringBuilder(QuerySnippet.Methods.Common.GetQueryFromString);
			methodBuilder.Replace("$QueryText$", preparedQueryText);

			QueryObjectData result = QueryObjectData.CreateFrom(data);
			result.Methods = result.Methods.Append(methodBuilder.ToString());
			return result;
		}

		/// <inheritdoc />
		public IEnumerable<string> GetDependencies() => Enumerable.Empty<string>();

		/// <inheritdoc />
		public string Name { get; } = KnownAbilityName.GetQueryText;
	}
}