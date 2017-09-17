using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using SqlFirst.Codegen.Impl;
using SqlFirst.Codegen.Text.PropertyGenerator;
using SqlFirst.Codegen.Text.Snippets;
using SqlFirst.Codegen.Trees;

namespace SqlFirst.Codegen.Text.ResultItemGenerators.Impl
{
	internal class NotifyPropertyChangedResultItemGenerator : PocoResultItemGenerator
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Object"></see> class.</summary>
		public NotifyPropertyChangedResultItemGenerator(PropertiesGeneratorBase propertiesGenerator)
			: base(propertiesGenerator)
		{
		}

		protected override IEnumerable<string> GetCommonUsings()
		{
			foreach (string @using in base.GetCommonUsings())
			{
				yield return @using;
			}

			yield return "System.ComponentModel";
		}

		protected override GeneratedResultItem GenerateResultItemInternal(ICodeGenerationContext context, IResultGenerationOptions options)
		{
			GeneratedResultItem result = base.GenerateResultItemInternal(context, options);

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

		protected override string GetTemplate() => Snippet.NotifyPropertyChangedResultItem;
	}
}