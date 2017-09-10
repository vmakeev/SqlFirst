
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
		string Length { get; set; }

		/// <summary>
		/// Имя параметра в запроса
		/// </summary>
		string DbName { get; set; }

		/// <summary>
		/// Тип параметра в БД
		/// </summary>
		string DbType { get; set; }

		/// <summary>
		/// Значение по умолчанию, указанное в запросе
		/// </summary>
		object DefaultValue { get; set; }
	}
}