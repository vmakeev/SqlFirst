using SqlFirst.Codegen.Text.Templating;

namespace SqlFirst.Codegen.Text.Snippets.Commentaries
{
	internal class CommentarySnippet : SqlFirstSnippet
	{
		public IRenderableTemplate XmlDocumentationCommentary => GetRenderableTemplate();

		public CommentarySnippet()
			: base("Commentaries")
		{
		}
	}
}