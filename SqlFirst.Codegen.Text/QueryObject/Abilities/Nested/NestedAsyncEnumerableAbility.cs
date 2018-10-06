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
			string externalControlledEnumerable = Snippet.Enumerable.Enumerable.Render();
			string asyncEnumerator = Snippet.Enumerable.AsyncEnumerator.Render();
			string dbAsyncEnumerator = Snippet.Enumerable.DbAsyncEnumerator.Render();
			string enumerableItem = Snippet.Enumerable.EnumerableItem.Render();

			QueryObjectData result = QueryObjectData.CreateFrom(data);

			result.Nested = result.Nested.AppendItems(externalControlledEnumerable, asyncEnumerator, dbAsyncEnumerator, enumerableItem);

			result.Usings = result.Usings.AppendItems(
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
		public IEnumerable<string> GetDependencies(ICodeGenerationContext context) => Enumerable.Empty<string>();

		/// <inheritdoc />
		public string Name { get; } = "AsyncEnumerable";
	}
}