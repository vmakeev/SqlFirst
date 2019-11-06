using System;
using System.Text;
using Common.Logging;
using Common.Logging.Simple;
using EnvDTE;
using SqlFirst.VisualStudio.Integration.Helpers;

namespace SqlFirst.VisualStudio.Integration.Logging
{
	public sealed class VsOutputLogger : AbstractSimpleLogger
	{
		private readonly OutputWindowPane _outPane;

		/// <inheritdoc />
		public VsOutputLogger(string logName, LogLevel logLevel, bool showlevel, bool showDateTime, bool showLogName, string dateTimeFormat)
			: base(logName, logLevel, showlevel, showDateTime, showLogName, dateTimeFormat)
		{
			_outPane = SqlFirstOutputWindow.Pane;
			_outPane.Activate();
		}

		/// <inheritdoc />
		protected override void WriteInternal(LogLevel level, object message, Exception exception)
		{
			var sb = new StringBuilder();
			FormatOutput(sb, level, message, exception);
			_outPane.OutputString(sb + Environment.NewLine);
		}
	}
}