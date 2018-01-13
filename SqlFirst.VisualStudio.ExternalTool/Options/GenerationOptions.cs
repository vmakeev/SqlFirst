namespace SqlFirst.VisualStudio.ExternalTool.Options
{
	internal class GenerationOptions
	{
		public string QueryFile { get; set; }

		public string Namespace { get; set; }

		public string ResultItemName { get; set; }

		public string ParameterItemName { get; set; }

		public string ProjectFile { get; set; }

		public string SolutionFile { get; set; }

		public bool? BeautifyFile { get; set; }

		public Dialect? Dialect { get; set; }

		public string ConnectionString { get; set; }

		public bool UpdateCsproj { get; set; } = true;
	}
}