using System.Collections.Generic;
using System.Linq;
using SqlFirst.Codegen.Impl;
using SqlFirst.Codegen.Text.Common.PropertyGenerator;
using SqlFirst.Codegen.Text.Templating;
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

			IRenderableTemplate template = GetTemplate();

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

			IEnumerable<string> backingFields = propertiesInfo
												.SelectMany(info => info.Properties)
												.Where(propertyPart => propertyPart.IsBackingField)
												.Select(propertyPart => propertyPart.Content);

			IEnumerable<string> properties = propertiesInfo
											.SelectMany(info => info.Properties)
											.Where(propertyPart => !propertyPart.IsBackingField)
											.Select(p => p.Content);

			string itemText = template.Render(new
			{
				ItemName = result.ItemName,
				Modificators = result.ItemModifiers.ToArray(),
				Fields = backingFields,
				Properties = properties
			});

			result.Item = itemText;

			return result;
		}

		/// <summary>
		/// Возвращает шаблон кода для генерации объекта
		/// </summary>
		/// <returns>Шаблон кода для генерации объекта</returns>
		protected abstract IRenderableTemplate GetTemplate();
	}
}