using System.Collections.Generic;
using NpgsqlTypes;
using SqlFirst.Core;

namespace SqlFirst.Providers.Postgres
{
	/// <inheritdoc />
	internal class NpgsqlProviderSpecificType : IProviderSpecificType
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Object"></see> class.</summary>
		public NpgsqlProviderSpecificType(NpgsqlDbType dbType)
		{
			ValueName = dbType.TryGetArrayItemType(out var arrayItemType) 
				? $"{arrayItemType:G} | NpgsqlDbType.Array" 
				: dbType.ToString("G");
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Object"></see> class.</summary>
		public NpgsqlProviderSpecificType(string valueName)
		{
			ValueName = valueName;
		}

		/// <inheritdoc />
		public string TypeName => typeof(NpgsqlDbType).Name;

		/// <inheritdoc />
		public string ValueName { get; }

		/// <inheritdoc />
		public IEnumerable<string> Usings { get; } = new[] { typeof(NpgsqlDbType).Namespace };
	}
}