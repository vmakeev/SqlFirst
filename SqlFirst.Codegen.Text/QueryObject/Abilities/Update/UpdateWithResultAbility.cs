﻿using System.Collections.Generic;
using System.Linq;
using SqlFirst.Codegen.Text.QueryObject.Data;
using SqlFirst.Codegen.Text.Snippets;
using SqlFirst.Codegen.Text.Templating;
using SqlFirst.Core;

namespace SqlFirst.Codegen.Text.QueryObject.Abilities.Update
{
	internal class UpdateWithResultAbility : QueryObjectAbilityBase
	{
		/// <inheritdoc />
		public override string Name { get; } = "Update";

		/// <inheritdoc />
		public override IQueryObjectData Apply(ICodeGenerationContext context, IQueryObjectData data)
		{
			IQueryParamInfo[] allParameters = context.IncomingParameters.ToArray();

			string resultItemType = context.GetQueryResultItemTypeName();

			IEnumerable<IRenderable> xmlParams = GetXmlParameters(context, allParameters);
			IEnumerable<IRenderable> incomingParams = GetIncomingParameters(context, allParameters);
			IEnumerable<IRenderable> addParams = GetAddParameters(context, allParameters, out IEnumerable<string> usings);

			string method = GetTemplate().Render(new
			{
				XmlParams = xmlParams,
				MethodParameters = incomingParams,
				AddParameters = addParams,
				ResultItemType = resultItemType
			});

			QueryObjectData result = QueryObjectData.CreateFrom(data);

			result.Methods = result.Methods.AppendItems(method);
			result.Usings = result.Usings.AppendItems(
									"System",
									"System.Data",
									"System.Collections.Generic")
								.Concat(usings);

			return result;
		}

		/// <inheritdoc />
		public override IEnumerable<string> GetDependencies()
		{
			yield return KnownAbilityName.GetQueryText;
			yield return KnownAbilityName.AddParameter;
			yield return KnownAbilityName.GetItemFromRecord;
			yield return KnownAbilityName.PrepareCommand;
		}

		protected virtual IRenderableTemplate GetTemplate() => Snippet.Query.Methods.Update.UpdateWithResult;

	}
}