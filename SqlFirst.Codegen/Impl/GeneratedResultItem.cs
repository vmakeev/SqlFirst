using System.Collections.Generic;
using SqlFirst.Codegen.Trees;

namespace SqlFirst.Codegen.Impl
{
	/// <inheritdoc />
	public class GeneratedResultItem : IGeneratedResultItem
	{
		/// <inheritdoc />
		public IEnumerable<string> Usings { get; set; } = new string[0];

		/// <inheritdoc />
		public string Namespace { get; set; }

		/// <inheritdoc />
		public string ItemName { get; set; }

		/// <inheritdoc />
		public IEnumerable<string> ItemModifiers { get; set; } = new string[0];

		/// <inheritdoc />
		public IEnumerable<IGeneratedType> BaseTypes { get; set; } = new IGeneratedType[0];

		/// <inheritdoc />
		public string Item { get; set; }
	}
}