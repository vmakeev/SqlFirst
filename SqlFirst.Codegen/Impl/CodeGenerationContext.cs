using System.Collections.Generic;
using SqlFirst.Core;

namespace SqlFirst.Codegen.Impl
{
	/// <inheritdoc />
	public class CodeGenerationContext : ICodeGenerationContext
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Object"></see> class.</summary>
		public CodeGenerationContext(IEnumerable<IQueryParamInfo> incomingParameters, IEnumerable<IFieldDetails> outgoingParameters, IReadOnlyDictionary<string, object> options, IDatabaseTypeMapper typeMapper)
		{
			TypeMapper = typeMapper;
			IncomingParameters = incomingParameters ?? throw new System.ArgumentNullException(nameof(incomingParameters));
			OutgoingParameters = outgoingParameters ?? throw new System.ArgumentNullException(nameof(outgoingParameters));
			Options = options ?? throw new System.ArgumentNullException(nameof(options));
		}

		/// <inheritdoc />
		public IEnumerable<IQueryParamInfo> IncomingParameters { get; }

		/// <inheritdoc />
		public IEnumerable<IFieldDetails> OutgoingParameters { get; }

		/// <inheritdoc />
		public IReadOnlyDictionary<string, object> Options { get; }

		/// <inheritdoc />
		public IDatabaseTypeMapper TypeMapper { get; }
	}
}