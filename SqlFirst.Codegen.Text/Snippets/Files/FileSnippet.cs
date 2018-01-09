using SqlFirst.Codegen.Text.Templating;

namespace SqlFirst.Codegen.Text.Snippets.Files
{
	internal class FileSnippet : SqlFirstSnippet
	{
		public IRenderableTemplate DefaultFile => GetRenderableTemplate();

		public IRenderableTemplate Using => GetRenderableTemplate();

		public IRenderableTemplate Namespace => GetRenderableTemplate();

		public FileSnippet()
			: base("Files")
		{
		}
	}
}