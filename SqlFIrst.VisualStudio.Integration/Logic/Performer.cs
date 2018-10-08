using System;
using System.IO;
using Common.Logging;
using EnvDTE;
using SqlFirst.Intelligence.Generators;
using SqlFirst.Intelligence.Options;
using SqlFirst.VisualStudio.Integration.Helpers;
using SqlFirst.VisualStudio.Integration.Logging;

namespace SqlFirst.VisualStudio.Integration.Logic
{
	internal static class Performer
	{
		private static readonly ILog _log;

		/// <inheritdoc />
		static Performer()
		{
			CommonLoggingConfiguration.EnsureOutputEnabled();
			_log = LogManager.GetLogger(typeof(Performer));
		}

		public static void BeautifyFile(ProjectItem item)
		{
			_log.Info("Formatting file started");
			GenerationOptions options = GetGenerationOptions(item);
			GeneratorBase generator = GetGenerator(options);

			string queryText = item.GetText();
			string formattedText = generator.FormatQuery(queryText, options);

			bool wasOpened = item.Document != null;

			if (wasOpened)
			{
				_log.Trace("Trying to close file..");
				item.Document?.Close(vsSaveChanges.vsSaveChangesYes);
			}

			if (item.IsDirty)
			{
				item.Save();
			}

			File.WriteAllText(item.GetFullPath(), formattedText);

			if (wasOpened)
			{
				_log.Trace("Trying to reopen file..");
				item.OpenFile();
			}

			_log.Trace("Formatting file successful");
		}

		public static void GenerateObjects(ProjectItem item)
		{
			item.EnsureIsEmbeddedResource(_log);

			GenerationOptions options = GetGenerationOptions(item);

			string queryText = item.GetText();

			GeneratorBase generator = GetGenerator(options);

			(string queryObject, string resultItem, (string data, string name)[] parameterItems) = generator.GenerateAll(queryText, options);

			_log.Debug("Query objects generated");

			string GetFileName(string name) => (name ?? throw new ArgumentNullException(nameof(name), "Item name can not be null")) + ".gen.cs";

			string queryObjectFileName = GetFileName(Path.GetFileNameWithoutExtension(options.Target));
			string resultItemFileName = GetFileName(options.ResultItemName);

			ProcessGeneratedObject(item, queryObjectFileName, queryObject);

			foreach ((string data, string name) in parameterItems)
			{
				ProcessGeneratedObject(item, GetFileName(name), data);
			}

			ProcessGeneratedObject(item, resultItemFileName, resultItem);
		}

		private static void ProcessGeneratedObject(ProjectItem item, string fileName, string content)
		{
			if (string.IsNullOrEmpty(content))
			{
				item.RemoveDependentObject(fileName);
			}
			else
			{
				item.AddDependentObject(fileName, content);
			}
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

		private static GenerationOptions GetGenerationOptions(ProjectItem item)
		{
			var options = new GenerationOptions
			{
				Target = item.GetFullPath(),
				ProjectFile = item.ContainingProject.FullName,
				SolutionFile = IdeHelper.GetSolution().FullName,
				Namespace = item.GetNamespace(),
				BeautifyFile = null,
				ConnectionString = null,
				Dialect = null,
				ParameterItemName = null,
				ResultItemName = null,
				UpdateCsproj = true
			};

			_log.Trace(p => p("GetGenerationOptions:\r\n" + options.ToString()));
			options.ApplyGlobalOptions();
			options.FillWithDefaults();
			(bool isValid, string error) = options.Validate();
			if (!isValid)
			{
				throw new Exception("Invalid generation options: " + error);
			}

			return options;
		}
	}
}
