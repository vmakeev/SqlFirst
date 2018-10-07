using System;
using System.Collections.Generic;

namespace SqlFirst.Core.Impl
{
	/// <inheritdoc />
	public class ComplexTypeData : IComplexTypeData
	{
		/// <inheritdoc />
		public ComplexTypeData(string name, string dbTypeDisplayedName, bool isTableType, bool allowNull, IEnumerable<IFieldDetails> fields)
		{
			if (string.IsNullOrWhiteSpace(name))
			{
				throw new ArgumentException("Name can not be empty.", nameof(name));
			}

			if (string.IsNullOrWhiteSpace(dbTypeDisplayedName))
			{
				throw new ArgumentException("DbTypeDisplayedName can not be empty.", nameof(name));
			}

			Name = name;
			DbTypeDisplayedName = dbTypeDisplayedName;
			Fields = fields ?? throw new ArgumentNullException(nameof(fields));
			AllowNull = allowNull;
			IsTableType = isTableType;
		}

		/// <inheritdoc />
		public string Name { get; }

		/// <inheritdoc />
		public bool IsTableType { get; }

		/// <inheritdoc />
		public bool AllowNull { get; }

		/// <inheritdoc />
		public string DbTypeDisplayedName { get; }

		/// <inheritdoc />
		public IEnumerable<IFieldDetails> Fields { get; }
	}
}