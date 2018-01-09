using SqlFirst.Codegen.Text.Snippets.Enumerables;
using SqlFirst.Codegen.Text.Snippets.Fields;
using SqlFirst.Codegen.Text.Snippets.Files;
using SqlFirst.Codegen.Text.Snippets.Items;
using SqlFirst.Codegen.Text.Snippets.Properties;
using SqlFirst.Codegen.Text.Snippets.QueryObjects;

namespace SqlFirst.Codegen.Text.Snippets
{
	internal static class Snippet
	{
		public static QueryObjectSnippet Query { get; } = new QueryObjectSnippet();

		public static ItemSnippet Item { get; } = new ItemSnippet();

		public static FileSnippet File { get; } = new FileSnippet();

		public static FieldSnippet Field { get; } = new FieldSnippet();

		public static PropertySnippet Property { get; } = new PropertySnippet();

		public static EnumerableSnippet Enumerable { get; } = new EnumerableSnippet();
	}
}