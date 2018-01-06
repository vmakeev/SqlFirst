using System.Diagnostics.CodeAnalysis;
using System.Linq;
using SqlFirst.Codegen.Text.QueryObject.Abilities.Common;
using SqlFirst.Codegen.Text.QueryObject.Abilities.Insert;
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

			var result = new QueryObjectTemplate();

			result.AddAbility<GetQueryTextFromStringAbility>();
			result.AddAbility<GetMultipleInsertQueryTextFromStringAbility>(() => useMultipleInsert);
			result.AddAbility<AddSqlConnectionParameterAbility>();

			result.AddAbility<InsertSingleValuePlainAbility>();
			result.AddAbility<InsertSingleValuePlainAsyncAbility>();
			
			result.AddAbility<InsertMultipleValuesAbility>(() => useMultipleInsert);
			result.AddAbility<InsertMultipleValuesAsyncAbility>(() => useMultipleInsert);

			return result;
		}
	}
}