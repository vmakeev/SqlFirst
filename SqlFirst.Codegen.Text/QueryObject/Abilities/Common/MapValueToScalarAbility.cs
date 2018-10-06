using System.Collections.Generic;
using SqlFirst.Codegen.Text.QueryObject.Data;
using SqlFirst.Codegen.Text.Snippets;

namespace SqlFirst.Codegen.Text.QueryObject.Abilities.Common
{
	internal class MapValueToScalarAbility : IQueryObjectAbility
	{
		/// <inheritdoc />
		public IQueryObjectData Apply(ICodeGenerationContext context, IQueryObjectData data)
		{
			string method = Snippet.Query.Methods.Common.GetScalarFromValue.Render();

			QueryObjectData result = QueryObjectData.CreateFrom(data);
			result.Methods = result.Methods.AppendItems(method);
			result.Usings = result.Usings.AppendItems(
				"System",
				"System.Data");

			return result;
		}

		/// <inheritdoc />
		public IEnumerable<string> GetDependencies(ICodeGenerationContext context)
		{
			yield break;
		}

		/// <inheritdoc />
		public string Name { get; } = KnownAbilityName.GetScalarFromValue;
	}
}