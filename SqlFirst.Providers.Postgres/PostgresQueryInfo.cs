using System.Collections.Generic;
using SqlFirst.Core;

namespace SqlFirst.Providers.Postgres
{
	/// <summary>
	/// Полная информация о запросе
	/// </summary>
	internal class PostgresQueryInfo : PostgresQueryBaseInfo, IQueryInfo
	{
		/// <inheritdoc />
		public IEnumerable<IQueryParamInfo> Parameters { get; set; }

		/// <inheritdoc />
		public IEnumerable<IFieldDetails> Results { get; set; }
	}
}