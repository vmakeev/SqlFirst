using SqlFirst.Codegen.Text.Templating;

namespace SqlFirst.Codegen.Text.Snippets.QueryObjects.Methods.Get.Snippets
{
	internal class GetInnerSnippet : SqlFirstSnippet
	{
		public IRenderableTemplate CallAddParameter => GetRenderableTemplate();

		public IRenderableTemplate CallAddParameterNumbered => GetRenderableTemplate();

		public IRenderableTemplate MethodParameter => GetRenderableTemplate();

		public IRenderableTemplate XmlParam => GetRenderableTemplate();

		public GetInnerSnippet()
			: base("QueryObjects.Methods.Get.Snippets")
		{
		}
	}
}