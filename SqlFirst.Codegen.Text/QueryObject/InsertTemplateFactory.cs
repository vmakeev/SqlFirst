using System.Diagnostics.CodeAnalysis;
using System.Linq;
using SqlFirst.Codegen.Text.QueryObject.Abilities.Common;
using SqlFirst.Codegen.Text.QueryObject.Abilities.Insert;
using SqlFirst.Codegen.Text.QueryObject.Data;
using SqlFirst.Codegen.Text.QueryObject.Options;

namespace SqlFirst.Codegen.Text.QueryObject
{
	internal static class InsertTemplateFactory
	{
		[SuppressMessage("ReSharper", "UnusedParameter.Global")]
		public static QueryObjectTemplate Build(ICodeGenerationContext context, InsertQueryObjectOptions options)
		{
			bool useMultipleInsert = context.IncomingParameters.Any(p => p.IsNumbered) && (options.GenerateAddMultipleMethods ?? true);
			bool useOutputValues = context.OutgoingParameters.Any();
			bool useResourceFile = options.UseQueryTextResourceFile ?? false;

			// todo: multiple enumeration 
			bool isScalarResult = context.OutgoingParameters.Count() == 1;

			var result = new QueryObjectTemplate();

			if (useResourceFile)
			{
				result.AddAbility<GetQueryTextFromResourceCacheableAbility>();
				result.AddAbility<GetMultipleInsertQueryTextRuntimeCachedAbility>(() => useMultipleInsert);
			}
			else
			{
				result.AddAbility<GetQueryTextFromStringAbility>();
				result.AddAbility<GetMultipleInsertQueryTextPrecompiledAbility>(() => useMultipleInsert);
			}

			result.AddAbility<AddSqlConnectionParameterAbility>();

			if (useOutputValues)
			{
				BuildWithOutput(result: result, options: options, isScalarResult: isScalarResult, useMultipleInsert: useMultipleInsert);
			}
			else
			{
				BuildWithRowsCountOutput(result: result, options: options, useMultipleInsert: useMultipleInsert);
			}

			return result;
		}

		private static void BuildWithRowsCountOutput(QueryObjectTemplate result, InsertQueryObjectOptions options, bool useMultipleInsert)
		{
			bool generateAsync = options.GenerateAsyncMethods ?? true;
			bool generateSync = options.GenerateSyncMethods ?? true;
			bool useSingleInsert = options.GenerateAddSingleMethods ?? true;

			result.AddAbility<InsertSingleValuePlainAbility>(() => useSingleInsert && generateSync);
			result.AddAbility<InsertSingleValuePlainAsyncAbility>(() => useSingleInsert && generateAsync);

			result.AddAbility<InsertMultipleValuesAbility>(() => useMultipleInsert && generateSync);
			result.AddAbility<InsertMultipleValuesAsyncAbility>(() => useMultipleInsert && generateAsync);
		}

		private static void BuildWithOutput(QueryObjectTemplate result, InsertQueryObjectOptions options, bool isScalarResult, bool useMultipleInsert)
		{
			if (isScalarResult)
			{
				BuildWithScalarOutput(result: result, options: options, useMultipleInsert: useMultipleInsert);
			}
			else
			{
				BuildWithObjectOutput(result: result, options: options, useMultipleInsert: useMultipleInsert);
			}
		}

		private static void BuildWithObjectOutput(QueryObjectTemplate result, InsertQueryObjectOptions options, bool useMultipleInsert)
		{
			bool generateAsync = options.GenerateAsyncMethods ?? true;
			bool generateSync = options.GenerateSyncMethods ?? true;
			bool useSingleInsert = options.GenerateAddSingleMethods ?? true;

			result.AddAbility<MapDataRecordToItemAbility>(() => generateSync || generateAsync);

			result.AddAbility<InsertSingleValuePlainWithResultAbility>(() => useSingleInsert && generateSync);
			result.AddAbility<InsertSingleValuePlainWithResultAsyncAbility>(() => useSingleInsert && generateAsync);

			result.AddAbility<InsertMultipleValuesWithResultAbility>(() => useMultipleInsert && generateSync);
			result.AddAbility<InsertMultipleValuesWithResultAsyncAbility>(() => useMultipleInsert && generateAsync);
		}

		private static void BuildWithScalarOutput(QueryObjectTemplate result, InsertQueryObjectOptions options, bool useMultipleInsert)
		{
			bool generateAsync = options.GenerateAsyncMethods ?? true;
			bool generateSync = options.GenerateSyncMethods ?? true;
			bool useSingleInsert = options.GenerateAddSingleMethods ?? true;

			result.AddAbility<MapDataRecordToScalarAbility>(() => useMultipleInsert);
			result.AddAbility<MapValueToScalarAbility>(() => generateSync || generateAsync);

			result.AddAbility<InsertSingleValuePlainWithScalarResultAbility>(() => useSingleInsert && generateSync);
			result.AddAbility<InsertSingleValuePlainWithScalarResultAsyncAbility>(() => useSingleInsert && generateAsync);

			result.AddAbility<InsertMultipleValuesWithScalarResultAbility>(() => useMultipleInsert && generateSync);
			result.AddAbility<InsertMultipleValuesWithScalarResultAsyncAbility>(() => useMultipleInsert && generateAsync);
		}
	}
}