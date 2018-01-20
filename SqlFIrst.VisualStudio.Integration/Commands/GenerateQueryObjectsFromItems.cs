using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Threading;
using Common.Logging;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using SqlFirst.Codegen;
using SqlFirst.VisualStudio.Integration.Helpers;
using SqlFirst.VisualStudio.Integration.Logic;
using SqlFIrst.VisualStudio.Integration.Helpers;
using Task = System.Threading.Tasks.Task;

namespace SqlFirst.VisualStudio.Integration.Commands
{
	/// <summary>
	/// Command handler
	/// </summary>
	internal sealed class GenerateQueryObjectsFromItems
	{
		/// <summary>
		/// Command ID.
		/// </summary>
		public const int CommandId = 0x962F;

		/// <summary>
		/// VS Package that provides this command, not null.
		/// </summary>
		private readonly Package _package;

		/// <summary>
		/// Command menu group (command set GUID).
		/// </summary>
		public static Guid CommandSet { get; } = new Guid("02a34c8a-02ae-401c-9566-7ed255a4d535");

		/// <summary>
		/// Gets the instance of the command.
		/// </summary>
		public static GenerateQueryObjectsFromItems Instance { get; private set; }

		/// <summary>
		/// Gets the service provider from the owner package.
		/// </summary>
		private IServiceProvider ServiceProvider => _package;

		/// <summary>
		/// Initializes a new instance of the <see cref="GenerateQueryObjectsFromItems" /> class.
		/// Adds our command handlers for menu (commands must exist in the command table file)
		/// </summary>
		/// <param name="package">Owner package, not null.</param>
		private GenerateQueryObjectsFromItems(Package package)
		{
			_package = package ?? throw new ArgumentNullException(nameof(package));

			if (ServiceProvider.GetService(typeof(IMenuCommandService)) is OleMenuCommandService commandService)
			{
				var menuCommandId = new CommandID(CommandSet, CommandId);
				var oleMenuItem = new OleMenuCommand(MenuItemCallback, menuCommandId);
				oleMenuItem.BeforeQueryStatus += OnBeforeQueryStatus;
				commandService.AddCommand(oleMenuItem);
			}
		}

		/// <summary>
		/// Initializes the singleton instance of the command.
		/// </summary>
		/// <param name="package">Owner package, not null.</param>
		public static void Initialize(Package package)
		{
			Instance = new GenerateQueryObjectsFromItems(package);
		}

		private void OnBeforeQueryStatus(object sender, EventArgs e)
		{
			if (sender is OleMenuCommand menuCommand)
			{
				IEnumerable<ProjectItem> selectedItems = IdeHelper.GetSelectedItems();
				menuCommand.Enabled = selectedItems.All(projectItem => projectItem.Name.EndsWith(".sql", StringComparison.OrdinalIgnoreCase));
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

			var itemsPerformer = new ProjectItemsPerformer(ServiceProvider);
			Task.Run(() => itemsPerformer.GenerateQueryObjectsAsync(selected, CancellationToken.None));
		}
	}
}