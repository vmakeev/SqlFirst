﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Common.Logging;
using SqlFirst.Codegen.Text.QueryObject.Abilities;
using SqlFirst.Codegen.Text.QueryObject.Abilities.Common;
using SqlFirst.Codegen.Text.QueryObject.Abilities.Nested;
using SqlFirst.Codegen.Text.QueryObject.Abilities.Select;
using SqlFirst.Codegen.Text.QueryObject.Data;
using SqlFirst.Codegen.Text.QueryObject.Factories.Options;

namespace SqlFirst.Codegen.Text.QueryObject.Factories
{
	internal static class SelectTemplateFactory
	{
		private static readonly ILog _log = LogManager.GetLogger(typeof(SelectTemplateFactory));

		[SuppressMessage("ReSharper", "UnusedParameter.Global")]
		public static QueryObjectTemplate Build(ICodeGenerationContext context, SelectQueryObjectOptions options)
		{
			var internalOptions = new InternalOptions(context, options);

			_log.Trace(p => p($"QueryObject for SELECT:\n{internalOptions}"));

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

			if (internalOptions.Calculated.IsScalarOutput)
			{
				AddScalarAbilities(result, internalOptions);
			}
			else
			{
				AddItemAbilities(result, internalOptions);
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

		private static void AddItemAbilities(QueryObjectTemplate template, InternalOptions options)
		{
			if (!template.IsExists(KnownAbilityName.AsyncEnumerable))
			{
				template.AddAbility<NestedAsyncEnumerableAbility>(() => options.User.GenerateAsyncMethods && options.User.UseGetAll);
			}

			template.AddAbility<MapDataRecordToItemAbility>(() =>
				(options.User.GenerateSyncMethods || options.User.GenerateAsyncMethods) && (options.User.UseGetFirst || options.User.UseGetAll));

			template.AddAbility<SelectFirstItemAbility>(() => options.User.GenerateSyncMethods && options.User.UseGetFirst);
			template.AddAbility<SelectFirstItemAsyncAbility>(() => options.User.GenerateAsyncMethods && options.User.UseGetFirst);

			template.AddAbility<SelectItemsLazyAbility>(() => options.User.GenerateSyncMethods && options.User.UseGetAll);
			template.AddAbility<SelectItemsIEnumerableAsyncNestedEnumerableAbility>(() => options.User.GenerateAsyncMethods && options.User.UseGetAll);
		}

		private static void AddScalarAbilities(QueryObjectTemplate template, InternalOptions options)
		{
			if (!template.IsExists(KnownAbilityName.AsyncEnumerable))
			{
				template.AddAbility<NestedAsyncEnumerableAbility>(() => options.User.GenerateAsyncMethods && options.User.UseGetAll);
			}

			template.AddAbility<MapDataRecordToScalarAbility>(() => (options.User.GenerateSyncMethods || options.User.GenerateAsyncMethods) && options.User.UseGetAll);
			template.AddAbility<MapValueToScalarAbility>(() =>
				(options.User.GenerateSyncMethods || options.User.GenerateAsyncMethods) && (options.User.UseGetFirst || options.User.UseGetAll));

			template.AddAbility<SelectScalarAbility>(() => options.User.GenerateSyncMethods && options.User.UseGetFirst);
			template.AddAbility<SelectScalarAsyncAbility>(() => options.User.GenerateAsyncMethods && options.User.UseGetFirst);

			template.AddAbility<SelectScalarsAbility>(() => options.User.GenerateSyncMethods && options.User.UseGetAll);
			template.AddAbility<SelectScalarsIEnumerableAsyncNestedEnumerableAbility>(() => options.User.GenerateAsyncMethods && options.User.UseGetAll);
		}

		#region Nested

		private class InternalOptions
		{
			public CalculatedOptions Calculated { get; }

			public UserOptions User { get; }

			/// <inheritdoc />
			public InternalOptions(ICodeGenerationContext context, SelectQueryObjectOptions options)
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
					IsScalarOutput = outgoungParametersCount == 1
				};

				User = new UserOptions
				{
					GenerateAsyncMethods = options.GenerateAsyncMethods ?? true,
					GenerateSyncMethods = options.GenerateSyncMethods ?? true,
					UseResourceFile = options.UseQueryTextResourceFile ?? false,
					UseGetAll = options.GenerateSelectAllMethods ?? true,
					UseGetFirst = options.GenerateSelectFirstMethods ?? true,
					GenerateCommandTimeoutPreprocessor = options.GenerateCommandTimeoutPreprocessor ?? true
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
			public bool IsScalarOutput { get; set; }

			/// <inheritdoc />
			public override string ToString()
			{
				string[] parameters =
				{
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

			public bool UseGetFirst { get; set; }

			public bool UseGetAll { get; set; }

			public bool GenerateCommandTimeoutPreprocessor { get; set; }

			/// <inheritdoc />
			public override string ToString()
			{
				string[] parameters =
				{
					$"UseResourceFile: {UseResourceFile}",
					$"GenerateAsyncMethods: {GenerateAsyncMethods}",
					$"GenerateSyncMethods: {GenerateSyncMethods}",
					$"UseGetAll: {UseGetAll}",
					$"UseGetFirst: {UseGetFirst}",
					$"GenerateCommandTimeoutPreprocessor: {GenerateCommandTimeoutPreprocessor}"
				};

				return "UserOptions:\r\n" + string.Join(Environment.NewLine, parameters).Indent("\t", 1);
			}
		}

		#endregion
	}
}