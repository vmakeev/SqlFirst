using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SqlFirst.Codegen.Text.ParameterItem;
using SqlFirst.Codegen.Text.QueryObject;
using SqlFirst.Codegen.Text.ResultItem;
using SqlFirst.Codegen.Text.ResultItem.TypedOptions;
using SqlFirst.Codegen.Text.Snippets;
using SqlFirst.Codegen.Trees;

namespace SqlFirst.Codegen.Text
{
	/// <summary>
	/// Генератор кода на основе текстовых шаблонов
	/// </summary>
	public class TextCodeGenerator : ICodeGenerator
	{
		/// <inheritdoc />
		public IGeneratedQueryObject GenerateQueryObject(ICodeGenerationContext context, IQueryGenerationOptions options)
		{
			var queryGenerator = new QueryObjectGenerator();
			return queryGenerator.GenerateQueryObject(context, options);
		}

		/// <inheritdoc />
		public IGeneratedResultItem GenerateResultItem(ICodeGenerationContext context, IResultGenerationOptions options)
		{
			var itemOptions = new ResultItemOptions(options.SqlFirstOptions?.ToArray());

			ResultItemGeneratorBase itemGenerator = ResultItemGeneratorFactory.Build(itemOptions);
			return itemGenerator.GenerateResultItem(context);
		}

		/// <inheritdoc />
		public IGeneratedParameterItem GenerateParameterItem(ICodeGenerationContext context, IParameterGenerationOptions options)
		{
			var itemOptions = new ParameterItemOptions(options.SqlFirstOptions?.ToArray());

			ParameterItemGeneratorBase itemGenerator = ParameterItemGeneratorFactory.Build(itemOptions);
			return itemGenerator.GenerateParameterItem(context);
		}

		/// <inheritdoc />
		public string GenerateFile(IEnumerable<IGeneratedItem> generatedItems)
		{
			IGeneratedItem[] items = generatedItems.ToArray();

			string usingSnippet = FileSnippet.Using;

			IEnumerable<string> usings = items
				.SelectMany(generatedItem => generatedItem.Usings)
				.Distinct(StringComparer.InvariantCultureIgnoreCase)
				.OrderBy(usingName => usingName)
				.Select(usingName => usingSnippet.Replace("$Using$", usingName));

			string usingsText = FileSnippet.Usings.Replace("$Usings$", string.Join(string.Empty, usings)).Trim();

			IGrouping<string, (string Namespace, string Data)>[] namespaceDataItems = items
				.Select(generatedItem => (Namespace: generatedItem.Namespace, Data: generatedItem.Item))
				.GroupBy(item => item.Namespace).ToArray();
			
			var namespaces = new StringBuilder();
			string namespaceSnippet = FileSnippet.Namespace;
			foreach (IGrouping<string, (string Namespace, string Data)> namespaceDataItem in namespaceDataItems)
			{
				string namespaceName = namespaceDataItem.Key;
				string[] data = namespaceDataItem.Select(p => p.Data).ToArray();

				string namespaceData = string.Join(Environment.NewLine + Environment.NewLine, data.Select(p => p.Indent(QuerySnippet.Indent, 1)));

				string namespaceText = namespaceSnippet
										.Replace("$Namespace$", namespaceName)
										.Replace("$Data$", namespaceData);

				namespaces.Append(namespaceText);
			}

			string namespacesText = namespaces.ToString();

			string result = FileSnippet.DefaultFile
				.Replace("$Usings$", usingsText)
				.Replace("$Namespaces$", namespacesText)
				.Trim();

			return result;
		}
	}
}