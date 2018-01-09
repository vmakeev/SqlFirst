﻿using SqlFirst.Codegen.Text.QueryObject.Data;
using SqlFirst.Codegen.Text.Snippets;
using SqlFirst.Codegen.Text.Templating;

namespace SqlFirst.Codegen.Text.QueryObject.Abilities.Insert
{
	internal class InsertMultipleValuesWithResultAsyncAbility : InsertMultipleValuesWithResultAbility
	{
		protected override IRenderableTemplate GetTemplate() => Snippet.Query.Methods.Add.AddMultipleWithResultAsync;

		/// <inheritdoc />
		public override IQueryObjectData Apply(ICodeGenerationContext context, IQueryObjectData data)
		{
			QueryObjectData result = QueryObjectData.CreateFrom(base.Apply(context, data));
			result.Usings = result.Usings.AppendItems(
				"System.Data.Common",
				"System.Threading",
				"System.Threading.Tasks");

			return result;
		}

		/// <inheritdoc />
		public override string Name { get; } = "InsertMultipleValuesAsync";
	}
}