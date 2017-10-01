using System.Collections.Generic;

namespace SqlFirst.Core
{
	/// <summary>
	/// Полная информация о запросе
	/// </summary>
	public interface IQueryInfo : IQueryBaseInfo
	{
		/// <summary>
		/// Параметры запроса
		/// </summary>
		IEnumerable<IQueryParamInfo> Parameters { get; }

		/// <summary>
		/// Результаты запроса
		/// </summary>
		IEnumerable<IFieldDetails> Results { get; }
	}
}