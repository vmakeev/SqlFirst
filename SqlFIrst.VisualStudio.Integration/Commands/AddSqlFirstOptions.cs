using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using SqlFIrst.VisualStudio.Integration.Helpers;

namespace SqlFirst.VisualStudio.Integration.Commands
{
	/// <summary>
	/// Command handler
	/// </summary>
	internal sealed class AddSqlFirstOptions
	{
		/// <summary>
		/// Command ID.
		/// </summary>
		public const int CommandId = 0x11DF;

		/// <summary>
		/// VS Package that provides this command, not null.
		/// </summary>
		private readonly Package _package;

		/// <summary>
		/// Gets the instance of the command.
		/// </summary>
		public static AddSqlFirstOptions Instance { get; private set; }

		/// <summary>
		/// Gets the service provider from the owner package.
		/// </summary>
		private IServiceProvider ServiceProvider => _package;

		/// <summary>
		/// Initializes a new instance of the <see cref="GenerateQueryObjectFromFolder" /> class.
		/// Adds our command handlers for menu (commands must exist in the command table file)
		/// </summary>
		/// <param name="package">Owner package, not null.</param>
		private AddSqlFirstOptions(Package package)
		{
			_package = package ?? throw new ArgumentNullException(nameof(package));

			if (ServiceProvider.GetService(typeof(IMenuCommandService)) is OleMenuCommandService commandService)
			{
				var menuCommandId = new CommandID(_commandSet, CommandId);
				var oleMenuItem = new OleMenuCommand(MenuItemCallback, menuCommandId);
				oleMenuItem.BeforeQueryStatus += OnBeforeQueryStatus;
				commandService.AddCommand(oleMenuItem);
			}
		}

		private void OnBeforeQueryStatus(object sender, EventArgs e)
		{
			if (sender is OleMenuCommand menuCommand)
			{
				const string fileName = "SqlFirst.options";

				ICollection<ProjectItem> items = IdeHelper.GetSelectedItems().ToList();
				if (items.Count == 1)
				{
					string path = Path.Combine(items.Single().GetFullPath(), fileName);
					menuCommand.Text = File.Exists(path)
						? "Modify SqlFirst.options"
						: "Add SqlFirst.options";
				}
			}
		}

		/// <summary>
		/// Initializes the singleton instance of the command.
		/// </summary>
		/// <param name="package">Owner package, not null.</param>
		public static void Initialize(Package package)
		{
			Instance = new AddSqlFirstOptions(package);
		}

		/// <summary>
		/// Command menu group (command set GUID).
		/// </summary>
		public static readonly Guid _commandSet = new Guid("b2723acd-ece2-457d-9c4c-bef9ad610585");

		/// <summary>
		/// This function is the callback used to execute the command when the menu item is clicked.
		/// See the constructor to see how the menu item is associated with this function using
		/// OleMenuCommandService service and MenuCommand class.
		/// </summary>
		/// <param name="sender">Event sender.</param>
		/// <param name="e">Event args.</param>
		private void MenuItemCallback(object sender, EventArgs e)
		{
			string content = GetDefaultOptionsText();
			const string fileName = "SqlFirst.options";

			IEnumerable<ProjectItem> items = IdeHelper.GetSelectedItems();
			foreach (ProjectItem item in items)
			{
				string path = Path.Combine(item.GetFullPath(), fileName);
				if (!File.Exists(path))
				{
					File.WriteAllText(path, content);
				}

				IdeHelper.ApplicationObject.ItemOperations.OpenFile(path, Constants.vsViewKindTextView);
			}
		}

		private string GetDefaultOptionsText()
		{
			const string resourceName = "SqlFirst.VisualStudio.Integration.SqlFirst.options";
			Stream stream = GetType().Assembly.GetManifestResourceStream(resourceName);
			string queryText = new StreamReader(stream ?? throw new Exception($"Resource not found: {resourceName}")).ReadToEnd();
			return queryText;
		}
	}
}