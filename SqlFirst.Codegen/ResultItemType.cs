namespace SqlFirst.Codegen
{
	/// <summary>
	/// Тип результата запроса
	/// </summary>
	public enum ResultItemType
	{
		/// <summary>
		/// Некорректный тип
		/// </summary>
		INVALID,

		/// <summary>
		/// Простой класс
		/// </summary>
		Poco,

		/// <summary>
		/// Класс с уведомлением об изменении свойств
		/// </summary>
		NotifyPropertyChanged,

		/// <summary>
		/// Структура
		/// </summary>
		Struct
	}
}