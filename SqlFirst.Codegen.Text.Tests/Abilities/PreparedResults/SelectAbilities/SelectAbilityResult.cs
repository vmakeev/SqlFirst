namespace SqlFirst.Codegen.Text.Tests.Abilities.PreparedResults.SelectAbilities
{
	internal class SelectAbilityResult : ResultBase
	{
		/// <inheritdoc />
		public SelectAbilityResult()
			: base("SelectAbilities")
		{
		}

		public string SelectItemsIAsyncEnumerableAsyncAbility_Test => GetText();

		public string SelectItemsIEnumerableAsyncNestedEnumerableAbility_Test => GetText();

		public string SelectFirstItemAbility_Test => GetText();

		public string SelectFirstItemAsyncAbility_Test => GetText();

		public string SelectItemsLazyAbility_Test => GetText();

		public string SelectScalarAbility_Test => GetText();

		public string SelectScalarAsyncAbility_Test => GetText();

		public string SelectScalarsAbility_Test => GetText();

		public string SelectScalarsIEnumerableAsyncNestedEnumerableAbility_Test => GetText();

		public string SelectScalarsIAsyncEnumerableAsync_Test => GetText();
	}
}