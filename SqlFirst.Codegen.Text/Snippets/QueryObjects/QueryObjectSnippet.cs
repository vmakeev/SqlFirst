using SqlFirst.Codegen.Text.Snippets.QueryObjects.Methods;
using SqlFirst.Codegen.Text.Templating;

namespace SqlFirst.Codegen.Text.Snippets.QueryObjects
{
	internal class QueryObjectSnippet : SqlFirstSnippet
	{
		public IRenderableTemplate QueryObject => GetRenderableTemplate();

		public MethodsSnippet Methods { get; } = new MethodsSnippet();

		/// <summary>Initializes a new instance of the <see cref="T:System.Object"></see> class.</summary>
		public QueryObjectSnippet()
			: base("QueryObjects")
		{
		}
	}
}