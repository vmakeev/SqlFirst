using System.Collections.Generic;
using SqlFirst.Core.Parsing;

namespace SqlFirst.Codegen.Text.PropertyGenerator
{
	internal abstract class PropertiesGeneratorBase
	{
		public abstract IEnumerable<GeneratedPropertyInfo> GenerateProperties(IEnumerable<IFieldDetails> results, PropertyType propertyType);
	}
}