using System.Collections.Generic;

namespace SqlFirst.Core
{
	public interface ISchemaFetcher
	{
		List<FieldDetails> GetFields(string connectionString, string query);
	}
}