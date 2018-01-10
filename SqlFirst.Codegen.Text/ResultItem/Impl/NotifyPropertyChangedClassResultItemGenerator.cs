using System.Collections.Generic;
using System.ComponentModel;
using SqlFirst.Codegen.Impl;
using SqlFirst.Codegen.Text.Common.PropertyGenerator;
using SqlFirst.Codegen.Text.QueryObject;
using SqlFirst.Codegen.Text.Snippets;
using SqlFirst.Codegen.Text.Templating;
using SqlFirst.Codegen.Trees;

namespace SqlFirst.Codegen.Text.ResultItem.Impl
{
	/// <summary>
	/// Генератор возвращаемого результата в виде класса, поддерживающего уведомление об изменении свойств
	/// </summary>
	internal class NotifyPropertyChangedClassResultItemGenerator : ResultItemGeneratorBase
	{
		/// <summary>
		/// Инициализирует новый экземпляр класса <see cref="NotifyPropertyChangedClassResultItemGenerator" />
		/// </summary>
		/// <param name="propertiesGenerator">Генератор свойств</param>
		public NotifyPropertyChangedClassResultItemGenerator(PropertiesGeneratorBase propertiesGenerator)
			: base(propertiesGenerator)
		{
		}

		/// <inheritdoc />
		protected override IEnumerable<string> GetCommonUsings()
		{
			return base.GetCommonUsings().AppendItems(typeof(INotifyPropertyChanged).Namespace);
		}

		/// <inheritdoc />
		protected override IEnumerable<IGeneratedType> GetBaseTypes()
		{
			yield return new GeneratedType(typeof(INotifyPropertyChanged));
		}

		/// <inheritdoc />
		protected override string ObjectType { get; } = ObjectTypes.Class;

		/// <inheritdoc />
		protected override IRenderableTemplate GetTemplate() => Snippet.Item.Content.ResultItemInpc;
	}
}