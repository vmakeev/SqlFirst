using System.Diagnostics;

namespace SqlFirst.Core.Impl
{
	/// <inheritdoc />
	[DebuggerDisplay("{DbName} ({SemanticName}): {DbType}({Length}) = {DefaultValue}, IsNumbered: {IsNumbered}")]
	public class QueryParamInfo : IQueryParamInfo
	{
		/// <inheritdoc />
		public string DbName { get; set; }

		/// <inheritdoc />
		public string SemanticName { get; set; }

		/// <inheritdoc />
		public string DbType { get; set; }

		/// <inheritdoc />
		public object DefaultValue { get; set; }

		/// <inheritdoc />
		public bool IsNumbered { get; set; }

		/// <inheritdoc />
		public string Length { get; set; }
	}
}