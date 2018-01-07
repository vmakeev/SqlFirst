using SqlFirst.Codegen.Text.QueryObject.Data;
using SqlFirst.Codegen.Text.Snippets;

namespace SqlFirst.Codegen.Text.QueryObject.Abilities.Insert
{
	internal class InsertMultipleValuesWithScalarResultAsyncAbility : InsertMultipleValuesWithScalarResultAbility
	{
		protected override string GetTemplate() => QuerySnippet.Methods.Add.AddMultipleWithScalarResultAsync;

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
		public override string Name { get; } = "InsertMultipleValuesAsync";
	}
}