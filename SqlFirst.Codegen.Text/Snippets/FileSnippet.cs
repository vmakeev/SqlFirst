using System.IO;
using System.Runtime.CompilerServices;

namespace SqlFirst.Codegen.Text.Snippets
{
	internal static class FileSnippet
	{
		public static string DefaultFile => GetSnippetText();
		public static string Usings => GetSnippetText();
		public static string Using => GetSnippetText();
		public static string Namespace => GetSnippetText();

		private static string GetSnippetText([CallerMemberName] string name = null)
		{
			string resourceName = $"SqlFirst.Codegen.Text.Snippets.Files.{name}.txt";
			Stream stream = typeof(ItemSnippet).Assembly.GetManifestResourceStream(resourceName);
			string queryText = new StreamReader(stream ?? throw new CodeGenerationException($"Resource not found: {resourceName}")).ReadToEnd();
			return queryText;
		}
	}
}