using SqlFirst.Codegen.Text.Templating;

namespace SqlFirst.Codegen.Text.Snippets.QueryObjects.Methods.Delete
{
	internal class DeleteSnippet : SqlFirstSnippet
	{
		public IRenderableTemplate Delete => GetRenderableTemplate();
		public IRenderableTemplate DeleteAsync => GetRenderableTemplate();
		public IRenderableTemplate DeleteWithResult => GetRenderableTemplate();
		public IRenderableTemplate DeleteWithResultAsync => GetRenderableTemplate();
		public IRenderableTemplate DeleteWithScalarResult => GetRenderableTemplate();
		public IRenderableTemplate DeleteWithScalarResultAsync => GetRenderableTemplate();

		/// <summary>Initializes a new instance of the <see cref="T:System.Object"></see> class.</summary>
		public DeleteSnippet()
			: base("QueryObjects.Methods.Delete")
		{
		}
	}
}