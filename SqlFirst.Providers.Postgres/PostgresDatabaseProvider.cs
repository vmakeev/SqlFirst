using System.Data;
using Npgsql;
using SqlFirst.Core;

namespace SqlFirst.Providers.Postgres
{
	internal class PostgresDatabaseProvider: IDatabaseProvider
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
	}
}