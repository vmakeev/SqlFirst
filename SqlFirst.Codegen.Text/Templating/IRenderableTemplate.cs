namespace SqlFirst.Codegen.Text.Templating
{
	public interface IRenderableTemplate<in T> : IRenderableTemplate where T : class
	{
		string Render(T model);
	}

	public interface IRenderableTemplate : IRenderable
	{
		string Render(object model);
	}
}