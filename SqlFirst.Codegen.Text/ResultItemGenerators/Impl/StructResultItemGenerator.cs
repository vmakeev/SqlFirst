using System;
using SqlFirst.Codegen.Text.PropertyGenerator;

namespace SqlFirst.Codegen.Text.ResultItemGenerators.Impl
{
	internal class StructResultItemGenerator : ResultItemGeneratorBase
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Object"></see> class.</summary>
		public StructResultItemGenerator(PropertiesGeneratorBase propertiesGenerator)
			: base(propertiesGenerator)
		{
		}

		public override IGeneratedResultItem GenerateResultItem(ICodeGenerationContext context, IResultGenerationOptions options)
		{
			throw new NotImplementedException();
		}
	}
}