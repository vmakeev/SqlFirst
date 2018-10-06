using System;

namespace SqlFirst.Codegen.Text
{
	/// <summary>
	/// Расширения для работы с параметрами контекста генерации кода
	/// </summary>
	internal static class ContextExtensions
	{
		/// <summary>
		/// Возвращает пространство имен, в котором должны выполняться генерация
		/// </summary>
		/// <param name="context">Контекст генерации кода</param>
		/// <returns>Пространство имен, в котором должны выполняться генерация</returns>
		public static string GetNamespace(this ICodeGenerationContext context)
		{
			return context.GetOption<string>("Namespace") ?? "DefaultNamespace";
		}

		/// <summary>
		/// Возвращает имя оригинального запроса
		/// </summary>
		/// <param name="context">Контекст генерации кода</param>
		/// <returns>Имя оригинального запроса</returns>
		public static string GetQueryName(this ICodeGenerationContext context)
		{
			return context.GetOption<string>("QueryName") ?? "Query";
		}

		/// <summary>
		/// Возвращает текст оригинального запроса
		/// </summary>
		/// <param name="context">Контекст генерации кода</param>
		/// <returns>Текст оригинального запроса</returns>
		public static string GetQueryText(this ICodeGenerationContext context)
		{
			return context.GetOption<string>("QueryText");
		}

		/// <summary>
		/// Возвращает полный текст оригинального запроса, включая комментарии и секции параметров
		/// </summary>
		/// <param name="context">Контекст генерации кода</param>
		/// <returns>Текст оригинального запроса, включая комментарии и секции параметров</returns>
		public static string GetQueryTextRaw(this ICodeGenerationContext context)
		{
			return context.GetOption<string>("QueryTextRaw");
		}

		/// <summary>
		/// Возвращает имя генерируемого класса с результатом
		/// </summary>
		/// <param name="context">Контекст генерации кода</param>
		/// <returns>Имя генерируемого класса с результатом</returns>
		public static string GetQueryResultItemTypeName(this ICodeGenerationContext context)
		{
			return context.GetOption<string>("QueryResultItemName") ?? "QueryItem";
		}

		/// <summary>
		/// Возвращает имя генерируемого класса, содержащего входящие параметры запроса
		/// </summary>
		/// <param name="context">Контекст генерации кода</param>
		/// <returns>Имя генерируемого класса, содержащего входящие параметры запроса</returns>
		public static string GetQueryParameterItemTypeName(this ICodeGenerationContext context)
		{
			return context.GetOption<string>("QueryParameterItemName") ?? "QueryParameter";
		}

		/// <summary>
		/// Возвращает путь к файлу ресурса, содержащему текст SQL запроса
		/// </summary>
		/// <param name="context">Контекст генерации кода</param>
		/// <returns>Путь к файлу ресурса, содержащему текст SQL запроса</returns>
		public static string GetResourcePath(this ICodeGenerationContext context)
		{
			return context.GetOption<string>("ResourcePath") ?? "DefaultNamespace.Query.sql";
		}

		/// <summary>
		/// Возвращает параметр указанного типа с указанным именем
		/// </summary>
		/// <typeparam name="T">Тип параметра</typeparam>
		/// <param name="context">Контекст генерации кода</param>
		/// <param name="option">Имя параметра</param>
		/// <returns>Параметр указанного типа с указанным именем</returns>
		private static T GetOption<T>(this ICodeGenerationContext context, string option) where T : class
		{
			if (option == null)
			{
				throw new ArgumentNullException(nameof(option));
			}

			if (context.Options == null)
			{
				return null;
			}

			if (!context.Options.TryGetValue(option, out object result))
			{
				return null;
			}

			// todo: null check?
			if (result is T typedResult)
			{
				return typedResult;
			}

			throw new InvalidCastException($"Can not cast context option [{option}] from '{result?.GetType().FullName ?? "null"}' to '{typeof(T).FullName}'");
		}
	}
}