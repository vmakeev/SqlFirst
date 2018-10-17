using SqlFirst.Codegen.Text.Templating;

namespace SqlFirst.Codegen.Text.Snippets.QueryObjects.Methods.Common.GetDataTableSnippets
{
	internal class GetDataTableSnippet : SqlFirstSnippet
	{
		public IRenderableTemplate AddDataTableColumn => GetRenderableTemplate();

		public IRenderableTemplate AddDataTableRowDbNull => GetRenderableTemplate();

		public IRenderableTemplate AddDataTableRowNullArgumentException => GetRenderableTemplate();

		public IRenderableTemplate AddDataTableRowProperty => GetRenderableTemplate();

		public IRenderableTemplate AddDataTableRowValue => GetRenderableTemplate();

		/// <summary>Initializes a new instance of the <see cref="T:System.Object"></see> class.</summary>
		public GetDataTableSnippet()
			: base("QueryObjects.Methods.Common.GetDataTableSnippets")
		{
		}
	}
}