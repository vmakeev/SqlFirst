using System.IO;
using System.Runtime.CompilerServices;

namespace SqlFirst.Codegen.Text.Snippets
{
	internal static class ItemSnippet
	{
		public static string NotifyPropertyChangedClassResultItem => GetSnippetText();

		public static string NotifyPropertyChangedStructResultItem => GetSnippetText();

		public static string PocoResultItem => GetSnippetText();

		public static string StructResultItem => GetSnippetText();

		private static string GetSnippetText([CallerMemberName] string name = null)
		{
			string resourceName = $"SqlFirst.Codegen.Text.Snippets.Items.{name}.txt";
			Stream stream = typeof(ItemSnippet).Assembly.GetManifestResourceStream(resourceName);
			string queryText = new StreamReader(stream ?? throw new CodeGenerationException($"Resource not found: {resourceName}")).ReadToEnd();
			return queryText;
		}
	}
}