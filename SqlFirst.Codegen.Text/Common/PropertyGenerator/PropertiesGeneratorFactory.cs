using System.Diagnostics.CodeAnalysis;
using SqlFirst.Codegen.Text.Common.PropertyGenerator.Impl;
using SqlFirst.Codegen.Text.ResultItem.TypedOptions;

namespace SqlFirst.Codegen.Text.Common.PropertyGenerator
{
	internal static class PropertiesGeneratorFactory
	{
		/// <summary>
		/// Возвращает генератор свойств
		/// </summary>
		/// <param name="options">Параметры генерации</param>
		/// <returns>Генератор свойств</returns>
		public static PropertiesGeneratorBase Build(ResultItemOptions options)
		{
			bool isReadOnly = (options.PropertyModifiers & PropertyModifiers.ReadOnly) == PropertyModifiers.ReadOnly;
			bool isVirtual = (options.PropertyModifiers & PropertyModifiers.Virtual) == PropertyModifiers.Virtual;

			var propertyGenerationOptions = new PropertyGenerationOptions(isReadOnly, isVirtual);

			switch (options.PropertyType)
			{
				case PropertyType.Auto:
					return new AutoPropertiesGenerator(propertyGenerationOptions);

				case PropertyType.BackingField:
					if ((options.ItemAbilities & ResultItemAbilities.NotifyPropertyChanged) == ResultItemAbilities.NotifyPropertyChanged)
					{
						return new NotifyPropertyChangedBackingFieldPropertiesGenerator(propertyGenerationOptions);
					}

					return new BackingFieldPropertiesGenerator(propertyGenerationOptions);

				case PropertyType.INVALID:
				default:
					throw new CodeGenerationException($"Unexpected options.propertyType [{options.PropertyType:G}]");
			}
		}

		/// <summary>
		/// Возвращает генератор свойств
		/// </summary>
		/// <param name="options">Параметры генерации</param>
		/// <returns>Генератор свойств</returns>
		[SuppressMessage("ReSharper", "UnusedParameter.Global")]
		public static PropertiesGeneratorBase Build(ParameterItemOptions options)
		{
			var propertyGenerationOptions = new PropertyGenerationOptions(isReadOnly: false, isVirtual: false);
			return new AutoPropertiesGenerator(propertyGenerationOptions);
		}
	}
}