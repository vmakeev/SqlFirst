using System;

namespace SqlFirst.Core
{
	/// <inheritdoc />
	public class QueryParamInfo : IQueryParamInfo
	{
		/// <inheritdoc />
		public string CsName { get; set; }

		/// <inheritdoc />
		public Type ClrType { get; set; }

		/// <inheritdoc />
		public string DbName { get; set; }

		/// <inheritdoc />
		public string DbType { get; set; }

		/// <inheritdoc />
		public int Length { get; set; }

		/// <inheritdoc />
		public int Precision { get; set; }

		/// <inheritdoc />
		public int Scale { get; set; }
	}
}