using System;

namespace SqlFirst.Codegen.Text.Templating
{
	public static class Renderable
	{
		public static IRenderable Create(string content) => new RenderableString(content);
		public static IRenderable Create(IRenderableTemplate template, object model) => new RenderableTemplateWithModel(template, model);
	}

	internal class RenderableTemplateWithModel : IRenderable
	{
		private readonly IRenderableTemplate _template;
		private readonly object _model;

		/// <summary>Initializes a new instance of the <see cref="T:System.Object"></see> class.</summary>
		public RenderableTemplateWithModel(IRenderableTemplate template, object model)
		{
			_template = template ?? throw new ArgumentNullException(nameof(template));
			_model = model;
		}

		public string Render()
		{
			return _template.Render(_model);
		}
	}
}