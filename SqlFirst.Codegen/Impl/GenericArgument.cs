using System.Collections.Generic;
using SqlFirst.Codegen.Trees;

namespace SqlFirst.Codegen.Impl
{
	/// <inheritdoc />
	public class GenericArgument : IGenericArgument
	{
		/// <inheritdoc />
		public string GenericTemplateName { get; set; }

		/// <inheritdoc />
		public bool IsGeneric { get; set; }

		/// <inheritdoc />
		public IEnumerable<IGenericArgument> GenericArguments { get; set; } = new IGenericArgument[0];
	}
}