using System;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using SqlFirst.Core;

namespace SqlFirst.Providers.MsSqlServer
{
	/// <inheritdoc />
	public class MsSqlServerFieldInfoProvider : IFieldInfoProvider
	{
		[SuppressMessage("ReSharper", "RedundantEmptySwitchSection")]
		private void FillFromColumn(FieldDetails details, string columnName, object value)
		{
			switch (columnName)
			{
				case "ColumnName":
					// sby : ColumnName might be null, in which case it will be created from ordinal.
					if (!string.IsNullOrEmpty(value.Unbox<string>()))
					{
						details.ColumnName = value.Unbox<string>();
					}
					break;

				case "ColumnOrdinal":
					details.ColumnOrdinal = value.Unbox<int>();
					if (string.IsNullOrEmpty(details.ColumnName))
					{
						details.ColumnName = "col" + details.ColumnOrdinal;
					}
					break;

				case "ColumnSize":
					details.ColumnSize = value.Unbox<int>();
					break;

				case "NumericPrecision":
					details.NumericPrecision = value.Unbox<short>();
					break;

				case "NumericScale":
					details.NumericScale = value.Unbox<short>();
					break;

				case "IsUnique":
					details.IsUnique = value.Unbox<bool>();
					break;

				case "BaseColumnName":
					details.BaseColumnName = value.Unbox<string>();
					break;

				case "BaseTableName":
					details.BaseTableName = value.Unbox<string>();
					break;

				case "DataType":
					details.ClrType = value.Unbox<Type>().FullName;
					break;

				case "AllowDBNull":
					details.AllowDbNull = value.Unbox<bool>();
					break;

				case "ProviderType":
					//details.ProviderType = value.Unbox<int>();
					break;

				case "IsIdentity":
					details.IsIdentity = value.Unbox<bool>();
					break;

				case "IsAutoIncrement":
					details.IsAutoIncrement = value.Unbox<bool>();
					break;

				case "IsRowVersion":
					details.IsRowVersion = value.Unbox<bool>();
					break;

				case "IsLong":
					details.IsLong = value.Unbox<bool>();
					break;

				case "IsReadOnly":
					details.IsReadOnly = value.Unbox<bool>();
					break;

				case "ProviderSpecificDataType":
					details.ProviderSpecificDataType = value.Unbox<Type>().FullName;
					break;

				case "DataTypeName":
					details.DbType = value.Unbox<string>();
					break;

				case "UdtAssemblyQualifiedName":
					details.UdtAssemblyQualifiedName = value.Unbox<string>();
					break;

				case "IsColumnSet":
					details.IsColumnSet = value.Unbox<bool>();
					break;

				case "NonVersionedProviderType":
					details.NonVersionedProviderType = value.Unbox<int>();
					break;

				default:
					// do nothing
					break;
			}
		}

		/// <inheritdoc />
		public FieldDetails GetFieldDetails(DataRow fieldMetadata)
		{
			var result = new FieldDetails();
			foreach (DataColumn column in fieldMetadata.Table.Columns)
			{
				if (column == null)
				{
					continue;
				}

				string columnName = column.ColumnName;
				object value = fieldMetadata[column];
				FillFromColumn(result, columnName, value);
			}

			return result;
		}
	}
}