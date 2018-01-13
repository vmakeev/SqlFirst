using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Common.Logging;
using Microsoft.Build.Evaluation;

namespace SqlFirst.VisualStudio.ExternalTool
{
	internal static class CsprojHelper
	{
		private static readonly ILog _log = LogManager.GetLogger(typeof(CsprojHelper));

		public static Project BeginUpdate(string csprojPath)
		{
			_log.Trace($"Trying to load project file [{csprojPath}]");
			var projectCollection = new ProjectCollection();
			Project result = projectCollection.LoadProject(csprojPath);
			_log.Trace("Project file loaded successfully");
			return result;
		}

		public static void EndUpdate(Project project)
		{
			_log.Trace($"Trying to save project [{project.FullPath}]");
			project.Save();
			_log.Trace("Project file saved successfully");
		}

		public static void AddItem(Project project, string relativeItemPath, ItemType itemType, string dependenceItemRelativePath = null)
		{
			if (Path.IsPathRooted(relativeItemPath))
			{
				throw new Exception($"{nameof(relativeItemPath)} must me relative path.");
			}

			if (!string.IsNullOrEmpty(dependenceItemRelativePath) && Path.IsPathRooted(dependenceItemRelativePath))
			{
				throw new Exception($"{nameof(relativeItemPath)} must me relative path.");
			}

			if (!string.IsNullOrEmpty(dependenceItemRelativePath))
			{
				_log.Trace($"AddItem: {itemType:G} [{relativeItemPath}] DependentUpon [{dependenceItemRelativePath}]");
				project.AddItem(itemType.ToString("G"), relativeItemPath, new[] { new KeyValuePair<string, string>("DependentUpon", dependenceItemRelativePath) });
			}
			else
			{
				_log.Trace($"AddItem: {itemType:G} [{relativeItemPath}]");
				project.AddItem(itemType.ToString("G"), relativeItemPath);
			}
		}

		public static void RemoveItem(Project project, string relativeItemPath)
		{
			if (Path.IsPathRooted(relativeItemPath))
			{
				throw new Exception($"{nameof(relativeItemPath)} must me relative path.");
			}

			_log.Trace($"RemoveItem: [{relativeItemPath}]");

			//ProjectItem[] existingItems = project.GetItemsByEvaluatedInclude(relativeItemPath).ToArray();
			ProjectItem[] existingItems = project.Items
												.Where(projectItem => string.Equals(projectItem.UnevaluatedInclude, relativeItemPath, StringComparison.InvariantCulture))
												.ToArray();
			_log.Trace($"{existingItems.Length} items will be deleted");
			project.RemoveItems(existingItems);
		}

		public static bool IsExists(Project project, ItemType itemType, string relativeItemPath)
		{
			if (Path.IsPathRooted(relativeItemPath))
			{
				throw new Exception($"{nameof(relativeItemPath)} must me relative path.");
			}

			IEnumerable<ProjectItem> existingItems = project.Items
										.Where(projectItem => string.Equals(projectItem.UnevaluatedInclude, relativeItemPath, StringComparison.InvariantCulture))
										.Where(projectItem => string.Equals(projectItem.ItemType, itemType.ToString("G"), StringComparison.InvariantCulture));
			return existingItems.Any();
		}
		public static bool IsExists(Project project, string relativeItemPath)
		{
			if (Path.IsPathRooted(relativeItemPath))
			{
				throw new Exception($"{nameof(relativeItemPath)} must me relative path.");
			}

			IEnumerable<ProjectItem> existingItems = project.Items
										.Where(projectItem => string.Equals(projectItem.UnevaluatedInclude, relativeItemPath, StringComparison.InvariantCulture));
			return existingItems.Any();
		}
	}
}