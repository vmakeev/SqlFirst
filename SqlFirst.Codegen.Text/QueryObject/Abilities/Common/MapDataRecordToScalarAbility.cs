using System.Collections.Generic;
using SqlFirst.Codegen.Text.QueryObject.Data;
using SqlFirst.Codegen.Text.Snippets;

namespace SqlFirst.Codegen.Text.QueryObject.Abilities.Common
{
	internal class MapDataRecordToScalarAbility : IQueryObjectAbility
	{
		/// <inheritdoc />
		public IQueryObjectData Apply(ICodeGenerationContext context, IQueryObjectData data)
		{
			string method = QuerySnippet.Methods.Common.GetScalarFromRecord;

			QueryObjectData result = QueryObjectData.CreateFrom(data);
			result.Methods = result.Methods.Append(method);
			result.Usings = result.Usings.Append(
				"System",
				"System.Data");

			return result;
		}

		/// <inheritdoc />
		public IEnumerable<string> GetDependencies()
		{
			yield return KnownAbilityName.GetScalarFromValue;
		}

		/// <inheritdoc />
		public string Name { get; } = KnownAbilityName.GetScalarFromRecord;
	}
}