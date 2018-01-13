using System;
using System.IO;
using Common.Logging;
using Microsoft.Build.Evaluation;
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

			GeneratorBase generator = GetGenerator(options);

			string queryObject = generator.GenerateQueryObjectCode(queryText, options);
			string parameterItem = generator.GenerateParameterItemCode(queryText, options);
			string resultItem = generator.GenerateResultItemCode(queryText, options);

			string queryObjectFileName = Path.GetFileNameWithoutExtension(options.QueryFile) + ".gen.cs";
			string parameterItemFileName = options.ParameterItemName + ".gen.cs";
			string resultItemFileName = options.ResultItemName + ".gen.cs";

			string queryObjectPath = Path.Combine(targetDirectory, queryObjectFileName);
			string parameterItemPath = Path.Combine(targetDirectory, parameterItemFileName);
			string resultItemPath = Path.Combine(targetDirectory, resultItemFileName);

			WriteFile(queryObject, queryObjectPath);
			WriteFile(parameterItem, parameterItemPath);
			WriteFile(resultItem, resultItemPath);

			_log.Debug("Query objects generated");

			if (options.UpdateCsproj)
			{
				UpdateCsprojFile(options, (queryObjectPath, queryObject), (parameterItemPath, parameterItem), (resultItemPath, resultItem));
			}

			_log.Info("Query objects generation was successfully completed.");
		}

		private static void UpdateCsprojFile(GenerationOptions options,
			(string Path, string Data) queryObject,
			(string Path, string Data) parameter,
			(string Path, string Data) result)
		{
			_log.Debug("Trying to modify project file");

			string csprojDirectoryName = Path.GetDirectoryName(options.ProjectFile);

			Project project = CsprojHelper.BeginUpdate(options.ProjectFile);

			string relativeQueryObjectPath = Path.IsPathRooted(queryObject.Path)
				? PathHelper.GetRelativePath(csprojDirectoryName, queryObject.Path)
				: queryObject.Path;

			string relativeParameterItemPath = Path.IsPathRooted(parameter.Path)
				? PathHelper.GetRelativePath(csprojDirectoryName, parameter.Path)
				: parameter.Path;

			string relativeResultItemPath = Path.IsPathRooted(result.Path)
				? PathHelper.GetRelativePath(csprojDirectoryName, result.Path)
				: result.Path;

			string relativeQuerySqlPath = Path.IsPathRooted(options.QueryFile)
				? PathHelper.GetRelativePath(csprojDirectoryName, options.QueryFile)
				: options.QueryFile;

			CsprojHelper.RemoveItem(project, relativeQueryObjectPath);
			CsprojHelper.RemoveItem(project, relativeParameterItemPath);
			CsprojHelper.RemoveItem(project, relativeResultItemPath);

			if (!CsprojHelper.IsExists(project, ItemType.EmbeddedResource, relativeQuerySqlPath))
			{
				CsprojHelper.AddItem(project, relativeQuerySqlPath, ItemType.EmbeddedResource);
			}

			if (!string.IsNullOrEmpty(queryObject.Data))
			{
				CsprojHelper.AddItem(project, relativeQueryObjectPath, ItemType.Compile, relativeQuerySqlPath);
			}

			if (!string.IsNullOrEmpty(parameter.Data))
			{
				CsprojHelper.AddItem(project, relativeParameterItemPath, ItemType.Compile, relativeQuerySqlPath);
			}

			if (!string.IsNullOrEmpty(result.Data))
			{
				CsprojHelper.AddItem(project, relativeResultItemPath, ItemType.Compile, relativeQuerySqlPath);
			}

			CsprojHelper.EndUpdate(project);
		}

		private static GeneratorBase GetGenerator(GenerationOptions options)
		{
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

			return generator;
		}

		private void WriteFile(string item, string path)
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
		}
	}
}