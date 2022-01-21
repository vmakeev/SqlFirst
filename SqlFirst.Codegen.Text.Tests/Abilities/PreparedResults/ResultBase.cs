using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace SqlFirst.Codegen.Text.Tests.Abilities.PreparedResults
{
	internal abstract class ResultBase
	{
		private readonly string _prefix;

		protected virtual string BaseNamespace { get; } = "SqlFirst.Codegen.Text.Tests.Abilities.PreparedResults";

		protected virtual string FileExtension { get; } = "txt";

		protected ResultBase(string prefix)
		{
			_prefix = prefix;
		}
		
		protected string GetText([CallerMemberName] string name = null)
		{
			string resourceName = string.Join(".", new[] { BaseNamespace, _prefix, name, FileExtension }.Where(item => !string.IsNullOrWhiteSpace(item)));
			Stream stream = GetType().Assembly.GetManifestResourceStream(resourceName);
			string queryText = new StreamReader(stream ?? throw new CodeGenerationException($"Resource not found: {resourceName}")).ReadToEnd();
			return queryText;
		}
	}
}