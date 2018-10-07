using System.Collections.Generic;
using System.Linq;
using SqlFirst.Codegen.Helpers;
using SqlFirst.Codegen.Impl;
using SqlFirst.Codegen.Text.Common.PropertyGenerator;
using SqlFirst.Codegen.Text.Snippets;
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
		private static readonly IEqualityComparer<IQueryParamInfo> _complexTypeParametersComparer = new ComplexTypeParametersComparer();

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
		public IEnumerable<IGeneratedParameterItem> GenerateParameterItems(ICodeGenerationContext context)
		{
			return GenerateParameterItemsInternal(context);
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
		protected virtual IEnumerable<IGeneratedParameterItem> GenerateParameterItemsInternal(ICodeGenerationContext context)
		{
			IGeneratedParameterItem numberedParameterItem = GenerateNumberedParameterItemInternal(context);
			IEnumerable<IGeneratedParameterItem> compositeParameters = GenerateCompositeParameterItemsInternal(context);

			// todo: is the name of the item unique?
			return compositeParameters
					.AppendItems(numberedParameterItem)
					.Where(item => item != null);
		}

		protected virtual IEnumerable<IGeneratedParameterItem> GenerateCompositeParameterItemsInternal(ICodeGenerationContext context)
		{
			IQueryParamInfo[] complexTypeParameters = context.IncomingParameters
															.Where(paramInfo => paramInfo.IsComplexType)
															.Distinct(_complexTypeParametersComparer)
															.ToArray();

			return complexTypeParameters.Select(paramInfo => GenerateCompositeParameterItemInternal(paramInfo, context));
		}

		protected virtual IGeneratedParameterItem GenerateCompositeParameterItemInternal(IQueryParamInfo item, ICodeGenerationContext context)
		{
			if (item.ComplexTypeData?.Fields == null)
			{
				return null;
			}

			string targetNamespace = context.GetNamespace();
			IRenderableTemplate template = GetTemplate();

			string itemName = CSharpCodeHelper.GetValidIdentifierName(item.ComplexTypeData.Name ?? item.DbType, NamingPolicy.Pascal);

			var result = new GeneratedParameterItem
			{
				Namespace = targetNamespace,
				Modifiers = new[] { Modifiers.Public },
				Name = itemName,
				BaseTypes = Enumerable.Empty<IGeneratedType>()
			};

			IEnumerable<CodeMemberInfo> memberInfos = item.ComplexTypeData.Fields
														.Select(fieldDetails => CodeMemberInfo.FromFieldDetails(fieldDetails, context.TypeMapper));
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
				Fields = backingFields,
				Properties = properties
			});

			result.Content = itemText;
			result.ObjectType = ObjectType;

			return result;
		}

		/// <summary>
		/// Выполняет непосредственную генерацию объекта нумерованного параметра запроса
		/// </summary>
		/// <param name="context">Контекст генерации</param>
		/// <returns>Сгенерированный код, описывающий нумерованный параметр запроса</returns>
		protected virtual IGeneratedParameterItem GenerateNumberedParameterItemInternal(ICodeGenerationContext context)
		{
			IQueryParamInfo[] targetParameters = context.IncomingParameters.Where(paramInfo => paramInfo.IsNumbered).ToArray();
			if (!targetParameters.Any())
			{
				return null;
			}

			string targetNamespace = context.GetNamespace();

			IRenderableTemplate template = GetTemplate();

			string itemName = context.GetQueryParameterItemTypeName();

			var result = new GeneratedParameterItem
			{
				Namespace = targetNamespace,
				Modifiers = new[] { Modifiers.Public },
				Name = itemName,
				BaseTypes = Enumerable.Empty<IGeneratedType>()
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
				Fields = backingFields,
				Properties = properties
			});

			result.Content = itemText;
			result.ObjectType = ObjectType;

			return result;
		}

		protected abstract string ObjectType { get; }

		/// <summary>
		/// Возвращает шаблон кода для генерации объекта
		/// </summary>
		/// <returns>Шаблон кода для генерации объекта</returns>
		protected IRenderableTemplate GetTemplate() => Snippet.Item.Content.ParameterItem;
	}
}