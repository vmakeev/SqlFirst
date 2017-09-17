using SqlFirst.Codegen.Text.Snippets;
using SqlFirst.Core;

namespace SqlFirst.Codegen.Text.PropertyGenerator.Impl
{
	internal class NotifyPropertyChangedBackingFieldPropertiesGenerator : BackingFieldPropertiesGenerator
	{
		public NotifyPropertyChangedBackingFieldPropertiesGenerator(IDatabaseTypeMapper typeMapper, bool isReadOnly)
			: base(typeMapper, isReadOnly)
		{
		}

		protected override string GetPropertyTemplate(bool isReadOnly)
		{
			return isReadOnly
				? Snippet.NotifyPropertyChangedReadOnlyBackingFieldProperty
				: Snippet.NotifyPropertyChangedBackingFieldProperty;
		}
	}
}