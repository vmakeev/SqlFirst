using SqlFirst.Codegen.Text.Snippets.Properties.BackingField.Inpc;
using SqlFirst.Codegen.Text.Snippets.Properties.BackingField.ReadOnly;

namespace SqlFirst.Codegen.Text.Snippets.Properties.BackingField
{
	internal class BackingFieldPropertiesSnippet : PropertiesSnippetBase
	{
		public BackingFieldReadOnlyPropertiesSnippet ReadOnly { get; } = new BackingFieldReadOnlyPropertiesSnippet();

		public BackingFieldInpcPropertiesSnippet NotifyPropertyChanged { get; } = new BackingFieldInpcPropertiesSnippet();

		public string BackingFieldPropertyVirtual => GetSnippetText();

		public string BackingFieldProperty => GetSnippetText();

		/// <summary>Initializes a new instance of the <see cref="T:System.Object"></see> class.</summary>
		public BackingFieldPropertiesSnippet()
			: base("BackingField")
		{
		}
	}
}