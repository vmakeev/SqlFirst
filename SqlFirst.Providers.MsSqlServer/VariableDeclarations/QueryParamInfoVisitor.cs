using System;
using System.Collections.Generic;
using System.Linq;
using SqlFirst.Core.Parsing;
using SqlFirst.Providers.MsSqlServer.VariableDeclarations.Generated;
using static SqlFirst.Providers.MsSqlServer.VariableDeclarations.Generated.SqlVariableDeclarationsParser;

namespace SqlFirst.Providers.MsSqlServer.VariableDeclarations
{
	public class QueryParamInfoVisitor : SqlVariableDeclarationsBaseVisitor<IEnumerable<IQueryParamInfo>>
	{
		private IQueryParamInfo _current;
		private List<IQueryParamInfo> _results;

		/// <inheritdoc />
		public override IEnumerable<IQueryParamInfo> VisitRoot(RootContext context)
		{
			_results = new List<IQueryParamInfo>();
			base.VisitRoot(context);
			return _results;
		}

		/// <inheritdoc />
		public override IEnumerable<IQueryParamInfo> VisitDeclaration(DeclarationContext context)
		{
			_current = new QueryParamInfo();
			VisitChildren(context);
			_results.Add(_current);
			return null;
		}

		/// <inheritdoc />
		public override IEnumerable<IQueryParamInfo> VisitVariable(VariableContext context)
		{
			string variableName = context.GetRuleContexts<IdentifierContext>().Single().GetText();

			_current.DbName = variableName;

			return base.VisitVariable(context);
		}

		/// <inheritdoc />
		public override IEnumerable<IQueryParamInfo> VisitTypeName(TypeNameContext context)
		{
			IEnumerable<string> typeNameParts = context.GetRuleContexts<IdentifierContext>().Select(p => p.GetText());

			_current.DbType = string.Join(" ", typeNameParts);

			return base.VisitTypeName(context);
		}

		/// <inheritdoc />
		public override IEnumerable<IQueryParamInfo> VisitSize(SizeContext context)
		{
			string sizeString = context.GetRuleContexts<IntContext>().Single().GetText();

			_current.Length = int.Parse(sizeString);

			return base.VisitSize(context);
		}

		/// <inheritdoc />
		public override IEnumerable<IQueryParamInfo> VisitValue(ValueContext context)
		{
			var stringContext = new Lazy<StringContext>(() => context.GetChild<StringContext>(0));
			var intContext = new Lazy<IntContext>(() => context.GetChild<IntContext>(0));
			var floatContext = new Lazy<FloatContext>(() => context.GetChild<FloatContext>(0));

			if (stringContext.Value != null)
			{
				_current.DefaultValue = stringContext.Value.GetText()?.Trim('\'');
			}
			else if (intContext.Value != null)
			{
				_current.DefaultValue = int.Parse(intContext.Value.GetText());
			}
			else if (floatContext.Value != null)
			{
				_current.DefaultValue = float.Parse(floatContext.Value.GetText());
			}

			return base.VisitValue(context);
		}
	}
}
