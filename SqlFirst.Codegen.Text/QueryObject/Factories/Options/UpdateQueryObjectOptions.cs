using System;
using System.Collections.Generic;
using System.Linq;
using SqlFirst.Core;
using Stateless;

namespace SqlFirst.Codegen.Text.QueryObject.Factories.Options
{
	internal class UpdateQueryObjectOptions
	{
		/// <summary>
		/// Генерировать ли асинхронные методы
		/// </summary>
		public bool? GenerateAsyncMethods { get; set; }

		/// <summary>
		/// Генерировать ли синхронные методы
		/// </summary>
		public bool? GenerateSyncMethods { get; set; }

		/// <summary>
		/// Использовать ли ресурсный файл для получения текста запроса
		/// </summary>
		public bool? UseQueryTextResourceFile { get; set; }

		/// <summary>
		/// Не выполнять проверку контрольной суммы запроса
		/// </summary>
		public bool? IgnoreChecksum { get; set; }

		/// <summary>
		/// Генерировать ли препроцессор команды, поддерживающий указание таймаута
		/// </summary>
		public bool? GenerateCommandTimeoutPreprocessor { get; set; }

		public UpdateQueryObjectOptions(IEnumerable<ISqlFirstOption> options, IReadOnlyDictionary<string, bool> optionDefaults)
		{
			if (options == null)
			{
				throw new ArgumentNullException(nameof(options));
			}

			ISqlFirstOption[] enumeratedOptions = options.ToArray();

			IEnumerable<ISqlFirstOption> generateMethods = enumeratedOptions.Where(option =>
				option.Parameters?.Length > 1 &&
				option.Name?.ToLowerInvariant() == QueryOptionsStateMachineFactory.Trigger.Generate &&
				option.Parameters.FirstOrDefault()?.ToLowerInvariant() == QueryOptionsStateMachineFactory.Trigger.Methods);

			IEnumerable<ISqlFirstOption> useQueryText = enumeratedOptions.Where(option =>
				option.Parameters?.Length > 1 &&
				option.Name?.ToLowerInvariant() == QueryOptionsStateMachineFactory.Trigger.Use &&
				option.Parameters.FirstOrDefault()?.ToLowerInvariant() == QueryOptionsStateMachineFactory.Trigger.QueryText);

			foreach (ISqlFirstOption option in generateMethods.Concat(useQueryText))
			{
				StateMachine<QueryOptionsStateMachineFactory.State, string> machine = QueryOptionsStateMachineFactory.Build(this);

				machine.Fire(option.Name.ToLowerInvariant());

				IEnumerable<string> parameters = option.Parameters.Select(parameter => parameter?.ToLowerInvariant());
				foreach (string parameter in parameters)
				{
					machine.Fire(parameter);
				}
			}

			GenerateAsyncMethods = ApplyDefaults(GenerateAsyncMethods, nameof(GenerateAsyncMethods), optionDefaults);
			GenerateSyncMethods = ApplyDefaults(GenerateSyncMethods, nameof(GenerateSyncMethods), optionDefaults);
			UseQueryTextResourceFile = ApplyDefaults(UseQueryTextResourceFile, nameof(UseQueryTextResourceFile), optionDefaults);
			IgnoreChecksum = ApplyDefaults(IgnoreChecksum, nameof(IgnoreChecksum), optionDefaults);
			GenerateCommandTimeoutPreprocessor = ApplyDefaults(GenerateCommandTimeoutPreprocessor, nameof(GenerateCommandTimeoutPreprocessor), optionDefaults);
		}

		private static bool? ApplyDefaults(bool? actualValue, string optionKey, IReadOnlyDictionary<string, bool> optionDefaults)
		{
			if (actualValue == null && optionDefaults.TryGetValue(optionKey, out bool value))
			{
				return value;
			}

			return actualValue;
		}
	}
}