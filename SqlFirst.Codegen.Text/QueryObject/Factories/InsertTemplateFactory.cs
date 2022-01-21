using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Common.Logging;
using SqlFirst.Codegen.Text.QueryObject.Abilities.Common;
using SqlFirst.Codegen.Text.QueryObject.Abilities.Insert;
using SqlFirst.Codegen.Text.QueryObject.Data;
using SqlFirst.Codegen.Text.QueryObject.Factories.Options;

namespace SqlFirst.Codegen.Text.QueryObject.Factories
{
	internal static class InsertTemplateFactory
	{
		private static readonly ILog _log = LogManager.GetLogger(typeof(InsertTemplateFactory));

		[SuppressMessage("ReSharper", "UnusedParameter.Global")]
		public static QueryObjectTemplate Build(ICodeGenerationContext context, InsertQueryObjectOptions options)
		{
			var internalOptions = new InternalOptions(context, options);

			_log.Trace(p => p($"QueryObject for INSERT:\n{internalOptions}"));

			var result = new QueryObjectTemplate();

			if (internalOptions.User.UseResourceFile)
			{
				result.AddAbility<ProcessCachedSqlPartialAbility>();
				
				result.AddAbility<GetQueryTextFromResourceCacheableWithCheckAbility>(() => !internalOptions.User.IgnoreChecksum);
				result.AddAbility<GetQueryTextFromResourceCacheableNoCheckAbility>(() => internalOptions.User.IgnoreChecksum);

				result.AddAbility<GetMultipleInsertQueryTextRuntimeCachedAbility>(() => internalOptions.User.UseMultipleInsert && internalOptions.Calculated.HasMultipleInsert);
			}
			else
			{
				result.AddAbility<GetQueryTextFromStringAbility>();
				result.AddAbility<GetMultipleInsertQueryTextPrecompiledAbility>(() => internalOptions.User.UseMultipleInsert && internalOptions.Calculated.HasMultipleInsert);
			}

			result.AddAbility<AddSqlConnectionParameterAbility>();

			if (internalOptions.Calculated.HasOutput)
			{
				BuildWithOutput(result, internalOptions);
			}
			else
			{
				BuildWithRowsCountOutput(result, internalOptions);
			}

			if (internalOptions.User.GenerateCommandTimeoutPreprocessor)
			{
				result.AddAbility<PrepareCommandWithTimeoutAbility>();
			}
			else
			{
				result.AddAbility<PrepareCommandPartialAbility>();
			}

			return result;
		}

		private static void BuildWithRowsCountOutput(QueryObjectTemplate template, InternalOptions options)
		{
			template.AddAbility<InsertSingleValuePlainAbility>(() => options.User.UseSingleInsert && options.User.GenerateSyncMethods);
			template.AddAbility<InsertSingleValuePlainAsyncAbility>(() => options.User.UseSingleInsert && options.User.GenerateAsyncMethods);

			template.AddAbility<InsertMultipleValuesAbility>(() => options.Calculated.HasMultipleInsert && options.User.UseMultipleInsert && options.User.GenerateSyncMethods);
			template.AddAbility<InsertMultipleValuesAsyncAbility>(() => options.Calculated.HasMultipleInsert && options.User.UseMultipleInsert && options.User.GenerateAsyncMethods);
		}

		private static void BuildWithOutput(QueryObjectTemplate template, InternalOptions options)
		{
			if (options.Calculated.IsScalarOutput)
			{
				BuildWithScalarOutput(template, options);
			}
			else
			{
				BuildWithObjectOutput(template, options);
			}
		}

		private static void BuildWithObjectOutput(QueryObjectTemplate template, InternalOptions options)
		{
			template.AddAbility<MapDataRecordToItemAbility>(() => options.User.GenerateSyncMethods || options.User.GenerateAsyncMethods);

			template.AddAbility<InsertSingleValuePlainWithResultAbility>(() => options.User.UseSingleInsert && options.User.GenerateSyncMethods);
			template.AddAbility<InsertSingleValuePlainWithResultAsyncAbility>(() => options.User.UseSingleInsert && options.User.GenerateAsyncMethods);

			template.AddAbility<InsertMultipleValuesWithResultAbility>(() => options.Calculated.HasMultipleInsert && options.User.UseMultipleInsert && options.User.GenerateSyncMethods);
			template.AddAbility<InsertMultipleValuesWithResultAsyncAbility>(() => options.Calculated.HasMultipleInsert && options.User.UseMultipleInsert && options.User.GenerateAsyncMethods);
		}

