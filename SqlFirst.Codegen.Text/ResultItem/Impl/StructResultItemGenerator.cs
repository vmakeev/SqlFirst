using SqlFirst.Codegen.Text.Common.PropertyGenerator;
using SqlFirst.Codegen.Text.Snippets;

namespace SqlFirst.Codegen.Text.ResultItem.Impl
{
	/// <summary>
	/// Генератор возвращаемого результата в виде значимого типа
	/// </summary>
	internal class StructResultItemGenerator : ResultItemGeneratorBase
	{
		/// <summary>
		/// Инициализирует новый экземпляр класса <see cref="StructResultItemGenerator"/>
		/// </summary>
		/// <param name="propertiesGenerator">Генератор свойств</param>
		public StructResultItemGenerator(PropertiesGeneratorBase propertiesGenerator)
			: base(propertiesGenerator)
		{
		}

		/// <inheritdoc />
		protected override string GetTemplate() => ResultItemSnippet.StructResultItem;
	}
}