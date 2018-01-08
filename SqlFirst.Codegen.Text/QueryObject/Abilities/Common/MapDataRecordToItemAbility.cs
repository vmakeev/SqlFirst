using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using SqlFirst.Codegen.Helpers;
using SqlFirst.Codegen.Text.QueryObject.Data;
using SqlFirst.Codegen.Text.Snippets;
using SqlFirst.Core;

namespace SqlFirst.Codegen.Text.QueryObject.Abilities.Common
{
	internal class MapDataRecordToItemAbility : IQueryObjectAbility
	{
		/// <inheritdoc />
		public IQueryObjectData Apply(ICodeGenerationContext context, IQueryObjectData data)
		{
			var mappingItems = new LinkedList<string>();

			int index = 0;
			foreach (IFieldDetails fieldDetails in context.OutgoingParameters)
			{
				string propertyName = CSharpCodeHelper.GetValidIdentifierName(fieldDetails.ColumnName, NamingPolicy.Pascal);
				Type propertyType = context.TypeMapper.MapToClrType(fieldDetails.DbType, fieldDetails.AllowDbNull);
				string propertyTypeString = CSharpCodeHelper.GetTypeBuiltInName(propertyType);

				string mapRecord = new StringBuilder(QuerySnippet.Methods.Common.Snippets.MapField)
					.Replace("$Index$", index++.ToString(CultureInfo.InvariantCulture))
					.Replace("$Property$", propertyName)
					.Replace("$PropertyType$", propertyTypeString)
					.ToString();

				mappingItems.AddLast(mapRecord);
			}

			string mappingsText = string.Join(Environment.NewLine, mappingItems).Indent(QuerySnippet.Indent, 1);

			string method = new StringBuilder(QuerySnippet.Methods.Common.GetItemFromRecord)
				.Replace("$ItemType$", context.GetQueryResultItemTypeName())
				.Replace("$MapDataRecord$", mappingsText)
				.ToString();

			QueryObjectData result = QueryObjectData.CreateFrom(data);
			result.Methods = result.Methods.Append(method);
			result.Usings = result.Usings.Append(
				"System",
				"System.Data");

			return result;
		}

		/// <inheritdoc />
		public IEnumerable<string> GetDependencies() => Enumerable.Empty<string>();

		/// <inheritdoc />
		public string Name { get; } = KnownAbilityName.GetItemFromRecord;
	}
}