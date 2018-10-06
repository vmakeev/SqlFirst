using System;
using System.Collections.Generic;
using SqlFirst.Core;

namespace SqlFirst.Providers.MsSqlServer
{
	/// <summary>
	/// Информация о поле
	/// </summary>
	public class MsSqlServerFieldDetails : IFieldDetails
	{
		/// <summary>
		/// Размер столбца
		/// </summary>
		public int ColumnSize { get; set; }

		/// <summary>
		/// Числовая точность поля
		/// </summary>
		public int NumericPrecision { get; set; }

		/// <summary>
		/// Точность дробной части
		/// </summary>
		public int NumericScale { get; set; }

		/// <summary>
		/// Признак уникальности поля
		/// </summary>
		public bool IsUnique { get; set; }

		/// <summary>
		/// Имя столбца в схеме таблицы
		/// </summary>
		public string BaseColumnName { get; set; }

		/// <summary>
		/// Имя таблицы в схеме таблицы
		/// </summary>
		public string BaseTableName { get; set; }

		/// <summary>
		/// Тип поставщика данных столбца
		/// </summary>
		public int ProviderType { get; set; }

		/// <summary>
		/// Признак идентификатора
		/// </summary>
		public bool IsIdentity { get; set; }

		/// <summary>
		/// Признак наличия автоинкремента
		/// </summary>
		public bool IsAutoIncrement { get; set; }

		/// <summary>
		/// Признак содержания сведений о версии строки
		/// </summary>
		public bool IsRowVersion { get; set; }

		/// <summary>
		/// Признак содержания большого объема данных в поле
		/// </summary>
		public bool IsLong { get; set; }

		/// <summary>
		/// Признак доступности только на чтение
		/// </summary>
		public bool IsReadOnly { get; set; }

		/// <summary>
		/// Тип данных столбца, специфичный для конкретного поставщика данных
		/// </summary>
		public string ProviderSpecificDataType { get; set; }

		/// <summary>
		/// Имя типа данных CLR
		/// </summary>
		public Type ClrType { get; set; }

		/// <summary>
		/// Имя сборки, в которой определен пользовательский тип
		/// </summary>
		public string UdtAssemblyQualifiedName { get; set; }

		public int NewVersionedProviderType { get; set; }

		public bool IsColumnSet { get; set; }

		public int NonVersionedProviderType { get; set; }

		/// <summary>
		/// Имя столбца в таблице
		/// </summary>
		public string ColumnName { get; set; }

		/// <summary>
		/// Индекс столбца в БД
		/// </summary>
		public int ColumnOrdinal { get; set; }

		/// <summary>
		/// Разрешено ли значение Null
		/// </summary>
		public bool AllowDbNull { get; set; }

		/// <summary>
		/// Имя типа данных БД
		/// </summary>
		public string DbType { get; set; }

		/// <inheritdoc />
		public IDictionary<string, object> DbTypeMetadata { get; set; }
	}
}