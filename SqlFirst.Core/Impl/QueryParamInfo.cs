using System.Diagnostics;

namespace SqlFirst.Core.Impl
{
	/// <inheritdoc />
	[DebuggerDisplay("{DbName}: {DbType}({Length}) = {DefaultValue}")]
	public class QueryParamInfo : IQueryParamInfo
	{
		/// <inheritdoc />
		public string DbName { get; set; }

		/// <inheritdoc />
		public string DbType { get; set; }

		/// <inheritdoc />
		public object DefaultValue { get; set; }

		/// <inheritdoc />
		public string Length { get; set; }
	}
}