using SqlFirst.Codegen.Text.Common.PropertyGenerator;
using SqlFirst.Codegen.Text.Snippets;
using SqlFirst.Codegen.Text.Templating;

namespace SqlFirst.Codegen.Text.ResultItem.Impl
{
	/// <summary>
	/// Генератор возвращаемого результата в виде обычного класса
	/// </summary>
	internal class PocoResultItemGenerator : ResultItemGeneratorBase
	{
		/// <summary>
		/// Инициализирует новый экземпляр класса <see cref="PocoResultItemGenerator" />
		/// </summary>
		/// <param name="propertiesGenerator">Генератор свойств</param>
		public PocoResultItemGenerator(PropertiesGeneratorBase propertiesGenerator)
			: base(propertiesGenerator)
		{
		}

		/// <inheritdoc />
		protected override IRenderableTemplate GetTemplate() => Snippet.Item.Result.PocoResultItem;
	}
}