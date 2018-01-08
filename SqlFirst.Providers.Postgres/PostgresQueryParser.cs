using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Npgsql;
using SqlFirst.Core;
using SqlFirst.Core.Impl;

namespace SqlFirst.Providers.Postgres
{
	public class PostgresQueryParser : QueryParserBase
	{
		private readonly Lazy<IDatabaseProvider> _databaseProvider = new Lazy<IDatabaseProvider>(() => new PostgresDatabaseProvider());
		private readonly Lazy<IFieldInfoProvider> _fieldInfoProvider = new Lazy<IFieldInfoProvider>(() => new PostgresFieldInfoProvider());
		private readonly Lazy<PostgresAdoTypeMapper> _adoTypeMapper = new Lazy<PostgresAdoTypeMapper>(() => new PostgresAdoTypeMapper());

		/// <inheritdoc />
		public override IEnumerable<IFieldDetails> GetResultDetails(string queryText, string connectionString)
		{
			DataTable schemaTable;

			try
			{
				schemaTable = GetQuerySchema(queryText, connectionString);
			}
			catch (Exception ex)
			{
				throw new QueryParsingException("Unable to determine query results", ex);
			}

			if (schemaTable == null)
			{
				yield break;
			}

			for (int i = 0; i <= schemaTable.Rows.Count - 1; i++)
			{
				DataRow fieldMetadata = schemaTable.Rows[i];
				IFieldDetails fieldDetails = _fieldInfoProvider.Value.GetFieldDetails(fieldMetadata);

				yield return fieldDetails;
			}
		}

		/// <inheritdoc />
		public override IQueryBaseInfo GetQueryBaseInfo(string queryText)
		{
			List<IQuerySection> sections = GetQuerySections(queryText).ToList();

			IQuerySection bodySection = sections.SingleOrDefault(querySection => querySection.Type == QuerySectionType.Body);

			var queryType = QueryType.Unknown;

			if (bodySection != null)
			{
				string queryBody = bodySection.Content.Trim().ToLowerInvariant();

				if (queryBody.StartsWith("select"))
				{
					queryType = QueryType.Read;
				}
				else if (queryBody.StartsWith("update"))
				{
					queryType = QueryType.Update;
				}
				else if (queryBody.StartsWith("insert"))
				{
					queryType = QueryType.Create;
				}
				else if (queryBody.StartsWith("delete"))
				{
					queryType = QueryType.Delete;
				}
				else if (queryBody.StartsWith("merge"))
				{
					queryType = QueryType.Merge;
				}
			}

			return new PostgresQueryBaseInfo
			{
				Type = queryType,
				Sections = sections,
				SqlFirstOptions = GetOptions(queryText)
			};
		}

		/// <inheritdoc />
		public override IQueryInfo GetQueryInfo(string queryText, string connectionString)
		{
			IQueryBaseInfo baseInfo = GetQueryBaseInfo(queryText);

			IEnumerable<IQueryParamInfo> declaredParameters = GetDeclaredParameters(queryText);
			IEnumerable<IQueryParamInfo> undeclaredParameters = GetUndeclaredParameters(queryText, connectionString);
			IEnumerable<IQueryParamInfo> parameters = declaredParameters.Concat(undeclaredParameters);

			IEnumerable<IFieldDetails> results = GetResultDetails(queryText, connectionString);


			IQueryInfo queryInfo = new PostgresQueryInfo
			{
				Type = baseInfo.Type,
				Parameters = parameters,
				Results = results,
				Sections = baseInfo.Sections,
				SqlFirstOptions = baseInfo.SqlFirstOptions
			};

			return queryInfo;
		}

		/// <summary>
		/// Возвращает информацию о явно объявленных в секции "queryParameters" параметров запроса
		/// </summary>
		/// <param name="parametersDeclaration">Строка с объявленными переменными</param>
		/// <returns>Информация о параметрах</returns>
		protected override IEnumerable<IQueryParamInfo> GetDeclaredParametersInternal(string parametersDeclaration)
		{
			yield break;
		}

		/// <summary>
		/// Возвращает информацию необъявленных в секции "queryParameters" параметров запроса
		/// </summary>
		/// <param name="declared">Уже объявленные параметры</param>
		/// <param name="queryText">Полный текст запроса</param>
		/// <param name="connectionString">Строка подключения к БД</param>
		/// <returns>Информация о параметрах</returns>
		protected override IEnumerable<IQueryParamInfo> GetUndeclaredParametersInternal(IEnumerable<IQueryParamInfo> declared, string queryText, string connectionString)
		{
			using (IDbConnection connection = _databaseProvider.Value.GetConnection(connectionString))
			{
				connection.Open();
				using (IDbCommand command = connection.CreateCommand())
				{
					command.CommandText = queryText;

					NpgsqlCommandBuilder.DeriveParameters((NpgsqlCommand)command);

					foreach (NpgsqlParameter parameter in command.Parameters.OfType<NpgsqlParameter>())
					{
						string dbName = parameter.ParameterName.TrimStart('@');
						string dbType = parameter.NpgsqlDbType.ToString("G");

						(bool isNumbered, string semanticName) = QueryParamInfoNameHelper.GetNameSemantic(dbName);

						yield return new QueryParamInfo
						{
							DbType = PostgresDbType.Normalize(dbType),
							Length = PostgresDbType.GetLength(dbType),
							DbName = parameter.ParameterName,
							DefaultValue = parameter.NpgsqlValue,
							IsNumbered = isNumbered,
							SemanticName = semanticName
						};
					}
				}
			}
		}

		/// <summary>
		/// Возвращает схему запроса
		/// </summary>
		/// <param name="queryText">Текст запроса</param>
		/// <param name="connectionString">Строка подключения к БД</param>
		/// <returns>Таблица со схемой запроса</returns>
		private DataTable GetQuerySchema(string queryText, string connectionString)
		{
			using (IDbConnection connection = _databaseProvider.Value.GetConnection(connectionString))
			{
				connection.Open();
				using (IDbCommand command = connection.CreateCommand())
				{
					command.CommandText = queryText;

					NpgsqlCommandBuilder.DeriveParameters((NpgsqlCommand)command);

					foreach (NpgsqlParameter parameter in command.Parameters.OfType<NpgsqlParameter>())
					{
						if (parameter.IsNullable)
						{
							parameter.Value = DBNull.Value;
						}
						else
						{
							Type clrType = _adoTypeMapper.Value.Map(parameter.DbType, false);

							if (clrType == typeof(string))
							{
								parameter.Value = string.Empty;
							}
							else if (clrType == typeof(DateTime))
							{
								parameter.Value = DateTime.UtcNow.Date;
							}
							else
							{
								parameter.Value = Activator.CreateInstance(clrType);
							}
						}
					}

					using (IDataReader dataReader = command.ExecuteReader(CommandBehavior.SchemaOnly | CommandBehavior.KeyInfo))
					{
						DataTable dtSchema = dataReader.GetSchemaTable();
						return dtSchema;
					}
				}
			}
		}
	}
}
