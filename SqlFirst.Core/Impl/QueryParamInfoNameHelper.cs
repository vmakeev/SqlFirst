using System;

namespace SqlFirst.Core.Impl
{
	/// <summary>
	/// Помощник работы с именами параметров запроса
	/// </summary>
	public static class QueryParamInfoNameHelper
	{
		private const string DefaultNumberPostfix = "_N";

		/// <summary>
		/// Возвращает признак "нумерованности" параметра, и его семантическое имя
		/// </summary>
		/// <param name="parameterDbName">Исходное имя параметра</param>
		/// <returns></returns>
		public static (bool isNumbered, string semanticName) GetNameSemantic(string parameterDbName)
		{
			return GetNameSemantic(parameterDbName, DefaultNumberPostfix);
		}

		/// <summary>
		/// Возвращает признак "нумерованности" параметра, и его семантическое имя
		/// </summary>
		/// <param name="parameterDbName">Исходное имя параметра</param>
		/// <param name="numberPostfix">Постфикс имени параметра, определяющий его нумерованность</param>
		/// <returns></returns>
		public static (bool isNumbered, string semanticName) GetNameSemantic(string parameterDbName, string numberPostfix)
		{
			if (string.IsNullOrEmpty(parameterDbName))
			{
				return (false, parameterDbName);
			}

			parameterDbName = parameterDbName.TrimStart('@');

			bool isNumbered = parameterDbName.EndsWith(numberPostfix);
			string semanticName = isNumbered ? parameterDbName.Substring(0, parameterDbName.Length - numberPostfix.Length) : parameterDbName;

			return (isNumbered, semanticName);
		}

		/// <summary>
		/// Возвращает шаблон нумерованного имени параметра, который можно использовать в string.Format()
		/// </summary>
		/// <param name="parameterSemanticName">Семантическое имя параметра</param>
		/// <returns>Шаблон нумерованного имени параметра</returns>
		public static string GetNumberedNameTemplate(string parameterSemanticName)
		{
			return parameterSemanticName + "_{0}";
		}

		/// <summary>
		/// Возвращает нумерованное имя параметра
		/// </summary>
		/// <param name="parameterSemanticName">Семантическое имя параметра</param>
		/// <param name="parameterNumber">Порядковый номер параметра</param>
		/// <returns>Нумерованное имя параметра</returns>
		public static string GetNumberedName(string parameterSemanticName, int parameterNumber)
		{
			if (parameterNumber < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(parameterNumber), parameterNumber, $"{nameof(parameterNumber)} can not be less than zero.");
			}

			string template = GetNumberedNameTemplate(parameterSemanticName);
			return string.Format(template, parameterNumber);
		}
	}
}