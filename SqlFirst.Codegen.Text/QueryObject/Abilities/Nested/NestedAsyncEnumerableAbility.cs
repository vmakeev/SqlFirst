using System.Collections.Generic;
using System.Linq;
using SqlFirst.Codegen.Text.QueryObject.Data;
using SqlFirst.Codegen.Text.Snippets;

namespace SqlFirst.Codegen.Text.QueryObject.Abilities.Nested
{
	internal class NestedAsyncEnumerableAbility : IQueryObjectAbility
	{
		/// <inheritdoc />
		public IQueryObjectData Apply(ICodeGenerationContext context, IQueryObjectData data)
		{
			string externalControlledEnumerable = EnumerableSnippet.Enumerable;
			string asyncEnumerator = EnumerableSnippet.AsyncEnumerator;
			string dbAsyncEnumerator = EnumerableSnippet.DbAsyncEnumerator;
			string enumerableItem = EnumerableSnippet.EnumerableItem;

			QueryObjectData result = QueryObjectData.CreateFrom(data);

			result.Nested = result.Nested.Append(externalControlledEnumerable, asyncEnumerator, dbAsyncEnumerator, enumerableItem);

			result.Usings = result.Usings.Append(
				"System",
				"System.Collections",
				"System.Collections.Generic",
				"System.Data",
				"System.Data.Common",
				"System.Threading",
				"System.Threading.Tasks");

			return result;
		}

		/// <inheritdoc />
		public IEnumerable<string> GetDependencies() => Enumerable.Empty<string>();

		/// <inheritdoc />
		public string Name { get; } = "AsyncEnumerable";
	}
}