using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using SqlFirst.Codegen.Helpers;
using SqlFirst.Codegen.Text.QueryObject.Data;
using SqlFirst.Codegen.Text.Snippets;
using SqlFirst.Core;

namespace SqlFirst.Codegen.Text.QueryObject.Abilities
{
	internal abstract class QueryObjectAbilityBase : IQueryObjectAbility
	{
		protected virtual string GetParameterName(IQueryParamInfo paramInfo)
		{
			return CSharpCodeHelper.GetValidIdentifierName(paramInfo.DbName, NamingPolicy.CamelCase);
		}

		/// <inheritdoc />
		public abstract IQueryObjectData Apply(ICodeGenerationContext context, IQueryObjectData data);

		/// <inheritdoc />
		public abstract IEnumerable<string> GetDependencies();

		/// <inheritdoc />
		public abstract string Name { get; }

		protected virtual string GetIncomingParameters(ICodeGenerationContext context, IEnumerable<IQueryParamInfo> targetParameters)
		{
			var parameters = new LinkedList<string>();
			foreach (IQueryParamInfo paramInfo in targetParameters)
			{
				string name = GetParameterName(paramInfo);
				Type type = context.TypeMapper.MapToClrType(paramInfo.DbType, true);
				string typeName = CSharpCodeHelper.GetTypeBuiltInName(type);

				string parameter = new StringBuilder(QuerySnippet.Methods.Get.Snippets.MethodParameter)
					.Replace("$Type$", typeName)
					.Replace("$Name$", name)
					.ToString();

				parameters.AddLast(parameter);
			}

			return string.Join(", ", parameters);
		}

		protected virtual string GetXmlParameters(ICodeGenerationContext context, IEnumerable<IQueryParamInfo> targetParameters)
		{
			var parameters = new LinkedList<string>();
			foreach (IQueryParamInfo paramInfo in targetParameters)
			{
				string name = GetParameterName(paramInfo);

				string parameter = new StringBuilder(QuerySnippet.Methods.Get.Snippets.XmlParam)
					.Replace("$Description$", paramInfo.DbName) // todo: description?
					.Replace("$Name$", name)
					.ToString();

				parameters.AddLast(parameter);
			}

			return string.Join(Environment.NewLine, parameters);
		}

		protected virtual string GetAddParameters(ICodeGenerationContext context, IEnumerable<IQueryParamInfo> targetParameters)
		{
			var parameters = new LinkedList<string>();
			foreach (IQueryParamInfo paramInfo in targetParameters)
			{
				string name = GetParameterName(paramInfo);

				// todo: добавить отдельный маппер
				if (!Enum.TryParse(paramInfo.DbType, true, out SqlDbType sqlDbType))
				{
					throw new CodeGenerationException($"Can not map [{paramInfo.DbType}] to SqlDbType.");
				}

				string parameter = new StringBuilder(QuerySnippet.Methods.Get.Snippets.CallAddParameter)
					.Replace("$SqlType$", sqlDbType.ToString("G"))
					.Replace("$SqlName$", paramInfo.DbName)
					.Replace("$Name$", name)
					.ToString();

				parameters.AddLast(parameter);
			}

			return string.Join(Environment.NewLine, parameters);
		}
	}
}