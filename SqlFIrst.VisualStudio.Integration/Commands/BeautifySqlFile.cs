using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using SqlFirst.VisualStudio.Integration.Logic;
using SqlFIrst.VisualStudio.Integration.Helpers;
using Task = System.Threading.Tasks.Task;

namespace SqlFirst.VisualStudio.Integration.Commands
{
	/// <summary>
	/// Command handler
	/// </summary>
	internal sealed class BeautifySqlFile
	{
		/// <summary>
		/// Command ID.
		/// </summary>
		public const int CommandId = 0x9630;

		/// <summary>
		/// VS Package that provides this command, not null.
		/// </summary>
		private readonly Package _package;

		/// <summary>
		/// Gets the instance of the command.
		/// </summary>
		public static BeautifySqlFile Instance { get; private set; }

		/// <summary>
		/// Gets the service provider from the owner package.
		/// </summary>
		private IServiceProvider ServiceProvider => _package;

		/// <summary>
		/// Initializes a new instance of the <see cref="BeautifySqlFile" /> class.
		/// Adds our command handlers for menu (commands must exist in the command table file)
		/// </summary>
		/// <param name="package">Owner package, not null.</param>
		private BeautifySqlFile(Package package)
		{
			_package = package ?? throw new ArgumentNullException(nameof(package));

			if (ServiceProvider.GetService(typeof(IMenuCommandService)) is OleMenuCommandService commandService)
			{
				var menuCommandId = new CommandID(_commandSet, CommandId);
				var menuItem = new MenuCommand(MenuItemCallback, menuCommandId);
				commandService.AddCommand(menuItem);
			}
		}

		/// <summary>
		/// Initializes the singleton instance of the command.
		/// </summary>
		/// <param name="package">Owner package, not null.</param>
		public static void Initialize(Package package)
		{
			Instance = new BeautifySqlFile(package);
		}

		/// <summary>
		/// Command menu group (command set GUID).
		/// </summary>
		public static readonly Guid _commandSet = new Guid("02a34c8a-02ae-401c-9566-7ed255a4d535");

		/// <summary>
		/// This function is the callback used to execute the command when the menu item is clicked.
		/// See the constructor to see how the menu item is associated with this function using
		/// OleMenuCommandService service and MenuCommand class.
		/// </summary>
		/// <param name="sender">Event sender.</param>
		/// <param name="e">Event args.</param>
		private void MenuItemCallback(object sender, EventArgs e)
		{
			SqlFirstOutputWindow.Pane.Clear();
			SqlFirstOutputWindow.Pane.Activate();
			IEnumerable<ProjectItem> selectedItems = IdeHelper.GetSelectedItems();
			var provider = new ProjectItemsPerformer(ServiceProvider);
			Task.Run(() => provider.BeautifyFiles(selectedItems));
		}
	}
}