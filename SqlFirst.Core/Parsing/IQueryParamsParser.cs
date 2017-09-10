using System.Collections.Generic;

namespace SqlFirst.Core.Parsing
{
	/// <summary>
	/// Парсер параметров запроса
	/// </summary>
	public interface IQueryParamsParser
	{
		/// <summary>
		/// Возвращает информацию о явно объявленных в секции "queryParameters" параметров запроса
		/// </summary>
		/// <param name="queryText">Полный текст запроса</param>
		/// <returns>Информация о параметрах</returns>
		IEnumerable<IQueryParamInfo> GetDeclaredParameters(string queryText);

		/// <summary>
		/// Возвращает информацию необъявленных в секции "queryParameters" параметров запроса
		/// </summary>
		/// <param name="queryText">Полный текст запроса</param>
		/// <param name="connectionString">Строка подключения к БД</param>
		/// <returns>Информация о параметрах</returns>
		IEnumerable<IQueryParamInfo> GetUndeclaredParameters(string queryText, string connectionString);
	}
}