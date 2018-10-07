using System.Collections.Generic;
using System.Linq;
using SqlFirst.Providers.MsSqlServer.VariableDeclarations.Generated;
using static SqlFirst.Providers.MsSqlServer.VariableDeclarations.Generated.SqlVariableDeclarationsParser;

namespace SqlFirst.Providers.MsSqlServer.VariableDeclarations
{
	internal class QueryParamInfoVisitor : SqlVariableDeclarationsBaseVisitor<IEnumerable<DeclaredQueryParamInfo>>
	{
		/// <inheritdoc />
		public override IEnumerable<DeclaredQueryParamInfo> VisitDeclaration(DeclarationContext context)
		{
			var declarationVisitor = new DeclarationVisitor();
			DeclaredQueryParamInfo info = declarationVisitor.VisitDeclaration(context);
			return new[] { info };
		}

		/// <inheritdoc />
		protected override IEnumerable<DeclaredQueryParamInfo> AggregateResult(IEnumerable<DeclaredQueryParamInfo> aggregate, IEnumerable<DeclaredQueryParamInfo> nextResult)
		{
			if (aggregate == null)
			{
				return nextResult;
			}

			if (nextResult == null)
			{
				return aggregate;
			}

			return aggregate.Concat(nextResult);
		}
	}
}