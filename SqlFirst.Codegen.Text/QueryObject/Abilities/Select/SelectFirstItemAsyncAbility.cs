﻿using System.Collections.Generic;
using System.Linq;
using SqlFirst.Codegen.Text.QueryObject.Data;
using SqlFirst.Codegen.Text.Snippets;
using SqlFirst.Codegen.Text.Templating;
using SqlFirst.Core;

namespace SqlFirst.Codegen.Text.QueryObject.Abilities.Select
{
	internal class SelectFirstItemAsyncAbility : QueryObjectAbilityBase
	{
		/// <inheritdoc />
		public override string Name { get; } = "GetFirstItemAsync";

		/// <inheritdoc />
		public override IQueryObjectData Apply(ICodeGenerationContext context, IQueryObjectData data)
		{
			IQueryParamInfo[] parameters = context.IncomingParameters.ToArray();

			IEnumerable<IRenderable> xmlParameters = GetXmlParameters(context, parameters);
			IEnumerable<IRenderable> methodParameters = GetIncomingParameters(context, parameters);

			IEnumerable<IRenderable> addParams = GetAddParameters(context, parameters.Where(p => !p.IsComplexType), out IEnumerable<string> sdtUsings);
			IEnumerable<IRenderable> addCustomParams = GetAddCustomParameters(context, parameters.Where(p => p.IsComplexType), out IEnumerable<string> customUsings);

			string method = Snippet.Query.Methods.Get.GetFirstAsync.Render(new
			{
				ItemType = context.GetQueryResultItemTypeName(),
				XmlParams = xmlParameters,
				MethodParameters = methodParameters,
				AddParameters = addParams,
				AddCustomParameters = addCustomParams
			});

			QueryObjectData result = QueryObjectData.CreateFrom(data);

			result.Methods = result.Methods.AppendItems(method);
			result.Usings = result.Usings.AppendItems(
									"System",
									"System.Data",
									"System.Data.Common",
									"System.Threading",
									"System.Threading.Tasks")
								.Concat(sdtUsings)
								.Concat(customUsings);

			return result;
		}

		/// <inheritdoc />
		public override IEnumerable<string> GetDependencies(ICodeGenerationContext context)
		{
			yield return KnownAbilityName.GetQueryText;

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
	}
}