﻿using System;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using SqlFirst.Core;

namespace SqlFirst.Providers.Postgres
{
	internal class PostgresFieldInfoProvider : IFieldInfoProvider
	{
		/// <inheritdoc />
		public IFieldDetails GetFieldDetails(DataRow fieldMetadata)
		{
			var result = new PostgresFieldDetails();
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

		[SuppressMessage("ReSharper", "RedundantEmptySwitchSection")]
		private void FillFromColumn(PostgresFieldDetails details, string columnName, object value)
		{
			switch (columnName)
			{
				case "ColumnName":
					if (!string.IsNullOrEmpty(value.Unbox<string>()))
					{
						details.ColumnName = value.Unbox<string>();
					}

					break;

				case "ColumnOrdinal":
					details.ColumnOrdinal = value.Unbox<int>();
					if (string.IsNullOrEmpty(details.ColumnName))
					{
						details.ColumnName = "column_" + details.ColumnOrdinal;
					}

					break;

				case "ColumnSize":
					details.ColumnSize = value.Unbox<int>();
					break;

				case "NumericPrecision":
					details.NumericPrecision = value.Unbox<int>();
					break;

				case "NumericScale":
					details.NumericScale = value.Unbox<int>();
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
					details.ClrType = value.Unbox<Type>();
					break;

				case "AllowDBNull":
					details.AllowDbNull = value.Unbox<bool?>() ?? true; // todo: research needed
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
					details.IsReadOnly = value.Unbox<bool?>() ?? false;
					break;

				case "ProviderSpecificDataType":
					details.ProviderSpecificDataType = value.Unbox<Type>()?.FullName;
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
	}
}