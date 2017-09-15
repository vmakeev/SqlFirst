using System.Collections.Generic;

namespace SqlFirst.Core.Parsing
{
	/// <summary>
	/// Полная информация о запросе
	/// </summary>
	public interface IQueryInfo: IQueryBaseInfo
	{
		/// <summary>
		/// Параметры запроса
		/// </summary>
		IEnumerable<IQueryParamInfo> Parameters { get; set; }

		/// <summary>
		/// Результаты запроса
		/// </summary>
		IEnumerable<IFieldDetails> Results { get; set; }
	}
}