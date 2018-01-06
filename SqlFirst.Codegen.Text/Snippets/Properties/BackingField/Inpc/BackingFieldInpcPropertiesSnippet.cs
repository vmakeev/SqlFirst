using SqlFirst.Codegen.Text.Snippets.Properties.BackingField.Inpc.ReadOnly;

namespace SqlFirst.Codegen.Text.Snippets.Properties.BackingField.Inpc
{
	internal class BackingFieldInpcPropertiesSnippet : PropertiesSnippetBase
	{
		public BackingFieldInpcReadOnlyPropertiesSnippet ReadOnly { get; } = new BackingFieldInpcReadOnlyPropertiesSnippet();

		public string NotifyPropertyChangedBackingFieldProperty => GetSnippetText();

		public string NotifyPropertyChangedBackingFieldPropertyVirtual => GetSnippetText();

		/// <summary>Initializes a new instance of the <see cref="T:System.Object"></see> class.</summary>
		public BackingFieldInpcPropertiesSnippet()
			: base("BackingField.Inpc")
		{
		}
	}
}