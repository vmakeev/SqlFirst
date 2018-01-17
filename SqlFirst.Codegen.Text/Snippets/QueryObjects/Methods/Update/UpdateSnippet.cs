using SqlFirst.Codegen.Text.Templating;

namespace SqlFirst.Codegen.Text.Snippets.QueryObjects.Methods.Update
{
	internal class UpdateSnippet : SqlFirstSnippet
	{
		public IRenderableTemplate Update => GetRenderableTemplate();
		public IRenderableTemplate UpdateAsync => GetRenderableTemplate();
		public IRenderableTemplate UpdateWithResult => GetRenderableTemplate();
		public IRenderableTemplate UpdateWithResultAsync => GetRenderableTemplate();
		public IRenderableTemplate UpdateWithScalarResult => GetRenderableTemplate();
		public IRenderableTemplate UpdateWithScalarResultAsync => GetRenderableTemplate();

		/// <summary>Initializes a new instance of the <see cref="T:System.Object"></see> class.</summary>
		public UpdateSnippet()
			: base("QueryObjects.Methods.Update")
		{
		}
	}
}