using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Common.Logging;
using SqlFirst.Codegen.Text.QueryObject.Abilities.Common;
using SqlFirst.Codegen.Text.QueryObject.Abilities.Update;
using SqlFirst.Codegen.Text.QueryObject.Data;
using SqlFirst.Codegen.Text.QueryObject.Factories.Options;

namespace SqlFirst.Codegen.Text.QueryObject.Factories
{
	internal static class UpdateTemplateFactory
	{
		private static readonly ILog _log = LogManager.GetLogger(typeof(UpdateTemplateFactory));

		[SuppressMessage("ReSharper", "UnusedParameter.Global")]
		public static QueryObjectTemplate Build(ICodeGenerationContext context, UpdateQueryObjectOptions options)
		{
			var internalOptions = new InternalOptions(context, options);

			_log.Trace(p => p($"QueryObject for UPDATE:\n{internalOptions}"));

			var result = new QueryObjectTemplate();

			if (internalOptions.User.UseResourceFile)
			{
				result.AddAbility<GetQueryTextFromResourceCacheableAbility>();
			}
			else
			{
				result.AddAbility<GetQueryTextFromStringAbility>();
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

			return result;
		}

		private static void BuildWithRowsCountOutput(QueryObjectTemplate template, InternalOptions options)
		{
			template.AddAbility<UpdateAbility>(() => options.User.GenerateSyncMethods);
			template.AddAbility<UpdateAsyncAbility>(() => options.User.GenerateAsyncMethods);
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

			template.AddAbility<UpdateWithResultAbility>(() => options.User.GenerateSyncMethods);
			template.AddAbility<UpdateWithResultAsyncAbility>(() => options.User.GenerateAsyncMethods);
		}

		private static void BuildWithScalarOutput(QueryObjectTemplate template, InternalOptions options)
		{
			template.AddAbility<MapDataRecordToScalarAbility>();
			template.AddAbility<MapValueToScalarAbility>(() => options.User.GenerateSyncMethods || options.User.GenerateAsyncMethods);

			template.AddAbility<UpdateWithScalarResultAbility>(() => options.User.GenerateSyncMethods);
			template.AddAbility<UpdateWithScalarResultAsyncAbility>(() => options.User.GenerateAsyncMethods);

		}

		#region Nested

		private class InternalOptions
		{
			public CalculatedOptions Calculated { get; }

			public UserOptions User { get; }

			/// <inheritdoc />
			public InternalOptions(ICodeGenerationContext context, UpdateQueryObjectOptions options)
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
					UseResourceFile = options.UseQueryTextResourceFile ?? false,
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

			/// <inheritdoc />
			public override string ToString()
			{
				string[] parameters =
				{
					$"UseResourceFile: {UseResourceFile}",
					$"GenerateAsyncMethods: {GenerateAsyncMethods}",
					$"GenerateSyncMethods: {GenerateSyncMethods}"
				};

				return "UserOptions:\r\n" + string.Join(Environment.NewLine, parameters).Indent("\t", 1);
			}
		}

		#endregion
	}
}