using SqlFirst.Codegen.Text.Templating;

namespace SqlFirst.Codegen.Text.Snippets.Properties.BackingField.ReadOnly
{
	internal class BackingFieldReadOnlyPropertiesSnippet : SqlFirstSnippet
	{
		public IRenderableTemplate ReadOnlyBackingFieldPropertyVirtual => GetRenderableTemplate();

		public IRenderableTemplate ReadOnlyBackingFieldProperty => GetRenderableTemplate();

		/// <summary>Initializes a new instance of the <see cref="T:System.Object"></see> class.</summary>
		public BackingFieldReadOnlyPropertiesSnippet()
			: base("Properties.BackingField.ReadOnly")
		{
		}
	}
}