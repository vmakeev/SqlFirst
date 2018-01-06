using System.IO;
using System.Runtime.CompilerServices;

namespace SqlFirst.Codegen.Text.Snippets
{
	internal static class EnumerableSnippet
	{
		public static string AsyncEnumerator => GetSnippetText();
		public static string DbAsyncEnumerator => GetSnippetText();
		public static string Enumerable => GetSnippetText();
		public static string EnumerableItem => GetSnippetText();

		private static string GetSnippetText([CallerMemberName] string name = null)
		{
			string resourceName = $"SqlFirst.Codegen.Text.Snippets.Enumerabes.{name}.txt";
			Stream stream = typeof(EnumerableSnippet).Assembly.GetManifestResourceStream(resourceName);
			string queryText = new StreamReader(stream ?? throw new CodeGenerationException($"Resource not found: {resourceName}")).ReadToEnd();
			return queryText;
		}
	}
}