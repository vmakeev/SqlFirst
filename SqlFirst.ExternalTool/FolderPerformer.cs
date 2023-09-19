using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Logging;
using SqlFirst.Intelligence.Options;

namespace SqlFirst.ExternalTool
{
	public class FolderPerformer : SingleFilePerformer
	{
		public override void Perform(GenerationOptions options)
		{
			string folder = options.Target;
			if (!Directory.Exists(folder))
			{
				throw new Exception($"Directory [{folder}] not found.");
			}

			GenerationOptions cleanOptions = CleanupOptions(options);

			IEnumerable<string> queryFiles = GetAllSqlFiles(folder);

			foreach (string queryFile in queryFiles)
			{
				var innerOptions = new GenerationOptions
				{
					Target = queryFile,
					BeautifyFile = cleanOptions.BeautifyFile,
					ProjectFile = cleanOptions.ProjectFile,
					Namespace = cleanOptions.Namespace,
					ResultItemName = cleanOptions.ResultItemName,
					ConnectionString = cleanOptions.ConnectionString,
					Dialect = cleanOptions.Dialect,
					ParameterItemName = cleanOptions.ParameterItemName,
					SolutionFile = cleanOptions.SolutionFile,
					UpdateCsproj = cleanOptions.UpdateCsproj
				};

				base.Perform(innerOptions);
			}
		}

		private GenerationOptions CleanupOptions(GenerationOptions source)
		{
			var target = new GenerationOptions
			{
				Target = source.Target,
				BeautifyFile = source.BeautifyFile,
				SolutionFile = source.SolutionFile,
				UpdateCsproj = source.UpdateCsproj,
				Dialect = source.Dialect,
				ConnectionString = source.ConnectionString,

				ProjectFile = null,
				Namespace = null,
				ResultItemName = null,
				ParameterItemName = null
			};

			if (Log.IsEnabled(LogLevel.Information))
			{
				var props = new List<string>(4);
				if (source.ProjectFile != null)
				{
					props.Add($"{nameof(source.ProjectFile)} [{source.ProjectFile}]");
				}

				if (source.Namespace != null)
				{
					props.Add($"{nameof(source.Namespace)} [{source.Namespace}]");
				}

				if (source.ResultItemName != null)
				{
					props.Add($"{nameof(source.ResultItemName)} [{source.ResultItemName}]");
				}

				if (source.ParameterItemName != null)
				{
					props.Add($"{nameof(source.ParameterItemName)} [{source.ParameterItemName}]");
				}

				if (props.Any())
				{
					Log.LogInformation("Following options will be ignored due to the [Folder] mode:\r\n\t" + string.Join(Environment.NewLine + "\t", props));
				}
			}

			return target;
		}

		private IEnumerable<string> GetAllSqlFiles(string folder)
		{
			return Directory.EnumerateFiles(folder, "*.sql", SearchOption.AllDirectories);
		}
	}
}