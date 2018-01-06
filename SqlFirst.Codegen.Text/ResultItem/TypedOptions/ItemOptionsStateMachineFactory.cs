using System;
using Stateless;

namespace SqlFirst.Codegen.Text.ResultItem.TypedOptions
{
	/// <summary>
	/// Фабрика по созданию конечных автоматов для настройки <see cref="ResultItemOptions"/>
	/// </summary>
	internal static class ItemOptionsStateMachineFactory
	{
		/// <summary>
		/// Разрешает выполнить повторный вход в текущее состояние с выполнением определенного действия
		/// </summary>
		/// <typeparam name="TState">Тип состояния конечного автомата</typeparam>
		/// <typeparam name="TTrigger">Тип триггера</typeparam>
		/// <param name="config">Текущая конфигурация конечного автомата</param>
		/// <param name="trigger">Разрешаемый триггер</param>
		/// <param name="action">Выполняемое действие</param>
		/// <returns></returns>
		private static StateMachine<TState, TTrigger>.StateConfiguration PermitReentry<TState, TTrigger>(this StateMachine<TState, TTrigger>.StateConfiguration config, TTrigger trigger, Action action)
		{
			return config.PermitReentry(trigger).OnEntryFrom(trigger, action);
		}

		/// <summary>
		/// Состояние автомата
		/// </summary>
		public enum State
		{
			/// <summary>
			/// Начальное состояние
			/// </summary>
			Generate,

			/// <summary>
			/// Общие настройки элемента
			/// </summary>
			Item,

			/// <summary>
			/// Настройки свойств элемента
			/// </summary>
			ItemProperties
		}

		/// <summary>
		/// Триггеры
		/// </summary>
		public static class Trigger
		{
			public const string Generate = "generate";

			public const string Result = "result";

			public const string Parameter = "parameter";

			public const string Class = "class";

			public const string Struct = "struct";

			public const string Immutable = "immutable";

			public const string Properties = "properties";

			public const string Virtual = "virtual";

			public const string NotifyPropertyChanged = "inpc";

			public const string ReadOnly = "readonly";

			public const string Auto = "auto";

			public const string BackingField = "backingfield";
		}

		/// <summary>
		/// Создает конечный автомат для настройки <see cref="ResultItemOptions"/>
		/// </summary>
		/// <param name="options">Конфигурируемые опции</param>
		/// <returns>Конечный автомат для настройки <see cref="ResultItemOptions"/></returns>
		public static StateMachine<State, string> Build(ResultItemOptions options)
		{
			var machine = new StateMachine<State, string>(State.Generate);

			machine.Configure(State.Generate)
					.Permit(Trigger.Result, State.Item);

			machine.Configure(State.Item)
					.PermitReentry(Trigger.Struct, () => options.ItemType = ItemType.Struct)
					.PermitReentry(Trigger.Class, () => options.ItemType = ItemType.Class)
					.PermitReentry(Trigger.Immutable, () => options.PropertyModifiers |= PropertyModifiers.ReadOnly)
					.PermitReentry(Trigger.NotifyPropertyChanged, () =>
					{
						options.ItemAbilities |= ResultItemAbilities.NotifyPropertyChanged;
						if (options.PropertyType == PropertyType.INVALID)
						{
							options.PropertyType = PropertyType.BackingField;
						}
					})
					.Permit(Trigger.Properties, State.ItemProperties);

			machine.Configure(State.ItemProperties)
					.PermitReentry(Trigger.Auto, () => options.PropertyType = PropertyType.Auto)
					.PermitReentry(Trigger.BackingField, () => options.PropertyType = PropertyType.BackingField)
					.PermitReentry(Trigger.NotifyPropertyChanged, () =>
					{
						options.ItemAbilities |= ResultItemAbilities.NotifyPropertyChanged;
						if (options.PropertyType == PropertyType.INVALID)
						{
							options.PropertyType = PropertyType.BackingField;
						}
					})
					.PermitReentry(Trigger.ReadOnly, () => options.PropertyModifiers |= PropertyModifiers.ReadOnly)
					.PermitReentry(Trigger.Virtual, () => options.PropertyModifiers |= PropertyModifiers.Virtual);

			machine.OnUnhandledTrigger((state, trigger) => throw new CodeGenerationException($"Can not parse result item generation options: unexpected trigger [{trigger}] at state [{state:G}]"));

			return machine;
		}

		/// <summary>
		/// Создает конечный автомат для настройки <see cref="ParameterItemOptions"/>
		/// </summary>
		/// <param name="options">Конфигурируемые опции</param>
		/// <returns>Конечный автомат для настройки <see cref="ParameterItemOptions"/></returns>
		public static StateMachine<State, string> Build(ParameterItemOptions options)
		{
			var machine = new StateMachine<State, string>(State.Generate);

			machine.Configure(State.Generate)
					.Permit(Trigger.Parameter, State.Item);

			machine.Configure(State.Item)
					.PermitReentry(Trigger.Struct, () => options.ItemType = ItemType.Struct)
					.PermitReentry(Trigger.Class, () => options.ItemType = ItemType.Class);

			machine.OnUnhandledTrigger((state, trigger) => throw new CodeGenerationException($"Can not parse parameter generation options: unexpected trigger [{trigger}] at state [{state:G}]"));

			return machine;
		}

	}
}