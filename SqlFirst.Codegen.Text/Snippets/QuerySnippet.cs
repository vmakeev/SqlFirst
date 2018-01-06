using System.IO;
using System.Runtime.CompilerServices;
using SqlFirst.Codegen.Text.Snippets.QueryObjects;

namespace SqlFirst.Codegen.Text.Snippets
{
	internal static class QuerySnippet
	{
		public const string Indent = "\t";

		public static MethodsSnippet Methods { get; } = new MethodsSnippet();

		public static string QueryObject => GetSnippetText();

		private static string GetSnippetText([CallerMemberName] string name = null)
		{
			string resourceName = $"SqlFirst.Codegen.Text.Snippets.QueryObjects.{name}.txt";
			Stream stream = typeof(QuerySnippet).Assembly.GetManifestResourceStream(resourceName);
			string queryText = new StreamReader(stream ?? throw new CodeGenerationException($"Resource not found: {resourceName}")).ReadToEnd();
			return queryText;
		}
	}
}