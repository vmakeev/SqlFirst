using System.Collections.Generic;
using System.Linq;
using SqlFirst.Codegen.Helpers;
using SqlFirst.Codegen.Trees;

namespace SqlFirst.Codegen.Impl
{
	/// <inheritdoc />
	public sealed class GeneratedParameterItem : IGeneratedParameterItem
	{
		private IEnumerable<string> _usings = Enumerable.Empty<string>();
		private IEnumerable<string> _modifiers = Enumerable.Empty<string>();
		private IEnumerable<IGeneratedType> _baseTypes = Enumerable.Empty<IGeneratedType>();
		private IEnumerable<string> _beforeItemData;

		/// <inheritdoc />
		public IEnumerable<string> Usings
		{
			get => _usings;
			set => _usings = value.AsCacheable();
		}

		/// <inheritdoc />
		public string Namespace { get; set; }

		/// <inheritdoc />
		public string Name { get; set; }

		/// <inheritdoc />
		public IEnumerable<string> Modifiers
		{
			get => _modifiers;
			set => _modifiers = value.AsCacheable();
		}

		/// <inheritdoc />
		public IEnumerable<IGeneratedType> BaseTypes
		{
			get => _baseTypes;
			set => _baseTypes = value.AsCacheable();
		}

		/// <inheritdoc />
		public string Content { get; set; }

		/// <inheritdoc />
		public string ObjectType { get; set; }

		/// <inheritdoc />
		public IEnumerable<string> BeforeItemData
		{
			get => _beforeItemData;
			set => _beforeItemData = value.AsCacheable();
		}
	}
}