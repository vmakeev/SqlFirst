using System.Collections.Generic;
using SqlFirst.Codegen.Helpers;
using SqlFirst.Core;

namespace SqlFirst.Codegen.Impl
{
	/// <inheritdoc />
	public sealed class ResultGenerationOptions : IResultGenerationOptions
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Object"></see> class.</summary>
		public ResultGenerationOptions(IEnumerable<ISqlFirstOption> sqlFirstOptions)
		{
			SqlFirstOptions = sqlFirstOptions.AsCacheable();
		}

		/// <inheritdoc />
		public IEnumerable<ISqlFirstOption> SqlFirstOptions { get; }
	}
}