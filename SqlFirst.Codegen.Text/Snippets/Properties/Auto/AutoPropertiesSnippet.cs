using SqlFirst.Codegen.Text.Snippets.Properties.Auto.ReadOnly;

namespace SqlFirst.Codegen.Text.Snippets.Properties.Auto
{
	internal class AutoPropertiesSnippet : PropertiesSnippetBase
	{
		public AutoReadonlyPropertiesSnippet ReadOnly { get; } = new AutoReadonlyPropertiesSnippet();

		public string AutoPropertyVirtual => GetSnippetText();

		public string AutoProperty => GetSnippetText();

		public string AutoPropertyVirtualWithDefault => GetSnippetText();

		public string AutoPropertyWithDefault => GetSnippetText();

		/// <summary>Initializes a new instance of the <see cref="T:System.Object"></see> class.</summary>
		public AutoPropertiesSnippet()
			: base("Auto")
		{
		}
	}
}