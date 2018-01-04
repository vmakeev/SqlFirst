using System.Collections.Generic;
using System.Linq;

namespace SqlFirst.Codegen.Text.QueryObject.Data
{
	internal class QueryObjectData : IQueryObjectData
	{
		public IEnumerable<string> Usings { get; set; } = Enumerable.Empty<string>();

		public IEnumerable<string> Nested { get; set; } = Enumerable.Empty<string>();

		public IEnumerable<string> Constants { get; set; } = Enumerable.Empty<string>();

		public IEnumerable<string> Fields { get; set; } = Enumerable.Empty<string>();

		public IEnumerable<string> Properties { get; set; } = Enumerable.Empty<string>();

		public IEnumerable<string> Methods { get; set; } = Enumerable.Empty<string>();

		public static QueryObjectData CreateFrom(IQueryObjectData source)
		{
			return new QueryObjectData
			{
				Methods = source.Methods ?? Enumerable.Empty<string>(),
				Constants = source.Constants ?? Enumerable.Empty<string>(),
				Fields = source.Fields ?? Enumerable.Empty<string>(),
				Usings = source.Usings ?? Enumerable.Empty<string>(),
				Nested = source.Nested ?? Enumerable.Empty<string>(),
				Properties = source.Properties ?? Enumerable.Empty<string>(),
			};
		}
	}
}