using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using Common.Logging;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using SqlFIrst.VisualStudio.Integration.Helpers;
using SqlFIrst.VisualStudio.Integration.Logging;
using SqlFIrst.VisualStudio.Integration.Logic;
using Task = System.Threading.Tasks.Task;

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

		private readonly SqlFirstErrorsWindow _errorWindow;

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
				var oleMenuItem = new OleMenuCommand(MenuItemCallback, menuCommandId);
				oleMenuItem.BeforeQueryStatus += OnBeforeQueryStatus;
				commandService.AddCommand(oleMenuItem);
			}

			_errorWindow = new SqlFirstErrorsWindow(ServiceProvider);
		}

		/// <summary>
		/// Initializes the singleton instance of the command.
		/// </summary>
		/// <param name="package">Owner package, not null.</param>
		public static void Initialize(Package package)
		{
			Instance = new GenerateQueryObjects(package);
		}

		private void OnBeforeQueryStatus(object sender, EventArgs e)
		{
			if (sender is OleMenuCommand menuCommand)
			{
				IEnumerable<ProjectItem> selectedItems = IdeHelper.GetSelectedItems();
				menuCommand.Visible = selectedItems.All(projectItem => projectItem.Name.EndsWith(".sql", StringComparison.OrdinalIgnoreCase));
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
			ProjectItem[] selected = IdeHelper.GetSelectedItems().ToArray();

			SqlFirstOutputWindow.Pane.Clear();
			SqlFirstOutputWindow.Pane.Activate();

			if (!selected.Any())
			{
				_log.Info("Nothing selected to generate query objects");
				return;
			}

			Task.Run(() => ProcessSelectedItems(selected));
		}

		private void ProcessSelectedItems(IEnumerable<ProjectItem> selected)
		{
			_errorWindow.ClearErrors();

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

			foreach (Project project in projects.Where(project => project.IsDirty))
			{
				project.Save();
			}

			if (errors.Any())
			{
				_errorWindow.DisplayErrors(errors);
			}
		}
	}
}