using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Common.Logging;
using Microsoft.Build.Construction;
using Microsoft.Build.Evaluation;

namespace SqlFirst.VisualStudio.ExternalTool
{
	public static class CsprojHelper
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
				throw new Exception($"{nameof(relativeItemPath)} must be a relative path.");
			}

			if (!string.IsNullOrEmpty(dependenceItemRelativePath) && Path.IsPathRooted(dependenceItemRelativePath))
			{
				throw new Exception($"{nameof(relativeItemPath)} must be a relative path.");
			}

			EnsureRemovePresent(project, relativeItemPath);

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

			project.ReevaluateIfNecessary();
		}

		public static void RemoveItem(Project project, string relativeItemPath)
		{
			project.ReevaluateIfNecessary();

			if (Path.IsPathRooted(relativeItemPath))
			{
				throw new Exception($"{nameof(relativeItemPath)} must be a relative path.");
			}

			_log.Trace($"RemoveItem: [{relativeItemPath}]");

			ProjectItem[] existingItems = project.AllEvaluatedItems
												.Where(projectItem => !projectItem.IsImported && projectItem.EvaluatedInclude == relativeItemPath)
												.ToArray();

			_log.Trace($"{existingItems.Length} items will be deleted");

			if (existingItems.Any())
			{
				foreach (ProjectItem existingItem in existingItems)
				{
					EnsureRemoveNotPresent(project, relativeItemPath);
					project.RemoveItem(existingItem);
				}

				project.ReevaluateIfNecessary();
			}
		}

		public static bool IsExists(Project project, ItemType itemType, string relativeItemPath)
		{
			if (Path.IsPathRooted(relativeItemPath))
			{
				throw new Exception($"{nameof(relativeItemPath)} must be a relative path.");
			}

			project.ReevaluateIfNecessary();

			ProjectItem existingItem = project.Items.FirstOrDefault(projectItem =>
				!projectItem.IsImported &&
				projectItem.EvaluatedInclude == relativeItemPath &&
				projectItem.ItemType == itemType.ToString("G"));

			_log.Trace(p => p($"CheckIsExists: ItemType: [{itemType:G}], path: [{relativeItemPath}] -> {existingItem != null}"));

			return existingItem != null;
		}

		public static bool IsExists(Project project, string relativeItemPath)
		{
			if (Path.IsPathRooted(relativeItemPath))
			{
				throw new Exception($"{nameof(relativeItemPath)} must be a relative path.");
			}

			project.ReevaluateIfNecessary();

			IEnumerable<ProjectItem> existingItems = project.Items.Where(projectItem => !projectItem.IsImported && projectItem.EvaluatedInclude == relativeItemPath);
			return existingItems.Any();
		}

		private static void EnsureRemovePresent(Project project, string relativeItemPath)
		{
			project.ReevaluateIfNecessary();

			ProjectItem[] explicitIncludedItems = project.Items
														.Where(p => p.EvaluatedInclude == relativeItemPath && !p.IsImported)
														.ToArray();

			ProjectItem[] autoIncludedItems = project.AllEvaluatedItems
													.Where(p => p.EvaluatedInclude == relativeItemPath && p.IsImported)
													.ToArray();

			ProjectItemElement[] excludes = project.Xml.ItemGroups
													.SelectMany(groupElement => groupElement.Items)
													.Where(p => p.Remove == relativeItemPath)
													.ToArray();

			if (explicitIncludedItems.Any())
			{
				foreach (ProjectItem explicitIncludedItem in explicitIncludedItems)
				{
					project.RemoveItem(explicitIncludedItem);
				}

				project.ReevaluateIfNecessary();
			}

			if (!excludes.Any())
			{
				foreach (ProjectItem autoIncludedItem in autoIncludedItems)
				{
					string type = autoIncludedItem.ItemType;
					string include = autoIncludedItem.EvaluatedInclude;
					project.AddItem(type, include);

					ProjectItem item = project.Items.First(projectItem => !projectItem.IsImported &&
																		projectItem.ItemType == type &&
																		projectItem.UnevaluatedInclude == include);

					item.Xml.Include = null;
					item.Xml.Remove = include;
				}
			}

			project.ReevaluateIfNecessary();
		}

		private static void EnsureRemoveNotPresent(Project project, string relativeItemPath)
		{
			project.ReevaluateIfNecessary();

			(ProjectItemGroupElement Parent, ProjectItemElement Item)[] excludes = project.Xml.ItemGroups
																						.SelectMany(groupElement => groupElement.Items.Select(item => (Parent: groupElement, Item: item)))
																						.Where(p => p.Item.Remove == relativeItemPath)
																						.ToArray();

			if (excludes.Any())
			{
				foreach ((ProjectItemGroupElement parent, ProjectItemElement item) in excludes)
				{
					parent.RemoveChild(item);
				}

				project.ReevaluateIfNecessary();
			}
		}
	}
}