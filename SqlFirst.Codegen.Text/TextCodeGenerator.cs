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
			switch (options.ItemType)
			{
				case ResultItemType.Class:
					return GetClassResultItemsGenerator(options, propertiesGenerator);

				case ResultItemType.Struct:
					return GetStructResultItemsGenerator(options, propertiesGenerator);

				default:
					throw new CodeGenerationException($"Unexpected options.ItemType [{options.ItemType:G}]");
			}
		}

		private static ResultItemGeneratorBase GetStructResultItemsGenerator(IResultGenerationOptions options, PropertiesGeneratorBase propertiesGenerator)
		{
			if ((options.PropertyModifiers & PropertyModifiers.Virtual) == PropertyModifiers.Virtual)
			{
				throw new CodeGenerationException("Struct can not contains virtual properties");
			}

			if ((options.ItemAbilities & ResultItemAbilities.NotifyPropertyChanged) != ResultItemAbilities.NotifyPropertyChanged)
			{
				return new StructResultItemGenerator(propertiesGenerator);
			}

			return new NotifyPropertyChangedStructResultItemGenerator(propertiesGenerator);
		}

		private static ResultItemGeneratorBase GetClassResultItemsGenerator(IResultGenerationOptions options, PropertiesGeneratorBase propertiesGenerator)
		{
			if ((options.ItemAbilities & ResultItemAbilities.NotifyPropertyChanged) != ResultItemAbilities.NotifyPropertyChanged)
			{
				return new PocoResultItemGenerator(propertiesGenerator);
			}

			if (options.PropertyType != PropertyType.BackingField)
			{
				throw new CodeGenerationException($"ResultItemType [{options.ItemType:G}] is incompatible with PropertyType [{options.PropertyType:G}]");
			}

			return new NotifyPropertyChangedClassResultItemGenerator(propertiesGenerator);
		}

		/// <summary>
		/// Возвращает генератор свойств
		/// </summary>
		/// <param name="options">Параметры генерации</param>
		/// <returns>Генератор свойств</returns>
		private PropertiesGeneratorBase GetPropertiesGenerator(IResultGenerationOptions options)
		{
			bool isReadOnly = (options.PropertyModifiers & PropertyModifiers.ReadOnly) == PropertyModifiers.ReadOnly;
			bool isVirtual = (options.PropertyModifiers & PropertyModifiers.Virtual) == PropertyModifiers.Virtual;

			var propertyGenerationOptions = new PropertyGenerationOptions(isReadOnly, isVirtual);

			switch (options.PropertyType)
			{
				case PropertyType.Auto:
					return new AutoPropertiesGenerator(_typeMapper, propertyGenerationOptions);

				case PropertyType.BackingField:
					if ((options.ItemAbilities & ResultItemAbilities.NotifyPropertyChanged) == ResultItemAbilities.NotifyPropertyChanged)
					{
						return new NotifyPropertyChangedBackingFieldPropertiesGenerator(_typeMapper, propertyGenerationOptions);
					}
					return new BackingFieldPropertiesGenerator(_typeMapper, propertyGenerationOptions);

				default:
					throw new CodeGenerationException($"Unexpected options.propertyType [{options.PropertyType:G}]");
			}
		}
	}
}