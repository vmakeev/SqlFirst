using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using SqlFirst.Codegen.Helpers;
using SqlFirst.Codegen.Text.QueryObject.Data;
using SqlFirst.Codegen.Text.Snippets;
using SqlFirst.Core;
using SqlFirst.Core.Impl;

namespace SqlFirst.Codegen.Text.QueryObject.Abilities.Insert
{
	internal class InsertMultipleValuesWithResultAbility : QueryObjectAbilityBase
	{
		[SuppressMessage("ReSharper", "UnusedParameter.Local")]
		private static string GetParameterName(ICodeGenerationContext context)
		{
			return "item";
		}

		protected virtual string GetTemplate() => QuerySnippet.Methods.Add.AddMultipleWithResult;

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
			string notNumberedXmlString = GetXmlParameters(context, notNumberedParameters);
			string notNumberedIncomingString = GetIncomingParameters(context, notNumberedParameters);
			string notNumberedAddParametersString = GetAddParameters(context, notNumberedParameters, out IEnumerable<string> notNumberedUsings).Indent(QuerySnippet.Indent, 2);
			string numberedAddParametersString = GetAddParametersNumbered(context, indexVariableName, numberedParameters, out IEnumerable<string> numberedUsings).Indent(QuerySnippet.Indent, 3);
			
			if (!string.IsNullOrEmpty(notNumberedXmlString))
			{
				notNumberedXmlString = notNumberedXmlString + Environment.NewLine;
			}

			if (!string.IsNullOrEmpty(notNumberedAddParametersString))
			{
				notNumberedAddParametersString = notNumberedAddParametersString + Environment.NewLine;
			}

			string method = new StringBuilder(GetTemplate())
							.Replace("$XmlParams$", notNumberedXmlString)
							.Replace("$MethodParameters$", string.IsNullOrEmpty(notNumberedIncomingString) ? string.Empty : ", " + notNumberedIncomingString)
							.Replace("$ParameterItemType$", parameterType)
							.Replace("$AddParameters$", notNumberedAddParametersString)
							.Replace("$IndexVariableName$", indexVariableName)
							.Replace("$ParameterVariableName$", parameterVariableName)
							.Replace("$AddParametersNumbered$", numberedAddParametersString)
							.Replace("$ResultItemType$", resultItemType)
							.ToString();

			QueryObjectData result = QueryObjectData.CreateFrom(data);

			IEnumerable<string> specificParametersUsings = numberedUsings.Concat(notNumberedUsings).Distinct();
			
			result.Methods = result.Methods.Append(method);
			result.Usings = result.Usings.Append(
				"System",
				"System.Data",
				"System.Collections.Generic")
				.Concat(specificParametersUsings);

			return result;
		}

		private string GetAddParametersNumbered(ICodeGenerationContext context, string indexVariableName, IEnumerable<IQueryParamInfo> targetParameters, out IEnumerable<string> specificUsings)
		{
			specificUsings = Enumerable.Empty<string>();
			string variableName = GetParameterName(context);

			var parameters = new LinkedList<string>();
			foreach (IQueryParamInfo paramInfo in targetParameters)
			{
				string propertyName = CSharpCodeHelper.GetValidIdentifierName(paramInfo.SemanticName, NamingPolicy.Pascal);
				string fullName = $"{variableName}.{propertyName}";

				IProviderSpecificType dbType = context.TypeMapper.MapToProviderSpecificType(paramInfo.DbType);
				specificUsings = specificUsings.Concat(dbType.Usings);

				string parameter = new StringBuilder(QuerySnippet.Methods.Get.Snippets.CallAddParameterNumbered)
									.Replace("$ParameterTypeTypeName$", dbType.TypeName)
									.Replace("$SqlType$", dbType.ValueName)
									.Replace("$SqlName$", QueryParamInfoNameHelper.GetNumberedNameTemplate(paramInfo.SemanticName))
									.Replace("$IndexVariableName$", indexVariableName)
									.Replace("$Name$", fullName)
									.ToString();

				parameters.AddLast(parameter);
			}

			return string.Join(Environment.NewLine, parameters);
		}

		/// <inheritdoc />
		public override IEnumerable<string> GetDependencies()
		{
			yield return KnownAbilityName.GetQueryTextMultipleInsert;
			yield return KnownAbilityName.AddParameter;
			yield return KnownAbilityName.GetItemFromRecord;
		}

		/// <inheritdoc />
		public override string Name { get; } = "InsertMultipleValues";
	}
}