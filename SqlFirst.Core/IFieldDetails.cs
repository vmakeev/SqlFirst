namespace SqlFirst.Core
{
	/// <summary>
	/// Информация о столбце в БД
	/// </summary>
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
		/// Разрешено ли значение Null
		/// </summary>
		bool AllowDbNull { get; }

		/// <summary>
		/// Имя типа данных БД
		/// </summary>
		string DbType { get; }
	}
}