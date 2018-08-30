using System.Collections.Generic;
using System.Linq;
using SqlFirst.Core;
using SqlFirst.Core.Impl;

namespace SqlFirst.Intelligence.Generators
{
	internal class QueryInfoWrapper : IQueryInfo
	{
		private static readonly ISqlFirstOption[] _useResourceFileOption = { new SqlFirstOption("use", new[] { "querytext", "resource" }) };

		public static QueryInfoWrapper Create(IQueryInfo info)
		{
			return new QueryInfoWrapper(info);
		}

		/// <inheritdoc />
		private QueryInfoWrapper(IQueryInfo info)
		{
			Type = info.Type;
			Parameters = info.Parameters?.ToArray() ?? Enumerable.Empty<IQueryParamInfo>();
			Results = info.Results?.ToArray() ?? Enumerable.Empty<IFieldDetails>();
			Sections = info.Sections?.ToArray() ?? Enumerable.Empty<IQuerySection>();
			SqlFirstOptions = info.SqlFirstOptions?.Concat(_useResourceFileOption).ToArray() ?? _useResourceFileOption;
		}

		/// <inheritdoc />
		public QueryType Type { get; }

		/// <inheritdoc />
		public IEnumerable<IQuerySection> Sections { get; }

		/// <inheritdoc />
		public IEnumerable<ISqlFirstOption> SqlFirstOptions { get; }

		/// <inheritdoc />
		public IEnumerable<IQueryParamInfo> Parameters { get; }

		/// <inheritdoc />
		public IEnumerable<IFieldDetails> Results { get; }
	}
}