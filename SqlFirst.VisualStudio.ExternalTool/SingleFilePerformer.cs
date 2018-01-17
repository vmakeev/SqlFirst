using System;
using System.IO;
using Common.Logging;
using Microsoft.Build.Evaluation;
using SqlFirst.Intelligence.Generators;
using SqlFirst.Intelligence.Helpers;
using SqlFirst.Intelligence.Options;

namespace SqlFirst.VisualStudio.ExternalTool
{
	public class SingleFilePerformer : IPerformer
	{
		private ILog _log;

		protected ILog Log => _log ?? (_log = LogManager.GetLogger(GetType()));

		public virtual void Perform(GenerationOptions options)
		{
			_log.Trace(p => p("IncomingOptions:\r\n" + options.ToString()));

			options.FillWithDefaults();
			options.ApplyGlobalOptions();
			(bool isValid, string error) = options.Validate();
			if (!isValid)
			{
				throw new Exception("Invalid generation options: " + error);
			}

			if (!File.Exists(options.Target))
			{
				throw new Exception("SQL file does not exists.");
			}

			string queryText = File.ReadAllText(options.Target);

			string targetDirectory = Path.GetDirectoryName(options.Target);
			if (string.IsNullOrEmpty(targetDirectory) || !Directory.Exists(targetDirectory))
			{
				throw new Exception("Target directory does not exists: " + (targetDirectory ?? "<null>"));
			}

			GeneratorBase generator = GetGenerator(options);

			string queryObject = generator.GenerateQueryObjectCode(queryText, options);
			string parameterItem = generator.GenerateParameterItemCode(queryText, options);
			string resultItem = generator.GenerateResultItemCode(queryText, options);

			Log.Debug("Query objects generated");

			string queryObjectFileName = Path.GetFileNameWithoutExtension(options.Target) + ".gen.cs";
			string parameterItemFileName = options.ParameterItemName + ".gen.cs";
			string resultItemFileName = options.ResultItemName + ".gen.cs";

			string queryObjectPath = Path.Combine(targetDirectory, queryObjectFileName);
			string parameterItemPath = Path.Combine(targetDirectory, parameterItemFileName);
			string resultItemPath = Path.Combine(targetDirectory, resultItemFileName);

			WriteFile(queryObject, queryObjectPath);
			WriteFile(parameterItem, parameterItemPath);
			WriteFile(resultItem, resultItemPath);

			if (options.UpdateCsproj)
			{
				UpdateCsprojFile(options, (queryObjectPath, queryObject), (parameterItemPath, parameterItem), (resultItemPath, resultItem));
			}

			Log.Info("Query objects generation was successfully completed.");
		}

		protected virtual void UpdateCsprojFile(GenerationOptions options,
			(string Path, string Data) queryObject,
			(string Path, string Data) parameter,
			(string Path, string Data) result)
		{
			Log.Debug("Trying to modify project file");

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

			string relativeQuerySqlPath = Path.IsPathRooted(options.Target)
				? PathHelper.GetRelativePath(csprojDirectoryName, options.Target)
				: options.Target;

			string querySqlFileName = Path.GetFileName(options.Target);

			CsprojHelper.RemoveItem(project, relativeQueryObjectPath);
			CsprojHelper.RemoveItem(project, relativeParameterItemPath);
			CsprojHelper.RemoveItem(project, relativeResultItemPath);

			if (!CsprojHelper.IsExists(project, ItemType.EmbeddedResource, relativeQuerySqlPath))
			{
				Log.Warn("Query SQL file is not an EmbeddedResource. Trying to fix it");
				CsprojHelper.RemoveItem(project, relativeQuerySqlPath);
				CsprojHelper.AddItem(project, relativeQuerySqlPath, ItemType.EmbeddedResource);
			}
			else
			{
				Log.Info("Query SQL file is EmbeddedResource, OK");
			}

			if (!string.IsNullOrEmpty(queryObject.Data))
			{
				CsprojHelper.AddItem(project, relativeQueryObjectPath, ItemType.Compile, querySqlFileName);
			}

			if (!string.IsNullOrEmpty(parameter.Data))
			{
				CsprojHelper.AddItem(project, relativeParameterItemPath, ItemType.Compile, querySqlFileName);
			}

			if (!string.IsNullOrEmpty(result.Data))
			{
				CsprojHelper.AddItem(project, relativeResultItemPath, ItemType.Compile, querySqlFileName);
			}

			CsprojHelper.EndUpdate(project);
		}

		protected virtual GeneratorBase GetGenerator(GenerationOptions options)
		{
			GeneratorBase generator;
			switch (options.Dialect)
			{
				case Dialect.MsSqlServer:
					Log.Debug($"{nameof(MsSqlServerGenerator)} will be used as generator.");
					generator = new MsSqlServerGenerator();
					break;

				case Dialect.Postgres:
					Log.Debug($"{nameof(PostgresGenerator)} will be used as generator.");
					generator = new PostgresGenerator();
					break;

				case null:
					throw new ArgumentOutOfRangeException($"{nameof(Dialect)}: null", (Exception)null);

				default:
					throw new ArgumentOutOfRangeException($"Unexpected {nameof(Dialect)}: {options.Dialect:G} ({options.Dialect:D})", (Exception)null);
			}

			return generator;
		}

		protected virtual void WriteFile(string item, string path)
		{
			if (string.IsNullOrEmpty(item))
			{
				if (File.Exists(path))
				{
					Log.Debug("File will be deleted: " + path);
					File.Delete(path);
				}
			}
			else
			{
				Log.Debug("File will be (re)created: " + path);
				File.WriteAllText(path, item);
			}
		}
	}
}