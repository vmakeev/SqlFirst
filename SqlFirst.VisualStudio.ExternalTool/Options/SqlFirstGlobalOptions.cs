namespace SqlFirst.VisualStudio.ExternalTool.Options
{
	internal class SqlFirstGlobalOptions
	{
		public string ConnectionString { get; set; }

		public bool? BeautifyFile { get; set; }

		public Dialect? Dialect { get; set; }
	}
}