using System;
using System.Collections.Generic;
using System.Data;
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
	internal class InsertMultipleValuesWithScalarResultAbility : QueryObjectAbilityBase
	{
		[SuppressMessage("ReSharper", "UnusedParameter.Local")]
		private static string GetParameterName(ICodeGenerationContext context)
		{
			return "item";
		}

		protected virtual string GetTemplate() => QuerySnippet.Methods.Add.AddMultipleWithScalarResult;

		/// <inheritdoc />
		public override IQueryObjectData Apply(ICodeGenerationContext context, IQueryObjectData data)
		{
			IFieldDetails firstParameter = context.OutgoingParameters.FirstOrDefault();
			if (firstParameter == null)
			{
				throw new CodeGenerationException($"Query must have at least one outgoing parameter to use ability [{Name}] ({GetType().Name}).");
			}

			Type scalarType = context.TypeMapper.MapToClrType(firstParameter.DbType, firstParameter.AllowDbNull);
			string scalarTypeString = CSharpCodeHelper.GetTypeBuiltInName(scalarType);
			string scalarTypeDescription = firstParameter.ColumnName;

			IQueryParamInfo[] allParameters = context.IncomingParameters.ToArray();
			IQueryParamInfo[] numberedParameters = allParameters.Where(paramInfo => paramInfo.IsNumbered).ToArray();
			IQueryParamInfo[] notNumberedParameters = allParameters.Where(paramInfo => !paramInfo.IsNumbered).ToArray();

			const string indexVariableName = "index";
			const string parameterVariableName = "item";
			string parameterType = context.GetQueryParameterItemTypeName();
			string notNumberedXmlString = GetXmlParameters(context, notNumberedParameters);
			string notNumberedIncomingString = GetIncomingParameters(context, notNumberedParameters);
			string notNumberedAddParametersString = GetAddParameters(context, notNumberedParameters).Indent(QuerySnippet.Indent, 2);
			string numberedAddParametersString = GetAddParametersNumbered(context, indexVariableName, numberedParameters).Indent(QuerySnippet.Indent, 3);
			
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
							.Replace("$ResultItemType$", scalarTypeString)
							.Replace("$ResultItemDescription$", scalarTypeDescription)
							.ToString();

			QueryObjectData result = QueryObjectData.CreateFrom(data);

			result.Methods = result.Methods.Append(method);
			result.Usings = result.Usings.Append(
				"System",
				"System.Data",
				"System.Collections.Generic");

			return result;
		}

		private string GetAddParametersNumbered(ICodeGenerationContext context, string indexVariableName, IEnumerable<IQueryParamInfo> targetParameters)
		{
			string variableName = GetParameterName(context);

			var parameters = new LinkedList<string>();
			foreach (IQueryParamInfo paramInfo in targetParameters)
			{
				string propertyName = CSharpCodeHelper.GetValidIdentifierName(paramInfo.SemanticName, NamingPolicy.Pascal);
				string fullName = $"{variableName}.{propertyName}";

				// todo: добавить отдельный маппер
				if (!Enum.TryParse(paramInfo.DbType, true, out SqlDbType sqlDbType))
				{
					throw new CodeGenerationException($"Can not map [{paramInfo.DbType}] to SqlDbType.");
				}

				string parameter = new StringBuilder(QuerySnippet.Methods.Get.Snippets.CallAddParameterNumbered)
									.Replace("$SqlType$", sqlDbType.ToString("G"))
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
			yield return KnownAbilityName.GetScalarFromRecord;
		}

		/// <inheritdoc />
		public override string Name { get; } = "InsertMultipleValues";
	}
}