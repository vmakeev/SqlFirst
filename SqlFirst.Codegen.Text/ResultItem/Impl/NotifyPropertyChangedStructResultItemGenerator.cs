using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using SqlFirst.Codegen.Impl;
using SqlFirst.Codegen.Text.Common.PropertyGenerator;
using SqlFirst.Codegen.Text.QueryObject;
using SqlFirst.Codegen.Text.Snippets;
using SqlFirst.Codegen.Text.Templating;
using SqlFirst.Codegen.Trees;

namespace SqlFirst.Codegen.Text.ResultItem.Impl
{
	/// <summary>
	/// Генератор возвращаемого результата в виде структуры, поддерживающей уведомление об изменении свойств
	/// </summary>
	internal class NotifyPropertyChangedStructResultItemGenerator : ResultItemGeneratorBase
	{
		/// <summary>
		/// Инициализирует новый экземпляр класса <see cref="NotifyPropertyChangedStructResultItemGenerator" />
		/// </summary>
		/// <param name="propertiesGenerator">Генератор свойств</param>
		public NotifyPropertyChangedStructResultItemGenerator(PropertiesGeneratorBase propertiesGenerator)
			: base(propertiesGenerator)
		{
		}

		/// <inheritdoc />
		protected override IEnumerable<string> GetCommonUsings()
		{
			return base.GetCommonUsings().AppendItems("System.ComponentModel");
		}

		/// <inheritdoc />
		protected override GeneratedResultItem GenerateResultItemInternal(ICodeGenerationContext context)
		{
			GeneratedResultItem result = base.GenerateResultItemInternal(context);

			var baseTypes = new List<IGeneratedType>
			{
				new GeneratedType
				{
					IsInterface = true,
					TypeName = nameof(INotifyPropertyChanged)
				}
			};

			result.BaseTypes = result.BaseTypes == null
				? baseTypes
				: result.BaseTypes.Concat(baseTypes);

			return result;
		}

		/// <inheritdoc />
		protected override IRenderableTemplate GetTemplate() => Snippet.Item.Result.NotifyPropertyChangedStructResultItem;
	}
}