using System.Collections.Generic;
using System.Linq;
using SqlFirst.Codegen.Text.QueryObject.Data;
using SqlFirst.Codegen.Text.Snippets;
using SqlFirst.Codegen.Text.Templating;

namespace SqlFirst.Codegen.Text.QueryObject.Abilities.StoredProcedure
{
	internal class StoredProcedureWithResultAsyncAbility : StoredProcedureWithResultAbility
	{
		/// <inheritdoc />
		public override string Name { get; } = "StoredProcedureAsync";

		/// <inheritdoc />
		public override IQueryObjectData Apply(ICodeGenerationContext context, IQueryObjectData data)
		{
			IQueryObjectData baseResult = base.Apply(context, data);
			QueryObjectData result = QueryObjectData.CreateFrom(baseResult);
			result.Usings = result.Usings.AppendItems(
				"System.Data.Common",
				"System.Threading",
				"System.Threading.Tasks");

			return result;
		}

		/// <inheritdoc />
		public override IEnumerable<string> GetDependencies(ICodeGenerationContext context)
		{
			yield return KnownAbilityName.GetQueryText;

			if (context.IncomingParameters.Any(p => !p.IsComplexType))
			{
				yield return KnownAbilityName.AddParameter;
			}

			if (context.IncomingParameters.Any(p => p.IsComplexType))
			{
				yield return KnownAbilityName.AddCustomParameter;
			}

			yield return KnownAbilityName.GetItemFromRecord;
		}

		protected override IRenderableTemplate GetTemplate() => Snippet.Query.Methods.StoredProcedure.StoredProcedureWithResultAsync;
	}
}