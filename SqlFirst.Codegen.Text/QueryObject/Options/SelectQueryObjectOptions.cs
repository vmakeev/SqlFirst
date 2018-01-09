using System;
using System.Collections.Generic;
using System.Linq;
using SqlFirst.Core;
using Stateless;

namespace SqlFirst.Codegen.Text.QueryObject.Options
{
	internal class SelectQueryObjectOptions
	{
		/// <summary>
		/// Генерировать ли запрос первого значения
		/// </summary>
		public bool? GenerateSelectFirstMethods { get; set; }

		/// <summary>
		/// Генерировать ли запрос всех значений
		/// </summary>
		public bool? GenerateSelectAllMethods { get; set; }

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

		public SelectQueryObjectOptions(IEnumerable<ISqlFirstOption> options)
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
		}
	}
}