		private static void BuildWithScalarOutput(QueryObjectTemplate template, InternalOptions options)
		{
			template.AddAbility<MapDataRecordToScalarAbility>(() => options.User.UseMultipleInsert && options.Calculated.HasMultipleInsert);
			template.AddAbility<MapValueToScalarAbility>(() => options.User.GenerateSyncMethods || options.User.GenerateAsyncMethods);

			template.AddAbility<InsertSingleValuePlainWithScalarResultAbility>(() => options.User.UseSingleInsert && options.User.GenerateSyncMethods);
			template.AddAbility<InsertSingleValuePlainWithScalarResultAsyncAbility>(() => options.User.UseSingleInsert && options.User.GenerateAsyncMethods);

			template.AddAbility<InsertMultipleValuesWithScalarResultAbility>(() => options.Calculated.HasMultipleInsert && options.User.UseMultipleInsert && options.User.GenerateSyncMethods);
			template.AddAbility<InsertMultipleValuesWithScalarResultAsyncAbility>(() => options.Calculated.HasMultipleInsert && options.User.UseMultipleInsert && options.User.GenerateAsyncMethods);
		}

		#region Nested

		private class InternalOptions
		{
			public CalculatedOptions Calculated { get; }

			public UserOptions User { get; }

			/// <inheritdoc />
			public InternalOptions(ICodeGenerationContext context, InsertQueryObjectOptions options)
			{
				if (context == null)
				{
					throw new ArgumentNullException(nameof(context));
				}

				if (options == null)
				{
					throw new ArgumentNullException(nameof(options));
				}

				int outgoungParametersCount = context.OutgoingParameters.Count();

				Calculated = new CalculatedOptions
				{
					HasMultipleInsert = context.IncomingParameters.Any(p => p.IsNumbered),
					HasOutput = outgoungParametersCount > 0,
					IsScalarOutput = outgoungParametersCount == 1
				};

				User = new UserOptions
				{
					GenerateAsyncMethods = options.GenerateAsyncMethods ?? true,
					GenerateSyncMethods = options.GenerateSyncMethods ?? true,
					UseMultipleInsert = options.GenerateAddMultipleMethods ?? true,
					UseResourceFile = options.UseQueryTextResourceFile ?? false,
					UseSingleInsert = options.GenerateAddSingleMethods ?? true,
					GenerateCommandTimeoutPreprocessor = options.GenerateCommandTimeoutPreprocessor ?? true,
					IgnoreChecksum = options.IgnoreChecksum ?? false
				};
			}

			/// <inheritdoc />
			public override string ToString()
			{
				return "InternalOptions:\r\n" + string.Join(Environment.NewLine, Calculated, User).Indent("\t", 1);
			}
		}

		private class CalculatedOptions
		{
			public bool HasMultipleInsert { get; set; }

			public bool HasOutput { get; set; }

			public bool IsScalarOutput { get; set; }

			/// <inheritdoc />
			public override string ToString()
			{
				string[] parameters =
				{
					$"HasMultipleInsert: {HasMultipleInsert}",
					$"HasOutput: {HasOutput}",
					$"IsScalarOutput: {IsScalarOutput}"
				};

				return "CalculatedOptions:\r\n" + string.Join(Environment.NewLine, parameters).Indent("\t", 1);
			}
		}

		private class UserOptions
		{
			public bool UseResourceFile { get; set; }

			public bool GenerateAsyncMethods { get; set; }

			public bool GenerateSyncMethods { get; set; }

			public bool UseSingleInsert { get; set; }

			public bool UseMultipleInsert { get; set; }

			public bool IgnoreChecksum { get; set; }

			public bool GenerateCommandTimeoutPreprocessor { get; set; }

			/// <inheritdoc />
			public override string ToString()
			{
				string[] parameters =
				{
					$"UseResourceFile: {UseResourceFile}",
					$"GenerateAsyncMethods: {GenerateAsyncMethods}",
					$"GenerateSyncMethods: {GenerateSyncMethods}",
					$"UseSingleInsert: {UseSingleInsert}",
					$"UseMultipleInsert: {UseMultipleInsert}",
					$"IgnoreChecksum: {IgnoreChecksum}",
					$"GenerateCommandTimeoutPreprocessor: {GenerateCommandTimeoutPreprocessor}"
				};

				return "UserOptions:\r\n" + string.Join(Environment.NewLine, parameters).Indent("\t", 1);
			}
		}

		#endregion
	}
}