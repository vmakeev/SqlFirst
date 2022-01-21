using System.Collections.Generic;
using System.Linq;
using SqlFirst.Codegen.Trees;

namespace SqlFirst.Intelligence.Generators
{
	internal class GeneratedItemExtender : IGeneratedItem
	{
		private readonly IGeneratedItem _inner;
		private readonly IEnumerable<string> _additionalUsings;

		public GeneratedItemExtender(IGeneratedItem inner, IEnumerable<string> additionalUsings)
		{
			_inner = inner;
			_additionalUsings = additionalUsings;
		}

		/// <inheritdoc />
		public IEnumerable<string> Usings => _inner.Usings.Concat(_additionalUsings);

		/// <inheritdoc />
		public string Namespace => _inner.Namespace;

		/// <inheritdoc />
		public string Name => _inner.Name;

		/// <inheritdoc />
		public IEnumerable<string> Modifiers => _inner.Modifiers;

		/// <inheritdoc />
		public IEnumerable<IGeneratedType> BaseTypes => _inner.BaseTypes;

		/// <inheritdoc />
		public string Content => _inner.Content;

		/// <inheritdoc />
		public string ObjectType => _inner.ObjectType;

		/// <inheritdoc />
		public IEnumerable<string> BeforeItemData => _inner.BeforeItemData;
	}
}