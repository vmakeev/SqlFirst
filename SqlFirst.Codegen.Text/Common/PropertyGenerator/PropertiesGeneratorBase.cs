using System.Collections.Generic;
using SqlFirst.Codegen.Text.Common.PropertyGenerator.Impl;

namespace SqlFirst.Codegen.Text.Common.PropertyGenerator
{
	/// <summary>
	/// Базовая реализация генератора свойств
	/// </summary>
	internal abstract class PropertiesGeneratorBase
	{
		protected readonly PropertyGenerationOptions _options;

		/// <summary>
		/// Инициализирует новый экземпляр класса <see cref="PropertiesGeneratorBase"/>
		/// </summary>
		/// <param name="options">Параметры генерации свойств</param>
		protected PropertiesGeneratorBase(PropertyGenerationOptions options)
		{
			_options = options ?? throw new System.ArgumentNullException(nameof(options));
		}

		/// <summary>
		/// Генерирует код для объявления свойств
		/// </summary>
		/// <param name="properties">Описание свойств</param>
		/// <returns>Сгенерированные единицы кода</returns>
		public abstract IEnumerable<GeneratedPropertyInfo> GenerateProperties(IEnumerable<ICodeMemberInfo> properties);

		/// <summary>
		/// Возвращает шаблон свойства
		/// </summary>
		/// <param name="options">Параметры генерации свойств</param>
		/// <param name="hasDefaultValue">Имеет ли свойство значение по умолчанию</param>
		/// <returns>Шаблон свойства</returns>
		protected abstract string GetPropertyTemplate(PropertyGenerationOptions options, bool hasDefaultValue);
	}
}