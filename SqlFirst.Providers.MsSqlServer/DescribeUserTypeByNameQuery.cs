using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;

namespace SqlFirst.Providers.MsSqlServer
{
	internal static class DescribeUserTypeByNameQuery
	{
		/// <summary>
		/// Возвращает текст запроса
		/// </summary>
		/// <returns>Текст запроса</returns>
		private static string GetQueryText()
		{
			return @"select * from sys.types where name = @typeName";
		}

		/// <summary>
		/// Добавляет параметр к команде
		/// </summary>
		/// <param name="command">Команда SQL</param>
		/// <param name="parameterType">Тип параметра</param>
		/// <param name="parameterName">Имя параметра</param>
		/// <param name="value">Значение параметра</param>
		/// <param name="length">Длина параметра</param>
		private static void AddParameter(IDbCommand command, SqlDbType parameterType, string parameterName, object value, int? length = null)
		{
			var parameter = new SqlParameter
			{
				ParameterName = parameterName,
				SqlDbType = parameterType,
				Value = value ?? DBNull.Value
			};

			if (length.HasValue && length.Value > 0)
			{
				parameter.Size = length.Value;
			}

			command.Parameters.Add(parameter);
		}

		/// <summary>
		/// Возвращает новый элемент, заполненный данными из <paramref name="record"/>
		/// </summary>
		/// <param name="record">Строка БД</param>
		/// <returns>Новый элемент, заполненный данными из <paramref name="record"/></returns>
		[SuppressMessage("ReSharper", "FunctionComplexityOverflow")]
		private static MsSqlServerTypeDescription GetItemFromRecord(IDataRecord record)
		{
			var result = new MsSqlServerTypeDescription();

			if (record[0] != null && record[0] != DBNull.Value)
			{
				result.Name = (string)record[0];
			}
			if (record[1] != null && record[1] != DBNull.Value)
			{
				result.SystemTypeId = (byte)record[1];
			}
			if (record[2] != null && record[2] != DBNull.Value)
			{
				result.UserTypeId = (int)record[2];
			}
			if (record[3] != null && record[3] != DBNull.Value)
			{
				result.SchemaId = (int)record[3];
			}
			if (record[4] != null && record[4] != DBNull.Value)
			{
				result.PrincipalId = (int?)record[4];
			}
			if (record[5] != null && record[5] != DBNull.Value)
			{
				result.MaxLength = (short)record[5];
			}
			if (record[6] != null && record[6] != DBNull.Value)
			{
				result.Precision = (byte)record[6];
			}
			if (record[7] != null && record[7] != DBNull.Value)
			{
				result.Scale = (byte)record[7];
			}
			if (record[8] != null && record[8] != DBNull.Value)
			{
				result.CollationName = (string)record[8];
			}
			if (record[9] != null && record[9] != DBNull.Value)
			{
				result.IsNullable = (bool?)record[9];
			}
			if (record[10] != null && record[10] != DBNull.Value)
			{
				result.IsUserDefined = (bool)record[10];
			}
			if (record[11] != null && record[11] != DBNull.Value)
			{
				result.IsAssemblyType = (bool)record[11];
			}
			if (record[12] != null && record[12] != DBNull.Value)
			{
				result.DefaultObjectId = (int)record[12];
			}
			if (record[13] != null && record[13] != DBNull.Value)
			{
				result.RuleObjectId = (int)record[13];
			}
			if (record[14] != null && record[14] != DBNull.Value)
			{
				result.IsTableType = (bool)record[14];
			}

			return result;
		}

		/// <summary>
		/// Выполняет загрузку первого элемента типа <see cref="MsSqlServerTypeDescription"/>
		/// </summary>
		/// <param name="connection">Подключение к БД</param>
		/// <param name="typeName">typeName</param>
		/// <returns>Первый элемент типа <see cref="MsSqlServerTypeDescription"/></returns>
		public static MsSqlServerTypeDescription GetFirst(IDbConnection connection, string typeName)
		{
			using (IDbCommand cmd = connection.CreateCommand())
			{
				cmd.CommandText = GetQueryText();
				AddParameter(cmd, SqlDbType.NVarChar, "@typeName", typeName);

				using (IDataReader reader = cmd.ExecuteReader())
				{
					if (!reader.Read())
					{
						return null;
					}

					return GetItemFromRecord(reader);
				}
			}
		}
	}
}