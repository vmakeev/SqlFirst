using System.Collections.Generic;
using System.Linq;
using SqlFirst.Core;
using Stateless;

namespace SqlFirst.Codegen.Text.ResultItem.TypedOptions
{
	/// <summary>
	/// Параметры создания результата запроса после парсинга
	/// </summary>
	internal class ResultItemOptions
	{
		/// <summary>
		/// Тип объекта
		/// </summary>
		public ItemType ItemType { get; internal set; }

		/// <summary>
		/// Особые умения
		/// </summary>
		public ResultItemAbilities ItemAbilities { get; internal set; }

		/// <summary>
		/// Тип свойств
		/// </summary>
		public PropertyType PropertyType { get; internal set; }

		/// <summary>
		/// Модификаторы свойств
		/// </summary>
		public PropertyModifiers PropertyModifiers { get; internal set; }

		/// <summary>
		/// Инициализирует новый экземпляр класса <see cref="ResultItemOptions" />
		/// </summary>
		/// <param name="options">Набор опций SqlFirst</param>
		public ResultItemOptions(IEnumerable<ISqlFirstOption> options)
		{
			IEnumerable<ISqlFirstOption> target = options.Where(option =>
				option.Parameters?.Length > 1 &&
				option.Name?.ToLowerInvariant() == ItemOptionsStateMachineFactory.Trigger.Generate &&
				option.Parameters.FirstOrDefault()?.ToLowerInvariant() == ItemOptionsStateMachineFactory.Trigger.Result);
			
			foreach (ISqlFirstOption option in target)
			{
				StateMachine<ItemOptionsStateMachineFactory.State, string> machine = ItemOptionsStateMachineFactory.Build(this);

				IEnumerable<string> parameters = option.Parameters.Select(parameter => parameter?.ToLowerInvariant());
				foreach (string parameter in parameters)
				{
					machine.Fire(parameter);
				}
			}

			ApplyDefaults();
		}

		private void ApplyDefaults()
		{
			if (ItemType == ItemType.INVALID)
			{
				ItemType = ItemType.Class;
			}

			if (PropertyType == PropertyType.INVALID)
			{
				PropertyType = PropertyType.Auto;
			}
		}
	}

	/// <summary>
	/// Параметры создания аргумента запроса после парсинга
	/// </summary>
	internal class ParameterItemOptions
	{
		/// <summary>
		/// Тип объекта
		/// </summary>
		public ItemType ItemType { get; internal set; }

		/// <summary>
		/// Инициализирует новый экземпляр класса <see cref="ParameterItemOptions" />
		/// </summary>
		/// <param name="options">Набор опций SqlFirst</param>
		public ParameterItemOptions(IEnumerable<ISqlFirstOption> options)
		{
			IEnumerable<ISqlFirstOption> target = options.Where(option =>
				option.Parameters?.Length > 1 &&
				option.Name?.ToLowerInvariant() == ItemOptionsStateMachineFactory.Trigger.Generate &&
				option.Parameters.FirstOrDefault()?.ToLowerInvariant() == ItemOptionsStateMachineFactory.Trigger.Parameter);
			
			foreach (ISqlFirstOption option in target)
			{
				StateMachine<ItemOptionsStateMachineFactory.State, string> machine = ItemOptionsStateMachineFactory.Build(this);

				IEnumerable<string> parameters = option.Parameters.Select(parameter => parameter?.ToLowerInvariant());
				foreach (string parameter in parameters)
				{
					machine.Fire(parameter);
				}
			}

			ApplyDefaults();
		}

		private void ApplyDefaults()
		{
			if (ItemType == ItemType.INVALID)
			{
				ItemType = ItemType.Class;
			}
		}
	}
}