using System.IO;
using System.Runtime.CompilerServices;

namespace SqlFirst.Codegen.Text.Snippets.Properties
{
	internal abstract class PropertiesSnippetBase
	{
		private readonly string _prefix;

		/// <summary>Initializes a new instance of the <see cref="T:System.Object"></see> class.</summary>
		protected PropertiesSnippetBase(string prefix)
		{
			_prefix = prefix;
		}

		protected string GetSnippetText([CallerMemberName] string name = null)
		{
			string resourceName = $"SqlFirst.Codegen.Text.Snippets.Properties.{_prefix}.{name}.txt";
			Stream stream = typeof(ResultItemSnippet).Assembly.GetManifestResourceStream(resourceName);
			string queryText = new StreamReader(stream ?? throw new CodeGenerationException($"Resource not found: {resourceName}")).ReadToEnd();
			return queryText;
		}
	}
}
