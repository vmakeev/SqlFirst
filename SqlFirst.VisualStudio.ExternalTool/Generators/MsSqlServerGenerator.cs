﻿using SqlFirst.Codegen;
using SqlFirst.Codegen.Text;
using SqlFirst.Core;
using SqlFirst.Providers.MsSqlServer;

namespace SqlFirst.VisualStudio.ExternalTool.Generators
{
	internal class MsSqlServerGenerator : GeneratorBase
	{
		public override IQueryParser QueryParser { get; } = new MsSqlServerQueryParser();

		public override ICodeGenerator CodeGenerator { get; } = new TextCodeGenerator();

		public override IDatabaseTypeMapper TypeMapper { get; } = new MsSqlServerTypeMapper();

		public override IDatabaseProvider DatabaseProvider { get; } = new MsSqlServerDatabaseProvider();
	}
}