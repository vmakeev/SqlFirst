﻿using System.Collections.Generic;
using System.Linq;
using SqlFirst.Codegen.Text.QueryObject.Data;
using SqlFirst.Codegen.Text.Snippets;
using SqlFirst.Codegen.Text.Templating;
using SqlFirst.Core;

namespace SqlFirst.Codegen.Text.QueryObject.Abilities.StoredProcedure.SmartTableParameters
{
	internal class StoredProcedureSmartTableAbility : QueryObjectAbilityBase
	{
		protected override IEnumerable<IRenderable> GetAddCustomParameters(ICodeGenerationContext context, IEnumerable<IQueryParamInfo> targetParameters, out IEnumerable<string> specificUsings)
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

		/// <inheritdoc />
		public override string Name { get; } = "StoredProcedure";

		/// <inheritdoc />
		public override IQueryObjectData Apply(ICodeGenerationContext context, IQueryObjectData data)
		{
			IQueryParamInfo[] allParameters = context.IncomingParameters.ToArray();

			IEnumerable<IRenderable> xmlParams = GetXmlParameters(context, allParameters);
			IEnumerable<IRenderable> incomingParams = GetIncomingParameters(context, allParameters);

			IEnumerable<IRenderable> addParams = GetAddParameters(context, allParameters.Where(p => !p.IsComplexType), out IEnumerable<string> sdtUsings);
			IEnumerable<IRenderable> addCustomParams = GetAddCustomParameters(context, allParameters.Where(p => p.IsComplexType), out IEnumerable<string> customUsings);

			string method = GetTemplate().Render(new
			{
				XmlParams = xmlParams,
				MethodParameters = incomingParams,
				AddParameters = addParams,
				AddCustomParameters = addCustomParams
			});

			QueryObjectData result = QueryObjectData.CreateFrom(data);


			result.Methods = result.Methods.AppendItems(method);
			result.Usings = result.Usings.AppendItems(
									"System",
									"System.Data")
								.Concat(sdtUsings)
								.Concat(customUsings);

			return result;
		}

		/// <inheritdoc />
		public override IEnumerable<string> GetDependencies(ICodeGenerationContext context)
		{
			if (context.IncomingParameters.Any(p => !p.IsComplexType))
			{
				yield return KnownAbilityName.AddParameter;
			}

			if (context.IncomingParameters.Any(p => p.IsComplexType))
			{
				yield return KnownAbilityName.AddCustomParameter;
			}

			yield return KnownAbilityName.GetQueryText;
		}

		protected virtual IRenderableTemplate GetTemplate() => Snippet.Query.Methods.StoredProcedure.StoredProcedure;

	}
}