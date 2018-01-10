using SqlFirst.Codegen.Text.Snippets.Items.Contents;
using SqlFirst.Codegen.Text.Snippets.Items.Types;
using SqlFirst.Codegen.Text.Templating;
using SqlFirst.Codegen.Trees;

namespace SqlFirst.Codegen.Text.Snippets.Items
{
	internal class ItemSnippet : SqlFirstSnippet
	{
		public ContentSnippet Content { get; } = new ContentSnippet();
		public TypeSnippet Type { get; } = new TypeSnippet();

		public IRenderableTemplate<IGeneratedItem> Item => new ItemRenderableTemplate(GetText(nameof(Item)));

		public ItemSnippet()
			: base("Items")
		{
		}
	}
}