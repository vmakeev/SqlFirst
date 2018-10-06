using System;
using Npgsql;
using NpgsqlTypes;
using SqlFirst.Core;

namespace SqlFirst.Providers.Postgres
{
	/// <inheritdoc />
	internal class PostgresProviderTypesInfo : IProviderTypesInfo
	{
		public static PostgresProviderTypesInfo Instance { get; } = new PostgresProviderTypesInfo();

		private PostgresProviderTypesInfo()
		{
		}

		/// <inheritdoc />
		public Type ConnectionType { get; } = typeof(NpgsqlConnection);

		/// <inheritdoc />
		public Type CommandType { get; } = typeof(NpgsqlCommand);

		/// <inheritdoc />
		public Type CommandParameterType { get; } = typeof(NpgsqlParameter);

		/// <inheritdoc />
		public Type CommandParameterSpecificDbTypePropertyType { get; } = typeof(NpgsqlDbType);

		/// <inheritdoc />
		public string CommandParameterSpecificDbTypePropertyName { get; } = nameof(NpgsqlParameter.NpgsqlDbType);

		/// <inheritdoc />
		public string CommandParameterSpecificCustomDbTypePropertyName { get; } = nameof(NpgsqlParameter.SpecificType);
	}
}