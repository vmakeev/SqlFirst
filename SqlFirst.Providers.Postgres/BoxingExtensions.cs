using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace SqlFirst.Providers.Postgres
{
	internal static class BoxingExtensions
	{
		public static T Unbox<T>(this object value)
		{
			Converter<object, T> converter = GetConverter<T>();
			return converter.Invoke(value);
		}

		private static T ReferenceField<T>(object value)
		{
			return value == DBNull.Value ? default(T) : (T)value;
		}

		private static T ValueField<T>(object value)
		{
			if (DBNull.Value == value)
			{
				throw new InvalidCastException($"{typeof(T)} can not be casted to null.");
			}

			return (T)value;
		}

		[SuppressMessage("ReSharper", "UnusedMember.Local")]
		private static TElem? NullableField<TElem>(object value) where TElem : struct
		{
			if (DBNull.Value == value)
			{
				return default(TElem?);
			}

			return (TElem)value;
		}

		private static bool IsNullableValueType(this Type type)
		{
			return type.IsGenericType && !type.IsGenericTypeDefinition && (typeof(Nullable<>) == type.GetGenericTypeDefinition());
		}

		private static Converter<object, T> GetConverter<T>()
		{
			Type type = typeof(T);

			Converter<object, T> typeConverter;

			if (!type.IsValueType)
			{
				typeConverter = ReferenceField<T>;
			}
			else
			{
				if (type.IsNullableValueType())
				{
					typeConverter = (Converter<object, T>)Delegate.CreateDelegate(
						typeof(Converter<object, T>),
						typeof(BoxingExtensions)
							.GetMethod("NullableField", BindingFlags.Static | BindingFlags.NonPublic)
							.MakeGenericMethod(type.GetGenericArguments()[0]));
				}
				else
				{
					typeConverter = ValueField<T>;
				}
			}

			return typeConverter;
		}
	}
}