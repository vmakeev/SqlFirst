using System.Collections.Generic;
using System.Linq;
using SqlFirst.Codegen.Impl;
using SqlFirst.Codegen.Text.Common.PropertyGenerator;
using SqlFirst.Codegen.Text.Templating;
using SqlFirst.Codegen.Trees;

namespace SqlFirst.Codegen.Text.ResultItem
{
	/// <summary>
	/// Базовая реализация генератора возвращаемого результата
	/// </summary>
	internal abstract class ResultItemGeneratorBase
	{
		protected readonly PropertiesGeneratorBase _propertiesGenerator;

		/// <summary>
		/// Инициализирует новый экземпляр класса <see cref="ResultItemGeneratorBase" />
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
		/// Выполняет непосредственную генерацию возвращаемого запросом результата
		/// </summary>
		/// <param name="context">Контекст генерации</param>
		/// <returns>Сгенерированный код, описывающий результат запроса</returns>
		protected virtual GeneratedResultItem GenerateResultItemInternal(ICodeGenerationContext context)
		{
			string targetNamespace = context.GetNamespace();

			IRenderableTemplate template = GetTemplate();

			string itemName = context.GetQueryResultItemTypeName();

			var result = new GeneratedResultItem
			{
				Namespace = targetNamespace,
				Modifiers = new[] { Modifiers.Public, Modifiers.Partial },
				Name = itemName,
				BaseTypes = GetBaseTypes(),
				ObjectType = ObjectType
			};

			IEnumerable<CodeMemberInfo> memberInfos = context.OutgoingParameters.Select(info => CodeMemberInfo.FromFieldDetails(info, context.TypeMapper));
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

			string item = template.Render(new
			{
				Fields = backingFields,
				Properties = properties
			});

			result.Content = item;

			return result;
		}

		/// <summary>
		/// Возвращает базовые для генерируемого объекта типы
		/// </summary>
		/// <returns></returns>
		protected abstract IEnumerable<IGeneratedType> GetBaseTypes();

		/// <summary>
		/// Тип объекта
		/// </summary>
		protected abstract string ObjectType { get; }

		/// <summary>
		/// Возвращает шаблон кода для генерации объекта
		/// </summary>
		/// <returns>Шаблон кода для генерации объекта</returns>
		protected abstract IRenderableTemplate GetTemplate();
	}
}