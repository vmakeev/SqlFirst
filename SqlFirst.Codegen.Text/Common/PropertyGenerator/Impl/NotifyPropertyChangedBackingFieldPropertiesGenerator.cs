using SqlFirst.Codegen.Text.Snippets.Properties;

namespace SqlFirst.Codegen.Text.Common.PropertyGenerator.Impl
{
	/// <summary>
	/// Генератор свойств c отдельным полем для чтения/записи данных и уведомлением об изменении их значений
	/// </summary>
	internal class NotifyPropertyChangedBackingFieldPropertiesGenerator : BackingFieldPropertiesGenerator
	{
		/// <summary>
		/// Инициализирует новый экземпляр класса <see cref="BackingFieldPropertiesGenerator" />
		/// </summary>
		/// <param name="options">Параметры генерации свойств</param>
		public NotifyPropertyChangedBackingFieldPropertiesGenerator(PropertyGenerationOptions options)
			: base(options)
		{
		}

		/// <inheritdoc />
		protected override string GetPropertyTemplate(PropertyGenerationOptions options, bool hasDefaultValue)
		{
			if (options.IsReadOnly)
			{
				return _options.IsVirtual
					? PropertySnippet.BackingField.NotifyPropertyChanged.ReadOnly.NotifyPropertyChangedReadOnlyBackingFieldPropertyVirtual
					: PropertySnippet.BackingField.NotifyPropertyChanged.ReadOnly.NotifyPropertyChangedReadOnlyBackingFieldProperty;
			}

			return _options.IsVirtual
				? PropertySnippet.BackingField.NotifyPropertyChanged.NotifyPropertyChangedBackingFieldPropertyVirtual
				: PropertySnippet.BackingField.NotifyPropertyChanged.NotifyPropertyChangedBackingFieldProperty;
		}
	}
}