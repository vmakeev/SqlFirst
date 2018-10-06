using System;
using SqlFirst.Core;

namespace SqlFirst.Demo.Wpf.Logic
{
	public abstract class SamplesBase
	{
		public string GetSample(QueryType queryType)
		{
			switch (queryType)
			{
				case QueryType.Create:
					return GetInsertQuery();

				case QueryType.Read:
					return GetSelectQuery();

				case QueryType.Update:
					return GetUpdateQuery();

				case QueryType.Delete:
					return GetDeleteQuery();

				case QueryType.StoredProcedure:
					return GetStoredProcedureQuery();

				case QueryType.INVALID:
				case QueryType.Unknown:
				case QueryType.Merge:
				default:
					throw new ArgumentOutOfRangeException(nameof(queryType), queryType, null);
			}
		}

		protected virtual string GetInsertQuery() => string.Empty;

		protected virtual string GetSelectQuery() => string.Empty;

		protected virtual string GetUpdateQuery() => string.Empty;

		protected virtual string GetDeleteQuery() => string.Empty;

		protected virtual string GetStoredProcedureQuery() => string.Empty;
	}
}