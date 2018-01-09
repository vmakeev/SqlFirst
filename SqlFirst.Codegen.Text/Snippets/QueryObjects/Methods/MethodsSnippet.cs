using SqlFirst.Codegen.Text.Snippets.QueryObjects.Methods.Add;
using SqlFirst.Codegen.Text.Snippets.QueryObjects.Methods.Common;
using SqlFirst.Codegen.Text.Snippets.QueryObjects.Methods.Get;

namespace SqlFirst.Codegen.Text.Snippets.QueryObjects.Methods
{
	internal class MethodsSnippet
	{
		public CommonSnippet Common { get; } = new CommonSnippet();

		public GetSnippet Get { get; } = new GetSnippet();

		public AddSnippet Add { get; } = new AddSnippet();
	}
}