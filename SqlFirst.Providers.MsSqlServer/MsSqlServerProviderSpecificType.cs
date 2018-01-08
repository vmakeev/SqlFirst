using System.Collections.Generic;
using System.Data;
using SqlFirst.Core;

namespace SqlFirst.Providers.MsSqlServer
{
	/// <inheritdoc />
	internal class MsSqlServerProviderSpecificType : IProviderSpecificType
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Object"></see> class.</summary>
		public MsSqlServerProviderSpecificType(SqlDbType dbType)
		{
			ValueName = dbType.ToString("G");
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Object"></see> class.</summary>
		public MsSqlServerProviderSpecificType(string valueName)
		{
			ValueName = valueName;
		}

		/// <inheritdoc />
		public string TypeName { get; } = typeof(SqlDbType).Name;

		/// <inheritdoc />
		public string ValueName { get; }

		/// <inheritdoc />
		public IEnumerable<string> Usings { get; } = new[] { typeof(SqlDbType).Namespace };
	}
}