using System.Diagnostics.CodeAnalysis;
using System.Linq;
using SqlFirst.Codegen.Text.QueryObject.Abilities;
using SqlFirst.Codegen.Text.QueryObject.Abilities.Common;
using SqlFirst.Codegen.Text.QueryObject.Abilities.Nested;
using SqlFirst.Codegen.Text.QueryObject.Abilities.Select;
using SqlFirst.Codegen.Text.QueryObject.Data;
using SqlFirst.Codegen.Text.QueryObject.Options;

namespace SqlFirst.Codegen.Text.QueryObject
{
	internal static class SelectTemplateFactory
	{
		[SuppressMessage("ReSharper", "UnusedParameter.Global")]
		public static QueryObjectTemplate Build(ICodeGenerationContext context, SelectQueryObjectOptions options)
		{
			bool useResourceFile = options.UseQueryTextResourceFile ?? false;

			var result = new QueryObjectTemplate();

			if (useResourceFile)
			{
				result.AddAbility<GetQueryTextFromResourceCacheableAbility>();
			}
			else
			{
				result.AddAbility<GetQueryTextFromStringAbility>();
			}

			result.AddAbility<AddSqlConnectionParameterAbility>();

			if (IsScalarResult(context))
			{
				AddScalarAbilities(result, options);
			}
			else
			{
				AddItemAbilities(result, options);
			}

			return result;
		}

		private static void AddItemAbilities(QueryObjectTemplate result, SelectQueryObjectOptions options)
		{
			bool generateSync = options.GenerateSyncMethods ?? true;
			bool generateAsync = options.GenerateAsyncMethods ?? true;
			bool generateGetFirst = options.GenerateSelectFirstMethods ?? true;
			bool generateGetAll = options.GenerateSelectAllMethods ?? true;

			if (!result.IsExists(KnownAbilityName.AsyncEnumerable))
			{
				result.AddAbility<NestedAsyncEnumerableAbility>(() => generateAsync && generateGetAll);
			}

			result.AddAbility<MapDataRecordToItemAbility>(() => (generateSync || generateAsync) && (generateGetFirst || generateGetAll));

			result.AddAbility<SelectFirstItemAbility>(() => generateSync && generateGetFirst);
			result.AddAbility<SelectFirstItemAsyncAbility>(() => generateAsync && generateGetFirst);

			result.AddAbility<SelectItemsLazyAbility>(() => generateSync && generateGetAll);
			result.AddAbility<SelectItemsIEnumerableAsyncNestedEnumerableAbility>(() => generateAsync && generateGetAll);
		}

		private static void AddScalarAbilities(QueryObjectTemplate result, SelectQueryObjectOptions options)
		{
			bool generateSync = options.GenerateSyncMethods ?? true;
			bool generateAsync = options.GenerateAsyncMethods ?? true;
			bool generateGetFirst = options.GenerateSelectFirstMethods ?? true;
			bool generateGetAll = options.GenerateSelectAllMethods ?? true;

			if (!result.IsExists(KnownAbilityName.AsyncEnumerable))
			{
				result.AddAbility<NestedAsyncEnumerableAbility>(() => generateAsync && generateGetAll);
			}

			result.AddAbility<MapDataRecordToScalarAbility>(() => (generateSync || generateAsync) && generateGetAll);
			result.AddAbility<MapValueToScalarAbility>(() => (generateSync || generateAsync) && (generateGetFirst || generateGetAll));

			result.AddAbility<SelectScalarAbility>(() => generateSync && generateGetFirst);
			result.AddAbility<SelectScalarAsyncAbility>(() => generateAsync && generateGetFirst);

			result.AddAbility<SelectScalarsAbility>(() => generateSync && generateGetAll);
			result.AddAbility<SelectScalarsIEnumerableAsyncNestedEnumerableAbility>(() => generateAsync && generateGetAll);
		}

		private static bool IsScalarResult(ICodeGenerationContext context)
		{
			// todo: multiple enumeration
			return context.OutgoingParameters.Count() == 1;
		}
	}
}
