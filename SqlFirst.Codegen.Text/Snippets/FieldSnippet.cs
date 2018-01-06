using System.IO;
using System.Runtime.CompilerServices;

namespace SqlFirst.Codegen.Text.Snippets
{
	internal static class FieldSnippet
	{
		public static string BackingField => GetSnippetText();
		public static string BackingFieldWithValue => GetSnippetText();

		public static string ReadOnlyField => GetSnippetText();

		private static string GetSnippetText([CallerMemberName] string name = null)
		{
			string resourceName = $"SqlFirst.Codegen.Text.Snippets.Fields.{name}.txt";
			Stream stream = typeof(FieldSnippet).Assembly.GetManifestResourceStream(resourceName);
			string queryText = new StreamReader(stream ?? throw new CodeGenerationException($"Resource not found: {resourceName}")).ReadToEnd();
			return queryText;
		}
	}
}