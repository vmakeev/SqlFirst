using System;
using SqlFirst.Codegen.Text.Common.PropertyGenerator;
using SqlFirst.Codegen.Text.ResultItem.Impl;
using SqlFirst.Codegen.Text.ResultItem.TypedOptions;

namespace SqlFirst.Codegen.Text.ResultItem
{
	internal static class ResultItemGeneratorFactory
	{
		/// <summary>
		/// Возвращает генератор результата выполнения запроса
		/// </summary>
		/// <param name="options">Параметры генерации</param>
		/// <returns>Генератор результата выполнения запроса</returns>
		public static ResultItemGeneratorBase Build(ResultItemOptions options)
		{
			switch (options.ItemType)
			{
				case ItemType.Class:
					return GetClassResultItemsGenerator(options);

				case ItemType.Struct:
					return GetStructResultItemsGenerator(options);

				default:
					throw new CodeGenerationException($"Unexpected options.ItemType [{options.ItemType:G}]");
			}
		}

		private static ResultItemGeneratorBase GetStructResultItemsGenerator(ResultItemOptions options)
		{
			var propertiesGeneratorFactory = new Lazy<PropertiesGeneratorBase>(() => PropertiesGeneratorFactory.Build(options));

			if ((options.PropertyModifiers & PropertyModifiers.Virtual) == PropertyModifiers.Virtual)
			{
				throw new CodeGenerationException("Struct can not contains virtual properties");
			}

			if ((options.ItemAbilities & ResultItemAbilities.NotifyPropertyChanged) != ResultItemAbilities.NotifyPropertyChanged)
			{
				return new StructResultItemGenerator(propertiesGeneratorFactory.Value);
			}

			return new NotifyPropertyChangedStructResultItemGenerator(propertiesGeneratorFactory.Value);
		}

		private static ResultItemGeneratorBase GetClassResultItemsGenerator(ResultItemOptions options)
		{
			var propertiesGeneratorFactory = new Lazy<PropertiesGeneratorBase>(() => PropertiesGeneratorFactory.Build(options));

			if ((options.ItemAbilities & ResultItemAbilities.NotifyPropertyChanged) != ResultItemAbilities.NotifyPropertyChanged)
			{
				return new PocoResultItemGenerator(propertiesGeneratorFactory.Value);
			}

			if (options.PropertyType != PropertyType.BackingField)
			{
				throw new CodeGenerationException($"Ability [{ResultItemAbilities.NotifyPropertyChanged:G}] is incompatible with PropertyType [{options.PropertyType:G}]");
			}

			return new NotifyPropertyChangedClassResultItemGenerator(propertiesGeneratorFactory.Value);
		}
	}
}