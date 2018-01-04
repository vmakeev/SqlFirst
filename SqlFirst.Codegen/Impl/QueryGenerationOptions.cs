using System.Collections.Generic;
using SqlFirst.Core;

namespace SqlFirst.Codegen.Impl
{
	/// <inheritdoc />
	public class QueryGenerationOptions : IQueryGenerationOptions
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Object"></see> class.</summary>
		public QueryGenerationOptions(QueryType queryType, IEnumerable<ISqlFirstOption> sqlFirstOptions)
		{
			SqlFirstOptions = sqlFirstOptions;
			QueryType = queryType;
		}

		public QueryType QueryType { get; }

		/// <inheritdoc />
		public IEnumerable<ISqlFirstOption> SqlFirstOptions { get; }
	}
}