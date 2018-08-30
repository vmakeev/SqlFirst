using Common.Logging;
using Common.Logging.Configuration;
using Common.Logging.Simple;

namespace SqlFIrst.VisualStudio.Integration.Logging
{
	public class VsOutputAdapter : AbstractSimpleLoggerFactoryAdapter
	{
		/// <inheritdoc />
		public VsOutputAdapter(NameValueCollection properties)
			: base(properties)
		{
		}

		public VsOutputAdapter() 
			: base(LogLevel.All, true, true, true, null)
		{
		}

		/// <inheritdoc />
		protected override ILog CreateLogger(string name, LogLevel level, bool showLevel, bool showDateTime, bool showLogName, string dateTimeFormat)
		{
			return new VsOutputLogger(name, level, showLevel, showDateTime, showLogName, dateTimeFormat);
		}
	}
}