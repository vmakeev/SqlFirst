﻿using System.Collections.Generic;
using SqlFirst.Core;

namespace SqlFirst.Providers.MsSqlServer
{
	/// <inheritdoc />
	internal class MsSqlServerQueryBaseInfo : IQueryBaseInfo
	{
		/// <inheritdoc />
		public QueryType Type { get; set; }

		/// <inheritdoc />
		public IEnumerable<IQuerySection> Sections { get; set; }

		/// <inheritdoc />
		public IEnumerable<ISqlFirstOption> SqlFirstOptions { get; set; }
	}
}