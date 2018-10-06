using System.Collections.Generic;

namespace SqlFirst.Codegen.Text.QueryObject.Abilities.Select
{
	internal class SelectItemsIEnumerableAsyncNestedEnumerableAbility : SelectItemsIEnumerableAsyncAbilityBase
	{
		/// <inheritdoc />
		public override IEnumerable<string> GetDependencies(ICodeGenerationContext context)
		{
			return base.GetDependencies(context).AppendItems(KnownAbilityName.AsyncEnumerable);
		}
	}
}