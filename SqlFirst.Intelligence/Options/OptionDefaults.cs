using System;
using System.Collections.Generic;
using SqlFirst.Core;

namespace SqlFirst.Intelligence.Options
{
	public class OptionDefaults : Dictionary<string, IReadOnlyDictionary<string, bool>>, IOptionDefaults
	{
		/// <inheritdoc />
		public OptionDefaults()
			: base(StringComparer.InvariantCultureIgnoreCase)
		{
		}
	}
}