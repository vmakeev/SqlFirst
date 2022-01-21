using System;
using System.Collections.Generic;

namespace SqlFirst.Intelligence.Options
{
	public class SqlFirstDefaultsSection : Dictionary<string, bool>
	{
		/// <inheritdoc />
		public SqlFirstDefaultsSection()
			: base(StringComparer.InvariantCultureIgnoreCase)
		{
		}

		public static SqlFirstDefaultsSection Empty => new SqlFirstDefaultsSection();
	}
}