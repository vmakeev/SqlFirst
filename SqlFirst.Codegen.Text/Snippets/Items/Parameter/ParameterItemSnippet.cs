using SqlFirst.Codegen.Text.Templating;

namespace SqlFirst.Codegen.Text.Snippets.Items.Parameter
{
	internal class ParameterItemSnippet : SqlFirstSnippet
	{
		public IRenderableTemplate PocoParameterItem => GetRenderableTemplate();

		public IRenderableTemplate StructParameterItem => GetRenderableTemplate();

		public ParameterItemSnippet()
			: base("Items.Parameter")
		{
		}
	}
}