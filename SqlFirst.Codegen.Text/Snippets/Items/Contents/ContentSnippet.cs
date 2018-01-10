using SqlFirst.Codegen.Text.Templating;

namespace SqlFirst.Codegen.Text.Snippets.Items.Contents
{
	internal class ContentSnippet : SqlFirstSnippet
	{
		public IRenderableTemplate ParameterItem => GetRenderableTemplate();

		public IRenderableTemplate ResultItemInpc => GetRenderableTemplate();

		public IRenderableTemplate ResultItem => GetRenderableTemplate();

		public ContentSnippet()
			: base("Items.Contents")
		{
		}
	}
}