using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using SqlFirst.Codegen.Helpers;
using SqlFirst.Codegen.Text.QueryObject.Data;
using SqlFirst.Codegen.Text.Snippets;
using SqlFirst.Codegen.Text.Templating;
using SqlFirst.Core;

namespace SqlFirst.Codegen.Text.QueryObject.Abilities.Common
{
	internal class GetDataTableAbility : IQueryObjectAbility
	{
		private readonly string _dbType;
		private readonly IComplexTypeData _complexTypeData;

		/// <inheritdoc />
		public GetDataTableAbility(string dbType, IComplexTypeData complexTypeData)
		{
			if (string.IsNullOrWhiteSpace(value: dbType))
			{
				throw new ArgumentException("dbType cannot be null or whitespace.", nameof(dbType));
			}

			_dbType = dbType;
			_complexTypeData = complexTypeData ?? throw new ArgumentNullException(nameof(complexTypeData));
			if (!complexTypeData.IsTableType)
			{
				throw new ArgumentException("Complex type must be a table type.", nameof(dbType));
			}

			Name = $"Get{_complexTypeData.DbTypeDisplayedName}DataTable";
		}

		/// <inheritdoc />
		public string Name { get; }

		/// <inheritdoc />
		public IQueryObjectData Apply(ICodeGenerationContext context, IQueryObjectData data)
		{
			bool canSimplifyComplexType = _complexTypeData.Fields.Count() == 1;

			string dbTypeDisplayedName = _complexTypeData.DbTypeDisplayedName ?? _dbType;
			string clrTypeName;
			if (canSimplifyComplexType)
			{
				IFieldDetails field = _complexTypeData.Fields.Single();
				Type clrType = context.TypeMapper.MapToClrType(field.DbType, field.AllowDbNull, field.DbTypeMetadata);
				clrTypeName = CSharpCodeHelper.GetTypeBuiltInName(clrType);
			}
			else
			{
				clrTypeName = CSharpCodeHelper.GetValidTypeName(
					name: _complexTypeData.Name ?? _dbType, 
					namingPolicy: NamingPolicy.Pascal, 
					allowBuiltInTypes: false);
			}

			const string dataTableVariableName = "dataTable";
			const string rowVariableName = "dataRow";
			const string valueVariableName = "value";

			IRenderableTemplate getDataTableTemplate = Snippet.Query.Methods.Common.GetDataTable;
			IRenderableTemplate addColumnTemplate = Snippet.Query.Methods.Common.Snippets.AddDataTableColumn;

			IRenderableTemplate addRowTemplate = canSimplifyComplexType
				? Snippet.Query.Methods.Common.Snippets.AddDataTableRowValue
				: Snippet.Query.Methods.Common.Snippets.AddDataTableRowProperty;

			var addColumnModels = new List<object>();
			var addRowModels = new List<object>();

			foreach (IFieldDetails fieldDetails in _complexTypeData.Fields.OrderBy(fieldDetails => fieldDetails.ColumnOrdinal))
			{
				string columnName = fieldDetails.ColumnName;
				Type columnClrType = context.TypeMapper
											.MapToClrType(fieldDetails.DbType, fieldDetails.AllowDbNull, fieldDetails.DbTypeMetadata);

				string columnClrTypeName = CSharpCodeHelper.GetTypeBuiltInName(columnClrType);

				string valuePropertyName = CSharpCodeHelper.GetValidIdentifierName(fieldDetails.ColumnName, NamingPolicy.Pascal);

				var addColumnModel = new
				{
					DataTableVariableName = dataTableVariableName,
					ColumnName = columnName,
					ColumnClrTypeName = columnClrTypeName
				};
				addColumnModels.Add(addColumnModel);

				var addRowModel = new
				{
					RowVariableName = rowVariableName,
					ColumnName = columnName,
					ValueVariableName = valueVariableName,
					ValuePropertyName = valuePropertyName
				};
				addRowModels.Add(addRowModel);
			}

			var getDataTableModel = new
			{
				DbTypeName = CSharpCodeHelper.GetValidTypeName(
					name: dbTypeDisplayedName, 
					namingPolicy: NamingPolicy.Pascal,
					allowBuiltInTypes: false),
				ClrTypeName = clrTypeName,
				DataTableVariableName = dataTableVariableName,
				RowVariableName = rowVariableName,
				ValueVariableName = valueVariableName,
				AddColumns = addColumnModels.Select(model => Renderable.Create(addColumnTemplate, model)),
				AddRows = addRowModels.Select(model => Renderable.Create(addRowTemplate, model)),
				ValueWhenNoInput = _complexTypeData.AllowNull
					? CSharpCodeHelper.GetValidValue(typeof(DataTable), null)
					: dataTableVariableName
			};

			string method = getDataTableTemplate.Render(getDataTableModel);

			QueryObjectData result = QueryObjectData.CreateFrom(data);

			result.Methods = result.Methods.AppendItems(method);
			result.Usings = result.Usings.AppendItems(
				"System",
				"System.Data",
				"System.Collections.Generic");

			return result;
		}

		/// <inheritdoc />
		public IEnumerable<string> GetDependencies(ICodeGenerationContext context) => Enumerable.Empty<string>();
	}
}