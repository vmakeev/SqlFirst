using System;
using SqlFirst.Codegen.Text.PropertyGenerator;
using SqlFirst.Codegen.Text.PropertyGenerator.Impl;
using SqlFirst.Codegen.Text.ResultItemGenerators;
using SqlFirst.Codegen.Text.ResultItemGenerators.Impl;
using SqlFirst.Core;

namespace SqlFirst.Codegen.Text
{
	/// <summary>
	/// Генератор кода на основе текстовых шаблонов
	/// </summary>
	public class TextCodeGenerator : ICodeGenerator
	{
		private readonly IDatabaseTypeMapper _typeMapper;

		/// <summary>Initializes a new instance of the <see cref="T:System.Object"></see> class.</summary>
		public TextCodeGenerator(IDatabaseTypeMapper typeMapper)
		{
			_typeMapper = typeMapper;
		}

		/// <inheritdoc />
		public IGeneratedQueryObject GenerateQueryObject(ICodeGenerationContext context, IQueryGenerationOptions options)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		public IGeneratedResultItem GenerateResultItem(ICodeGenerationContext context, IResultGenerationOptions options)
		{
			PropertiesGeneratorBase propertiesGenerator = GetPropertiesGenerator(options);
			ResultItemGeneratorBase itemGenerator = GetResultItemGenerator(options, propertiesGenerator);
			return itemGenerator.GenerateResultItem(context);
		}

		/// <summary>
		/// Возвращает генератор результата выполнения запроса
		/// </summary>
		/// <param name="options">Параметры генерации</param>
		/// <param name="propertiesGenerator">Генератор свойств</param>
		/// <returns>Генератор результата выполнения запроса</returns>
		private static ResultItemGeneratorBase GetResultItemGenerator(IResultGenerationOptions options, PropertiesGeneratorBase propertiesGenerator)
		{
			ResultItemGeneratorBase itemGenerator;
			switch (options.ItemType)
			{
				case ResultItemType.Poco:
					itemGenerator = new PocoResultItemGenerator(propertiesGenerator);
					break;

				case ResultItemType.NotifyPropertyChanged:
					if (options.PropertyType == PropertyType.BackingField)
					{
						itemGenerator = new NotifyPropertyChangedResultItemGenerator(propertiesGenerator);
					}
					else
					{
						throw new CodeGenerationException($"ResultItemType [{options.ItemType:G}] is incompatible with PropertyType [{options.PropertyType:G}]");
					}
					break;

				case ResultItemType.Struct:
					if ((options.PropertyModifiers & PropertyModifiers.Virtual) == PropertyModifiers.Virtual)
					{
						throw new CodeGenerationException("Struct can not contains virtual properties");
					}
					itemGenerator = new StructResultItemGenerator(propertiesGenerator);
					break;

				default:
					throw new CodeGenerationException($"Unexpected options.ItemType [{options.ItemType:G}]");
			}

			return itemGenerator;
		}

		/// <summary>
		/// Возвращает генератор свойств
		/// </summary>
		/// <param name="options">Параметры генерации</param>
		/// <returns>Генератор свойств</returns>
		private PropertiesGeneratorBase GetPropertiesGenerator(IResultGenerationOptions options)
		{
			PropertiesGeneratorBase propertiesGenerator;

			bool isReadOnly = (options.PropertyModifiers & PropertyModifiers.ReadOnly) == PropertyModifiers.ReadOnly;
			bool isVirtual = (options.PropertyModifiers & PropertyModifiers.Virtual) == PropertyModifiers.Virtual;

			var propertyGenerationOptions = new PropertyGenerationOptions(isReadOnly, isVirtual);

			switch (options.PropertyType)
			{
				case PropertyType.Auto:
					propertiesGenerator = new AutoPropertiesGenerator(_typeMapper, propertyGenerationOptions);
					break;

				case PropertyType.BackingField:
					propertiesGenerator = options.ItemType == ResultItemType.NotifyPropertyChanged
						? new NotifyPropertyChangedBackingFieldPropertiesGenerator(_typeMapper, propertyGenerationOptions)
						: new BackingFieldPropertiesGenerator(_typeMapper, propertyGenerationOptions);
					break;

				default:
					throw new CodeGenerationException($"Unexpected options.propertyType [{options.PropertyType:G}]");
			}

			return propertiesGenerator;
		}
	}
}