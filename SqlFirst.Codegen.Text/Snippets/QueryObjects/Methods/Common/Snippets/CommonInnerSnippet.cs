using SqlFirst.Codegen.Text.Templating;

namespace SqlFirst.Codegen.Text.Snippets.QueryObjects.Methods.Common.Snippets
{
	internal class CommonInnerSnippet : SqlFirstSnippet
	{
		public IRenderableTemplate MapField => GetRenderableTemplate();

		public IRenderable GetQueryTemplates => GetRenderable();

		public IRenderable GetInsertedValuesSection => GetRenderable();

		public IRenderable GetNumberedParameters => GetRenderable();

		public IRenderable NumberedParameterInfo => GetRenderable();

		public IRenderableTemplate CalculateChecksum => GetRenderableTemplate();

		public IRenderableTemplate CallAddParameter => GetRenderableTemplate();

		public IRenderableTemplate CallAddCustomParameter => GetRenderableTemplate();

		public IRenderableTemplate CallAddParameterNumbered => GetRenderableTemplate();

		public IRenderableTemplate MethodParameter => GetRenderableTemplate();

		public IRenderableTemplate XmlParam => GetRenderableTemplate();

		public IRenderableTemplate AddDataTableColumn => GetRenderableTemplate();

		public IRenderableTemplate AddDataTableRow => GetRenderableTemplate();

		public IRenderableTemplate CallGetDataTableInline => GetRenderableTemplate();

		public CommonInnerSnippet()
			: base("QueryObjects.Methods.Common.Snippets")
		{
		}
	}
}