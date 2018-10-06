﻿using System.Collections.Generic;
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
		public override IEnumerable<string> GetDependencies()
		{
			yield return KnownAbilityName.GetQueryText;
			yield return KnownAbilityName.AddParameter;
			yield return KnownAbilityName.GetItemFromRecord;
		}

		protected override IRenderableTemplate GetTemplate() => Snippet.Query.Methods.StoredProcedure.StoredProcedureWithResultAsync;
	}
}