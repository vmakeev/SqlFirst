using System.Collections.Generic;
using SqlFirst.Core.Parsing;

namespace SqlFirst.Providers.MsSqlServer
{
	/// <inheritdoc />
	internal class MsSqlServerQueryInfo : IQueryInfo
	{
		/// <inheritdoc />
		public QueryType QueryType { get; set; }

		/// <inheritdoc />
		public IEnumerable<IQueryParamInfo> Parameters { get; set; }

		/// <inheritdoc />
		public IEnumerable<IFieldDetails> Results { get; set; }
	}
}