using System.Collections.Generic;
using SqlFirst.Codegen.Trees;

namespace SqlFirst.Codegen.Impl
{
	public class GeneratedQueryObject : IGeneratedQueryObject
	{
		/// <inheritdoc />
		public IEnumerable<string> Usings { get; set; }

		/// <inheritdoc />
		public string Namespace { get; set; }

		/// <inheritdoc />
		public string ItemName { get; set; }

		/// <inheritdoc />
		public IEnumerable<string> ItemModifiers { get; set; }

		/// <inheritdoc />
		public IEnumerable<IGeneratedType> BaseTypes { get; set; }

		/// <inheritdoc />
		public string Item { get; set; }
	}
}