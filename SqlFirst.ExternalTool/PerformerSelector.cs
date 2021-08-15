using System.IO;
using SqlFirst.Intelligence.Options;

namespace SqlFirst.ExternalTool
{
	public static class PerformerSelector
	{
		public static IPerformer Select(GenerationOptions options)
		{
			if (!string.IsNullOrEmpty(options.Target) && Directory.Exists(options.Target))
			{
				return new FolderPerformer();
			}

			return new SingleFilePerformer();
		}
	}
}