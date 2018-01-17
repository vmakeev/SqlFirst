using System;
using Stateless;

namespace SqlFirst.Codegen.Text.QueryObject.Factories.Options
{
	/// <summary>
	/// Фабрика по созданию конечных автоматов для настройки опций запросов
	/// </summary>
	internal static class QueryOptionsStateMachineFactory
	{
		/// <summary>
		/// Состояние автомата
		/// </summary>
		public enum State
		{
			/// <summary>
			/// Начальное состояние
			/// </summary>
			Start,

			/// <summary>
			/// Начальное состояние параметров генерации
			/// </summary>
			Generate,

			/// <summary>
			/// Общие настройки генерации методов
			/// </summary>
			Methods,

			/// <summary>
			/// Начальное состояние параметров использования различных способностей
			/// </summary>
			Use,

			/// <summary>
			/// Настройки способа получения текста запроса
			/// </summary>
			QueryText,
		}

		/// <summary>
		/// Триггеры
		/// </summary>
		public static class Trigger
		{
			public const string Generate = "generate";

			public const string Methods = "methods";

			public const string Async = "async";

			public const string Sync = "sync";

			public const string AddSingle = "add_single";

			public const string AddMultiple = "add_multiple";

			public const string GetFirst = "get_first";

			public const string GetAll = "get_all";

			public const string Use = "use";

			public const string QueryText = "querytext";

			public const string Resource = "resource";

			public const string String = "string";
		}

		/// <summary>
		/// Создает конечный автомат для настройки <see cref="InsertQueryObjectOptions" />
		/// </summary>
		/// <param name="options">Конфигурируемые опции</param>
		/// <returns>Конечный автомат для настройки <see cref="InsertQueryObjectOptions" /></returns>
		public static StateMachine<State, string> Build(InsertQueryObjectOptions options)
		{
			var machine = new StateMachine<State, string>(State.Start);

			machine.Configure(State.Start)
					.Permit(Trigger.Generate, State.Generate)
					.Permit(Trigger.Use, State.Use);

			machine.Configure(State.Generate)
					.Permit(Trigger.Methods, State.Methods);

			machine.Configure(State.Methods)
					.PermitReentry(Trigger.Sync, () =>
					{
						options.GenerateSyncMethods = true;
						if (options.GenerateAsyncMethods == null)
						{
							options.GenerateAsyncMethods = false;
						}
					})
					.PermitReentry(Trigger.Async, () =>
					{
						options.GenerateAsyncMethods = true;
						if (options.GenerateSyncMethods == null)
						{
							options.GenerateSyncMethods = false;
						}
					})
					.PermitReentry(Trigger.AddSingle, () =>
					{
						options.GenerateAddSingleMethods = true;
						if (options.GenerateAddMultipleMethods == null)
						{
							options.GenerateAddMultipleMethods = false;
						}
					})
					.PermitReentry(Trigger.AddMultiple, () =>
					{
						options.GenerateAddMultipleMethods = true;
						if (options.GenerateAddSingleMethods == null)
						{
							options.GenerateAddSingleMethods = false;
						}
					});

			machine.Configure(State.Use)
					.Permit(Trigger.QueryText, State.QueryText);

			machine.Configure(State.QueryText)
					.PermitReentry(Trigger.Resource, () => options.UseQueryTextResourceFile = true)
					.PermitReentry(Trigger.String, () => options.UseQueryTextResourceFile = false);

			machine.OnUnhandledTrigger((state, trigger) =>
				throw new CodeGenerationException($"Can not parse insert query generation options: unexpected trigger [{trigger}] at state [{state:G}]"));

			return machine;
		}

		/// <summary>
		/// Создает конечный автомат для настройки <see cref="SelectQueryObjectOptions" />
		/// </summary>
		/// <param name="options">Конфигурируемые опции</param>
		/// <returns>Конечный автомат для настройки <see cref="SelectQueryObjectOptions" /></returns>
		public static StateMachine<State, string> Build(SelectQueryObjectOptions options)
		{
			var machine = new StateMachine<State, string>(State.Start);

			machine.Configure(State.Start)
					.Permit(Trigger.Generate, State.Generate)
					.Permit(Trigger.Use, State.Use);

			machine.Configure(State.Generate)
					.Permit(Trigger.Methods, State.Methods);

			machine.Configure(State.Methods)
					.PermitReentry(Trigger.Sync, () =>
					{
						options.GenerateSyncMethods = true;
						if (options.GenerateAsyncMethods == null)
						{
							options.GenerateAsyncMethods = false;
						}
					})
					.PermitReentry(Trigger.Async, () =>
					{
						options.GenerateAsyncMethods = true;
						if (options.GenerateSyncMethods == null)
						{
							options.GenerateSyncMethods = false;
						}
					})
					.PermitReentry(Trigger.GetFirst, () =>
					{
						options.GenerateSelectFirstMethods = true;
						if (options.GenerateSelectAllMethods == null)
						{
							options.GenerateSelectAllMethods = false;
						}
					})
					.PermitReentry(Trigger.GetAll, () =>
					{
						options.GenerateSelectAllMethods = true;
						if (options.GenerateSelectFirstMethods == null)
						{
							options.GenerateSelectFirstMethods = false;
						}
					});

			machine.Configure(State.Use)
					.Permit(Trigger.QueryText, State.QueryText);

			machine.Configure(State.QueryText)
					.PermitReentry(Trigger.Resource, () => options.UseQueryTextResourceFile = true)
					.PermitReentry(Trigger.String, () => options.UseQueryTextResourceFile = false);

			machine.OnUnhandledTrigger((state, trigger) =>
				throw new CodeGenerationException($"Can not parse select query generation options: unexpected trigger [{trigger}] at state [{state:G}]"));

			return machine;
		}

