using System.Data;

namespace SqlFirst.Core
{
	/// <summary>
	/// Провайдер работы с определенным типом БД
	/// </summary>
	public interface IDatabaseProvider
	{
		/// <summary>
		/// Создает новое подключение к БД
		/// </summary>
		/// <param name="connectionString">Строка подключения</param>
		/// <returns>Подключение к БД</returns>
		IDbConnection GetConnection(string connectionString);

		/// <summary>
		/// Информация о специфичных для провайдера
		/// </summary>
		IProviderTypesInfo ProviderTypesInfo { get; }

	}
}