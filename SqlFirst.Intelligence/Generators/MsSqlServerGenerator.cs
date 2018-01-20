using SqlFirst.Codegen;
using SqlFirst.Codegen.Text;
using SqlFirst.Core;
using SqlFirst.Providers.MsSqlServer;

namespace SqlFirst.Intelligence.Generators
{
	/// <inheritdoc />
	public class MsSqlServerGenerator : GeneratorBase
	{
		/// <inheritdoc />
		public override ISqlEmitter SqlEmitter { get; } = new MsSqlServerCodeEmitter();

		/// <inheritdoc />
		public override IQueryParser QueryParser { get; } = new MsSqlServerQueryParser();

		/// <inheritdoc />
		public override ICodeGenerator CodeGenerator { get; } = new TextCodeGenerator();

		/// <inheritdoc />
		public override IDatabaseTypeMapper TypeMapper { get; } = new MsSqlServerTypeMapper();

		/// <inheritdoc />
		public override IDatabaseProvider DatabaseProvider { get; } = new MsSqlServerDatabaseProvider();
	}
}