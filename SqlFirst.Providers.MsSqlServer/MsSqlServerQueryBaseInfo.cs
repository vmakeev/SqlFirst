using SqlFirst.Core.Parsing;

namespace SqlFirst.Providers.MsSqlServer
{
	/// <inheritdoc />
	internal class MsSqlServerQueryBaseInfo : IQueryBaseInfo
	{
		/// <inheritdoc />
		public QueryType QueryType { get; set; }
	}
}