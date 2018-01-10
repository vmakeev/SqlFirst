using System.Collections.Generic;
using SqlFirst.Codegen.Text.Common.PropertyGenerator;
using SqlFirst.Codegen.Text.Snippets;
using SqlFirst.Codegen.Text.Templating;
using SqlFirst.Codegen.Trees;

namespace SqlFirst.Codegen.Text.ResultItem.Impl
{
	/// <summary>
	/// Генератор возвращаемого результата в виде значимого типа
	/// </summary>
	internal class StructResultItemGenerator : ResultItemGeneratorBase
	{
		/// <summary>
		/// Инициализирует новый экземпляр класса <see cref="StructResultItemGenerator" />
		/// </summary>
		/// <param name="propertiesGenerator">Генератор свойств</param>
		public StructResultItemGenerator(PropertiesGeneratorBase propertiesGenerator)
			: base(propertiesGenerator)
		{
		}

		/// <inheritdoc />
		protected override IEnumerable<IGeneratedType> GetBaseTypes()
		{
			yield break;
		}

		/// <inheritdoc />
		protected override string ObjectType { get; } = ObjectTypes.Struct;

		/// <inheritdoc />
		protected override IRenderableTemplate GetTemplate() => Snippet.Item.Content.ResultItem;
	}
}