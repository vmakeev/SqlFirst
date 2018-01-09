using System.Collections.Generic;
using System.Linq;
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

			string method = Snippet.Query.Methods.Common.AddParameter.Render(new
			{
				ParameterTypeTypeName = parameterTypeTypeName,
				ParameterTypeName = parameterTypeName,
				ParameterSpecificDbTypePropertyName = parameterSpecificDbTypePropertyName
			});

			string[] usings = new[] { "System", "System.Data" }.AppendItems(
				info.CommandParameterSpecificDbTypePropertyType.Namespace,
				info.CommandParameterType.Namespace).Distinct().ToArray();

			QueryObjectData result = QueryObjectData.CreateFrom(data);
			result.Methods = result.Methods.AppendItems(method);
			result.Usings = result.Usings.AppendItems(usings);
			return result;
		}

		/// <inheritdoc />
		public IEnumerable<string> GetDependencies() => Enumerable.Empty<string>();

		/// <inheritdoc />
		public string Name { get; } = KnownAbilityName.AddParameter;
	}
}