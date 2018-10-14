using System;
using System.Collections.Generic;
using SqlFirst.Codegen.Helpers;
using SqlFirst.Core;

namespace SqlFirst.Codegen.Impl
{
	/// <inheritdoc />
	public sealed class CodeGenerationContext : ICodeGenerationContext
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Object"></see> class.</summary>
		public CodeGenerationContext(IEnumerable<IQueryParamInfo> incomingParameters,
			IEnumerable<IFieldDetails> outgoingParameters,
			IReadOnlyDictionary<string, object> options,
			IDatabaseTypeMapper typeMapper,
			IDatabaseProvider databaseProvider)
		{
			IncomingParameters = incomingParameters.AsCacheable() ?? throw new ArgumentNullException(nameof(incomingParameters));
			OutgoingParameters = outgoingParameters.AsCacheable() ?? throw new ArgumentNullException(nameof(outgoingParameters));
			TypeMapper = typeMapper ?? throw new ArgumentNullException(nameof(typeMapper));
			DatabaseProvider = databaseProvider ?? throw new ArgumentNullException(nameof(databaseProvider));
			Options = options ?? throw new ArgumentNullException(nameof(options));
		}

		/// <inheritdoc />
		public IEnumerable<IQueryParamInfo> IncomingParameters { get; }

		/// <inheritdoc />
		public IEnumerable<IFieldDetails> OutgoingParameters { get; }

		/// <inheritdoc />
		public IReadOnlyDictionary<string, object> Options { get; }

		/// <inheritdoc />
		public IDatabaseTypeMapper TypeMapper { get; }

		/// <inheritdoc />
		public IDatabaseProvider DatabaseProvider { get; }
	}
}