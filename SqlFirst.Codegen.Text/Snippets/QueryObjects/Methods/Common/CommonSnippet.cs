using SqlFirst.Codegen.Text.Snippets.QueryObjects.Methods.Common.Snippets;
using SqlFirst.Codegen.Text.Templating;

namespace SqlFirst.Codegen.Text.Snippets.QueryObjects.Methods.Common
{
	internal class CommonSnippet : SqlFirstSnippet
	{
		public CommonInnerSnippet Snippets { get; } = new CommonInnerSnippet();

		public IRenderableTemplate AddParameter => GetRenderableTemplate();

		public IRenderableTemplate AddCustomParameter => GetRenderableTemplate();

		public IRenderableTemplate GetItemFromRecord => GetRenderableTemplate();

		public IRenderable GetScalarFromRecord => GetRenderable();

		public IRenderable GetScalarFromValue => GetRenderable();

		public IRenderableTemplate GetQueryFromResourceCacheable => GetRenderableTemplate();

		public IRenderableTemplate GetQueryFromString => GetRenderableTemplate();

		public IRenderableTemplate GetQueryFromStringMultipleInsert => GetRenderableTemplate();

		public IRenderable GetQueryRuntimeGeneratedMultipleInsert => GetRenderable();

		/// <summary>Initializes a new instance of the <see cref="T:System.Object"></see> class.</summary>
		public CommonSnippet()
			: base("QueryObjects.Methods.Common")
		{
		}
	}
}