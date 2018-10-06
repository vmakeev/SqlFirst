using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using SqlFirst.Codegen.Helpers;
using SqlFirst.Codegen.Text.QueryObject.Data;
using SqlFirst.Codegen.Text.Snippets;
using SqlFirst.Codegen.Text.Templating;
using SqlFirst.Core;

namespace SqlFirst.Codegen.Text.QueryObject.Abilities.Common
{
	internal class MapDataRecordToItemAbility : IQueryObjectAbility
	{
		/// <inheritdoc />
		public IQueryObjectData Apply(ICodeGenerationContext context, IQueryObjectData data)
		{
			IRenderable[] mappingItems = context.OutgoingParameters.Select((fieldDetails, index) => GetMapping(context, fieldDetails, index)).ToArray();

			string method = Snippet.Query.Methods.Common.GetItemFromRecord.Render(new
			{
				ItemType = context.GetQueryResultItemTypeName(),
				MapDataRecord = mappingItems
			});

			QueryObjectData result = QueryObjectData.CreateFrom(data);
			result.Methods = result.Methods.AppendItems(method);
			result.Usings = result.Usings.AppendItems(
				"System",
				"System.Data");

			return result;
		}

		/// <inheritdoc />
		public IEnumerable<string> GetDependencies(ICodeGenerationContext context) => Enumerable.Empty<string>();

		/// <inheritdoc />
		public string Name { get; } = KnownAbilityName.GetItemFromRecord;

		private IRenderable GetMapping(ICodeGenerationContext context, IFieldDetails fieldDetails, int index)
		{
			string propertyName = CSharpCodeHelper.GetValidIdentifierName(fieldDetails.ColumnName, NamingPolicy.Pascal);
			Type propertyType = context.TypeMapper.MapToClrType(fieldDetails.DbType, fieldDetails.AllowDbNull);
			string propertyTypeString = CSharpCodeHelper.GetTypeBuiltInName(propertyType);

			IRenderableTemplate template = Snippet.Query.Methods.Common.Snippets.MapField;
			var model = new
			{
				Index = index.ToString(CultureInfo.InvariantCulture),
				Property = propertyName,
				PropertyType = propertyTypeString
			};

			return Renderable.Create(template, model);
		}
	}
}