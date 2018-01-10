using System;
using System.Collections.Generic;
using System.Linq;
using SqlFirst.Codegen.Helpers;
using SqlFirst.Codegen.Trees;

namespace SqlFirst.Codegen.Impl
{
	/// <inheritdoc />
	public class GeneratedType : IGeneratedType
	{
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

		private string CleanTypeName(Type type)
		{
			string name = CSharpCodeHelper.GetTypeBuiltInName(type);

			int genericIndex = name.IndexOf('<');
			if (genericIndex >= 0)
			{
				name = name.Substring(0, genericIndex);
			}

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
		public IEnumerable<IGenericArgument> GenericArguments { get; set; } = Enumerable.Empty<IGenericArgument>();
	}
}