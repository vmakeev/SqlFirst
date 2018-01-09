using System;

namespace SqlFirst.Codegen.Text.Templating
{
	public class RenderableString : IRenderable
	{
		private readonly string _content;

		/// <summary>Initializes a new instance of the <see cref="T:System.Object"></see> class.</summary>
		public RenderableString(string content)
		{
			_content = content ?? throw new ArgumentNullException(nameof(content));
		}

		public string Render()
		{
			return _content;
		}
	}
}