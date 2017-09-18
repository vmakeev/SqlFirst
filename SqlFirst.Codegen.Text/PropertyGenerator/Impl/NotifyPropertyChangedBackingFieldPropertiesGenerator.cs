using SqlFirst.Codegen.Text.Snippets;
using SqlFirst.Core;

namespace SqlFirst.Codegen.Text.PropertyGenerator.Impl
{
	/// <summary>
	/// Генератор свойств c отдельным полем для чтения/записи данных и уведомлением об изменении их значений
	/// </summary>
	internal class NotifyPropertyChangedBackingFieldPropertiesGenerator : BackingFieldPropertiesGenerator
	{
		/// <summary>
		/// Инициализирует новый экземпляр класса <see cref="BackingFieldPropertiesGenerator" />
		/// </summary>
		/// <param name="typeMapper">Преобразователь типов данных БД в типы CLR</param>
		/// <param name="options">Параметры генерации свойств</param>
		public NotifyPropertyChangedBackingFieldPropertiesGenerator(IDatabaseTypeMapper typeMapper, PropertyGenerationOptions options)
			: base(typeMapper, options)
		{
		}

		/// <inheritdoc />
		protected override string GetPropertyTemplate(PropertyGenerationOptions options)
		{
			if (options.IsReadOnly)
			{
				return _options.IsVirtual
					? PropertySnippet.NotifyPropertyChangedReadOnlyBackingFieldPropertyVirtual
					: PropertySnippet.NotifyPropertyChangedReadOnlyBackingFieldProperty;
			}

			return _options.IsVirtual
				? PropertySnippet.NotifyPropertyChangedBackingFieldPropertyVirtual
				: PropertySnippet.NotifyPropertyChangedBackingFieldProperty;
		}
	}
}