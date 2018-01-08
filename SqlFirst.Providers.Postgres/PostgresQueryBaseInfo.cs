using System.Collections.Generic;
using SqlFirst.Core;

namespace SqlFirst.Providers.Postgres
{
	/// <inheritdoc />
	internal class PostgresQueryBaseInfo : IQueryBaseInfo
	{
		/// <inheritdoc />
		public QueryType Type { get; set; }

		/// <inheritdoc />
		public IEnumerable<IQuerySection> Sections { get; set; }

		/// <inheritdoc />
		public IEnumerable<ISqlFirstOption> SqlFirstOptions { get; set; }
	}
}