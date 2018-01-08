using System.Collections.Generic;
using System.Linq;
using System.Text;
using SqlFirst.Codegen.Text.QueryObject.Data;
using SqlFirst.Codegen.Text.Snippets;
using SqlFirst.Core;

namespace SqlFirst.Codegen.Text.QueryObject.Abilities.Common
{
	internal class AddSqlConnectionParameterAbility : IQueryObjectAbility
	{
		/// <inheritdoc />
		public IQueryObjectData Apply(ICodeGenerationContext context, IQueryObjectData data)
		{
			IProviderTypesInfo info = context.DatabaseProvider.ProviderTypesInfo;

			string parameterTypeTypeName = info.CommandParameterSpecificDbTypePropertyType.Name;
			string parameterTypeName = info.CommandParameterType.Name;
			string parameterSpecificDbTypePropertyName = info.CommandParameterSpecificDbTypePropertyName;

			string method = new StringBuilder(QuerySnippet.Methods.Common.AddParameter)
				.Replace("$ParameterTypeTypeName$", parameterTypeTypeName)
				.Replace("$ParameterTypeName$", parameterTypeName)
				.Replace("$ParameterSpecificDbTypePropertyName$", parameterSpecificDbTypePropertyName)
				.ToString();

			string[] usings = new[] { "System", "System.Data" }.Append(
				info.CommandParameterSpecificDbTypePropertyType.Namespace,
				info.CommandParameterType.Namespace).Distinct().ToArray();

			QueryObjectData result = QueryObjectData.CreateFrom(data);
			result.Methods = result.Methods.Append(method);
			result.Usings = result.Usings.Append(usings);
			return result;
		}

		/// <inheritdoc />
		public IEnumerable<string> GetDependencies() => Enumerable.Empty<string>();

		/// <inheritdoc />
		public string Name { get; } = KnownAbilityName.AddParameter;
	}
}