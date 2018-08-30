namespace SqlFirst.Intelligence.Options
{
	public class SqlFirstGlobalOptions
	{
		public string ConnectionString { get; set; }

		public bool? BeautifyFile { get; set; }

		public Dialect? Dialect { get; set; }
	}
}