using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using SqlFirst.Codegen.Helpers;
using SqlFirst.Codegen.Text.QueryObject.Data;
using SqlFirst.Codegen.Text.Snippets;
using SqlFirst.Codegen.Text.Templating;
using SqlFirst.Core;

namespace SqlFirst.Codegen.Text.QueryObject.Abilities.Select
{
	internal abstract class SelectScalarsIEnumerableAsyncAbilityBase : QueryObjectAbilityBase
	{
		/// <inheritdoc />
		public override string Name { get; } = "GetScalarsIEnumerableAsync";

		[SuppressMessage("ReSharper", "EmptyConstructor")]
		protected SelectScalarsIEnumerableAsyncAbilityBase()
		{
		}

		/// <inheritdoc />
		public override IQueryObjectData Apply(ICodeGenerationContext context, IQueryObjectData data)
		{
			IQueryParamInfo[] parameters = context.IncomingParameters.ToArray();

			IEnumerable<IRenderable> xmlParameters = GetXmlParameters(context, parameters);
			IEnumerable<IRenderable> methodParameters = GetIncomingParameters(context, parameters);

			IEnumerable<IRenderable> addParams = GetAddParameters(context, parameters.Where(p => !p.IsComplexType), out IEnumerable<string> sdtUsings);
			IEnumerable<IRenderable> addCustomParams = GetAddCustomParameters(context, parameters.Where(p => p.IsComplexType), out IEnumerable<string> customUsings);

			IFieldDetails firstParameter = context.OutgoingParameters.FirstOrDefault();
			if (firstParameter == null)
			{
				throw new CodeGenerationException($"Query must have at least one outgoing parameter to use ability [{Name}] ({GetType().Name}).");
			}

			Type scalarType = context.TypeMapper.MapToClrType(firstParameter.DbType, firstParameter.AllowDbNull);
			string scalarTypeString = CSharpCodeHelper.GetTypeBuiltInName(scalarType);

			string method = Snippet.Query.Methods.Get.GetScalarsAsync.Render(new
			{
				ItemType = scalarTypeString,
				XmlParams = xmlParameters,
				MethodParameters = methodParameters,
				AddParameters = addParams,
				AddCustomParameters = addCustomParams
			});

			QueryObjectData result = QueryObjectData.CreateFrom(data);

			result.Methods = result.Methods.AppendItems(method);
			result.Usings = result.Usings.AppendItems(
									"System",
									"System.Collections.Generic",
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

			yield return KnownAbilityName.GetScalarFromRecord;
		}
	}
}