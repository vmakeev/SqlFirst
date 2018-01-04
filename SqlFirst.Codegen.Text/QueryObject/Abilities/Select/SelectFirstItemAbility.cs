using System.Collections.Generic;
using System.Text;
using SqlFirst.Codegen.Text.QueryObject.Data;
using SqlFirst.Codegen.Text.Snippets;

namespace SqlFirst.Codegen.Text.QueryObject.Abilities.Select
{
	internal class SelectFirstItemAbility : QueryObjectAbilityBase
	{
		/// <inheritdoc />
		public override string Name { get; } = "GetFirstItem";

		/// <inheritdoc />
		public override IQueryObjectData Apply(ICodeGenerationContext context, IQueryObjectData data)
		{
			string xmlParameters = GetXmlParameters(context);
			string methodParameters = GetIncomingParameters(context);
			string addParameters = GetAddParameters(context).Indent("\t");

			string method = new StringBuilder(QuerySnippet.Methods.Get.GetFirst)
				.Replace("$ItemType$", context.GetQueryResultItemName())
				.Replace("$XmlParams$", "$XmlParams$").Replace("$XmlParams$", xmlParameters)
				.Replace("$MethodParameters$", string.IsNullOrEmpty(methodParameters) ? string.Empty : ", " + methodParameters)
				.Replace("$AddParameters$", "$AddParameters$").Replace("$AddParameters$", addParameters)
				.ToString();

			QueryObjectData result = QueryObjectData.CreateFrom(data);

			result.Methods = result.Methods.Append(method);
			result.Usings = result.Usings.Append(
				"System",
				"System.Data");

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