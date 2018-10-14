using System.Collections.Generic;
using SqlFirst.Codegen.Helpers;
using SqlFirst.Core;

namespace SqlFirst.Codegen.Impl
{
	/// <inheritdoc />
	public sealed class ParameterGenerationOptions : IParameterGenerationOptions
	{
		private readonly IEnumerable<ISqlFirstOption> _sqlFirstOptions;

		/// <summary>Initializes a new instance of the <see cref="T:System.Object"></see> class.</summary>
		public ParameterGenerationOptions(IEnumerable<ISqlFirstOption> sqlFirstOptions)
		{
			_sqlFirstOptions = sqlFirstOptions;
		}

		/// <inheritdoc />
		public IEnumerable<ISqlFirstOption> SqlFirstOptions => _sqlFirstOptions.AsCacheable();
	}
}