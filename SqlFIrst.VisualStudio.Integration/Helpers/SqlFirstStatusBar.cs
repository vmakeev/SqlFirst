using System;
using Microsoft.VisualStudio.Shell.Interop;

namespace SqlFirst.VisualStudio.Integration.Helpers
{
	internal class SqlFirstStatusBar
	{
		private readonly IVsStatusbar _statusBar;

		/// <inheritdoc />
		public SqlFirstStatusBar(IServiceProvider serviceProvider)
		{
			_statusBar = (IVsStatusbar)serviceProvider.GetService(typeof(SVsStatusbar));
		}

		public void Display(string text)
		{
			_statusBar.IsFrozen(out int frozen);
			if (frozen != 0)
			{
				_statusBar.FreezeOutput(0);
			}

			_statusBar.SetText(text);
		}
	}
}