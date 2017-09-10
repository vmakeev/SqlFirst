using System.Collections.Generic;
using System.Data;
using SqlFirst.Core.Parsing;

namespace SqlFirst.Core
{
	/// <summary>
	/// Провайдер работы с определенным типом БД
	/// </summary>
	public interface IDatabaseProvider
	{
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
	}
}