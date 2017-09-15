using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using SqlFirst.Core.Parsing;

namespace SqlFirst.Providers.MsSqlServer
{
	/// <summary>
	/// Генерирует корректный SQL
	/// </summary>
	public class MsSqlServerCodeEmitter
	{
		/// <summary>
		/// Возвращает корректное константное значение, которое может быть использовано в SQL
		/// </summary>
		/// <param name="value">Значение</param>
		/// <param name="dbType">Тип значения в БД</param>
		/// <returns>Константное значение, которое может быть использовано в SQL</returns>
		public string EmitValue(object value, string dbType = null)
		{
			bool IsUnicodeString(string type)
			{
				return type == MsSqlDbType.NVarChar ||
						type == MsSqlDbType.NText ||
						type == MsSqlDbType.NChar;
			}

			switch (value)
			{
				case null:
					return null;

				case string stringValue when IsUnicodeString(dbType):
					return $"N'{stringValue}'";

				case string stringValue:
					return $"'{stringValue}'";

				case int intValue:
					return intValue.ToString(CultureInfo.InvariantCulture);

				case float floatValue:
					return floatValue.ToString(CultureInfo.InvariantCulture);

				default:
					throw new InvalidCastException($"Valid defaultValue type expected, actual is {value.GetType().Name}");
			}
		}

		/// <summary>
		/// Возвращает текст SQL для объявления указанного параметра
		/// </summary>
		/// <param name="info">Информация о параметре</param>
		/// <returns>Текст SQL для объявления указанного параметра</returns>
		public string EmitDeclaration(IQueryParamInfo info)
		{
			if (info == null)
			{
				throw new ArgumentNullException(nameof(info));
			}

			string fullTypeName = string.IsNullOrEmpty(info.Length)
				? info.DbType
				: $"{info.DbType}({info.Length})";

			string defaultValue = EmitValue(info.DefaultValue, info.DbType);

			string result = string.IsNullOrEmpty(defaultValue)
				? $@"DECLARE @{info.DbName} {fullTypeName};"
				: $@"DECLARE @{info.DbName} {fullTypeName} = {defaultValue};";

			return result;
		}

		/// <summary>
		/// Возвращает текст SQL для объявления указанных параметров
		/// </summary>
		/// <param name="infos">Информация о параметрах</param>
		/// <returns>Текст SQL для объявления указанных параметров</returns>
		public string EmitDeclarations(IEnumerable<IQueryParamInfo> infos)
		{
			if (infos == null)
			{
				throw new ArgumentNullException(nameof(infos));
			}

			IEnumerable<string> declarations = infos.Select(EmitDeclaration);
			string result = string.Join(Environment.NewLine, declarations);
			return result;
		}
	}
}
