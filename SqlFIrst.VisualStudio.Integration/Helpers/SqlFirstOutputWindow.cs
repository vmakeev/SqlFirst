using System;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;

namespace SqlFirst.VisualStudio.Integration.Helpers
{
	internal static class SqlFirstOutputWindow
	{
		private static OutputWindowPane _pane;

		public static OutputWindowPane Pane => _pane ?? (_pane = GetPane());

		private static OutputWindowPane GetPane()
		{
			var dte = (DTE2)Package.GetGlobalService(typeof(DTE));
			OutputWindow outputWindow = dte.ToolWindows.OutputWindow;

			for (uint i = 1; i <= outputWindow.OutputWindowPanes.Count; i++)
			{
				if (outputWindow.OutputWindowPanes.Item(i).Name.Equals("SqlFirst", StringComparison.CurrentCultureIgnoreCase))
				{
					return outputWindow.OutputWindowPanes.Item(i);
				}
			}

			return outputWindow.OutputWindowPanes.Add("SqlFirst");
		}
	}
}