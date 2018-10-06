using System;
using System.Collections.Generic;
using System.Linq;
using SqlFirst.Codegen.Helpers;
using SqlFirst.Codegen.Text.QueryObject.Data;
using SqlFirst.Codegen.Text.Snippets;
using SqlFirst.Codegen.Text.Templating;
using SqlFirst.Core;

namespace SqlFirst.Codegen.Text.QueryObject.Abilities.Insert
{
	internal class InsertSingleValuePlainWithScalarResultAsyncAbility : QueryObjectAbilityBase
	{
		/// <inheritdoc />
		public override string Name { get; } = "InsertSingleValuePlainAsync";

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

			IQueryParamInfo[] parameters = context.IncomingParameters.ToArray();

			IEnumerable<IRenderable> xmlParameters = GetXmlParameters(context, parameters);
			IEnumerable<IRenderable> methodParameters = GetIncomingParameters(context, parameters);

			IEnumerable<IRenderable> addParams = GetAddParameters(context, parameters.Where(p => !p.IsComplexType), out IEnumerable<string> sdtUsings);
			IEnumerable<IRenderable> addCustomParams = GetAddCustomParameters(context, parameters.Where(p => p.IsComplexType), out IEnumerable<string> customUsings);

			string method = Snippet.Query.Methods.Add.AddSingleWithScalarResultAsync.Render(new
			{
				XmlParams = xmlParameters,
				MethodParameters = methodParameters,
				AddParameters = addParams,
				AddCustomParameters = addCustomParams,
				ResultItemType = scalarTypeString,
				ResultItemDescription = scalarTypeDescription
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

			yield return KnownAbilityName.GetScalarFromValue;
		}

		/// <inheritdoc />
		protected override string GetParameterName(IQueryParamInfo paramInfo)
		{
			return CSharpCodeHelper.GetValidIdentifierName(paramInfo.SemanticName, NamingPolicy.CamelCase);
		}
	}
}