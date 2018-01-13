﻿using System;
using System.Linq;

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

		/// <inheritdoc />
		public override string ToString()
		{
			string[] properties =
			{
				$"QueryFile: {QueryFile}",
				$"ProjectFile: {ProjectFile}",
				$"SolutionFile: {SolutionFile}",
				$"Namespace: {Namespace}",
				$"ResultItemName: {ResultItemName}",
				$"ParameterItemName: {ParameterItemName}",
				$"BeautifyFile: {BeautifyFile}",
				$"Dialect: {Dialect?.ToString("G")}",
				$"ConnectionString: {ConnectionString}",
				$"UpdateCsproj: {UpdateCsproj}"
			};

			return "GenerationOptions:\r\n" + string.Join(Environment.NewLine, properties.Select(s => "\t" + s));
		}
	}
}