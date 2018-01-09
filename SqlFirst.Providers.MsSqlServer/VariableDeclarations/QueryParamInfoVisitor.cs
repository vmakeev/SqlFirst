using System.Collections.Generic;
using System.Linq;
using SqlFirst.Core;
using SqlFirst.Providers.MsSqlServer.VariableDeclarations.Generated;
using static SqlFirst.Providers.MsSqlServer.VariableDeclarations.Generated.SqlVariableDeclarationsParser;

namespace SqlFirst.Providers.MsSqlServer.VariableDeclarations
{
	internal class QueryParamInfoVisitor : SqlVariableDeclarationsBaseVisitor<IEnumerable<IQueryParamInfo>>
	{
		/// <inheritdoc />
		public override IEnumerable<IQueryParamInfo> VisitDeclaration(DeclarationContext context)
		{
			var declarationVisitor = new DeclarationVisitor();
			IQueryParamInfo info = declarationVisitor.VisitDeclaration(context);
			return new[] { info };
		}

		/// <inheritdoc />
		protected override IEnumerable<IQueryParamInfo> AggregateResult(IEnumerable<IQueryParamInfo> aggregate, IEnumerable<IQueryParamInfo> nextResult)
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