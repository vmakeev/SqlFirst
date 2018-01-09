using SqlFirst.Codegen.Text.Snippets.Items.Parameter;
using SqlFirst.Codegen.Text.Snippets.Items.Result;

namespace SqlFirst.Codegen.Text.Snippets.Items
{
	internal class ItemSnippet
	{
		public ResultItemSnippet Result { get; } = new ResultItemSnippet();

		public ParameterItemSnippet Parameter { get; } = new ParameterItemSnippet();
	}
}