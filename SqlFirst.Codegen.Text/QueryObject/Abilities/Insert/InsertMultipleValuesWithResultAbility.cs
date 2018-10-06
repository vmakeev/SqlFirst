﻿using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using SqlFirst.Codegen.Helpers;
using SqlFirst.Codegen.Text.QueryObject.Data;
using SqlFirst.Codegen.Text.Snippets;
using SqlFirst.Codegen.Text.Templating;
using SqlFirst.Core;
using SqlFirst.Core.Impl;

namespace SqlFirst.Codegen.Text.QueryObject.Abilities.Insert
{
	internal class InsertMultipleValuesWithResultAbility : QueryObjectAbilityBase
	{
		/// <inheritdoc />
		public override string Name { get; } = "InsertMultipleValues";

		/// <inheritdoc />
		public override IQueryObjectData Apply(ICodeGenerationContext context, IQueryObjectData data)
		{
			IQueryParamInfo[] allParameters = context.IncomingParameters.ToArray();
			IQueryParamInfo[] numberedParameters = allParameters.Where(paramInfo => paramInfo.IsNumbered).ToArray();
			IQueryParamInfo[] notNumberedParameters = allParameters.Where(paramInfo => !paramInfo.IsNumbered).ToArray();

			const string indexVariableName = "index";
			const string parameterVariableName = "item";
			string resultItemType = context.GetQueryResultItemTypeName();
			string parameterType = context.GetQueryParameterItemTypeName();

			IEnumerable<IRenderable> notNumberedXml = GetXmlParameters(context, notNumberedParameters);
			IEnumerable<IRenderable> notNumberedIncoming = GetIncomingParameters(context, notNumberedParameters);
			
			IEnumerable<IRenderable> notNumberedAddParams = GetAddParameters(context, notNumberedParameters.Where(p => !p.IsComplexType), out IEnumerable<string> notNumberedSdtUsings);
			IEnumerable<IRenderable> notNumberedAddCustomParams = GetAddCustomParameters(context, notNumberedParameters.Where(p => p.IsComplexType), out IEnumerable<string> notNumberedCustomUsings);

			IEnumerable<IRenderable> numberedAddParameters = GetAddParametersNumbered(context, indexVariableName, numberedParameters.Where(p => !p.IsComplexType), out IEnumerable<string> numberedSdtUsings);
			IEnumerable<IRenderable> numberedAddCustomParameters = GetAddParametersNumbered(context, indexVariableName, numberedParameters.Where(p => p.IsComplexType), out IEnumerable<string> numberedCustomUsings);

			string method = GetTemplate().Render(new
			{
				XmlParams = notNumberedXml,
				MethodParameters = notNumberedIncoming,
				ParameterItemType = parameterType,
				AddParameters = notNumberedAddParams,
				AddCustomParameters = notNumberedAddCustomParams,
				IndexVariableName = indexVariableName,
				ParameterVariableName = parameterVariableName,
				AddParametersNumbered = numberedAddParameters,
				AddCustomParametersNumbered = numberedAddCustomParameters,
				ResultItemType = resultItemType
			});

			QueryObjectData result = QueryObjectData.CreateFrom(data);

			IEnumerable<string> specificParametersUsings = notNumberedSdtUsings
															.Concat(notNumberedCustomUsings)
															.Concat(numberedSdtUsings)
															.Concat(numberedCustomUsings)
															.Distinct();

			result.Methods = result.Methods.AppendItems(method);
			result.Usings = result.Usings.AppendItems(
									"System",
									"System.Data",
									"System.Collections.Generic")
								.Concat(specificParametersUsings);

			return result;
		}

		/// <inheritdoc />
		public override IEnumerable<string> GetDependencies(ICodeGenerationContext context)
		{
			yield return KnownAbilityName.GetQueryTextMultipleInsert;

			if (context.IncomingParameters.Any(p => !p.IsComplexType))
			{
				yield return KnownAbilityName.AddParameter;
			}

			if (context.IncomingParameters.Any(p => p.IsComplexType))
			{
				yield return KnownAbilityName.AddCustomParameter;
			}

			yield return KnownAbilityName.GetItemFromRecord;
		}

		protected virtual IRenderableTemplate GetTemplate() => Snippet.Query.Methods.Add.AddMultipleWithResult;

		[SuppressMessage("ReSharper", "UnusedParameter.Local")]
		private static string GetParameterName(ICodeGenerationContext context)
		{
			return "item";
		}

		private IEnumerable<IRenderable> GetAddParametersNumbered(ICodeGenerationContext context,
			string indexVariableName,
			IEnumerable<IQueryParamInfo> targetParameters,
			out IEnumerable<string> specificUsings)
		{
			specificUsings = Enumerable.Empty<string>();
			string variableName = GetParameterName(context);

			IRenderableTemplate template = Snippet.Query.Methods.Common.Snippets.CallAddParameterNumbered;

			var parameters = new LinkedList<IRenderable>();
			foreach (IQueryParamInfo paramInfo in targetParameters)
			{
				string propertyName = CSharpCodeHelper.GetValidIdentifierName(paramInfo.SemanticName, NamingPolicy.Pascal);
				string fullName = $"{variableName}.{propertyName}";

				IProviderSpecificType dbType = context.TypeMapper.MapToProviderSpecificType(paramInfo.DbType, paramInfo.DbTypeMetadata);
				specificUsings = specificUsings.Concat(dbType.Usings);

				var model = new
				{
					ParameterTypeTypeName = dbType.TypeName,
					SqlType = dbType.ValueName,
					SqlName = QueryParamInfoNameHelper.GetNumberedNameTemplate(paramInfo.SemanticName),
					IndexVariableName = indexVariableName,
					Name = fullName
				};

				parameters.AddLast(Renderable.Create(template, model));
			}

			return parameters;
		}
	}
}