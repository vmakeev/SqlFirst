using System.Linq;
using SqlFirst.Codegen.Impl;
using SqlFirst.Codegen.Text.Common.PropertyGenerator;
using SqlFirst.Codegen.Text.Snippets;
using SqlFirst.Core;

namespace SqlFirst.Codegen.Text.ParameterItem.Impl
{
	/// <summary>
	/// Генератор аргумента в виде значимого типа
	/// </summary>
	internal class StructParameterItemGenerator : ParameterItemGeneratorBase
	{
		/// <summary>
		/// Инициализирует новый экземпляр класса <see cref="ParameterItemGeneratorBase" />
		/// </summary>
		/// <param name="propertiesGenerator">Генератор свойств</param>
		public StructParameterItemGenerator(PropertiesGeneratorBase propertiesGenerator)
			: base(propertiesGenerator)
		{
		}

		/// <summary>
		/// Выполняет непосредственную генерацию объекта параметра запроса
		/// </summary>
		/// <param name="context">Контекст генерации</param>
		/// <returns>Сгенерированный код, описывающий параметры запроса</returns>
		protected override GeneratedParameterItem GenerateParameterItemInternal(ICodeGenerationContext context)
		{
			IQueryParamInfo[] paramsWithDefaults = context.IncomingParameters.Where(paramInfo => paramInfo.DefaultValue != null).ToArray();

			if (paramsWithDefaults.Any())
			{
				string names = string.Join(", ", paramsWithDefaults.Select(paramInfo => paramInfo.DbName));
				throw new CodeGenerationException($"Struct parameter item can not contains default values. Invalid parameters list: {names}");
			}

			return base.GenerateParameterItemInternal(context);
		}

		/// <summary>
		/// Возвращает шаблон кода для генерации объекта
		/// </summary>
		/// <returns>Шаблон кода для генерации объекта</returns>
		protected override string GetTemplate() => ParameterItemSnippet.StructParameterItem;
	}
}