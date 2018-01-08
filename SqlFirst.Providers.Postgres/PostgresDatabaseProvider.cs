using System.Data;
using Npgsql;
using SqlFirst.Core;

namespace SqlFirst.Providers.Postgres
{
	public class PostgresDatabaseProvider : IDatabaseProvider
	{
		/// <summary>
		/// Создает новое подключение к БД
		/// </summary>
		/// <param name="connectionString">Строка подключения</param>
		/// <returns>Подключение к БД</returns>
		public IDbConnection GetConnection(string connectionString)
		{
			return new NpgsqlConnection(connectionString);
		}

		/// <summary>
		/// Информация о специфичных для провайдера
		/// </summary>
		public IProviderTypesInfo ProviderTypesInfo { get; } = PostgresProviderTypesInfo.Instance;
	}
}