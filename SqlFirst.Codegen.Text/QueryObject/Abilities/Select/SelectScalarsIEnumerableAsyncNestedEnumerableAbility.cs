using System.Collections.Generic;

namespace SqlFirst.Codegen.Text.QueryObject.Abilities.Select
{
	internal class SelectScalarsIEnumerableAsyncNestedEnumerableAbility : SelectScalarsIEnumerableAsyncAbilityBase
	{
		/// <inheritdoc />
		public override IEnumerable<string> GetDependencies()
		{
			return base.GetDependencies().AppendItems(KnownAbilityName.AsyncEnumerable);
		}
	}
}