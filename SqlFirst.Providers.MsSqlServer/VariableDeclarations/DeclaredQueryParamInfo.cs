namespace SqlFirst.Providers.MsSqlServer.VariableDeclarations
{
	internal class DeclaredQueryParamInfo
	{
		public string DbType { get; set; }

		public string DbName { get; set; }

		public bool IsNumbered { get; set; }

		public string SemanticName { get; set; }

		public string Length { get; set; }

		public object DefaultValue { get; set; }
	}
}