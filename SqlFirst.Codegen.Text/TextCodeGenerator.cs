using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SqlFirst.Codegen.Text.PropertyGenerator;
using SqlFirst.Codegen.Text.PropertyGenerator.Impl;
using SqlFirst.Codegen.Text.ResultItemGenerators;
using SqlFirst.Codegen.Text.ResultItemGenerators.Impl;
using SqlFirst.Codegen.Text.Snippets;
using SqlFirst.Codegen.Text.TypedOptions;
using SqlFirst.Codegen.Trees;
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
			var itemOptions = new ItemOptions(options.SqlFirstOptions?.ToArray());

			PropertiesGeneratorBase propertiesGenerator = GetPropertiesGenerator(itemOptions);
			ResultItemGeneratorBase itemGenerator = GetResultItemGenerator(itemOptions, propertiesGenerator);
			return itemGenerator.GenerateResultItem(context);
		}

		/// <inheritdoc />
		public string GenerateFile(IEnumerable<IGeneratedItem> generatedItems)
		{
			IGeneratedItem[] items = generatedItems.ToArray();

			string usingSnipper = FileSnippet.Using;

			IEnumerable<string> usings = items
				.SelectMany(generatedItem => generatedItem.Usings)
				.Distinct(StringComparer.InvariantCultureIgnoreCase)
				.OrderBy(usingName => usingName)
				.Select(usingName => usingSnipper.Replace("$Using$", usingName));

			string usingsText = FileSnippet.Usings.Replace("$Usings$", string.Join(string.Empty, usings)).Trim();

			IGrouping<string, (string Namespace, string Data)>[] namespaceDataItems = items
				.Select(generatedItem => (Namespace: generatedItem.Namespace, Data: generatedItem.Item))
				.GroupBy(item => item.Namespace).ToArray();


			var namespaces = new StringBuilder();
			string namespaceSnippet = FileSnippet.Namespace;
			foreach (IGrouping<string, (string Namespace, string Data)> namespaceDataItem in namespaceDataItems)
			{
				string namespaceName = namespaceDataItem.Key;
				string[] data = namespaceDataItem.Select(p => p.Data).ToArray();

				string namespaceData = string.Join(Environment.NewLine + Environment.NewLine, data.Select(p => p.Indent("\t")));

				string namespaceText = namespaceSnippet
										.Replace("$Namespace$", namespaceName)
										.Replace("$Data$", namespaceData);

				namespaces.Append(namespaceText);
			}

			string namespacesText = namespaces.ToString();

			string result = FileSnippet.DefaultFile
				.Replace("$Usings$", usingsText)
				.Replace("$Namespaces$", namespacesText)
				.Trim();

			return result;
		}

		/// <summary>
		/// Возвращает генератор результата выполнения запроса
		/// </summary>
		/// <param name="options">Параметры генерации</param>
		/// <param name="propertiesGenerator">Генератор свойств</param>
		/// <returns>Генератор результата выполнения запроса</returns>
		private static ResultItemGeneratorBase GetResultItemGenerator(ItemOptions options, PropertiesGeneratorBase propertiesGenerator)
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

		private static ResultItemGeneratorBase GetStructResultItemsGenerator(ItemOptions options, PropertiesGeneratorBase propertiesGenerator)
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

		private static ResultItemGeneratorBase GetClassResultItemsGenerator(ItemOptions options, PropertiesGeneratorBase propertiesGenerator)
		{
			if ((options.ItemAbilities & ResultItemAbilities.NotifyPropertyChanged) != ResultItemAbilities.NotifyPropertyChanged)
			{
				return new PocoResultItemGenerator(propertiesGenerator);
			}

			if (options.PropertyType != PropertyType.BackingField)
			{
				throw new CodeGenerationException($"Ability [{ResultItemAbilities.NotifyPropertyChanged:G}] is incompatible with PropertyType [{options.PropertyType:G}]");
			}

			return new NotifyPropertyChangedClassResultItemGenerator(propertiesGenerator);
		}

		/// <summary>
		/// Возвращает генератор свойств
		/// </summary>
		/// <param name="options">Параметры генерации</param>
		/// <returns>Генератор свойств</returns>
		private PropertiesGeneratorBase GetPropertiesGenerator(ItemOptions options)
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