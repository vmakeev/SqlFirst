using System;
using SqlFirst.Codegen.Text.PropertyGenerator;
using SqlFirst.Codegen.Text.PropertyGenerator.Impl;
using SqlFirst.Codegen.Text.ResultItemGenerators;
using SqlFirst.Codegen.Text.ResultItemGenerators.Impl;
using SqlFirst.Core;

namespace SqlFirst.Codegen.Text
{
	public class TextCodeGenerator : ICodeGenerator
	{
		private readonly IDatabaseTypeMapper _typeMapper;

		/// <summary>Initializes a new instance of the <see cref="T:System.Object"></see> class.</summary>
		public TextCodeGenerator(IDatabaseTypeMapper typeMapper)
		{
			_typeMapper = typeMapper;
		}

		/// <summary>
		/// Выполняет генерацию объекта, представляющего запрос
		/// </summary>
		/// <param name="context">Контекст генерации кода</param>
		/// <param name="options">Параметры генерации</param>
		/// <returns>Сгенерированный объект</returns>
		public IGeneratedQueryObject GenerateQueryObject(ICodeGenerationContext context, IQueryGenerationOptions options)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Выполняет генерацию объекта, представляющего результат выполнения запроса
		/// </summary>
		/// <param name="context">Контекст генерации кода</param>
		/// <param name="options">Параметры генерации</param>
		/// <returns>Сгенерированный объект</returns>
		public IGeneratedResultItem GenerateResultItem(ICodeGenerationContext context, IResultGenerationOptions options)
		{
			PropertiesGeneratorBase propertiesGenerator = GetPropertiesGenerator(options);
			ResultItemGeneratorBase itemGenerator = GetResultItemGenerator(options, propertiesGenerator);
			return itemGenerator.GenerateResultItem(context, options);
		}

		private static ResultItemGeneratorBase GetResultItemGenerator(IResultGenerationOptions options, PropertiesGeneratorBase propertiesGenerator)
		{
			ResultItemGeneratorBase itemGenerator;
			switch (options.ItemType)
			{
				case ResultItemType.Poco:
					itemGenerator = new PocoResultItemGenerator(propertiesGenerator);
					break;

				case ResultItemType.NotifyPropertyChanged when options.PropertyType != PropertyType.BackingField:
					throw new ArgumentException($"ResultItemType [{options.ItemType:G}] is incompatible with PropertyType [{options.PropertyType:G}]");

				case ResultItemType.NotifyPropertyChanged:
					itemGenerator = new NotifyPropertyChangedResultItemGenerator(propertiesGenerator);
					break;

				case ResultItemType.Struct:
					itemGenerator = new StructResultItemGenerator(propertiesGenerator);
					break;

				default:
					throw new ArgumentOutOfRangeException($"Unexpected options.ItemType [{options.ItemType:G}]", (Exception)null);
			}

			return itemGenerator;
		}

		private PropertiesGeneratorBase GetPropertiesGenerator(IResultGenerationOptions options)
		{
			PropertiesGeneratorBase propertiesGenerator;
			switch (options.PropertyType)
			{
				case PropertyType.Auto:
					propertiesGenerator = new AutoPropertiesGenerator(_typeMapper, false);
					break;

				case PropertyType.AutoReadOnly:
					propertiesGenerator = new AutoPropertiesGenerator(_typeMapper, true);
					break;

				case PropertyType.BackingField:
					propertiesGenerator = options.ItemType == ResultItemType.NotifyPropertyChanged
						? new NotifyPropertyChangedBackingFieldPropertiesGenerator(_typeMapper, false)
						: new BackingFieldPropertiesGenerator(_typeMapper, false);
					break;

				case PropertyType.BackingFieldReadOnly:
					propertiesGenerator = options.ItemType == ResultItemType.NotifyPropertyChanged
						? new NotifyPropertyChangedBackingFieldPropertiesGenerator(_typeMapper, true)
						: new BackingFieldPropertiesGenerator(_typeMapper, true);
					break;

				default:
					throw new ArgumentOutOfRangeException($"Unexpected options.propertyType [{options.PropertyType:G}]", (Exception)null);
			}

			return propertiesGenerator;
		}
	}
}