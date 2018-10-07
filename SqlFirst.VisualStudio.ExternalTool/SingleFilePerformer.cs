using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
			IEnumerable<(string text, string name)> parameterItems = generator.GenerateParameterItemsCode(queryText, options);
			string resultItem = generator.GenerateResultItemCode(queryText, options);

			Log.Debug("Query objects generated");

			string queryObjectFileName = Path.GetFileNameWithoutExtension(options.Target) + ".gen.cs";
			string GetParameterItemFileName(string name) => name + ".gen.cs";
			string resultItemFileName = options.ResultItemName + ".gen.cs";

			string queryObjectPath = Path.Combine(targetDirectory, queryObjectFileName);
			string resultItemPath = Path.Combine(targetDirectory, resultItemFileName);
			string GetParameterItemPath(string name) => Path.Combine(targetDirectory, GetParameterItemFileName(name));

			(string Path, string Data)[] parameterItemsPaths = parameterItems.Select(p => (GetParameterItemPath(p.name), p.text)).ToArray();

			WriteFile(queryObject, queryObjectPath);
			foreach ((string path, string data) in parameterItemsPaths)
			{
				WriteFile(data, path);
			}
			WriteFile(resultItem, resultItemPath);

			if (options.UpdateCsproj)
			{
				UpdateCsprojFile(options, (queryObjectPath, queryObject), parameterItemsPaths, (resultItemPath, resultItem));
			}

			Log.Info("Query objects generation was successfully completed.");
		}

		protected virtual void UpdateCsprojFile(GenerationOptions options,
			(string Path, string Data) queryObject,
			(string Path, string Data)[] parameters,
			(string Path, string Data) result)
		{
			Log.Debug("Trying to modify project file");

			string csprojDirectoryName = Path.GetDirectoryName(options.ProjectFile);

			Project project = CsprojHelper.BeginUpdate(options.ProjectFile);

			string GetRelativePath(string path)
			{
				return Path.IsPathRooted(path)
					? PathHelper.GetRelativePath(csprojDirectoryName, path)
					: path;
			}

			string relativeQueryObjectPath = GetRelativePath(queryObject.Path);

			(string Data, string RelativePath)[] relativeParameterItemsPaths = parameters
													.Select(parameter => (parameter.Data, GetRelativePath(parameter.Path)))
													.ToArray();

			string relativeResultItemPath = GetRelativePath(result.Path);

			string relativeQuerySqlPath = GetRelativePath(options.Target);

			string querySqlFileName = Path.GetFileName(options.Target);

			CsprojHelper.RemoveItem(project, relativeQueryObjectPath);
			foreach ((string _, string relativeParameterItemPath) in relativeParameterItemsPaths)
			{
				CsprojHelper.RemoveItem(project, relativeParameterItemPath);
			}
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

			foreach ((string Data, string RelativePath) parameter in relativeParameterItemsPaths)
			{
				if (!string.IsNullOrEmpty(parameter.Data))
				{
					CsprojHelper.AddItem(project, parameter.RelativePath, ItemType.Compile, querySqlFileName);
				}
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