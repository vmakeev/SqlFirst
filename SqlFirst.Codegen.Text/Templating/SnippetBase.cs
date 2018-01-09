using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace SqlFirst.Codegen.Text.Templating
{
	public abstract class SnippetBase
	{
		protected abstract string BaseNamespace { get; }
		protected abstract string FileExtension { get; }

		private readonly string _prefix;

		/// <summary>Initializes a new instance of the <see cref="T:System.Object"></see> class.</summary>
		protected SnippetBase(string prefix)
		{
			_prefix = prefix;
		}

		protected virtual IRenderableTemplate GetRenderableTemplate([CallerMemberName] string name = null)
		{
			return new RenderableTemplate(GetText(name));
		}

		protected virtual IRenderable GetRenderable([CallerMemberName] string name = null)
		{
			return new RenderableString(GetText(name));
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