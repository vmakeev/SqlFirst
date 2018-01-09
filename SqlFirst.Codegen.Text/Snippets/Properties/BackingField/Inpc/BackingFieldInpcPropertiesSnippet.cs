using SqlFirst.Codegen.Text.Snippets.Properties.BackingField.Inpc.ReadOnly;
using SqlFirst.Codegen.Text.Templating;

namespace SqlFirst.Codegen.Text.Snippets.Properties.BackingField.Inpc
{
	internal class BackingFieldInpcPropertiesSnippet : SqlFirstSnippet
	{
		public BackingFieldInpcReadOnlyPropertiesSnippet ReadOnly { get; } = new BackingFieldInpcReadOnlyPropertiesSnippet();

		public IRenderableTemplate NotifyPropertyChangedBackingFieldProperty => GetRenderableTemplate();

		public IRenderableTemplate NotifyPropertyChangedBackingFieldPropertyVirtual => GetRenderableTemplate();

		/// <summary>Initializes a new instance of the <see cref="T:System.Object"></see> class.</summary>
		public BackingFieldInpcPropertiesSnippet()
			: base("Properties.BackingField.Inpc")
		{
		}
	}
}