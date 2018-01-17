using System;
using System.IO;
using System.Linq;
using System.Text;
using Common.Logging;
using Newtonsoft.Json;
using SqlFirst.Intelligence.Helpers;

namespace SqlFirst.Intelligence.Options
{
	public static class OptionsPreprocessor
	{
		private static readonly ILog _log = LogManager.GetLogger(typeof(OptionsPreprocessor));

		public static void ApplyGlobalOptions(this GenerationOptions generationOptions)
		{
			SqlFirstGlobalOptions optionsFile = ReadGlobalOptionsFile(generationOptions);

			if (optionsFile == null)
			{
				_log.Trace("Global options are missing: 'SqlFirst.options' was not found.");
				return;
			}

			_log.Debug("Applying global options");

			if (optionsFile.ConnectionString != null)
			{
				generationOptions.ConnectionString = optionsFile.ConnectionString;
			}

			if (optionsFile.BeautifyFile != null)
			{
				generationOptions.BeautifyFile = optionsFile.BeautifyFile;
			}

			if (optionsFile.Dialect != null)
			{
				generationOptions.Dialect = optionsFile.Dialect;
			}

			_log.Trace(p => p("ApplyGlobalOptions:\r\n" + generationOptions.ToString()));
		}

		public static void FillWithDefaults(this GenerationOptions generationOptions)
		{
			if (!string.IsNullOrEmpty(generationOptions.Target))
			{
				if (generationOptions.ProjectFile == null)
				{
					_log.Debug($"Trying to specify {nameof(generationOptions.ProjectFile)}");
					generationOptions.ProjectFile = FindProjectFile(generationOptions.Target);
				}

				if (generationOptions.Namespace == null && generationOptions.ProjectFile != null)
				{
					_log.Debug($"Trying to specify {nameof(generationOptions.Namespace)}");
					generationOptions.Namespace = GetNamespace(generationOptions.ProjectFile, generationOptions.Target);
				}

				if (generationOptions.ResultItemName == null)
				{
					_log.Debug($"Trying to specify {nameof(generationOptions.ResultItemName)}");
					generationOptions.ResultItemName = Path.GetFileNameWithoutExtension(generationOptions.Target) + "Result";
				}

				if (generationOptions.ParameterItemName == null)
				{
					_log.Debug($"Trying to specify {nameof(generationOptions.ParameterItemName)}");
					generationOptions.ParameterItemName = Path.GetFileNameWithoutExtension(generationOptions.Target) + "Parameter";
				}
			}

			if (generationOptions.BeautifyFile == null)
			{
				generationOptions.BeautifyFile = false;
			}
			

			_log.Trace(p => p("FillWithDefaults:\r\n" + generationOptions.ToString()));
		}


		private static string GetNamespace(string projectFile, string queryFile)
		{
			string projectPath = Path.GetDirectoryName(projectFile);
			string queryPath = Path.GetDirectoryName(queryFile);

			if (string.IsNullOrEmpty(projectPath) || string.IsNullOrEmpty(queryPath))
			{
				_log.Warn("Can not specify namespace. Default will be used.");
				return "DefaultNamespace";
			}

			string relativeUri = PathHelper.GetRelativePath(projectPath, queryPath);

			string rootNamespace = Path.GetFileNameWithoutExtension(projectFile);

			var sb = new StringBuilder(string.Join(".", new[] { rootNamespace, relativeUri }.Where(item => !string.IsNullOrEmpty(item))));
			sb.Replace(Path.DirectorySeparatorChar, '.');
			sb.Replace(Path.AltDirectorySeparatorChar, '.');

			return sb.ToString();
		}

		private static string FindProjectFile(string queryFile)
		{
			char[] separators = { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar };
			string directory = Path.GetDirectoryName(queryFile);

			while (!string.IsNullOrEmpty(directory) && Directory.Exists(directory))
			{
				string[] found = Directory.GetFiles(directory, "*.csproj", SearchOption.TopDirectoryOnly);

				switch (found.Length)
				{
					case 0:
						_log.Trace($"{nameof(FindProjectFile)}: file not found at [{directory}]");
						break;

					case 1:
						_log.Trace($"{nameof(FindProjectFile)}: file successfully found at [{directory}]");
						return Path.Combine(directory, found.Single());

					default:
						throw new Exception($"{found.Length} '.csproj' files found. Specify [ProjectRoot] manually.");
				}

				directory = Path.GetDirectoryName(directory.TrimEnd(separators));
			}

			_log.Warn($"{nameof(FindProjectFile)}: project file not found.");
			return null;
		}

		private static SqlFirstGlobalOptions ReadGlobalOptionsFile(GenerationOptions generationOptions)
		{
			string rootFile = generationOptions.SolutionFile;
			if (string.IsNullOrEmpty(rootFile))
			{
				_log.Debug($"{nameof(ReadGlobalOptionsFile)}: {nameof(generationOptions.SolutionFile)} not specified");
				rootFile = generationOptions.ProjectFile;
			}

			if (string.IsNullOrEmpty(rootFile))
			{
				_log.Debug($"{nameof(ReadGlobalOptionsFile)}: {nameof(generationOptions.ProjectFile)} not specified");
				rootFile = generationOptions.Target;
			}

			if (string.IsNullOrEmpty(rootFile))
			{
				_log.Debug($"{nameof(ReadGlobalOptionsFile)}: {nameof(generationOptions.Target)} not specified");
				_log.Debug($"{nameof(ReadGlobalOptionsFile)}: Unable to start options file search: can not determine effective root folder.");
				return null;
			}

			string root = Path.GetDirectoryName(rootFile);

			char[] separators = { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar };
			string directory = Path.GetDirectoryName(generationOptions.Target);
			string preRoot = Path.GetDirectoryName(root)?.TrimEnd(separators);

			while (!string.IsNullOrEmpty(directory) && Directory.Exists(directory) && !string.Equals(directory.TrimEnd(separators), preRoot, StringComparison.OrdinalIgnoreCase))
			{
				string[] found = Directory.GetFiles(directory, "SqlFirst.options", SearchOption.TopDirectoryOnly);

				switch (found.Length)
				{
					case 0:
						_log.Trace($"{nameof(ReadGlobalOptionsFile)}: SqlFirst.options not found at [{directory}].");
						break;

					case 1:
						try
						{
							string optionsFilePath = Path.Combine(directory, found.Single());
							_log.Debug($"{nameof(ReadGlobalOptionsFile)}: SqlFirst.options successfully found at [{optionsFilePath}].");
							string optionsFileContent = File.ReadAllText(optionsFilePath);

							return string.IsNullOrEmpty(optionsFileContent)
								? null
								: JsonConvert.DeserializeObject<SqlFirstGlobalOptions>(optionsFileContent);
						}
						catch (Exception ex)
						{
							throw new Exception($"Can not load global options: {ex.Message}", ex);
						}

					default:
						throw new Exception($"Error: {found.Length} 'SqlFirst.options' files found at same folder, but 1 expected."); // should never occurs
				}

				directory = Path.GetDirectoryName(directory.TrimEnd(separators));
			}

			return null;
		}
	}
}