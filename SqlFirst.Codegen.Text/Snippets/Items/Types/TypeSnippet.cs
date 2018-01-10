using SqlFirst.Codegen.Text.Templating;

namespace SqlFirst.Codegen.Text.Snippets.Items.Types
{
	internal class TypeSnippet : SqlFirstSnippet
	{
		public IRenderableTemplate Type => GetRenderableTemplate();

		public GeneratedTypeTemplate GeneratedType { get; } = new GeneratedTypeTemplate();

		public TypeSnippet()
			: base("Items.Types")
		{
		}
	}
}