using SqlFirst.Codegen.Text.Templating;
using SqlFirst.Codegen.Trees;

namespace SqlFirst.Codegen.Text.Snippets.Items
{
	internal class ItemRenderableTemplate : RenderableTemplate<IGeneratedItem>
	{
		/// <inheritdoc />
		public ItemRenderableTemplate(string template)
			: base(template)
		{
		}

		/// <inheritdoc />
		protected override string GetValue(object modelValue, string enumerableDelimiter)
		{
			if (modelValue is IGeneratedType generatedType)
			{
				return Snippet.Item.Type.GeneratedType.Render(generatedType);
			}

			return base.GetValue(modelValue, enumerableDelimiter);
		}

	}
}
