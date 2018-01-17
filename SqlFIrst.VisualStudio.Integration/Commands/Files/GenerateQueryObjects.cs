using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using Common.Logging;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using SqlFIrst.VisualStudio.Integration.Helpers;
using SqlFIrst.VisualStudio.Integration.Logging;
using SqlFIrst.VisualStudio.Integration.Logic;

namespace SqlFIrst.VisualStudio.Integration.Commands.Files
{
	/// <summary>
	/// Command handler
	/// </summary>
	internal sealed class GenerateQueryObjects
	{
		/// <summary>
		/// Command ID.
		/// </summary>
		public const int CommandId = 0x70E7;

		private readonly ILog _log;

		/// <summary>
		/// VS Package that provides this command, not null.
		/// </summary>
		private readonly Package _package;

		private ErrorListProvider _errorListProvider;

		/// <summary>
		/// Command menu group (command set GUID).
		/// </summary>
		public static Guid CommandSet { get; } = new Guid("02a34c8a-02ae-401c-9566-7ed255a4d535");

		/// <summary>
		/// Gets the instance of the command.
		/// </summary>
		public static GenerateQueryObjects Instance { get; private set; }

		/// <summary>
		/// Gets the service provider from the owner package.
		/// </summary>
		private IServiceProvider ServiceProvider => _package;

		/// <summary>
		/// Initializes a new instance of the <see cref="GenerateQueryObjects" /> class.
		/// Adds our command handlers for menu (commands must exist in the command table file)
		/// </summary>
		/// <param name="package">Owner package, not null.</param>
		private GenerateQueryObjects(Package package)
		{
			CommonLoggingConfiguration.EnsureOutputEnabled();
			_log = LogManager.GetLogger<GenerateQueryObjects>();

			_package = package ?? throw new ArgumentNullException(nameof(package));

			if (ServiceProvider.GetService(typeof(IMenuCommandService)) is OleMenuCommandService commandService)
			{
				var menuCommandId = new CommandID(CommandSet, CommandId);
				var menuItem = new MenuCommand(MenuItemCallback, menuCommandId);
				commandService.AddCommand(menuItem);
			}

			_errorListProvider = new ErrorListProvider(ServiceProvider)
			{
				ProviderName = "SqlFirstErrorProvider",
				ProviderGuid = Guid.Parse("e5ef6ee6-13ba-45ea-97e8-e2becaeb6cad")
			};


		}

		/// <summary>
		/// Initializes the singleton instance of the command.
		/// </summary>
		/// <param name="package">Owner package, not null.</param>
		public static void Initialize(Package package)
		{
			Instance = new GenerateQueryObjects(package);
		}

		private IEnumerable<ProjectItem> GetSelectedItems()
		{
			var applicationObject = (DTE2)Package.GetGlobalService(typeof(DTE));
			UIHierarchy solutionExplorer = applicationObject.ToolWindows.SolutionExplorer;

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

		/// <summary>
		/// This function is the callback used to execute the command when the menu target is clicked.
		/// See the constructor to see how the menu target is associated with this function using
		/// OleMenuCommandService service and MenuCommand class.
		/// </summary>
		/// <param name="sender">Event sender.</param>
		/// <param name="e">Event args.</param>
		private void MenuItemCallback(object sender, EventArgs e)
		{
			ProjectItem[] selected = GetSelectedItems().ToArray();

			SqlFirstOutputWindow.Pane.Clear();
			SqlFirstOutputWindow.Pane.Activate();

			if (!selected.Any())
			{
				_log.Info("Nothing selected to generate query objects");
				return;
			}

			var errors = new LinkedList<(ProjectItem, Exception)>();
			var projects = new List<Project>();

			foreach (ProjectItem projectItem in selected)
			{
				try
				{
					Performer.Perform(projectItem);
				}
				catch (Exception ex)
				{
					_log.Error(ex);
					errors.AddLast((projectItem, ex));
				}

				if (!projects.Contains(projectItem.ContainingProject))
				{
					projects.Add(projectItem.ContainingProject);
				}
			}

			SaveDirtyProjects(projects);

			if (errors.Any())
			{
				DisplayErrors(errors);
			}
		}

		private void DisplayErrors(LinkedList<(ProjectItem, Exception)> errors)
		{
			_errorListProvider.Tasks.Clear();

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
					HierarchyItem = item.ContainingProject.GetIVsHierarchy(),
				};

				task.Navigate += (sender, args) =>
				{
					var applicationObject = (DTE2)Package.GetGlobalService(typeof(DTE));
					UIHierarchy solutionExplorer = applicationObject.ToolWindows.SolutionExplorer;

					UIHierarchyItem hierarchyItem = FindInExplorer(solutionExplorer.UIHierarchyItems, item);
					hierarchyItem?.Select(vsUISelectionType.vsUISelectionTypeSelect);
				};

				_errorListProvider.Tasks.Add(task);
			}
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

		private static void SaveDirtyProjects(IEnumerable<Project> projects)
		{
			foreach (Project project in projects)
			{
				if (project.IsDirty)
				{
					project.Save();
				}
			}
		}
	}
}