using System.IO;
using System.Runtime.CompilerServices;

namespace SqlFirst.Codegen.Text.Snippets
{
	internal static class ParameterItemSnippet
	{
		public static string PocoParameterItem => GetSnippetText();

		public static string StructParameterItem => GetSnippetText();

		private static string GetSnippetText([CallerMemberName] string name = null)
		{
			string resourceName = $"SqlFirst.Codegen.Text.Snippets.Items.{name}.txt";
			Stream stream = typeof(ResultItemSnippet).Assembly.GetManifestResourceStream(resourceName);
			string queryText = new StreamReader(stream ?? throw new CodeGenerationException($"Resource not found: {resourceName}")).ReadToEnd();
			return queryText;
		}
	}
}