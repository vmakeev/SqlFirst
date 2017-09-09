using System.Collections.Generic;
using System.Data;

namespace SqlFirst.Core
{
	/// <summary>
	/// Провайдер работы с определенным типом БД
	/// </summary>
	public interface IDatabaseProvider
	{
		/// <summary>
		/// Возвращает информацию об уже объявленных параметрах, присутствующих в тексте запроса
		/// </summary>
		/// <param name="queryText">Текст запроса</param>
		/// <returns>Информация о параметрах</returns>
		List<IQueryParamInfo> GetDeclaredParameters(string queryText);

		/// <summary>
		/// Возвращает информацию о необъявленных параметрах, присутствующих в тексте запроса
		/// </summary>
		/// <param name="queryText">Текст запроса</param>
		/// <param name="connectionString">Строка подключения к БД</param>
		/// <returns>Информация о параметрах</returns>
		List<IQueryParamInfo> GetUndeclaredParameters(string queryText, string connectionString);

		/// <summary>
		/// Добавляет необъявленные параметры, присутствующие в теле запроса, для успешного выполнения команды
		/// </summary>
		/// <param name="command">Комманда, в которую следует добавить параметры</param>
		void PrepareParametersForSchemaFetching(IDbCommand command);

		/// <summary>
		/// Генерирует SQL с объявлением указанного набора параметров
		/// </summary>
		/// <param name="parameters">Набор параметров</param>
		/// <returns></returns>
		string BuildParameterDeclarations(List<IQueryParamInfo> parameters);

		/// <summary>
		/// Создает новое подключение к БД
		/// </summary>
		/// <param name="connectionString">Строка подключения</param>
		/// <returns>Подключение к БД</returns>
		IDbConnection GetConnection(string connectionString);

		/// <summary>
		/// Возвращает имя типа CLR, который может быть безопасно использован для представления указанного типа данных в БД
		/// </summary>
		/// <param name="dbType">Название типа данных в БД</param>
		/// <param name="dbTypeNormalized">Нормализованное название типа данных в БД</param>
		/// <param name="nullable">Поддерживается ли значение null</param>
		/// <returns>Имя типа CLR</returns>
		string GetClrTypeName(string dbType, out string dbTypeNormalized, bool nullable = true);
	}
}