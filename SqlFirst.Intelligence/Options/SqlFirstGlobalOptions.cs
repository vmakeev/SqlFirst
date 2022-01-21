using System.Collections.Generic;

namespace SqlFirst.Intelligence.Options
{
	public class SqlFirstGlobalOptions
	{
		public string ConnectionString { get; set; }

		public bool? BeautifyFile { get; set; }

		public Dialect? Dialect { get; set; }

		public string Indent { get; set; }

		public Dictionary<string, ExternalItem> ExternalResultItemsMap { get; set; }
		
		public Dictionary<string, ExternalItem> ExternalParameterItemsMap { get; set; }

		public string[] PreprocessQueryRegexes { get; set; }

		public SqlFirstDefaults OptionDefaults { get; set; }
	}

	public class ExternalItem
	{
		public string Name { get; set; }
		public string Namespace { get; set; }
	}
}