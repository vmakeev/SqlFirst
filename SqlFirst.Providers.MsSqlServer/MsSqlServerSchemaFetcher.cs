using SqlFirst.Core;

namespace SqlFirst.Providers.MsSqlServer
{
	public class MsSqlServerSchemaFetcher : SchemaFetcher
	{
		protected override IDatabaseProvider DatabaseProvider { get; } = new MsSqlServerDatabaseProvider();

		protected override IFieldInfoProvider FieldInfoProvider { get; } = new MsSqlServerFieldInfoProvider();
	}
}