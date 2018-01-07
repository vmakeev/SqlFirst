using System.Diagnostics.CodeAnalysis;
using System.Linq;
using SqlFirst.Codegen.Text.QueryObject.Abilities.Common;
using SqlFirst.Codegen.Text.QueryObject.Abilities.Insert;
using SqlFirst.Codegen.Text.QueryObject.Abilities.Nested;
using SqlFirst.Codegen.Text.QueryObject.Data;

namespace SqlFirst.Codegen.Text.QueryObject
{
	internal static class InsertTemplateFactory
	{
		//todo: use options
		[SuppressMessage("ReSharper", "UnusedParameter.Global")]
		public static QueryObjectTemplate Build(ICodeGenerationContext context, IQueryGenerationOptions options)
		{
			// todo: move to options
			bool useMultipleInsert = context.IncomingParameters.Any(p => p.IsNumbered);
			bool useOutputValues = context.OutgoingParameters.Any();

			// todo: multiple enumeration 
			bool isScalarResult = context.OutgoingParameters.Count() == 1;

			var result = new QueryObjectTemplate();

			result.AddAbility<GetQueryTextFromStringAbility>();
			//result.AddAbility<GetMultipleInsertQueryTextPrecompiledAbility>(() => useMultipleInsert);
			result.AddAbility<GetMultipleInsertQueryTextRuntimeCachedAbility>(() => useMultipleInsert);
			result.AddAbility<AddSqlConnectionParameterAbility>();

			if (useOutputValues)
			{
				BuildWithOutput(result: result, isScalarResult: isScalarResult, useMultipleInsert: useMultipleInsert);
			}
			else
			{
				BuildWithRowsCountOutput(result: result, useMultipleInsert: useMultipleInsert);
			}

			return result;
		}

		private static void BuildWithRowsCountOutput(QueryObjectTemplate result, bool useMultipleInsert)
		{
			result.AddAbility<InsertSingleValuePlainAbility>();
			result.AddAbility<InsertSingleValuePlainAsyncAbility>();

			result.AddAbility<InsertMultipleValuesAbility>(() => useMultipleInsert);
			result.AddAbility<InsertMultipleValuesAsyncAbility>(() => useMultipleInsert);
		}

		private static void BuildWithOutput(QueryObjectTemplate result, bool isScalarResult, bool useMultipleInsert)
		{
			result.AddAbility<NestedAsyncEnumerableAbility>();

			if (isScalarResult)
			{
				BuildWithScalarOutput(result: result, useMultipleInsert: useMultipleInsert);
			}
			else
			{
				BuildWithObjectOutput(result: result, useMultipleInsert: useMultipleInsert);
			}
		}

		private static void BuildWithObjectOutput(QueryObjectTemplate result, bool useMultipleInsert)
		{
			result.AddAbility<MapDataRecordToItemAbility>();

			result.AddAbility<InsertSingleValuePlainWithResultAbility>();
			result.AddAbility<InsertSingleValuePlainWithResultAsyncAbility>();

			result.AddAbility<InsertMultipleValuesWithResultAbility>(() => useMultipleInsert);
			result.AddAbility<InsertMultipleValuesWithResultAsyncAbility>(() => useMultipleInsert);
		}

		private static void BuildWithScalarOutput(QueryObjectTemplate result, bool useMultipleInsert)
		{
			result.AddAbility<MapDataRecordToScalarAbility>(() => useMultipleInsert);
			result.AddAbility<MapValueToScalarAbility>();

			result.AddAbility<InsertSingleValuePlainWithScalarResultAbility>();
			result.AddAbility<InsertSingleValuePlainWithScalarResultAsyncAbility>();

			result.AddAbility<InsertMultipleValuesWithScalarResultAbility>(() => useMultipleInsert);
			result.AddAbility<InsertMultipleValuesWithScalarResultAsyncAbility>(() => useMultipleInsert);
		}
	}
}