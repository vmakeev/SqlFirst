using SqlFirst.Codegen.Text.Templating;

namespace SqlFirst.Codegen.Text.Snippets.Enumerables
{
	internal class EnumerableSnippet : SqlFirstSnippet
	{
		public IRenderableTemplate AsyncEnumerator => GetRenderableTemplate();

		public IRenderableTemplate DbAsyncEnumerator => GetRenderableTemplate();

		public IRenderableTemplate Enumerable => GetRenderableTemplate();

		public IRenderableTemplate EnumerableItem => GetRenderableTemplate();

		public EnumerableSnippet()
			: base("Enumerables")
		{
		}
	}
}