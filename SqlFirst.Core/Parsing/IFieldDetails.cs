using System;

namespace SqlFirst.Core.Parsing
{
	public interface IFieldDetails
	{
		/// <summary>
		/// Имя столбца в таблице
		/// </summary>
		string ColumnName { get; }

		/// <summary>
		/// Индекс столбца в БД
		/// </summary>
		int ColumnOrdinal { get; }

		/// <summary>
		/// Размер столбца
		/// </summary>
		int ColumnSize { get; }

		/// <summary>
		/// Числовая точность поля
		/// </summary>
		int NumericPrecision { get; }

		/// <summary>
		/// Точность дробной части
		/// </summary>
		int NumericScale { get; }

		/// <summary>
		/// Признак уникальности поля
		/// </summary>
		bool IsUnique { get; }

		/// <summary>
		/// Имя столбца в схеме таблицы
		/// </summary>
		string BaseColumnName { get; }

		/// <summary>
		/// Имя таблицы в схеме таблицы
		/// </summary>
		string BaseTableName { get; }

		/// <summary>
		/// Разрешено ли значение Null
		/// </summary>
		bool AllowDbNull { get; }

		/// <summary>
		/// Тип поставщика данных столбца
		/// </summary>
		int ProviderType { get; }

		/// <summary>
		/// Признак идентификатора
		/// </summary>
		bool IsIdentity { get; }

		/// <summary>
		/// Признак наличия автоинкремента
		/// </summary>
		bool IsAutoIncrement { get; }

		/// <summary>
		/// Признак содержания сведений о версии строки
		/// </summary>
		bool IsRowVersion { get; }

		/// <summary>
		/// Признак содержания большого объема данных в поле
		/// </summary>
		bool IsLong { get; }

		/// <summary>
		/// Признак доступности только на чтение
		/// </summary>
		bool IsReadOnly { get; }

		/// <summary>
		/// Тип данных столбца, специфичный для конкретного поставщика данных
		/// </summary>
		string ProviderSpecificDataType { get; }

		/// <summary>
		/// Имя типа данных БД
		/// </summary>
		string DbType { get; }

		/// <summary>
		/// Имя типа данных CLR
		/// </summary>
		Type ClrType { get; }

		string UdtAssemblyQualifiedName { get; }

		int NewVersionedProviderType { get; }

		bool IsColumnSet { get; }

		int NonVersionedProviderType { get; }
	}
}