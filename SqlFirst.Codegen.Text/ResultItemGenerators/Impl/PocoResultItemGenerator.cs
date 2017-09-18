using SqlFirst.Codegen.Text.PropertyGenerator;
using SqlFirst.Codegen.Text.Snippets;

namespace SqlFirst.Codegen.Text.ResultItemGenerators.Impl
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