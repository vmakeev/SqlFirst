namespace SqlFirst.Core
{
	/// <summary>
	/// Информация о параметре запроса
	/// </summary>
	public interface IQueryParamInfo
	{
		/// <summary>
		/// Объявленная длина
		/// </summary>
		string Length { get; }

		/// <summary>
		/// Имя параметра в запроса
		/// </summary>
		string DbName { get; }

		/// <summary>
		/// Значимое имя параметра (без нумерации, если присутствует)
		/// </summary>
		string SemanticName { get; }

		/// <summary>
		/// Тип параметра в БД
		/// </summary>
		string DbType { get; }

		/// <summary>
		/// Значение по умолчанию, указанное в запросе
		/// </summary>
		object DefaultValue { get; }

		/// <summary>
		/// Является ли параметр запроса нумерованным
		/// </summary>
		bool IsNumbered { get; }
	}
}