using SqlFirst.Codegen.Text.Templating;

namespace SqlFirst.Codegen.Text.Snippets.QueryObjects.Methods.StoredProcedure
{
	internal class StoredProcedureSnippet : SqlFirstSnippet
	{
		public IRenderableTemplate StoredProcedure => GetRenderableTemplate();
		public IRenderableTemplate StoredProcedureAsync => GetRenderableTemplate();
		public IRenderableTemplate StoredProcedureWithResult => GetRenderableTemplate();
		public IRenderableTemplate StoredProcedureWithResultAsync => GetRenderableTemplate();
		public IRenderableTemplate StoredProcedureWithScalarResult => GetRenderableTemplate();
		public IRenderableTemplate StoredProcedureWithScalarResultAsync => GetRenderableTemplate();

		/// <summary>Initializes a new instance of the <see cref="T:System.Object"></see> class.</summary>
		public StoredProcedureSnippet()
			: base("QueryObjects.Methods.StoredProcedure")
		{
		}
	}
}