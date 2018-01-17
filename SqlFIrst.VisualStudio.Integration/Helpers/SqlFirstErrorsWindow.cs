using System;
using System.Collections.Generic;
using System.Linq;
using EnvDTE;
using Microsoft.VisualStudio.Shell;

namespace SqlFIrst.VisualStudio.Integration.Helpers
{
	public class SqlFirstErrorsWindow
	{
		private readonly ErrorListProvider _errorListProvider;

		public SqlFirstErrorsWindow(IServiceProvider serviceProvider)
		{
			if (serviceProvider == null)
			{
				throw new ArgumentNullException(nameof(serviceProvider));
			}

			_errorListProvider = new ErrorListProvider(serviceProvider)
			{
				ProviderName = "SqlFirstErrorProvider",
				ProviderGuid = Guid.Parse("e5ef6ee6-13ba-45ea-97e8-e2becaeb6cad")
			};
		}

		public void ClearErrors()
		{
			Task[] tasks = _errorListProvider.Tasks.OfType<Task>().ToArray();
			_errorListProvider.SuspendRefresh();
			foreach (Task task in tasks)
			{
				_errorListProvider.Tasks.Remove(task);
			}

			_errorListProvider.ResumeRefresh();
		}

		public void DisplayErrors(IEnumerable<(ProjectItem, Exception)> errors)
		{
			_errorListProvider.SuspendRefresh();
			foreach ((ProjectItem item, Exception error) in errors)
			{
				Exception innerError = error;
				while (innerError?.InnerException != null)
				{
					innerError = innerError.InnerException;
				}

				var task = new ErrorTask(innerError)
				{
					CanDelete = true,
					Category = TaskCategory.User,
					ErrorCategory = TaskErrorCategory.Error,
					Document = item.Name,
					HierarchyItem = item.ContainingProject.GetIVsHierarchy()
				};

				task.Navigate += (sender, args) =>
				{
					UIHierarchy solutionExplorer = IdeHelper.ApplicationObject.ToolWindows.SolutionExplorer;
					UIHierarchyItem hierarchyItem = FindInExplorer(solutionExplorer.UIHierarchyItems, item);
					hierarchyItem?.Select(vsUISelectionType.vsUISelectionTypeSelect);
				};

				_errorListProvider.Tasks.Add(task);
			}

			_errorListProvider.ResumeRefresh();
			_errorListProvider.Show();
			_errorListProvider.BringToFront();
		}

		private UIHierarchyItem FindInExplorer(UIHierarchyItems items, ProjectItem target)
		{
			foreach (UIHierarchyItem hierarchyItem in items.OfType<UIHierarchyItem>())
			{
				if (hierarchyItem.Object is ProjectItem projectItem && projectItem == target)
				{
					return hierarchyItem;
				}

				if (hierarchyItem.UIHierarchyItems != null)
				{
					UIHierarchyItem inner = FindInExplorer(hierarchyItem.UIHierarchyItems, target);
					if (inner != null)
					{
						return inner;
					}
				}
			}

			return null;
		}

	}
}