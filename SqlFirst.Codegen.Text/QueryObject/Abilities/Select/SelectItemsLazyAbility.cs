using System.Collections.Generic;
using System.Linq;
using System.Text;
using SqlFirst.Codegen.Text.QueryObject.Data;
using SqlFirst.Codegen.Text.Snippets;
using SqlFirst.Core;

namespace SqlFirst.Codegen.Text.QueryObject.Abilities.Select
{
	internal class SelectItemsLazyAbility : QueryObjectAbilityBase
	{
		/// <inheritdoc />
		public override string Name { get; } = "GetItemsLazy";

		/// <inheritdoc />
		public override IQueryObjectData Apply(ICodeGenerationContext context, IQueryObjectData data)
		{
			IQueryParamInfo[] parameters = context.IncomingParameters.ToArray();

			string xmlParameters = GetXmlParameters(context, parameters);
			string methodParameters = GetIncomingParameters(context, parameters);
			string addParameters = GetAddParameters(context, parameters).Indent(QuerySnippet.Indent, 2);

			string method = new StringBuilder(QuerySnippet.Methods.Get.GetIEnumerable)
				.Replace("$ItemType$", context.GetQueryResultItemTypeName())
				.Replace("$XmlParams$", xmlParameters)
				.Replace("$MethodParameters$", string.IsNullOrEmpty(methodParameters) ? string.Empty : ", " + methodParameters)
				.Replace("$AddParameters$", addParameters)
				.ToString();

			QueryObjectData result = QueryObjectData.CreateFrom(data);

			result.Methods = result.Methods.Append(method);
			result.Usings = result.Usings.Append(
				"System", 
				"System.Data", 
				"System.Collections.Generic");

			return result;
		}

		/// <inheritdoc />
		public override IEnumerable<string> GetDependencies()
		{
			yield return KnownAbilityName.GetQueryText;
			yield return KnownAbilityName.AddParameter;
			yield return KnownAbilityName.GetItemFromRecord;
		}
	}
}