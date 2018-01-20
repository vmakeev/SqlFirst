using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;

namespace SqlFIrst.VisualStudio.Integration.Helpers
{
	internal static class IdeHelper
	{
		public static DTE2 ApplicationObject { get; } = (DTE2)Package.GetGlobalService(typeof(DTE));

		[SuppressMessage("ReSharper", "SuspiciousTypeConversion.Global")]
		public static Solution2 GetSolution()
		{
			return (Solution2)ApplicationObject.Solution;
		}
		public static void OpenFile(string path)
		{
			ApplicationObject.ItemOperations.OpenFile(path, Constants.vsViewKindTextView);
		}

		public static void OpenFile(this ProjectItem item)
		{
			OpenFile(item.GetFullPath());
		}

		public static IEnumerable<ProjectItem> GetSelectedItems()
		{
			UIHierarchy solutionExplorer = ApplicationObject.ToolWindows.SolutionExplorer;

			if (solutionExplorer.SelectedItems is Array selectedItems)
			{
				foreach (UIHierarchyItem selectedItem in selectedItems)
				{
					if (selectedItem.Object is ProjectItem projectItem)
					{
						yield return projectItem;
					}
				}
			}
		}

		public static IEnumerable<ProjectItem> GetNestedItemsRecursive(ProjectItem source, Func<ProjectItem, bool> predicate)
		{
			if (source == null)
			{
				yield break;
			}

			if (predicate(source))
			{
				yield return source;
			}

			if (source.ProjectItems != null)
			{
				foreach (ProjectItem innerItem in source.ProjectItems.OfType<ProjectItem>())
				{
					foreach (ProjectItem item in GetNestedItemsRecursive(innerItem, predicate))
					{
						yield return item;
					}
				}
			}
		}
	}
}