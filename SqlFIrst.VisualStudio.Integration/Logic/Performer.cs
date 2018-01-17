using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using Common.Logging;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using SqlFirst.Intelligence.Generators;
using SqlFirst.Intelligence.Helpers;
using SqlFirst.Intelligence.Options;
using SqlFIrst.VisualStudio.Integration.Logging;

namespace SqlFIrst.VisualStudio.Integration.Logic
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

		private static void EnsureIsEmbeddedResource(ProjectItem item)
		{
			const string embeddedResource = "EmbeddedResource";

			string itemType = item.Properties.Item("ItemType").Value.ToString();
			if (itemType != embeddedResource)
			{
				_log.Warn($"Query SQL file \"{item.Name}\" is not an {embeddedResource}. Trying to fix it");
				item.Properties.Item("ItemType").Value = embeddedResource;
			}
		}

		[SuppressMessage("ReSharper", "SuspiciousTypeConversion.Global")]
		private static Solution2 GetSolution()
		{
			var applicationObject = (DTE2)Package.GetGlobalService(typeof(DTE));
			return (Solution2)applicationObject.Solution;
		}

		[SuppressMessage("ReSharper", "PossibleNullReferenceException")]
		private static string GetNamespace(this ProjectItem item)
		{
			string rootNamespace = item.ContainingProject.Properties.Item("DefaultNamespace").Value.ToString();

			string projectFolder = Path.GetDirectoryName(item.ContainingProject.FullName);
			string itemFolder = Path.GetDirectoryName(item.GetFullPath());

			string relativePath = PathHelper.GetRelativePath(projectFolder, itemFolder);
			string relativeNamespace = relativePath
										.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
										.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar)
										.Replace(Path.DirectorySeparatorChar, '.');

			string itemNamespace = string.Join(".", rootNamespace, relativeNamespace);
			return itemNamespace;
		}

		private static string GetItemText(this ProjectItem item)
		{
			if (item.IsDirty)
			{
				item.Save();
			}

			string itemPath = item.GetFullPath();
			return File.ReadAllText(itemPath);
		}

		public static void Perform(ProjectItem item)
		{
			EnsureIsEmbeddedResource(item);

			GenerationOptions options = GetGenerationOptions(item);

			string queryText = item.GetItemText();

			GeneratorBase generator = GetGenerator(options);

			string queryObject = generator.GenerateQueryObjectCode(queryText, options);
			string parameterItem = generator.GenerateParameterItemCode(queryText, options);
			string resultItem = generator.GenerateResultItemCode(queryText, options);

			_log.Debug("Query objects generated");

			string queryObjectFileName = Path.GetFileNameWithoutExtension(options.Target) + ".gen.cs";
			string parameterItemFileName = options.ParameterItemName + ".gen.cs";
			string resultItemFileName = options.ResultItemName + ".gen.cs";

			ProcessGeneratedObject(item, queryObjectFileName, queryObject);
			ProcessGeneratedObject(item, parameterItemFileName, parameterItem);
			ProcessGeneratedObject(item, resultItemFileName, resultItem);
		}

		private static void ProcessGeneratedObject(ProjectItem item, string fileName, string content)
		{
			if (string.IsNullOrEmpty(content))
			{
				RemoveObject(item, fileName);
			}
			else
			{
				AddDependentObject(item, fileName, content);
			}
		}

		private static void RemoveObject(ProjectItem item, string fileName)
		{
			foreach (ProjectItem projectItem in item.ProjectItems)
			{
				if (projectItem.Name == fileName)
				{
					string filePath = projectItem.GetFullPath();
					
					projectItem.Delete();

					if (File.Exists(filePath))
					{
						File.Delete(filePath);
					}

					return;
				}
			}
		}

		private static string GetFullPath(this ProjectItem item)
		{
			return item.Properties.Item("FullPath").Value.ToString();
		}

		[SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
		private static void AddDependentObject(ProjectItem item, string fileName, string content)
		{
			string targetFolder = Path.GetDirectoryName(item.GetFullPath());
			string objectFileName = Path.Combine(targetFolder, fileName);
			File.WriteAllText(objectFileName, content);
			// todo: netstandard projects doesn't make dependentupon https://github.com/dotnet/project-system/issues/1870
			item.ProjectItems.AddFromFile(objectFileName);
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
				SolutionFile = GetSolution().FullName,
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
