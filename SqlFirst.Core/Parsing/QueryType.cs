namespace SqlFirst.Core.Parsing
{
	/// <summary>
	/// Тип запроса
	/// </summary>
	public enum QueryType
	{
		/// <summary>
		/// Некорректный тип
		/// </summary>
		INVALID,

		/// <summary>
		/// Неизвестный тип запроса
		/// </summary>
		Unknown,

		/// <summary>
		/// Создание записей
		/// </summary>
		Create,

		/// <summary>
		/// Чтение данных
		/// </summary>
		Read,

		/// <summary>
		/// Обновление данных
		/// </summary>
		Update,

		/// <summary>
		/// Удаление данных
		/// </summary>
		Delete
	}
}