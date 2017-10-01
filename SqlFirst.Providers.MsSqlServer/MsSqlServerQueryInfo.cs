using System.Collections.Generic;
using SqlFirst.Core;

namespace SqlFirst.Providers.MsSqlServer
{
	/// <summary>
	/// Полная информация о запросе
	/// </summary>
	internal class MsSqlServerQueryInfo : MsSqlServerQueryBaseInfo, IQueryInfo
	{
		/// <inheritdoc />
		public IEnumerable<IQueryParamInfo> Parameters { get; set; }

		/// <inheritdoc />
		public IEnumerable<IFieldDetails> Results { get; set; }
	}
}