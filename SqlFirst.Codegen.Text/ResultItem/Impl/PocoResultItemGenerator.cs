using SqlFirst.Codegen.Text.ResultItem.PropertyGenerator;
using SqlFirst.Codegen.Text.Snippets;

namespace SqlFirst.Codegen.Text.ResultItem.Impl
{
	/// <summary>
	/// Генератор возвращаемого результата в виде обычного класса
	/// </summary>
	internal class PocoResultItemGenerator : ResultItemGeneratorBase
	{
		/// <summary>
		/// Инициализирует новый экземпляр класса <see cref="PocoResultItemGenerator"/>
		/// </summary>
		/// <param name="propertiesGenerator">Генератор свойств</param>
		public PocoResultItemGenerator(PropertiesGeneratorBase propertiesGenerator)
			: base(propertiesGenerator)
		{
		}

		/// <inheritdoc />
		protected override string GetTemplate() => ItemSnippet.PocoResultItem;
	}
}