using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using SqlFirst.Codegen.Text.QueryObject.Data;
using SqlFirst.Codegen.Text.Snippets;
using SqlFirst.Core;

namespace SqlFirst.Codegen.Text.QueryObject.Abilities.Select
{
	internal abstract class SelectItemsIEnumerableAsyncAbilityBase : QueryObjectAbilityBase
	{
		/// <inheritdoc />
		public override string Name { get; } = "GetItemsIEnumerableAsync";

		[SuppressMessage("ReSharper", "EmptyConstructor")]
		protected SelectItemsIEnumerableAsyncAbilityBase()
		{
		}

		/// <inheritdoc />
		public override IQueryObjectData Apply(ICodeGenerationContext context, IQueryObjectData data)
		{
			IQueryParamInfo[] parameters = context.IncomingParameters.ToArray();

			string xmlParameters = GetXmlParameters(context, parameters);
			string methodParameters = GetIncomingParameters(context, parameters);
			string addParameters = GetAddParameters(context, parameters).Indent(QuerySnippet.Indent, 2);

			string method = new StringBuilder(QuerySnippet.Methods.Get.GetIEnumerableAsync)
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
				"System.Data.Common",
				"System.Threading",
				"System.Threading.Tasks",
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