namespace SqlFirst.Codegen.Text.Snippets.Properties.Auto.ReadOnly
{
	internal class AutoReadonlyPropertiesSnippet : PropertiesSnippetBase
	{
		public string ReadOnlyAutoProperty => GetSnippetText();

		public string ReadOnlyAutoPropertyVirtual => GetSnippetText();

		public string ReadOnlyAutoPropertyWithDefault => GetSnippetText();

		public string ReadOnlyAutoPropertyVirtualWithDefault => GetSnippetText();


		/// <summary>Initializes a new instance of the <see cref="T:System.Object"></see> class.</summary>
		public AutoReadonlyPropertiesSnippet()
			: base("Auto.ReadOnly")
		{
		}
	}
}