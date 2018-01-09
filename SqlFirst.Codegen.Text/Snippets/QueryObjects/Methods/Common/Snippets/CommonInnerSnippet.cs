using SqlFirst.Codegen.Text.Templating;

namespace SqlFirst.Codegen.Text.Snippets.QueryObjects.Methods.Common.Snippets
{
	internal class CommonInnerSnippet : SqlFirstSnippet
	{
		public CommonInnerSnippet()
			: base("QueryObjects.Methods.Common.Snippets")
		{
		}

		public IRenderableTemplate MapField => GetRenderableTemplate();

		public IRenderable GetQueryTemplates => GetRenderable();

		public IRenderable GetInsertedValuesSection => GetRenderable();

		public IRenderable GetNumberedParameters => GetRenderable();

		public IRenderable NumberedParameterInfo => GetRenderable();
		public IRenderableTemplate CalculateChecksum => GetRenderableTemplate();
	}
}