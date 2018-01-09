using SqlFirst.Codegen.Text.Snippets.Properties.Auto;
using SqlFirst.Codegen.Text.Snippets.Properties.BackingField;

namespace SqlFirst.Codegen.Text.Snippets.Properties
{
	internal class PropertySnippet
	{
		public AutoPropertiesSnippet Auto { get; } = new AutoPropertiesSnippet();

		public BackingFieldPropertiesSnippet BackingField { get; } = new BackingFieldPropertiesSnippet();
	}
}