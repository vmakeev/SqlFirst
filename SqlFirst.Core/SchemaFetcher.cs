using System.Collections.Generic;
using System.Data;

namespace SqlFirst.Core
{
	/// <inheritdoc />
	public abstract class SchemaFetcher : ISchemaFetcher
	{
		/// <summary>
		/// Поставщик доступа к методам БД
		/// </summary>
		protected abstract IDatabaseProvider DatabaseProvider { get; }

		/// <summary>
		/// Поставщик доступа к информации о полях БД
		/// </summary>
		protected abstract IFieldInfoProvider FieldInfoProvider { get; }

		/// <inheritdoc />
		public List<FieldDetails> GetResults(string connectionString, string query)
		{
			DataTable schemaTable = GetQuerySchema(connectionString, query);

			var result = new List<FieldDetails>();

			if (schemaTable == null)
			{
				return result;
			}

			for (int i = 0; i <= schemaTable.Rows.Count - 1; i++)
			{
				DataRow fieldMetadata = schemaTable.Rows[i];
				FieldDetails fieldDetails = FieldInfoProvider.GetFieldDetails(fieldMetadata);

				result.Add(fieldDetails);
			}

			return result;
		}

		/// <summary>
		/// Возвращает схему запроса
		/// </summary>
		/// <param name="connectionString">Строка подключения к БД</param>
		/// <param name="sql">Текст запроса</param>
		/// <returns>Таблица со схемой запроса</returns>
		private DataTable GetQuerySchema(string connectionString, string sql)
		{
			using (IDbConnection connection = DatabaseProvider.GetConnection(connectionString))
			{
				connection.Open();
				using (IDbCommand command = connection.CreateCommand())
				{
					command.CommandText = sql;
					DatabaseProvider.PrepareParametersForSchemaFetching(command);
					using (IDataReader dataReader = command.ExecuteReader(CommandBehavior.SchemaOnly))
					{
						DataTable dtSchema = dataReader.GetSchemaTable();
						return dtSchema;
					}
				}
			}
		}
	}
}