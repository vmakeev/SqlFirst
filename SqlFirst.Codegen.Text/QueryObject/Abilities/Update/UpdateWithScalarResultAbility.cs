using System;
using System.Collections.Generic;
using System.Linq;
using SqlFirst.Codegen.Helpers;
using SqlFirst.Codegen.Text.QueryObject.Data;
using SqlFirst.Codegen.Text.Snippets;
using SqlFirst.Codegen.Text.Templating;
using SqlFirst.Core;

namespace SqlFirst.Codegen.Text.QueryObject.Abilities.Update
{
	internal class UpdateWithScalarResultAbility : QueryObjectAbilityBase
	{
		/// <inheritdoc />
		public override string Name { get; } = "Update";

		/// <inheritdoc />
		public override IQueryObjectData Apply(ICodeGenerationContext context, IQueryObjectData data)
		{
			IFieldDetails firstParameter = context.OutgoingParameters.FirstOrDefault();
			if (firstParameter == null)
			{
				throw new CodeGenerationException($"Query must have at least one outgoing parameter to use ability [{Name}] ({GetType().Name}).");
			}

			Type scalarType = context.TypeMapper.MapToClrType(firstParameter.DbType, firstParameter.AllowDbNull);
			string scalarTypeString = CSharpCodeHelper.GetTypeBuiltInName(scalarType);
			string scalarTypeDescription = firstParameter.ColumnName;

			IQueryParamInfo[] allParameters = context.IncomingParameters.ToArray();

			IEnumerable<IRenderable> xmlParams = GetXmlParameters(context, allParameters);
			IEnumerable<IRenderable> incomingParams = GetIncomingParameters(context, allParameters);
			IEnumerable<IRenderable> addParams = GetAddParameters(context, allParameters, out IEnumerable<string> usings);

			string method = GetTemplate().Render(new
			{
				XmlParams = xmlParams,
				MethodParameters = incomingParams,
				AddParameters = addParams,
				ResultItemType = scalarTypeString,
				ResultItemDescription = scalarTypeDescription
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
			yield return KnownAbilityName.GetScalarFromRecord;
		}

		protected virtual IRenderableTemplate GetTemplate() => Snippet.Query.Methods.Update.UpdateWithScalarResult;

	}
}