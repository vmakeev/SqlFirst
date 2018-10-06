using Common.Logging;

namespace SqlFirst.VisualStudio.Integration.Logging
{
	internal static class CommonLoggingConfiguration
	{
		private static bool _isInitialized;
		private static readonly object _locker = new object();

		public static void EnsureOutputEnabled()
		{
			lock (_locker)
			{
				if (_isInitialized)
				{
					return;
				}

				LogManager.Adapter = new VsOutputAdapter();

				_isInitialized = true;
			}
		}
	}
}
