using System.Collections.Generic;
using System.Linq;
using System.Text;
using SqlFirst.Codegen.Helpers;
using SqlFirst.Codegen.Text.QueryObject.Data;
using SqlFirst.Codegen.Text.Snippets;
using SqlFirst.Core;

namespace SqlFirst.Codegen.Text.QueryObject.Abilities.Insert
{
	internal class InsertSingleValuePlainAsyncAbility : QueryObjectAbilityBase
	{
		/// <inheritdoc />
		protected override string GetParameterName(IQueryParamInfo paramInfo)
		{
			return CSharpCodeHelper.GetValidIdentifierName(paramInfo.SemanticName, NamingPolicy.CamelCase);
		}

		/// <inheritdoc />
		public override IQueryObjectData Apply(ICodeGenerationContext context, IQueryObjectData data)
		{
			IQueryParamInfo[] parameters = context.IncomingParameters.ToArray();

			string xmlParameters = GetXmlParameters(context, parameters);
			string methodParameters = GetIncomingParameters(context, parameters);
			string addParameters = GetAddParameters(context, parameters).Indent(QuerySnippet.Indent, 2);

			string intType = CSharpCodeHelper.GetTypeBuiltInName(typeof(int));

			string method = new StringBuilder(QuerySnippet.Methods.Add.AddSingleAsync)
							.Replace("$ItemType$", intType)
							.Replace("$XmlParams$", "$XmlParams$").Replace("$XmlParams$", xmlParameters)
							.Replace("$MethodParameters$", string.IsNullOrEmpty(methodParameters) ? string.Empty : ", " + methodParameters)
							.Replace("$AddParameters$", "$AddParameters$").Replace("$AddParameters$", addParameters)
							.ToString();

			QueryObjectData result = QueryObjectData.CreateFrom(data);

			result.Methods = result.Methods.Append(method);
			result.Usings = result.Usings.Append(
				"System",
				"System.Data",
				"System.Data.Common",
				"System.Threading",
				"System.Threading.Tasks");

			return result;
		}

		/// <inheritdoc />
		public override IEnumerable<string> GetDependencies()
		{
			yield return KnownAbilityName.GetQueryText;
			yield return KnownAbilityName.AddParameter;
		}

		/// <inheritdoc />
		public override string Name { get; } = "InsertSingleValuePlainAsync";
	}
}