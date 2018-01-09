using SqlFirst.Codegen.Text.Snippets.Properties.Auto.ReadOnly;
using SqlFirst.Codegen.Text.Templating;

namespace SqlFirst.Codegen.Text.Snippets.Properties.Auto
{
	internal class AutoPropertiesSnippet : SqlFirstSnippet
	{
		public AutoReadonlyPropertiesSnippet ReadOnly { get; } = new AutoReadonlyPropertiesSnippet();

		public IRenderableTemplate AutoPropertyVirtual => GetRenderableTemplate();

		public IRenderableTemplate AutoProperty => GetRenderableTemplate();

		public IRenderableTemplate AutoPropertyVirtualWithDefault => GetRenderableTemplate();

		public IRenderableTemplate AutoPropertyWithDefault => GetRenderableTemplate();

		/// <summary>Initializes a new instance of the <see cref="T:System.Object"></see> class.</summary>
		public AutoPropertiesSnippet()
			: base("Properties.Auto")
		{
		}
	}
}