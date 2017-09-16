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
		/// <param name="queryText">Полный текст запроса</param>
		/// <param name="connectionString">Строка подключения к БД</param>
		/// <returns>Подробная информация о возвращаемых запросом значениях</returns>
		IEnumerable<IFieldDetails> GetResultDetails(string queryText, string connectionString);

		/// <summary>
		/// Возвращает базовую информацию о запросе
		/// </summary>
		/// <param name="queryText">Полный текст запроса</param>
		/// <returns>Краткая информация о запросе</returns>
		IQueryBaseInfo GetQueryBaseInfo(string queryText);

		/// <summary>
		/// Возвращает информацию о запросе
		/// </summary>
		/// <param name="queryText">Полный текст запроса</param>
		/// <param name="connectionString">Строка подключения к БД</param>
		/// <returns>Краткая информация о запросе</returns>
		IQueryInfo GetQueryInfo(string queryText, string connectionString);

		/// <summary>
		/// Возвращает содержимое всех разделов запроса
		/// </summary>
		/// <param name="query">Полный текст файла SQL</param>
		/// <returns>Список найденных разделов</returns>
		IEnumerable<IQuerySection> GetQuerySections(string query);

		/// <summary>
		/// Возвращает содержимое разделов запроса с указанным именем
		/// </summary>
		/// <param name="query">Полный текст файла SQL</param>
		/// <param name="sectionName">Имя раздела</param>
		/// <returns>Список найденных разделов</returns>
		IEnumerable<IQuerySection> GetQuerySections(string query, string sectionName);

		/// <summary>
		/// Возвращает содержимое разделов запроса с указанным типом
		/// </summary>
		/// <param name="query">Полный текст файла SQL</param>
		/// <param name="sectionType">Тип раздела</param>
		/// <returns>Список найденных разделов</returns>
		IEnumerable<IQuerySection> GetQuerySections(string query, QuerySectionType sectionType);
	}
}