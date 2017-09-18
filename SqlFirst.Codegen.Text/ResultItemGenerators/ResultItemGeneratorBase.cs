using System;
using System.Collections.Generic;
using System.Linq;
using SqlFirst.Codegen.Helpers;
using SqlFirst.Codegen.Impl;
using SqlFirst.Codegen.Text.PropertyGenerator;
using SqlFirst.Codegen.Trees;

namespace SqlFirst.Codegen.Text.ResultItemGenerators
{
	/// <summary>
	/// Базовая реализация генератора возвращаемого результата
	/// </summary>
	internal abstract class ResultItemGeneratorBase
	{
		protected readonly PropertiesGeneratorBase _propertiesGenerator;

		/// <summary>
		/// Инициализирует новый экземпляр класса <see cref="ResultItemGeneratorBase"/>
		/// </summary>
		/// <param name="propertiesGenerator">Генератор свойств</param>
		protected ResultItemGeneratorBase(PropertiesGeneratorBase propertiesGenerator)
		{
			_propertiesGenerator = propertiesGenerator;
		}

		/// <summary>
		/// Выполняет генерацию возвращаемого запросом результата
		/// </summary>
		/// <param name="context">Контекст генерации</param>
		/// <returns>Сгенерированный код, описывающий результат запроса</returns>
		public IGeneratedResultItem GenerateResultItem(ICodeGenerationContext context)
		{
			return GenerateResultItemInternal(context);
		}

		/// <summary>
		/// Возвращает импорты, требуемые для генерируемого объекта, не включая требуемые для его свойств
		/// </summary>
		/// <returns>Требуемые импорты</returns>
		protected virtual IEnumerable<string> GetCommonUsings()
		{
			yield break;
		}

		/// <summary>
		/// Выполняет непосрезственную генерацию возвращаемого запросом результата
		/// </summary>
		/// <param name="context">Контекст генерации</param>
		/// <returns>Сгенерированный код, описывающий результат запроса</returns>
		protected virtual GeneratedResultItem GenerateResultItemInternal(ICodeGenerationContext context)
		{
			string targetNamespace = context.GetNamespace();

			string template = GetTemplate();

			const string postfix = @"Query";
			string queryName = context.GetQueryName();
			if (queryName.EndsWith(postfix) && queryName.Length > postfix.Length)
			{
				queryName = queryName.Substring(0, queryName.Length - postfix.Length);
			}

			string itemName = CSharpCodeHelper.GetValidIdentifierName(queryName, NamingPolicy.Pascal) + "Item";

			var result = new GeneratedResultItem
			{
				Namespace = targetNamespace,
				ItemModifiers = new[] { Modifiers.Public, Modifiers.Partial },
				ItemName = itemName,
				BaseTypes = new IGeneratedType[0]
			};

			GeneratedPropertyInfo[] propertiesInfo = _propertiesGenerator.GenerateProperties(context.OutgoingParameters).ToArray();

			IEnumerable<string> allUsings = GetCommonUsings().Concat(propertiesInfo.SelectMany(p => p.Usings));
			result.Usings = allUsings.Distinct().OrderBy(@using => @using);

			string space = Environment.NewLine + Environment.NewLine;
			const string indent = "\t";

			IEnumerable<string> backingFields = propertiesInfo
				.SelectMany(info => info.Properties)
				.Where(propertyPart => propertyPart.IsBackingField)
				.Select(propertyPart => propertyPart.Content);
			string backingFieldsText = string.Join(Environment.NewLine, backingFields.Select(field => field.Indent(indent)));

			IEnumerable<string> properties = propertiesInfo
				.SelectMany(info => info.Properties)
				.Where(propertyPart => !propertyPart.IsBackingField)
				.Select(p => p.Content);
			string propertiesText = string.Join(space, properties.Select(property => property.Indent(indent)));

			string fullPropertiesText = string.IsNullOrEmpty(backingFieldsText)
				? propertiesText
				: backingFieldsText + space + propertiesText;

			string itemText = template
				.Replace("$ItemName$", result.ItemName)
				.Replace("$Modificators$", string.Join(" ", result.ItemModifiers))
				.Replace("$Properties$", fullPropertiesText);

			result.Item = itemText;

			return result;
		}

		/// <summary>
		/// Возвращает шаблон кода для генерации объекта
		/// </summary>
		/// <returns>Шаблон кода для генерации объекта</returns>
		protected abstract string GetTemplate();
	}
}