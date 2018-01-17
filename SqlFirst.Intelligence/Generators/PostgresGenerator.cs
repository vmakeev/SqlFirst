using SqlFirst.Codegen;
using SqlFirst.Codegen.Text;
using SqlFirst.Core;
using SqlFirst.Providers.Postgres;

namespace SqlFirst.Intelligence.Generators
{
	public class PostgresGenerator : GeneratorBase
	{
		public override IQueryParser QueryParser { get; } = new PostgresQueryParser();

		public override ICodeGenerator CodeGenerator { get; } = new TextCodeGenerator();

		public override IDatabaseTypeMapper TypeMapper { get; } = new PostgresDatabaseTypeMapper();

		public override IDatabaseProvider DatabaseProvider { get; } = new PostgresDatabaseProvider();
	}
}