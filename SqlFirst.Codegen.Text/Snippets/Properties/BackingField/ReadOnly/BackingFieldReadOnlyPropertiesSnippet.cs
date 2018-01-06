namespace SqlFirst.Codegen.Text.Snippets.Properties.BackingField.ReadOnly
{
	internal class BackingFieldReadOnlyPropertiesSnippet : PropertiesSnippetBase
	{
		public string ReadOnlyBackingFieldPropertyVirtual => GetSnippetText();

		public string ReadOnlyBackingFieldProperty => GetSnippetText();

		/// <summary>Initializes a new instance of the <see cref="T:System.Object"></see> class.</summary>
		public BackingFieldReadOnlyPropertiesSnippet()
			: base("BackingField.ReadOnly")
		{
		}
	}
}