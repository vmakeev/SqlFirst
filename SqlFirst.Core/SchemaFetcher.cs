using System.Collections.Generic;
using System.Data;

namespace SqlFirst.Core
{
	public abstract class SchemaFetcher : ISchemaFetcher
	{
		protected abstract IDatabaseProvider DatabaseProvider { get; }

		protected abstract IFieldInfoProvider FieldInfoProvider { get; }

		public List<FieldDetails> GetFields(string connectionString, string query)
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