using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Common.Logging;
using SqlFirst.VisualStudio.ExternalTool.Generators;
using SqlFirst.VisualStudio.ExternalTool.Options;

namespace SqlFirst.VisualStudio.ExternalTool
{
	internal class Performer
	{
		private static readonly ILog _log = LogManager.GetLogger(nameof(Performer));

		public void Perform(GenerationOptions options)
		{
			options.FillWithDefaults();
			(bool isValid, string error) = options.Validate();
			if (!isValid)
			{
				throw new Exception("Invalid generation options: " + error);
			}

			if (!File.Exists(options.QueryFile))
			{
				throw new Exception("SQL file does not exists.");
			}

			string queryText = File.ReadAllText(options.QueryFile);

			string targetDirectory = Path.GetDirectoryName(options.QueryFile);
			if (string.IsNullOrEmpty(targetDirectory) || !Directory.Exists(targetDirectory))
			{
				throw new Exception("Target directory does not exists: " + (targetDirectory ?? "<null>"));
			}

			GeneratorBase generator;

			switch (options.Dialect)
			{
				case Dialect.MsSqlServer:
					_log.Debug($"{nameof(MsSqlServerGenerator)} will be used as generator.");
					generator = new MsSqlServerGenerator();
					break;

				case Dialect.Postgres:
					_log.Debug($"{nameof(PostgresGenerator)} will be used as generator.");
					generator = new PostgresGenerator();
					break;

				case null:
					throw new ArgumentOutOfRangeException($"{nameof(Dialect)}: null", (Exception)null);

				default:
					throw new ArgumentOutOfRangeException($"Unexpected {nameof(Dialect)}: {options.Dialect:G} ({options.Dialect:D})", (Exception)null);
			}

			string queryObject = generator.GenerateQueryObjectCode(queryText, options);
			string parameterItem = generator.GenerateParameterItemCode(queryText, options);
			string resultItem = generator.GenerateResultItemCode(queryText, options);

			string queryObjectFileName = Path.GetFileNameWithoutExtension(options.QueryFile) + ".gen.cs";
			string parameterItemFileName = options.ParameterItemName + ".gen.cs";
			string resultItemFileName = options.ResultItemName + ".gen.cs";

			string queryObjectPath = Path.Combine(targetDirectory, queryObjectFileName);
			string parameterItemPath = Path.Combine(targetDirectory, parameterItemFileName);
			string resultItemPath = Path.Combine(targetDirectory, resultItemFileName);

			ProcessItem(queryObject, queryObjectPath, options.ProjectFile);
			ProcessItem(parameterItem, parameterItemPath, options.ProjectFile);
			ProcessItem(resultItem, resultItemPath, options.ProjectFile);

			_log.Info("Query objects generation was successfully completed.");
		}

		[SuppressMessage("ReSharper", "UnusedParameter.Local")]
		private void ProcessItem(string item, string path, string projectFilePath)
		{
			if (string.IsNullOrEmpty(item))
			{
				if (File.Exists(path))
				{
					_log.Debug("File will be deleted: " + path);
					File.Delete(path);
				}
			}
			else
			{
				_log.Debug("File will be (re)created: " + path);
				File.WriteAllText(path, item);
			}

			// todo register at .csproj if needed
		}
	}
}