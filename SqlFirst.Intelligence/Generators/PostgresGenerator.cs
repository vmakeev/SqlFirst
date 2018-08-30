using SqlFirst.Codegen;
using SqlFirst.Codegen.Text;
using SqlFirst.Core;
using SqlFirst.Providers.Postgres;

namespace SqlFirst.Intelligence.Generators
{
	/// <inheritdoc />
	public class PostgresGenerator : GeneratorBase
	{
		/// <inheritdoc />
		public override ISqlEmitter SqlEmitter { get; } = new PostgresCodeEmitter();

		/// <inheritdoc />
		public override IQueryParser QueryParser { get; } = new PostgresQueryParser();

		/// <inheritdoc />
		public override ICodeGenerator CodeGenerator { get; } = new TextCodeGenerator();

		/// <inheritdoc />
		public override IDatabaseTypeMapper TypeMapper { get; } = new PostgresDatabaseTypeMapper();

		/// <inheritdoc />
		public override IDatabaseProvider DatabaseProvider { get; } = new PostgresDatabaseProvider();
	}
}