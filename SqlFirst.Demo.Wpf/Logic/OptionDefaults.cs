using System.Collections.Generic;
using SqlFirst.Core;

namespace SqlFirst.Demo.Wpf.Logic
{
	internal class OptionDefaults : Dictionary<string, IReadOnlyDictionary<string, bool>>, IOptionDefaults
	{
		public static OptionDefaults Empty => new OptionDefaults();
	}
}