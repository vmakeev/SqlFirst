using System.Collections.Generic;
using SqlFirst.Codegen.Text.QueryObject.Data;
using SqlFirst.Codegen.Text.Snippets;

namespace SqlFirst.Codegen.Text.QueryObject.Abilities.Common
{
	internal class ProcessCachedSqlPartialAbility : IQueryObjectAbility
	{
		/// <inheritdoc />
		public IQueryObjectData Apply(ICodeGenerationContext context, IQueryObjectData data)
		{
			string method = Snippet.Query.Methods.Common.ProcessCachedSqlPartial.Render();

			QueryObjectData result = QueryObjectData.CreateFrom(data);
			result.Methods = result.Methods.AppendItems(method);

			return result;
		}

		/// <inheritdoc />
		public IEnumerable<string> GetDependencies()
		{
			yield break;
		}

		/// <inheritdoc />
		public string Name { get; } = KnownAbilityName.ProcessCachedSql;
	}
}