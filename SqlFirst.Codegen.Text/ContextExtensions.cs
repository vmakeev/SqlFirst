﻿using System;

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
			return context.GetOption<string>("Namespace");
		}

		/// <summary>
		/// Возвращает имя оригинального запроса
		/// </summary>
		/// <param name="context">Контекст генерации кода</param>
		/// <returns>Имя оригинального запроса</returns>
		public static string GetQueryName(this ICodeGenerationContext context)
		{
			return context.GetOption<string>("QueryName");
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
			if (context?.Options == null)
			{
				return null;
			}

			if (!context.Options.TryGetValue(option, out object result))
			{
				return null;
			}

			if (result is T typedResult)
			{
				return typedResult;
			}

			throw new InvalidCastException();
		}
	}
}