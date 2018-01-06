using System;
using System.Collections.Generic;
using System.Linq;
using SqlFirst.Codegen.Impl;
using SqlFirst.Codegen.Text.Common.PropertyGenerator;
using SqlFirst.Codegen.Text.Snippets;
using SqlFirst.Codegen.Trees;
using SqlFirst.Core;

namespace SqlFirst.Codegen.Text.ParameterItem
{
	/// <summary>
	/// Базовая реализация генератора объекта параметра запроса
	/// </summary>
	internal abstract class ParameterItemGeneratorBase
	{
		protected readonly PropertiesGeneratorBase _propertiesGenerator;

		/// <summary>
		/// Инициализирует новый экземпляр класса <see cref="ParameterItemGeneratorBase" />
		/// </summary>
		/// <param name="propertiesGenerator">Генератор свойств</param>
		protected ParameterItemGeneratorBase(PropertiesGeneratorBase propertiesGenerator)
		{
			_propertiesGenerator = propertiesGenerator;
		}

		/// <summary>
		/// Выполняет генерацию объекта параметра запроса
		/// </summary>
		/// <param name="context">Контекст генерации</param>
		/// <returns>Сгенерированный код, описывающий параметры запроса</returns>
		public IGeneratedParameterItem GenerateParameterItem(ICodeGenerationContext context)
		{
			return GenerateParameterItemInternal(context);
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
		/// Выполняет непосредственную генерацию объекта параметра запроса
		/// </summary>
		/// <param name="context">Контекст генерации</param>
		/// <returns>Сгенерированный код, описывающий параметры запроса</returns>
		protected virtual GeneratedParameterItem GenerateParameterItemInternal(ICodeGenerationContext context)
		{
			IQueryParamInfo[] targetParameters = context.IncomingParameters.Where(paramInfo => paramInfo.IsNumbered).ToArray();
			
			string targetNamespace = context.GetNamespace();

			string template = GetTemplate();

			string itemName = context.GetQueryParameterItemTypeName();

			var result = new GeneratedParameterItem
			{
				Namespace = targetNamespace,
				ItemModifiers = new[] { Modifiers.Public },
				ItemName = itemName,
				BaseTypes = new IGeneratedType[0]
			};

			IEnumerable<CodeMemberInfo> memberInfos = targetParameters.Select(info => CodeMemberInfo.FromQueryParamInfo(info, context.TypeMapper));
			GeneratedPropertyInfo[] propertiesInfo = _propertiesGenerator.GenerateProperties(memberInfos).ToArray();

			IEnumerable<string> allUsings = GetCommonUsings().Concat(propertiesInfo.SelectMany(p => p.Usings));
			result.Usings = allUsings.Distinct().OrderBy(@using => @using);

			string space = Environment.NewLine + Environment.NewLine;

			IEnumerable<string> backingFields = propertiesInfo
												.SelectMany(info => info.Properties)
												.Where(propertyPart => propertyPart.IsBackingField)
												.Select(propertyPart => propertyPart.Content);
			string backingFieldsText = string.Join(Environment.NewLine, backingFields.Select(field => field.Indent(QuerySnippet.Indent, 1)));

			IEnumerable<string> properties = propertiesInfo
											.SelectMany(info => info.Properties)
											.Where(propertyPart => !propertyPart.IsBackingField)
											.Select(p => p.Content);
			string propertiesText = string.Join(space, properties.Select(property => property.Indent(QuerySnippet.Indent, 1)));

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