using System.Collections.Generic;
using System.Linq;
using SqlFirst.Core;
using Stateless;

namespace SqlFirst.Codegen.Text.ResultItem.TypedOptions
{
	/// <summary>
	/// Параметры создания результата запроса после парсинга
	/// </summary>
	internal class ItemOptions
	{
		/// <summary>
		/// Тип объекта
		/// </summary>
		public ResultItemType ItemType { get; internal set; }

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
		/// Инициализирует новый экземпляр класса <see cref="ItemOptions" />
		/// </summary>
		/// <param name="options">Набор опций SqlFirst</param>
		public ItemOptions(IEnumerable<ISqlFirstOption> options)
		{
			IEnumerable<ISqlFirstOption> target = options.Where(option =>
				option.Parameters?.Length > 1 &&
				option.Name?.ToLowerInvariant() == ItemOptionsStateMachineFactory.Trigger.Generate &&
				option.Parameters.FirstOrDefault()?.ToLowerInvariant() == ItemOptionsStateMachineFactory.Trigger.Item);
			
			foreach (ISqlFirstOption option in target)
			{
				StateMachine<ItemOptionsStateMachineFactory.State, string> machine = ItemOptionsStateMachineFactory.CreateStateMachine(this);

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
			if (ItemType == ResultItemType.INVALID)
			{
				ItemType = ResultItemType.Class;
			}

			if (PropertyType == PropertyType.INVALID)
			{
				PropertyType = PropertyType.Auto;
			}
		}
	}
}