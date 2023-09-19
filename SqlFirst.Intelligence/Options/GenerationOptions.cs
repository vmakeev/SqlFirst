using System;
using System.Collections.Generic;
using System.Linq;
using SqlFirst.Core;

namespace SqlFirst.Intelligence.Options
{
	public class GenerationOptions
	{
		public string Target { get; set; }

		public string Namespace { get; set; }

		public string ResultItemName { get; set; }

		public string ResultItemMappedFrom { get; set; }
		
		public string ParameterItemName { get; set; }
		
		
		public string ParameterItemMappedFrom { get; set; }

		public string ProjectFile { get; set; }

		public string SolutionFile { get; set; }

		public bool? BeautifyFile { get; set; }

		public Dialect? Dialect { get; set; }

		public string ConnectionString { get; set; }

		public bool UpdateCsproj { get; set; } 

		public string ReplaceIndent { get; set; }
		
		public string[][] PreprocessQueryRegexes { get; set; }

		public IReadOnlyDictionary<string, ExternalItem> ExternalResultItemsMap { get; set; }
		
		public IReadOnlyDictionary<string, ExternalItem> ExternalParameterItemsMap { get; set; }

		public IOptionDefaults OptionDefaults { get; set; }

		/// <inheritdoc />
		public override string ToString()
		{
			IEnumerable<string> preprocessRegexes = 
				(PreprocessQueryRegexes ?? Array.Empty<string[]>())
				.Select(item => string.Join(" => ", item));

			IEnumerable<string> externalResultItemsMap = (ExternalResultItemsMap ?? new Dictionary<string, ExternalItem>())
				.Select(keyValuePair => $"{keyValuePair.Key} => {keyValuePair.Value.Namespace}.{keyValuePair.Value.Name}");

			IEnumerable<string> externalParameterItemsMap = (ExternalParameterItemsMap ?? new Dictionary<string, ExternalItem>())
				.Select(keyValuePair => $"{keyValuePair.Key} => {keyValuePair.Value.Namespace}.{keyValuePair.Value.Name}");
			
			string[] properties =
			{
				$"Target: {Target}",
				$"ProjectFile: {ProjectFile}",
				$"SolutionFile: {SolutionFile}",
				$"Namespace: {Namespace}",
				$"ResultItemName: {ResultItemName}",
				$"ParameterItemName: {ParameterItemName}",
				$"BeautifyFile: {BeautifyFile}",
				$"Dialect: {Dialect?.ToString("G")}",
				$"ConnectionString: {ConnectionString}",
				$"ReplaceIndent: [{ReplaceIndent}]",
				$"UpdateCsproj: {UpdateCsproj}",
				$"PreprocessQueryRegexes: {string.Join("\r\n", preprocessRegexes)}",
				$"ExternalResultItemsMap: {string.Join("\r\n", externalResultItemsMap)}",
				$"ExternalParameterItemsMap: {string.Join("\r\n", externalParameterItemsMap)}"
			};

			return "GenerationOptions:\r\n" + string.Join(Environment.NewLine, properties.Select(s => "\t" + s));
		}
	}
}