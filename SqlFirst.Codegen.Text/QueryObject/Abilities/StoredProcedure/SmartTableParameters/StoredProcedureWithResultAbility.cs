using System;
using System.Collections.Generic;
using System.Linq;
using SqlFirst.Codegen.Helpers;
using SqlFirst.Codegen.Text.Snippets;
using SqlFirst.Codegen.Text.Templating;
using SqlFirst.Core;

namespace SqlFirst.Codegen.Text.QueryObject.Abilities.StoredProcedure.SmartTableParameters
{
	internal class StoredProcedureWithResultSmartTableAbility : StoredProcedureWithResultAbility
	{
		/// <inheritdoc />
		public override IEnumerable<string> GetDependencies(ICodeGenerationContext context)
		{
			foreach (string dependency in base.GetDependencies(context))
			{
				yield return dependency;
			}

			foreach (IQueryParamInfo queryParamInfo in context.IncomingParameters
															.Where(p => p.IsComplexType && p.ComplexTypeData?.IsTableType == true)
															.DistinctBy(p => p.DbName))
			{
				string typeName = queryParamInfo.ComplexTypeData.DbTypeDisplayedName ?? queryParamInfo.DbName;
				yield return KnownAbilityName.GetDataTable(CSharpCodeHelper.GetValidIdentifierName(typeName, NamingPolicy.Pascal));
			}
		}

		protected override IEnumerable<IRenderable> GetAddCustomParameters(ICodeGenerationContext context,
																					IEnumerable<IQueryParamInfo> targetParameters,
																					out IEnumerable<string> specificUsings)
		{
			specificUsings = Enumerable.Empty<string>();

			var results = new LinkedList<IRenderable>();

			IRenderableTemplate addCustomParameterTemplate = Snippet.Query.Methods.Common.Snippets.CallAddCustomParameter;
			IRenderableTemplate callGetDataTableInlineTemplate = Snippet.Query.Methods.Common.Snippets.CallGetDataTableInline;

			foreach (IQueryParamInfo paramInfo in targetParameters)
			{
				string parameterName = GetParameterName(paramInfo);

				object model;

				if (paramInfo.IsComplexType && paramInfo.ComplexTypeData?.IsTableType == true)
				{
					string dbTypeName = CSharpCodeHelper.GetValidIdentifierName(paramInfo.ComplexTypeData.DbTypeDisplayedName ?? paramInfo.DbType, NamingPolicy.Pascal);
					var dataTypeModel = new
					{
						DbTypeName = dbTypeName,
						Name = parameterName
					};

					model = new
					{
						SqlType = paramInfo.DbType,
						SqlName = paramInfo.DbName,
						Name = Renderable.Create(callGetDataTableInlineTemplate, dataTypeModel)
					};
				}
				else
				{
					model = new
					{
						SqlType = paramInfo.DbType,
						SqlName = paramInfo.DbName,
						Name = parameterName
					};
				}

				results.AddLast(Renderable.Create(addCustomParameterTemplate, model));
			}

			specificUsings = specificUsings.Distinct().ToArray();
			return results;
		}
		/// <inheritdoc />
		protected override IEnumerable<IRenderable> GetIncomingParameters(ICodeGenerationContext context, IEnumerable<IQueryParamInfo> targetParameters)
		{
			IRenderableTemplate methodParameterTemplate = Snippet.Query.Methods.Common.Snippets.MethodParameter;

			foreach (IQueryParamInfo paramInfo in targetParameters)
			{
				string parameterName = GetParameterName(paramInfo);
				string typeName;

				if (paramInfo.IsComplexType && paramInfo.ComplexTypeData?.IsTableType == true)
				{
					typeName = CSharpCodeHelper.GetGenericType(typeof(IEnumerable<>), paramInfo.ComplexTypeData.Name);
				}
				else
				{
					Type parameterType = context.TypeMapper.MapToClrType(paramInfo.DbType, true, paramInfo.DbTypeMetadata);
					typeName = CSharpCodeHelper.GetTypeBuiltInName(parameterType);
				}

				var model = new
				{
					Type = typeName,
					Name = parameterName
				};
				yield return Renderable.Create(methodParameterTemplate, model);
			}
		}
	}
}