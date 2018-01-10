using System.Collections.Generic;
using System.Linq;
using SqlFirst.Codegen.Trees;

namespace SqlFirst.Codegen.Impl
{
	public class GeneratedQueryObject : IGeneratedQueryObject
	{
		/// <inheritdoc />
		public IEnumerable<string> Usings { get; set; } = Enumerable.Empty<string>();

		/// <inheritdoc />
		public string Namespace { get; set; }

		/// <inheritdoc />
		public string Name { get; set; }

		/// <inheritdoc />
		public IEnumerable<string> Modifiers { get; set; } = Enumerable.Empty<string>();

		/// <inheritdoc />
		public IEnumerable<IGeneratedType> BaseTypes { get; set; } = Enumerable.Empty<IGeneratedType>();

		/// <inheritdoc />
		public string Content { get; set; }

		/// <inheritdoc />
		public string ObjectType { get; set; }
	}
}