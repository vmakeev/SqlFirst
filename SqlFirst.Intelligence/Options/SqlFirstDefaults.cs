namespace SqlFirst.Intelligence.Options
{
	public class SqlFirstDefaults
	{
		public SqlFirstDefaultsSection Common { get; set; }
		
		public SqlFirstDefaultsSection Insert { get; set; }
		public SqlFirstDefaultsSection Select { get; set; }
		public SqlFirstDefaultsSection Update { get; set; }
		public SqlFirstDefaultsSection Delete { get; set; }
	}
}