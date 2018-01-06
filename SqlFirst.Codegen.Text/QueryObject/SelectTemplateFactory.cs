using System.Diagnostics.CodeAnalysis;
using System.Linq;
using SqlFirst.Codegen.Text.QueryObject.Abilities;
using SqlFirst.Codegen.Text.QueryObject.Abilities.Common;
using SqlFirst.Codegen.Text.QueryObject.Abilities.Nested;
using SqlFirst.Codegen.Text.QueryObject.Abilities.Select;
using SqlFirst.Codegen.Text.QueryObject.Data;

namespace SqlFirst.Codegen.Text.QueryObject
{
	//todo: use options
	internal static class SelectTemplateFactory
	{
		[SuppressMessage("ReSharper", "UnusedParameter.Global")]
		public static QueryObjectTemplate Build(ICodeGenerationContext context, IQueryGenerationOptions options)
		{
			var result = new QueryObjectTemplate();

			//result.AddAbility<GetQueryTextFromResourceCacheableAbility>();
			result.AddAbility<GetQueryTextFromStringAbility>();
			result.AddAbility<AddSqlConnectionParameterAbility>();

			if (IsScalarResult(context))
			{
				AddScalarAbilities(result);
			}
			else
			{
				AddItemAbilities(result);
			}

			return result;
		}

		private static void AddItemAbilities(QueryObjectTemplate result)
		{
			if (!result.IsExists(KnownAbilityName.AsyncEnumerable))
			{
				result.AddAbility<NestedAsyncEnumerableAbility>();
			}

			result.AddAbility<MapDataRecordToItemAbility>();

			result.AddAbility<SelectFirstItemAbility>();
			result.AddAbility<SelectFirstItemAsyncAbility>();

			result.AddAbility<SelectItemsLazyAbility>();
			result.AddAbility<SelectItemsIEnumerableAsyncNestedEnumerableAbility>();
		}

		private static void AddScalarAbilities(QueryObjectTemplate result)
		{
			if (!result.IsExists(KnownAbilityName.AsyncEnumerable))
			{
				result.AddAbility<NestedAsyncEnumerableAbility>();
			}

			result.AddAbility<SelectScalarAbility>();
			result.AddAbility<SelectScalarAsyncAbility>();

			result.AddAbility<SelectScalarsAbility>();
			result.AddAbility<SelectScalarsIEnumerableAsyncNestedEnumerableAbility>();
		}

		private static bool IsScalarResult(ICodeGenerationContext context)
		{
			// todo: multiple enumeration
			return context.OutgoingParameters.Count() == 1;
		}
	}
}
