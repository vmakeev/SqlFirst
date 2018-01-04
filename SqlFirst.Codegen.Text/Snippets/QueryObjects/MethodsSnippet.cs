using SqlFirst.Codegen.Text.Snippets.QueryObjects.Methods;

namespace SqlFirst.Codegen.Text.Snippets.QueryObjects
{
	internal class MethodsSnippet
	{
		public CommonSnippet Common { get; } = new CommonSnippet();
		public GetSnippet Get { get; } = new GetSnippet();
	}
}