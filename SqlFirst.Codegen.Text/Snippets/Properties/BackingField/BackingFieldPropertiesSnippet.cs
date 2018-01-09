using SqlFirst.Codegen.Text.Snippets.Properties.BackingField.Inpc;
using SqlFirst.Codegen.Text.Snippets.Properties.BackingField.ReadOnly;
using SqlFirst.Codegen.Text.Templating;

namespace SqlFirst.Codegen.Text.Snippets.Properties.BackingField
{
	internal class BackingFieldPropertiesSnippet : SqlFirstSnippet
	{
		public BackingFieldReadOnlyPropertiesSnippet ReadOnly { get; } = new BackingFieldReadOnlyPropertiesSnippet();

		public BackingFieldInpcPropertiesSnippet NotifyPropertyChanged { get; } = new BackingFieldInpcPropertiesSnippet();

		public IRenderableTemplate BackingFieldPropertyVirtual => GetRenderableTemplate();

		public IRenderableTemplate BackingFieldProperty => GetRenderableTemplate();

		/// <summary>Initializes a new instance of the <see cref="T:System.Object"></see> class.</summary>
		public BackingFieldPropertiesSnippet()
			: base("Properties.BackingField")
		{
		}
	}
}