using SqlFirst.Codegen.Text.Templating;

namespace SqlFirst.Codegen.Text.Snippets.Properties.Auto.ReadOnly
{
	internal class AutoReadonlyPropertiesSnippet : SqlFirstSnippet
	{
		public IRenderableTemplate ReadOnlyAutoProperty => GetRenderableTemplate();

		public IRenderableTemplate ReadOnlyAutoPropertyVirtual => GetRenderableTemplate();

		public IRenderableTemplate ReadOnlyAutoPropertyWithDefault => GetRenderableTemplate();

		public IRenderableTemplate ReadOnlyAutoPropertyVirtualWithDefault => GetRenderableTemplate();


		/// <summary>Initializes a new instance of the <see cref="T:System.Object"></see> class.</summary>
		public AutoReadonlyPropertiesSnippet()
			: base("Properties.Auto.ReadOnly")
		{
		}
	}
}