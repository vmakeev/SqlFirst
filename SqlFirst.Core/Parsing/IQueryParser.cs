using System.Collections.Generic;

namespace SqlFirst.Core.Parsing
{
	/// <summary>
	/// Парсер параметров запроса
	/// </summary>
	public interface IQueryParser
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

		/// <summary>
		/// Возвращает подробную информацию о возвращаемых запросом значениях
		/// </summary>
		/// <param name="queryText">Собственно запрос</param>
		/// <param name="connectionString">Строка подключения к БД</param>
		/// <returns>Подробная информация о возвращаемых запросом значениях</returns>
		IEnumerable<IFieldDetails> GetResultDetails(string queryText, string connectionString);
	}
}