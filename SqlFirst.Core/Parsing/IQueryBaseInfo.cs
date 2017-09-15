namespace SqlFirst.Core.Parsing
{
	/// <summary>
	/// Базовая информация о запросе
	/// </summary>
	public interface IQueryBaseInfo
	{
		/// <summary>
		/// Тип запроса
		/// </summary>
		QueryType QueryType { get; }
	}
}