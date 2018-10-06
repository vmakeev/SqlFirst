using SqlFirst.Codegen.Text.Snippets.QueryObjects.Methods.Add;
using SqlFirst.Codegen.Text.Snippets.QueryObjects.Methods.Common;
using SqlFirst.Codegen.Text.Snippets.QueryObjects.Methods.Delete;
using SqlFirst.Codegen.Text.Snippets.QueryObjects.Methods.Get;
using SqlFirst.Codegen.Text.Snippets.QueryObjects.Methods.StoredProcedure;
using SqlFirst.Codegen.Text.Snippets.QueryObjects.Methods.Update;

namespace SqlFirst.Codegen.Text.Snippets.QueryObjects.Methods
{
	internal class MethodsSnippet
	{
		public CommonSnippet Common { get; } = new CommonSnippet();

		public GetSnippet Get { get; } = new GetSnippet();

		public AddSnippet Add { get; } = new AddSnippet();

		public UpdateSnippet Update { get; } = new UpdateSnippet();

		public DeleteSnippet Delete { get; } = new DeleteSnippet();

		public StoredProcedureSnippet StoredProcedure { get; } = new StoredProcedureSnippet();
	}
}