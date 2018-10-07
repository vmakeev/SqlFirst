using System;
using System.Collections.Generic;
using System.Linq;
using SqlFirst.Codegen.Text.ParameterItem;
using SqlFirst.Codegen.Text.QueryObject;
using SqlFirst.Codegen.Text.ResultItem;
using SqlFirst.Codegen.Text.ResultItem.TypedOptions;
using SqlFirst.Codegen.Text.Snippets;
using SqlFirst.Codegen.Text.Templating;
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
		public IEnumerable<IGeneratedParameterItem> GenerateParameterItems(ICodeGenerationContext context, IParameterGenerationOptions options)
		{
			var itemOptions = new ParameterItemOptions(options.SqlFirstOptions?.ToArray());

			ParameterItemGeneratorBase itemGenerator = ParameterItemGeneratorFactory.Build(itemOptions);
			return itemGenerator.GenerateParameterItems(context);
		}

		/// <inheritdoc />
		public string GenerateFile(IEnumerable<IGeneratedItem> generatedItems)
		{
			IGeneratedItem[] items = generatedItems.ToArray();

			IRenderableTemplate usingSnippet = Snippet.File.Using;

			IEnumerable<IRenderable> usings = items
											.SelectMany(generatedItem => generatedItem.Usings)
											.Distinct(StringComparer.InvariantCulture)
											.OrderBy(usingName => usingName)
											.Select(usingName => Renderable.Create(usingSnippet, new { Using = usingName }));

			IRenderableTemplate<IGeneratedItem> itemTemplate = Snippet.Item.Item;
			IGrouping<string, (string Namespace, IRenderable Item)>[] namespaceDataItems = items
																					.Select(generatedItem => (Namespace: generatedItem.Namespace, Renderable.Create(itemTemplate, generatedItem)))
																					.GroupBy(item => item.Namespace).ToArray();

			var namespaces = new LinkedList<IRenderable>();

			IRenderableTemplate namespaceTemplate = Snippet.File.Namespace;
			foreach (IGrouping<string, (string Namespace, IRenderable Item)> namespaceDataItem in namespaceDataItems)
			{
				string namespaceName = namespaceDataItem.Key;
				IEnumerable<IRenderable> dataItems = namespaceDataItem.Select(p => p.Item);

				var model = new
				{
					Namespace = namespaceName,
					Data = dataItems
				};

				namespaces.AddLast(Renderable.Create(namespaceTemplate, model));
			}

			string result = Snippet.File.DefaultFile.Render(new
			{
				Usings = usings,
				Namespaces = namespaces
			});

			return result;
		}
	}
}