using System.Collections.Generic;

namespace SqlFirst.Core
{
	/// <summary>
	/// Поставщик данных о схеме запроса
	/// </summary>
	public interface ISchemaFetcher
	{
		/// <summary>
		/// Возвращает подробную информацию о возвращаемых запросом значениях
		/// </summary>
		/// <param name="connectionString">Строка подключения к БД</param>
		/// <param name="query">Собственно запрос</param>
		/// <returns>Подробная информация о возвращаемых запросом значениях</returns>
		List<FieldDetails> GetResults(string connectionString, string query);
	}
}