using System.Collections.Generic;
using System.Linq;
using SqlFirst.Core;
using SqlFirst.Core.Impl;

namespace SqlFirst.Intelligence.Generators
{
	internal class QueryInfoWrapper : IQueryInfo
	{
		private static readonly ISqlFirstOption[] _useResourceFileOption = { new SqlFirstOption("use", new[] { "querytext", "resource" }) };

		private readonly IQueryInfo _info;

		/// <inheritdoc />
		public QueryInfoWrapper(IQueryInfo info)
		{
			_info = info;
		}

		/// <inheritdoc />
		public QueryType Type => _info.Type;

		/// <inheritdoc />
		public IEnumerable<IQuerySection> Sections => _info.Sections;

		/// <inheritdoc />
		public IEnumerable<ISqlFirstOption> SqlFirstOptions
		{
			get
			{
				if (_info.SqlFirstOptions == null)
				{
					return _useResourceFileOption;
				}

				return _info.SqlFirstOptions.Concat(_useResourceFileOption);
			}
		}

		/// <inheritdoc />
		public IEnumerable<IQueryParamInfo> Parameters => _info.Parameters;

		/// <inheritdoc />
		public IEnumerable<IFieldDetails> Results => _info.Results;
	}
}