namespace SqlFirst.Codegen.Text.Templating
{
	internal class SqlFirstSnippet : SnippetBase
	{
		protected sealed override string BaseNamespace { get; } = "SqlFirst.Codegen.Text.Snippets";

		protected sealed override string FileExtension { get; } = "txt";

		public SqlFirstSnippet(string prefix)
			: base(prefix)
		{
		}
	}
}