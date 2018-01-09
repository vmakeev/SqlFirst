using SqlFirst.Codegen.Text.Templating;

namespace SqlFirst.Codegen.Text.Snippets.Properties.BackingField.Inpc.ReadOnly
{
	internal class BackingFieldInpcReadOnlyPropertiesSnippet : SqlFirstSnippet
	{
		public IRenderableTemplate NotifyPropertyChangedReadOnlyBackingFieldProperty => GetRenderableTemplate();

		public IRenderableTemplate NotifyPropertyChangedReadOnlyBackingFieldPropertyVirtual => GetRenderableTemplate();

		/// <summary>Initializes a new instance of the <see cref="T:System.Object"></see> class.</summary>
		public BackingFieldInpcReadOnlyPropertiesSnippet()
			: base("Properties.BackingField.Inpc.ReadOnly")
		{
		}
	}
}