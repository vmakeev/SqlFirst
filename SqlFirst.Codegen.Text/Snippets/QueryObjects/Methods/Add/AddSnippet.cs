
using SqlFirst.Codegen.Text.Templating;

namespace SqlFirst.Codegen.Text.Snippets.QueryObjects.Methods.Add
{
	internal class AddSnippet : SqlFirstSnippet
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Object"></see> class.</summary>
		public AddSnippet()
			: base("QueryObjects.Methods.Add")
		{
		}

		public IRenderableTemplate AddSingle => GetRenderableTemplate();
		public IRenderableTemplate AddSingleAsync => GetRenderableTemplate();

		public IRenderableTemplate AddSingleWithResult => GetRenderableTemplate();
		public IRenderableTemplate AddSingleWithResultAsync => GetRenderableTemplate();

		public IRenderableTemplate AddSingleWithScalarResult => GetRenderableTemplate();
		public IRenderableTemplate AddSingleWithScalarResultAsync => GetRenderableTemplate();

		public IRenderableTemplate AddMultiple => GetRenderableTemplate();
		public IRenderableTemplate AddMultipleAsync => GetRenderableTemplate();

		public IRenderableTemplate AddMultipleWithResult => GetRenderableTemplate();
		public IRenderableTemplate AddMultipleWithResultAsync => GetRenderableTemplate();

		public IRenderableTemplate AddMultipleWithScalarResult => GetRenderableTemplate();
		public IRenderableTemplate AddMultipleWithScalarResultAsync => GetRenderableTemplate();

	}
}