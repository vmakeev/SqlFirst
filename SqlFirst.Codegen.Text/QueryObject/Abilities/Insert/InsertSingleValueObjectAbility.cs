using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using SqlFirst.Codegen.Helpers;
using SqlFirst.Codegen.Text.Snippets;
using SqlFirst.Core;

namespace SqlFirst.Codegen.Text.QueryObject.Abilities.Insert
{
	internal class InsertSingleValueObjectAbility : InsertSingleValuePlainAbility
	{
		[SuppressMessage("ReSharper", "UnusedParameter.Local")]
		private static string GetParameterObjectName(ICodeGenerationContext context)
		{
			return "item";
		}

		/// <inheritdoc />
		protected override string GetIncomingParameters(ICodeGenerationContext context, IEnumerable<IQueryParamInfo> targetParameters)
		{
			string typeName = context.GetQueryParameterItemTypeName();
			string name = GetParameterObjectName(context);

			string parameter = new StringBuilder(QuerySnippet.Methods.Get.Snippets.MethodParameter)
								.Replace("$Type$", typeName)
								.Replace("$Name$", name)
								.ToString();

			return parameter;
		}

		/// <inheritdoc />
		protected override string GetXmlParameters(ICodeGenerationContext context, IEnumerable<IQueryParamInfo> targetParameters)
		{
			string description = "Объект, содержащий добавляемые данные";
			string name = GetParameterObjectName(context);

			string parameter = new StringBuilder(QuerySnippet.Methods.Get.Snippets.XmlParam)
								.Replace("$Description$", description)
								.Replace("$Name$", name)
								.ToString();

			return parameter;
		}

		/// <inheritdoc />
		protected override string GetAddParameters(ICodeGenerationContext context, IEnumerable<IQueryParamInfo> targetParameters)
		{
			string variableName = GetParameterObjectName(context);

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

				string parameter = new StringBuilder(QuerySnippet.Methods.Get.Snippets.CallAddParameter)
									.Replace("$SqlType$", sqlDbType.ToString("G"))
									.Replace("$SqlName$", paramInfo.DbName)
									.Replace("$Name$", fullName)
									.ToString();

				parameters.AddLast(parameter);
			}

			return string.Join(Environment.NewLine, parameters);
		}

		/// <inheritdoc />
		public override string Name { get; } = "InsertSingleValueObject";
	}
}