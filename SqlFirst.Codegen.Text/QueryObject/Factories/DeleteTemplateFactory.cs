﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.Extensions.Logging;
using SqlFirst.Codegen.Text.QueryObject.Abilities.Common;
using SqlFirst.Codegen.Text.QueryObject.Abilities.Delete;
using SqlFirst.Codegen.Text.QueryObject.Data;
using SqlFirst.Codegen.Text.QueryObject.Factories.Options;
using SqlFirst.Core;

namespace SqlFirst.Codegen.Text.QueryObject.Factories
{
	internal static class DeleteTemplateFactory
	{
		private static readonly ILogger _log = LogManager.GetLogger(typeof(DeleteTemplateFactory));

		[SuppressMessage("ReSharper", "UnusedParameter.Global")]
		public static QueryObjectTemplate Build(ICodeGenerationContext context, DeleteQueryObjectOptions options)
		{
			var internalOptions = new InternalOptions(context, options);

			_log.LogTrace($"QueryObject for DELETE:\n{internalOptions}");

			var result = new QueryObjectTemplate();

			if (internalOptions.User.UseResourceFile)
			{
				result.AddAbility<ProcessCachedSqlPartialAbility>();
				
				result.AddAbility<GetQueryTextFromResourceCacheableWithCheckAbility>(() => !internalOptions.User.IgnoreChecksum);
				result.AddAbility<GetQueryTextFromResourceCacheableNoCheckAbility>(() => internalOptions.User.IgnoreChecksum);
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
			template.AddAbility<DeleteAbility>(() => options.User.GenerateSyncMethods);
			template.AddAbility<DeleteAsyncAbility>(() => options.User.GenerateAsyncMethods);
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

			template.AddAbility<DeleteWithResultAbility>(() => options.User.GenerateSyncMethods);
			template.AddAbility<DeleteWithResultAsyncAbility>(() => options.User.GenerateAsyncMethods);
		}

		private static void BuildWithScalarOutput(QueryObjectTemplate template, InternalOptions options)
		{
			template.AddAbility<MapDataRecordToScalarAbility>();
			template.AddAbility<MapValueToScalarAbility>(() => options.User.GenerateSyncMethods || options.User.GenerateAsyncMethods);

			template.AddAbility<DeleteWithScalarResultAbility>(() => options.User.GenerateSyncMethods);
			template.AddAbility<DeleteWithScalarResultAsyncAbility>(() => options.User.GenerateAsyncMethods);

		}

		#region Nested

		private class InternalOptions
		{
			public CalculatedOptions Calculated { get; }

			public UserOptions User { get; }

			/// <inheritdoc />
			public InternalOptions(ICodeGenerationContext context, DeleteQueryObjectOptions options)
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

			public bool GenerateCommandTimeoutPreprocessor { get; set; }
			
			public bool IgnoreChecksum { get; set; }

			/// <inheritdoc />
			public override string ToString()
			{
				string[] parameters =
				{
					$"UseResourceFile: {UseResourceFile}",
					$"GenerateAsyncMethods: {GenerateAsyncMethods}",
					$"GenerateSyncMethods: {GenerateSyncMethods}",
					$"IgnoreChecksum: {IgnoreChecksum}",
					$"GenerateCommandTimeoutPreprocessor: {GenerateCommandTimeoutPreprocessor}"
				};

				return "UserOptions:\r\n" + string.Join(Environment.NewLine, parameters).Indent("\t", 1);
			}
		}

		#endregion
	}
}