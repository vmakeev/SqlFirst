namespace SqlFirst.Core.Impl
{
	/// <summary>
	/// Тип раздела
	/// </summary>
	public enum QuerySectionType
	{
		/// <summary>
		/// Некорректный тип
		/// </summary>
		INVALID,

		/// <summary>
		/// Опции SqlFirst
		/// </summary>
		Options,

		/// <summary>
		/// Объявления переменных
		/// </summary>
		Declarations,

		/// <summary>
		/// Тело запроса
		/// </summary>
		Body,

		/// <summary>
		/// Неопознанный раздел
		/// </summary>
		Unknown,

		/// <summary>
		/// Раздел с особым пользовательским именем
		/// </summary>
		Custom
	}
}