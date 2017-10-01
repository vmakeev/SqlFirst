using System.Collections.Generic;

namespace SqlFirst.Core
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

		/// <summary>
		/// Опции SqlFirst
		/// </summary>
		IEnumerable<ISqlFirstOption> SqlFirstOptions { get; }
	}
}