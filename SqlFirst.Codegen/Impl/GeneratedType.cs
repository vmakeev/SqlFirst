using System;
using System.Collections.Generic;
using System.Linq;
using SqlFirst.Codegen.Helpers;
using SqlFirst.Codegen.Trees;

namespace SqlFirst.Codegen.Impl
{
	/// <inheritdoc />
	public sealed class GeneratedType : IGeneratedType
	{
		private IEnumerable<IGenericArgument> _genericArguments = Enumerable.Empty<IGenericArgument>();

		/// <inheritdoc />
		public GeneratedType()
		{
		}

		/// <inheritdoc />
		public GeneratedType(Type type)
		{
			Name = CleanTypeName(type);
			IsInterface = type.IsInterface;
			IsGeneric = type.IsGenericType;

			if (IsGeneric)
			{
				GenericArguments = GetGenericArguments(type);
			}
		}

		private static string CleanTypeName(Type type)
		{
			string name = CSharpCodeHelper.GetTypeBuiltInName(type);
			name = CSharpCodeHelper.GetValidTypeName(name, true);

			return name;
		}

		private IEnumerable<IGenericArgument> GetGenericArguments(Type type)
		{
			if (!type.IsGenericType)
			{
				yield break;
			}

			foreach (Type argument in type.GenericTypeArguments)
			{
				yield return GetGenericArgument(argument);
			}
		}

		private IGenericArgument GetGenericArgument(Type argument)
		{
			var result = new GenericArgument
			{
				Type = CleanTypeName(argument),
				IsGeneric = argument.IsGenericType,
				GenericArguments = argument.IsGenericType
					? GetGenericArguments(argument)
					: Enumerable.Empty<IGenericArgument>()
			};

			return result;
		}

		/// <inheritdoc />
		public string Name { get; set; }

		/// <inheritdoc />
		public bool IsInterface { get; set; }

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