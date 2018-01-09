using System;
using System.Data;
using System.Data.SqlClient;
using SqlFirst.Core;

namespace SqlFirst.Providers.MsSqlServer
{
	/// <inheritdoc />
	internal class MsSqlServerProviderTypesInfo : IProviderTypesInfo
	{
		public static MsSqlServerProviderTypesInfo Instance { get; } = new MsSqlServerProviderTypesInfo();

		private MsSqlServerProviderTypesInfo()
		{
		}

		/// <inheritdoc />
		public Type ConnectionType { get; } = typeof(SqlConnection);

		/// <inheritdoc />
		public Type CommandType { get; } = typeof(SqlCommand);

		/// <inheritdoc />
		public Type CommandParameterType { get; } = typeof(SqlParameter);

		/// <inheritdoc />
		public Type CommandParameterSpecificDbTypePropertyType { get; } = typeof(SqlDbType);

		/// <inheritdoc />
		public string CommandParameterSpecificDbTypePropertyName { get; } = nameof(SqlParameter.SqlDbType);
	}
}