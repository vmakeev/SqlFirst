namespace SqlFirst.Codegen.Text.Templating
{
	public static class Renderable
	{
		public static IRenderable Create(string content) => new RenderableString(content);

		public static IRenderable Create(IRenderableTemplate template, object model) => new RenderableTemplateWithModel(template, model);
	}
}