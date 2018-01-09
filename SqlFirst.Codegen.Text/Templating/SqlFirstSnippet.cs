namespace SqlFirst.Codegen.Text.Templating
{
	internal class SqlFirstSnippet : SnippetBase
	{
		public SqlFirstSnippet(string prefix)
			: base(prefix)
		{
		}
		
		protected sealed override string BaseNamespace { get; } = "SqlFirst.Codegen.Text.Snippets";

		protected sealed override string FileExtension { get; } = "txt";
	}
}