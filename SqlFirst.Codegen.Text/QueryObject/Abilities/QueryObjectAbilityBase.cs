using System;
using System.Collections.Generic;
using System.Linq;
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

		protected virtual string GetAddParameters(ICodeGenerationContext context, IEnumerable<IQueryParamInfo> targetParameters, out IEnumerable<string> specificUsings)
		{
			specificUsings = Enumerable.Empty<string>();

			var parameters = new LinkedList<string>();
			foreach (IQueryParamInfo paramInfo in targetParameters)
			{
				string name = GetParameterName(paramInfo);

				IProviderSpecificType dbType = context.TypeMapper.MapToProviderSpecificType(paramInfo.DbType);
				specificUsings = specificUsings.Concat(dbType.Usings);

				string parameter = new StringBuilder(QuerySnippet.Methods.Get.Snippets.CallAddParameter)
					.Replace("$ParameterTypeTypeName$", dbType.TypeName)
					.Replace("$SqlType$", dbType.ValueName)
					.Replace("$SqlName$", paramInfo.DbName)
					.Replace("$Name$", name)
					.ToString();

				parameters.AddLast(parameter);
			}

			specificUsings = specificUsings.Distinct().ToArray();
			return string.Join(Environment.NewLine, parameters);
		}
	}
}