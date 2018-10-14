using System.Collections.Generic;
using SqlFirst.Codegen.Helpers;
using SqlFirst.Codegen.Trees;

namespace SqlFirst.Codegen.Impl
{
	/// <inheritdoc />
	public sealed class GenericArgument : IGenericArgument
	{
		private IEnumerable<IGenericArgument> _genericArguments = new IGenericArgument[0];

		/// <inheritdoc />
		public string Type { get; set; }

		/// <inheritdoc />
		public bool IsGeneric { get; set; }

		/// <inheritdoc />
		public IEnumerable<IGenericArgument> GenericArguments
		{
			get => _genericArguments;
			set => _genericArguments = value.AsCacheable();
		}
	}
}