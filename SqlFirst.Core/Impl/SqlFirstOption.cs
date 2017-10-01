using System.Collections.Generic;
using System.Linq;

namespace SqlFirst.Core.Impl
{
	/// <inheritdoc />
	public class SqlFirstOption : ISqlFirstOption
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Object"></see> class.</summary>
		public SqlFirstOption(string name, IEnumerable<string> parameters)
		{
			Name = name;
			Parameters = parameters?.ToArray() ?? new string[0];
		}

		/// <inheritdoc />
		public string Name { get; }

		/// <inheritdoc />
		public string[] Parameters { get; }
	}
}