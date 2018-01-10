using System;
using System.Collections.Generic;
using System.Linq;
using SqlFirst.Codegen.Text.Templating;
using SqlFirst.Codegen.Trees;

namespace SqlFirst.Codegen.Text.Snippets.Items.Types
{
	internal sealed class GeneratedTypeTemplate : IRenderableTemplate<IGeneratedType>
	{
		/// <inheritdoc />
		public string Render(IGeneratedType model)
		{
			return GetGeneratedTypeType(model);
		}

		/// <inheritdoc />
		[Obsolete("Empty model is not supported", true)]
		public string Render()
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		[Obsolete("RAW model is not supported", true)]
		public string Render(object model)
		{
			throw new NotImplementedException();
		}

		private string GetGenericArgumentType(IGenericArgument genericArgument)
		{
			if (genericArgument.IsGeneric)
			{
				string typeName = genericArgument.Type;
				IEnumerable<string> arguments = genericArgument.GenericArguments.Select(GetGenericArgumentType);

				return Snippet.Item.Type.Type.Render(new { TypeName = typeName, TypeArguments = arguments });
			}

			return Snippet.Item.Type.Type.Render(new { TypeName = genericArgument.Type });
		}

		private string GetGeneratedTypeType(IGeneratedType generatedType)
		{
			if (generatedType.IsGeneric)
			{
				string typeName = generatedType.Name;
				IEnumerable<string> arguments = generatedType.GenericArguments.Select(GetGenericArgumentType);

				return Snippet.Item.Type.Type.Render(new { TypeName = typeName, TypeArguments = arguments });
			}

			return Snippet.Item.Type.Type.Render(new { TypeName = generatedType.Name });
		}
	}
}