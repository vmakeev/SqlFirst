using System;
using System.Collections.Generic;
using System.Linq;
using SqlFirst.Codegen.Helpers;
using SqlFirst.Codegen.Text.QueryObject.Data;
using SqlFirst.Codegen.Text.Snippets;
using SqlFirst.Codegen.Text.Templating;
using SqlFirst.Core;

namespace SqlFirst.Codegen.Text.QueryObject.Abilities
{
	internal abstract class QueryObjectAbilityBase : IQueryObjectAbility
	{
		/// <inheritdoc />
		public abstract IQueryObjectData Apply(ICodeGenerationContext context, IQueryObjectData data);

		/// <inheritdoc />
		public abstract IEnumerable<string> GetDependencies(ICodeGenerationContext context);

		/// <inheritdoc />
		public abstract string Name { get; }

		protected virtual string GetParameterName(IQueryParamInfo paramInfo)
		{
			return CSharpCodeHelper.GetValidIdentifierName(paramInfo.DbName, NamingPolicy.CamelCase);
		}

		protected virtual IEnumerable<IRenderable> GetIncomingParameters(ICodeGenerationContext context, IEnumerable<IQueryParamInfo> targetParameters)
		{
			foreach (IQueryParamInfo paramInfo in targetParameters)
			{
				string name = GetParameterName(paramInfo);
				Type type = context.TypeMapper.MapToClrType(paramInfo.DbType, true, paramInfo.DbTypeMetadata);
				string typeName = CSharpCodeHelper.GetTypeBuiltInName(type);

				IRenderableTemplate template = Snippet.Query.Methods.Common.Snippets.MethodParameter;
				var model = new
				{
					Type = typeName,
					Name = name
				};

				yield return Renderable.Create(template, model);
			}
		}

		protected virtual IEnumerable<IRenderable> GetXmlParameters(ICodeGenerationContext context, IEnumerable<IQueryParamInfo> targetParameters)
		{
			foreach (IQueryParamInfo paramInfo in targetParameters)
			{
				string name = GetParameterName(paramInfo);

				IRenderableTemplate template = Snippet.Query.Methods.Common.Snippets.XmlParam;
				var model = new
				{
					Description = paramInfo.DbName, // todo: description?,
					Name = name
				};

				yield return Renderable.Create(template, model);
			}
		}

		protected virtual IEnumerable<IRenderable> GetAddParameters(ICodeGenerationContext context, IEnumerable<IQueryParamInfo> targetParameters, out IEnumerable<string> specificUsings)
		{
			specificUsings = Enumerable.Empty<string>();

			var results = new LinkedList<IRenderable>();
			foreach (IQueryParamInfo paramInfo in targetParameters)
			{
				string name = GetParameterName(paramInfo);

				IProviderSpecificType dbType = context.TypeMapper.MapToProviderSpecificType(paramInfo.DbType, paramInfo.DbTypeMetadata);
				specificUsings = specificUsings.Concat(dbType.Usings);

				IRenderableTemplate template = Snippet.Query.Methods.Common.Snippets.CallAddParameter;
				var model = new
				{
					ParameterTypeTypeName = dbType.TypeName,
					SqlType = dbType.ValueName,
					SqlName = paramInfo.DbName,
					Name = name
				};

				results.AddLast(Renderable.Create(template, model));
			}

			specificUsings = specificUsings.Distinct().ToArray();
			return results;
		}

		protected virtual IEnumerable<IRenderable> GetAddCustomParameters(ICodeGenerationContext context, IEnumerable<IQueryParamInfo> targetParameters, out IEnumerable<string> specificUsings)
		{
			specificUsings = Enumerable.Empty<string>();

			var results = new LinkedList<IRenderable>();
			foreach (IQueryParamInfo paramInfo in targetParameters)
			{
				string name = GetParameterName(paramInfo);

				IRenderableTemplate template = Snippet.Query.Methods.Common.Snippets.CallAddCustomParameter;
				var model = new
				{
					SqlType = paramInfo.DbType,
					SqlName = paramInfo.DbName,
					Name = name
				};

				results.AddLast(Renderable.Create(template, model));
			}

			specificUsings = specificUsings.Distinct().ToArray();
			return results;
		}
	}
}