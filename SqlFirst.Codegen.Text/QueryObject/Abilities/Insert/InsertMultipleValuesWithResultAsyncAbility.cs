using System.Collections.Generic;
using SqlFirst.Codegen.Text.QueryObject.Data;
using SqlFirst.Codegen.Text.Snippets;

namespace SqlFirst.Codegen.Text.QueryObject.Abilities.Insert
{
	internal class InsertMultipleValuesWithResultAsyncAbility : InsertMultipleValuesWithResultAbility
	{
		protected override string GetTemplate() => QuerySnippet.Methods.Add.AddMultipleWithResultAsync;

		/// <inheritdoc />
		public override IQueryObjectData Apply(ICodeGenerationContext context, IQueryObjectData data)
		{
			QueryObjectData result = QueryObjectData.CreateFrom(base.Apply(context, data));
			result.Usings = result.Usings.Append(
				"System.Data.Common",
				"System.Threading",
				"System.Threading.Tasks");

			return result;
		}
		/// <inheritdoc />
		public override IEnumerable<string> GetDependencies()
		{
			return base.GetDependencies().Append(KnownAbilityName.AsyncEnumerable);
		}

		/// <inheritdoc />
		public override string Name { get; } = "InsertMultipleValuesAsync";
	}
}