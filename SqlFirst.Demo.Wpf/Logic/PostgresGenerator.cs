using SqlFirst.Codegen;
using SqlFirst.Codegen.Text;
using SqlFirst.Core;
using SqlFirst.Providers.Postgres;

namespace SqlFirst.Demo.Wpf.Logic
{
	public class PostgresGenerator : GeneratorBase
	{
		public override SamplesBase Samples { get; } = new PostgresSamples();

		public override IQueryParser QueryParser { get; } = new PostgresQueryParser();

		public override ICodeGenerator CodeGenerator { get; } = new TextCodeGenerator();

		public override IDatabaseTypeMapper TypeMapper { get; } = new PostgresDatabaseTypeMapper();

		public override IDatabaseProvider DatabaseProvider { get; } = new PostgresDatabaseProvider();
	}
}