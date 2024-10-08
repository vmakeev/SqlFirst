﻿using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using SqlFirst.Codegen.Text.QueryObject.Data;
using SqlFirst.Codegen.Text.Snippets;
using SqlFirst.Codegen.Text.Templating;
using SqlFirst.Core;

namespace SqlFirst.Codegen.Text.QueryObject.Abilities.Select
{
	internal class SelectItemsIEnumerableAsyncAbility : QueryObjectAbilityBase
	{
		/// <inheritdoc />
		public override string Name { get; } = "GetItemsIEnumerableAsync";

		/// <inheritdoc />
		public override IQueryObjectData Apply(ICodeGenerationContext context, IQueryObjectData data)
		{
			IQueryParamInfo[] parameters = context.IncomingParameters.ToArray();

			IEnumerable<IRenderable> xmlParameters = GetXmlParameters(context, parameters);
			IEnumerable<IRenderable> methodParameters = GetIncomingParameters(context, parameters);
			IEnumerable<IRenderable> addParameters = GetAddParameters(context, parameters, out IEnumerable<string> parameterSpecificUsings);

			string method = Snippet.Query.Methods.Get.GetIEnumerableAsync.Render(new
			{
				ItemType = context.GetQueryResultItemTypeName(),
				XmlParams = xmlParameters,
				MethodParameters = methodParameters,
				AddParameters = addParameters
			});

			QueryObjectData result = QueryObjectData.CreateFrom(data);

			result.Methods = result.Methods.AppendItems(method);
			result.Usings = result.Usings.AppendItems(
									"System",
									"System.Data",
									"System.Data.Common",
									"System.Threading",
									"System.Threading.Tasks",
									"System.Collections.Generic")
								.Concat(parameterSpecificUsings);

			return result;
		}

		/// <inheritdoc />
		public override IEnumerable<string> GetDependencies()
		{
			yield return KnownAbilityName.GetQueryText;
			yield return KnownAbilityName.AddParameter;
			yield return KnownAbilityName.GetItemFromRecord;
			yield return KnownAbilityName.PrepareCommand;
			yield return KnownAbilityName.AsyncEnumerable;
		}
	}
}