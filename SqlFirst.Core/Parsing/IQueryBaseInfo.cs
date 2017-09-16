using System.Collections.Generic;

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
		QueryType Type { get; }

		/// <summary>
		/// Разделы запроса
		/// </summary>
		IEnumerable<IQuerySection> Sections { get; }
	}
}