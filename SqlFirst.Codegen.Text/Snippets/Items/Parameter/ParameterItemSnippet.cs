using SqlFirst.Codegen.Text.Templating;

namespace SqlFirst.Codegen.Text.Snippets.Items.Parameter
{
	internal class ParameterItemSnippet : SqlFirstSnippet
	{
		public ParameterItemSnippet()
			: base("Items.Parameter")
		{
		}

		public IRenderableTemplate PocoParameterItem => GetRenderableTemplate();

		public IRenderableTemplate StructParameterItem => GetRenderableTemplate();
	}
}