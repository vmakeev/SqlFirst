using System.Collections.Generic;
using SqlFirst.Codegen.Trees;

namespace SqlFirst.Codegen.Impl
{
	/// <inheritdoc />
	public class GeneratedType : IGeneratedType
	{
		/// <inheritdoc />
		public string TypeName { get; set; }

		/// <inheritdoc />
		public bool IsInterface { get; set; }

		/// <inheritdoc />
		public bool IsGeneric { get; set; }

		/// <inheritdoc />
		public IEnumerable<IGenericArgument> GenericArguments { get; set; } = new IGenericArgument[0];

		/// <inheritdoc />
		public IEnumerable<string> GenericConditions { get; set; } = new string[0];
	}
}