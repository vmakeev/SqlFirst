using System;

namespace SqlFirst.Codegen.Helpers
{
	/// <summary>
	/// Помощник работы с типами
	/// </summary>
	internal static class TypesHelper
	{
		/// <summary>
		/// Проверяет, является ли тип обобщенным
		/// </summary>
		/// <param name="type">Тип</param>
		/// <param name="underlyingTypes">Найденные обобщенные параметры</param>
		/// <returns>True, если тип обобщенный и не открытый</returns>
		public static bool IsGenericType(Type type, out Type[] underlyingTypes)
		{
			if (type.IsGenericType && type.IsConstructedGenericType)
			{
				underlyingTypes = type.GenericTypeArguments;
				return true;
			}

			underlyingTypes = null;
			return false;
		}

		/// <summary>
		/// Проверяет, является ли тип Nullable&lt;T&gt;
		/// </summary>
		/// <param name="type">Тип</param>
		/// <param name="underlyingType">Собственно значимый тип</param>
		/// <returns>True, тип есть Nullable&lt;T&gt;</returns>
		public static bool IsNullableValueType(Type type, out Type underlyingType)
		{
			if (type.IsGenericType && type.IsConstructedGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
			{
				underlyingType = type.GenericTypeArguments[0];
				return true;
			}

			underlyingType = null;
			return false;
		}

		/// <summary>
		/// Проверяет, является ли тип массивом
		/// </summary>
		/// <param name="type">Тип</param>
		/// <param name="itemType">Тип элемента массива</param>
		/// <returns>True, тип является массивом</returns>
		public static bool IsArrayType(Type type, out Type itemType)
		{
			if (type.IsArray)
			{
				itemType = type.GetElementType();
				return true;
			}

			itemType = null;
			return false;
		}
	}
}