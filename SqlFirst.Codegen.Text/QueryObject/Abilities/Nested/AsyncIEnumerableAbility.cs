using System.Collections.Generic;
using System.Linq;
using SqlFirst.Codegen.Text.QueryObject.Data;

namespace SqlFirst.Codegen.Text.QueryObject.Abilities.Nested
{
	internal class AsyncIEnumerableAbility : IQueryObjectAbility
	{
		/// <inheritdoc />
		public IQueryObjectData Apply(ICodeGenerationContext context, IQueryObjectData data)
		{
			QueryObjectData result = QueryObjectData.CreateFrom(data);
			result.Usings = result.Usings.AppendItems(
				"System.Collections.Generic",
				"System.Runtime.CompilerServices"
			);
			return result;
		}

		/// <inheritdoc />
		public IEnumerable<string> GetDependencies() => Enumerable.Empty<string>();

		/// <inheritdoc />
		public string Name { get; } = "AsyncEnumerable";
	}
}