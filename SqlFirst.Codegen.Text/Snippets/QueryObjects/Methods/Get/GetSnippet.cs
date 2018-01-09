using SqlFirst.Codegen.Text.Snippets.QueryObjects.Methods.Get.Snippets;
using SqlFirst.Codegen.Text.Templating;

namespace SqlFirst.Codegen.Text.Snippets.QueryObjects.Methods.Get
{
	internal class GetSnippet : SqlFirstSnippet
	{
		public GetInnerSnippet Snippets { get; } = new GetInnerSnippet();

		public IRenderableTemplate GetFirst => GetRenderableTemplate();

		public IRenderableTemplate GetFirstAsync => GetRenderableTemplate();

		public IRenderableTemplate GetIEnumerable => GetRenderableTemplate();

		public IRenderableTemplate GetIEnumerableAsync => GetRenderableTemplate();

		public IRenderableTemplate GetScalar => GetRenderableTemplate();

		public IRenderableTemplate GetScalarAsync => GetRenderableTemplate();

		public IRenderableTemplate GetScalars => GetRenderableTemplate();

		public IRenderableTemplate GetScalarsAsync => GetRenderableTemplate();

		/// <summary>Initializes a new instance of the <see cref="T:System.Object"></see> class.</summary>
		public GetSnippet()
			: base("QueryObjects.Methods.Get")
		{
		}
	}
}