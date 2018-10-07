﻿using SqlFirst.Codegen.Text.QueryObject.Data;
using SqlFirst.Codegen.Text.Snippets;
using SqlFirst.Codegen.Text.Templating;

namespace SqlFirst.Codegen.Text.QueryObject.Abilities.StoredProcedure.SmartTableParameters
{
	internal class StoredProcedureWithResultSmartTableAsyncAbility : StoredProcedureWithResultSmartTableAbility
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

		protected override IRenderableTemplate GetTemplate() => Snippet.Query.Methods.StoredProcedure.StoredProcedureWithResultAsync;
	}
}