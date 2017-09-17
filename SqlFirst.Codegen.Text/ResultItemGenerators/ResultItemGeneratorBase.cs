using SqlFirst.Codegen.Text.PropertyGenerator;

namespace SqlFirst.Codegen.Text.ResultItemGenerators
{
	internal abstract class ResultItemGeneratorBase
	{
		protected readonly PropertiesGeneratorBase _propertiesGenerator;

		/// <summary>Initializes a new instance of the <see cref="T:System.Object"></see> class.</summary>
		protected ResultItemGeneratorBase(PropertiesGeneratorBase propertiesGenerator)
		{
			_propertiesGenerator = propertiesGenerator;
		}

		public abstract IGeneratedResultItem GenerateResultItem(ICodeGenerationContext context, IResultGenerationOptions options);
	}
}