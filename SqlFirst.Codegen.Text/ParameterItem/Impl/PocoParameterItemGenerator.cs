using SqlFirst.Codegen.Text.Common.PropertyGenerator;
using SqlFirst.Codegen.Text.Snippets;

namespace SqlFirst.Codegen.Text.ParameterItem.Impl
{
	/// <summary>
	/// Генератор аргумента в виде обычного класса
	/// </summary>
	internal class PocoParameterItemGenerator : ParameterItemGeneratorBase
	{
		/// <summary>
		/// Инициализирует новый экземпляр класса <see cref="ParameterItemGeneratorBase" />
		/// </summary>
		/// <param name="propertiesGenerator">Генератор свойств</param>
		public PocoParameterItemGenerator(PropertiesGeneratorBase propertiesGenerator)
			: base(propertiesGenerator)
		{
		}

		/// <summary>
		/// Возвращает шаблон кода для генерации объекта
		/// </summary>
		/// <returns>Шаблон кода для генерации объекта</returns>
		protected override string GetTemplate() => ParameterItemSnippet.PocoParameterItem;
	}
}