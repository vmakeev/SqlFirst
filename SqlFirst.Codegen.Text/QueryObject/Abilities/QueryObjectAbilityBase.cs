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
		/// <inheritdoc />
		public abstract IQueryObjectData Apply(ICodeGenerationContext context, IQueryObjectData data);

		/// <inheritdoc />
		public abstract IEnumerable<string> GetDependencies();

		/// <inheritdoc />
		public abstract string Name { get; }

		protected string GetIncomingParameters(ICodeGenerationContext context)
		{
			var parameters = new LinkedList<string>();
			foreach (IQueryParamInfo paramInfo in context.IncomingParameters)
			{
				string name = CSharpCodeHelper.GetValidIdentifierName(paramInfo.DbName, NamingPolicy.CamelCase);
				Type type = context.TypeMapper.Map(paramInfo.DbType, true);
				string typeName = CSharpCodeHelper.GetTypeBuiltInName(type);

				string parameter = new StringBuilder(QuerySnippet.Methods.Get.Snippets.MethodParameter)
					.Replace("$Type$", typeName)
					.Replace("$Name$", name)
					.ToString();

				parameters.AddLast(parameter);
			}

			return string.Join(", ", parameters);
		}

		protected string GetXmlParameters(ICodeGenerationContext context)
		{
			var parameters = new LinkedList<string>();
			foreach (IQueryParamInfo paramInfo in context.IncomingParameters)
			{
				string name = CSharpCodeHelper.GetValidIdentifierName(paramInfo.DbName, NamingPolicy.CamelCase);

				string parameter = new StringBuilder(QuerySnippet.Methods.Get.Snippets.XmlParam)
					.Replace("$Description$", paramInfo.DbName) // todo: description?
					.Replace("$Name$", name)
					.ToString();

				parameters.AddLast(parameter);
			}

			return string.Join(Environment.NewLine, parameters);
		}

		protected string GetAddParameters(ICodeGenerationContext context)
		{
			var parameters = new LinkedList<string>();
			foreach (IQueryParamInfo paramInfo in context.IncomingParameters)
			{
				string name = CSharpCodeHelper.GetValidIdentifierName(paramInfo.DbName, NamingPolicy.CamelCase);

				// todo: добавить отдельный маппер
				if (!Enum.TryParse(paramInfo.DbType, true, out SqlDbType sqlDbType))
				{
					throw new CodeGenerationException($"Can not map [{paramInfo.DbType}] to SqlDbType.");
				}

				string parameter = new StringBuilder(QuerySnippet.Methods.Get.Snippets.AddParameter)
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