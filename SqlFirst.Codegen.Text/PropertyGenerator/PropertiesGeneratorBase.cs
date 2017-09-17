using System.Collections.Generic;
using SqlFirst.Codegen.Text.PropertyGenerator.Impl;
using SqlFirst.Core;
using SqlFirst.Core.Parsing;

namespace SqlFirst.Codegen.Text.PropertyGenerator
{
	/// <summary>
	/// Базовая реализация генератора свойств
	/// </summary>
	internal abstract class PropertiesGeneratorBase
	{
		protected readonly IDatabaseTypeMapper _typeMapper;
		protected readonly PropertyGenerationOptions _options;

		/// <summary>
		/// Инициализирует новый экземпляр класса <see cref="PropertiesGeneratorBase"/>
		/// </summary>
		/// <param name="typeMapper">Преобразователь типов данных БД в типы CLR</param>
		/// <param name="options">Параметры генерации свойств</param>
		protected PropertiesGeneratorBase(IDatabaseTypeMapper typeMapper, PropertyGenerationOptions options)
		{
			_typeMapper = typeMapper ?? throw new System.ArgumentNullException(nameof(typeMapper));
			_options = options ?? throw new System.ArgumentNullException(nameof(options));
		}

		/// <summary>
		/// Генерирует код для объявления свойств
		/// </summary>
		/// <param name="results">Описание результатов запроса</param>
		/// <returns>Сгенерированные единицы кода</returns>
		public abstract IEnumerable<GeneratedPropertyInfo> GenerateProperties(IEnumerable<IFieldDetails> results);

		/// <summary>
		/// Возвращает шаблон свойства
		/// </summary>
		/// <param name="options">Параметры генерации свойств</param>
		/// <returns>Шаблон свойства</returns>
		protected abstract string GetPropertyTemplate(PropertyGenerationOptions options);
	}
}