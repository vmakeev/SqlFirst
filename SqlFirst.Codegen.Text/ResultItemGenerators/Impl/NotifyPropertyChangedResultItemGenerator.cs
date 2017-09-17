using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using SqlFirst.Codegen.Impl;
using SqlFirst.Codegen.Text.PropertyGenerator;
using SqlFirst.Codegen.Text.Snippets;
using SqlFirst.Codegen.Trees;

namespace SqlFirst.Codegen.Text.ResultItemGenerators.Impl
{
	/// <summary>
	/// Генератор возвращаемого результата в виде класса, поддерживающего уведомление об изменении свойств
	/// </summary>
	internal class NotifyPropertyChangedResultItemGenerator : ResultItemGeneratorBase
	{
		/// <summary>
		/// Инициализирует новый экземпляр класса <see cref="NotifyPropertyChangedResultItemGenerator"/>
		/// </summary>
		/// <param name="propertiesGenerator">Генератор свойств</param>
		public NotifyPropertyChangedResultItemGenerator(PropertiesGeneratorBase propertiesGenerator)
			: base(propertiesGenerator)
		{
		}

		/// <inheritdoc />
		protected override IEnumerable<string> GetCommonUsings()
		{
			foreach (string @using in base.GetCommonUsings())
			{
				yield return @using;
			}

			yield return "System.ComponentModel";
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
		protected override string GetTemplate() => Snippet.NotifyPropertyChangedResultItem;
	}
}