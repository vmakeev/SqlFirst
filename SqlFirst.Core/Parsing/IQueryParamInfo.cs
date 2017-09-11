
namespace SqlFirst.Core.Parsing
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
		/// Тип параметра в БД
		/// </summary>
		string DbType { get; }

		/// <summary>
		/// Значение по умолчанию, указанное в запросе
		/// </summary>
		object DefaultValue { get; }
	}
}