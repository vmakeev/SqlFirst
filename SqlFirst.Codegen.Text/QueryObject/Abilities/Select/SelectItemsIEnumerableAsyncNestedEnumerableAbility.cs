using System.Collections.Generic;

namespace SqlFirst.Codegen.Text.QueryObject.Abilities.Select
{
	internal class SelectItemsIEnumerableAsyncNestedEnumerableAbility : SelectItemsIEnumerableAsyncAbilityBase
	{
		/// <inheritdoc />
		public override IEnumerable<string> GetDependencies()
		{
			return base.GetDependencies().AppendItems(KnownAbilityName.AsyncEnumerable);
		}
	}
}