using System.Collections.Generic;

namespace SqlFirst.Codegen.Text.QueryObject.Data
{
	internal interface IQueryObjectData
	{
		IEnumerable<string> Usings { get; }

		IEnumerable<string> Nested { get; }

		IEnumerable<string> Constants { get; }

		IEnumerable<string> Fields { get; }

		IEnumerable<string> Properties { get; }

		IEnumerable<string> Methods { get; }
	}
}