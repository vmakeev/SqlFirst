using SqlFirst.Codegen.Text.Snippets.Properties.Auto;
using SqlFirst.Codegen.Text.Snippets.Properties.BackingField;

namespace SqlFirst.Codegen.Text.Snippets.Properties
{
	internal static class PropertySnippet
	{
		public static AutoPropertiesSnippet Auto { get; } = new AutoPropertiesSnippet();

		public static BackingFieldPropertiesSnippet BackingField { get; } = new BackingFieldPropertiesSnippet();
	}
}