		/// <summary>
		/// Создает конечный автомат для настройки <see cref="UpdateQueryObjectOptions" />
		/// </summary>
		/// <param name="options">Конфигурируемые опции</param>
		/// <returns>Конечный автомат для настройки <see cref="UpdateQueryObjectOptions" /></returns>
		public static StateMachine<State, string> Build(UpdateQueryObjectOptions options)
		{
			var machine = new StateMachine<State, string>(State.Start);

			machine.Configure(State.Start)
					.Permit(Trigger.Generate, State.Generate)
					.Permit(Trigger.Use, State.Use);

			machine.Configure(State.Generate)
					.Permit(Trigger.Methods, State.Methods);

			machine.Configure(State.Methods)
					.PermitReentry(Trigger.Sync, () =>
					{
						options.GenerateSyncMethods = true;
						if (options.GenerateAsyncMethods == null)
						{
							options.GenerateAsyncMethods = false;
						}
					})
					.PermitReentry(Trigger.Async, () =>
					{
						options.GenerateAsyncMethods = true;
						if (options.GenerateSyncMethods == null)
						{
							options.GenerateSyncMethods = false;
						}
					});

			machine.Configure(State.Use)
					.Permit(Trigger.QueryText, State.QueryText);

			machine.Configure(State.QueryText)
					.PermitReentry(Trigger.Resource, () => options.UseQueryTextResourceFile = true)
					.PermitReentry(Trigger.String, () => options.UseQueryTextResourceFile = false);

			machine.OnUnhandledTrigger((state, trigger) =>
				throw new CodeGenerationException($"Can not parse update query generation options: unexpected trigger [{trigger}] at state [{state:G}]"));

			return machine;
		}

		/// <summary>
		/// Создает конечный автомат для настройки <see cref="DeleteQueryObjectOptions" />
		/// </summary>
		/// <param name="options">Конфигурируемые опции</param>
		/// <returns>Конечный автомат для настройки <see cref="DeleteQueryObjectOptions" /></returns>
		public static StateMachine<State, string> Build(DeleteQueryObjectOptions options)
		{
			var machine = new StateMachine<State, string>(State.Start);

			machine.Configure(State.Start)
					.Permit(Trigger.Generate, State.Generate)
					.Permit(Trigger.Use, State.Use);

			machine.Configure(State.Generate)
					.Permit(Trigger.Methods, State.Methods);

			machine.Configure(State.Methods)
					.PermitReentry(Trigger.Sync, () =>
					{
						options.GenerateSyncMethods = true;
						if (options.GenerateAsyncMethods == null)
						{
							options.GenerateAsyncMethods = false;
						}
					})
					.PermitReentry(Trigger.Async, () =>
					{
						options.GenerateAsyncMethods = true;
						if (options.GenerateSyncMethods == null)
						{
							options.GenerateSyncMethods = false;
						}
					});

			machine.Configure(State.Use)
					.Permit(Trigger.QueryText, State.QueryText);

			machine.Configure(State.QueryText)
					.PermitReentry(Trigger.Resource, () => options.UseQueryTextResourceFile = true)
					.PermitReentry(Trigger.String, () => options.UseQueryTextResourceFile = false);

			machine.OnUnhandledTrigger((state, trigger) =>
				throw new CodeGenerationException($"Can not parse delete query generation options: unexpected trigger [{trigger}] at state [{state:G}]"));

			return machine;
		}

		/// <summary>
		/// Разрешает выполнить повторный вход в текущее состояние с выполнением определенного действия
		/// </summary>
		/// <typeparam name="TState">Тип состояния конечного автомата</typeparam>
		/// <typeparam name="TTrigger">Тип триггера</typeparam>
		/// <param name="config">Текущая конфигурация конечного автомата</param>
		/// <param name="trigger">Разрешаемый триггер</param>
		/// <param name="action">Выполняемое действие</param>
		/// <returns></returns>
		private static StateMachine<TState, TTrigger>.StateConfiguration PermitReentry<TState, TTrigger>(this StateMachine<TState, TTrigger>.StateConfiguration config,
			TTrigger trigger,
			Action action)
		{
			return config.PermitReentry(trigger).OnEntryFrom(trigger, action);
		}
	}
}