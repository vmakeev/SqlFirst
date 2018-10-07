using System;
using System.Collections.Generic;
using System.Linq;
using SqlFirst.Core.Impl;
using SqlFirst.Providers.MsSqlServer.VariableDeclarations.Generated;

namespace SqlFirst.Providers.MsSqlServer.VariableDeclarations
{
	internal class DeclarationVisitor : SqlVariableDeclarationsBaseVisitor<DeclaredQueryParamInfo>
	{
		/// <inheritdoc />
		public override DeclaredQueryParamInfo VisitVariable(SqlVariableDeclarationsParser.VariableContext context)
		{
			string variableName = context.GetRuleContexts<SqlVariableDeclarationsParser.IdentifierContext>().Single().GetText();

			string dbName = variableName;
			(bool isNumbered, string semanticName) = QueryParamInfoNameHelper.GetNameSemantic(dbName);

			return new DeclaredQueryParamInfo
			{
				DbName = dbName,
				IsNumbered = isNumbered,
				SemanticName = semanticName
			};
		}

		/// <inheritdoc />
		public override DeclaredQueryParamInfo VisitTypeName(SqlVariableDeclarationsParser.TypeNameContext context)
		{
			IEnumerable<string> typeNameParts = context.GetRuleContexts<SqlVariableDeclarationsParser.IdentifierContext>().Select(p => p.GetText());

			return new DeclaredQueryParamInfo
			{
				DbType = string.Join(" ", typeNameParts)
			};
		}

		/// <inheritdoc />
		public override DeclaredQueryParamInfo VisitLength(SqlVariableDeclarationsParser.LengthContext context)
		{
			var maxValueContext = new Lazy<SqlVariableDeclarationsParser.MaxValueContext>(() => context.GetRuleContext<SqlVariableDeclarationsParser.MaxValueContext>(0));
			var intContext = new Lazy<SqlVariableDeclarationsParser.IntValueContext>(() => context.GetRuleContext<SqlVariableDeclarationsParser.IntValueContext>(0));

			DeclaredQueryParamInfo result = null;

			if (intContext.Value != null)
			{
				result = new DeclaredQueryParamInfo
				{
					Length = intContext.Value.GetText()
				};
			}
			else if (maxValueContext.Value != null)
			{
				result = new DeclaredQueryParamInfo
				{
					Length = maxValueContext.Value.GetText()
				};
			}

			return result;
		}

		/// <inheritdoc />
		public override DeclaredQueryParamInfo VisitValue(SqlVariableDeclarationsParser.ValueContext context)
		{
			var stringContext = new Lazy<SqlVariableDeclarationsParser.StringValueContext>(() => context.GetRuleContext<SqlVariableDeclarationsParser.StringValueContext>(0));
			var intContext = new Lazy<SqlVariableDeclarationsParser.IntValueContext>(() => context.GetRuleContext<SqlVariableDeclarationsParser.IntValueContext>(0));
			var floatContext = new Lazy<SqlVariableDeclarationsParser.FloatValueContext>(() => context.GetRuleContext<SqlVariableDeclarationsParser.FloatValueContext>(0));

			DeclaredQueryParamInfo result = null;

			if (stringContext.Value != null)
			{
				result = new DeclaredQueryParamInfo
				{
					DefaultValue = stringContext.Value.GetText()?.Trim('\'')
				};
			}
			else if (intContext.Value != null)
			{
				result = new DeclaredQueryParamInfo
				{
					DefaultValue = int.Parse(intContext.Value.GetText())
				};
			}
			else if (floatContext.Value != null)
			{
				result = new DeclaredQueryParamInfo
				{
					DefaultValue = float.Parse(floatContext.Value.GetText())
				};
			}

			return result;
		}

		/// <inheritdoc />
		protected override DeclaredQueryParamInfo AggregateResult(DeclaredQueryParamInfo aggregate, DeclaredQueryParamInfo nextResult)
		{
			if (aggregate == null)
			{
				return nextResult;
			}

			if (nextResult == null)
			{
				return aggregate;
			}

			if (aggregate.Length == null && nextResult.Length != null)
			{
				aggregate.Length = nextResult.Length;
			}

			if (aggregate.DbName == null && nextResult.DbName != null)
			{
				aggregate.DbName = nextResult.DbName;
			}

			if (aggregate.SemanticName == null && nextResult.SemanticName != null)
			{
				aggregate.SemanticName = nextResult.SemanticName;
			}

			if (!aggregate.IsNumbered && nextResult.IsNumbered)
			{
				aggregate.IsNumbered = true;
			}

			if (aggregate.DbType == null && nextResult.DbType != null)
			{
				aggregate.DbType = nextResult.DbType;
			}

			if (aggregate.DefaultValue == null && nextResult.DefaultValue != null)
			{
				aggregate.DefaultValue = nextResult.DefaultValue;
			}

			return aggregate;
		}
	}
}