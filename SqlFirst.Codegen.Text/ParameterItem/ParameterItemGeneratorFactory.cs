using SqlFirst.Codegen.Text.Common.PropertyGenerator;
using SqlFirst.Codegen.Text.ParameterItem.Impl;
using SqlFirst.Codegen.Text.ResultItem.TypedOptions;

namespace SqlFirst.Codegen.Text.ParameterItem
{
	internal static class ParameterItemGeneratorFactory
	{
		/// <summary>
		/// Возвращает генератор аргумента запроса
		/// </summary>
		/// <param name="options">Параметры генерации</param>
		/// <returns>Генератор аргумента запроса</returns>
		public static ParameterItemGeneratorBase Build(ParameterItemOptions options)
		{
			switch (options.ItemType)
			{
				case ItemType.Class:
					return GetClassResultItemsGenerator(options);

				case ItemType.Struct:
					return GetStructResultItemsGenerator(options);

				default:
					throw new CodeGenerationException($"Unexpected {nameof(ParameterItemOptions)}.{nameof(ParameterItemOptions.ItemType)} [{options.ItemType:G}]");
			}
		}

		private static ParameterItemGeneratorBase GetStructResultItemsGenerator(ParameterItemOptions options)
		{
			PropertiesGeneratorBase propertiesGenerator = PropertiesGeneratorFactory.Build(options);
			return new StructParameterItemGenerator(propertiesGenerator);
		}

		private static ParameterItemGeneratorBase GetClassResultItemsGenerator(ParameterItemOptions options)
		{
			PropertiesGeneratorBase propertiesGenerator = PropertiesGeneratorFactory.Build(options);
			return new PocoParameterItemGenerator(propertiesGenerator);
		}
	}
}