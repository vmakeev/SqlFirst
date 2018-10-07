using System;
using System.Collections;
using System.Collections.Generic;
using SqlFirst.Core;

namespace SqlFirst.Providers.MsSqlServer
{
	internal class MsSqlServerTypeMetadata : IDictionary<string, object>
	{
		private static class Keys
		{
			public const string IsTableType = "IsTableType";
			public const string IsNullable = "IsNullable";
			public const string TableTypeColumns = "TableTypeColumns";
		}

		private readonly IDictionary<string, object> _innerDictionary;

		/// <summary>
		/// Является ли данный тип табличным
		/// </summary>
		public bool IsTableType
		{
			get => GetValue(Keys.IsTableType, false);
			set => _innerDictionary[Keys.IsTableType] = value;
		}

		/// <summary>
		/// Поддерживает ли тип значение null
		/// </summary>
		public bool? IsNullable
		{
			get => GetValue<bool?>(Keys.IsNullable, null);
			set => _innerDictionary[Keys.IsNullable] = value;
		}

		/// <summary>
		/// Описание столбцов для табличного типа
		/// </summary>
		public IEnumerable<IFieldDetails> TableTypeColumns
		{
			get => GetValue<IEnumerable<IFieldDetails>>(Keys.TableTypeColumns, null);
			set => _innerDictionary[Keys.TableTypeColumns] = value;
		}

		/// <inheritdoc />
		public MsSqlServerTypeMetadata()
			: this(new Dictionary<string, object>())
		{
		}

		/// <inheritdoc />
		public MsSqlServerTypeMetadata(IDictionary<string, object> dictionary)
		{
			_innerDictionary = dictionary ?? throw new ArgumentNullException(nameof(dictionary));
		}

		public static MsSqlServerTypeMetadata FromData(IDictionary<string, object> dictionary)
		{
			Dictionary<string, object> data = dictionary == null
				? new Dictionary<string, object>()
				: new Dictionary<string, object>(dictionary);

			return new MsSqlServerTypeMetadata(data);
		}

		private T GetValue<T>(string key, T defaultValue)
		{
			if (_innerDictionary.TryGetValue(key, out object valueObject) && valueObject is T value)
			{
				return value;
			}

			return defaultValue;
		}

		#region IDictionary impl

		/// <inheritdoc />
		IEnumerator<KeyValuePair<string, object>> IEnumerable<KeyValuePair<string, object>>.GetEnumerator()
		{
			return _innerDictionary.GetEnumerator();
		}

		/// <inheritdoc />
		IEnumerator IEnumerable.GetEnumerator()
		{
			return _innerDictionary.GetEnumerator();
		}

		/// <inheritdoc />
		void ICollection<KeyValuePair<string, object>>.Add(KeyValuePair<string, object> item)
		{
			_innerDictionary.Add(item);
		}

		/// <inheritdoc />
		void ICollection<KeyValuePair<string, object>>.Clear()
		{
			_innerDictionary.Clear();
		}

		/// <inheritdoc />
		bool ICollection<KeyValuePair<string, object>>.Contains(KeyValuePair<string, object> item)
		{
			return _innerDictionary.Contains(item);
		}

		/// <inheritdoc />
		void ICollection<KeyValuePair<string, object>>.CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
		{
			_innerDictionary.CopyTo(array, arrayIndex);
		}

		/// <inheritdoc />
		bool ICollection<KeyValuePair<string, object>>.Remove(KeyValuePair<string, object> item)
		{
			return _innerDictionary.Remove(item);
		}

		/// <inheritdoc />
		int ICollection<KeyValuePair<string, object>>.Count => _innerDictionary.Count;

		/// <inheritdoc />
		bool ICollection<KeyValuePair<string, object>>.IsReadOnly => _innerDictionary.IsReadOnly;

		/// <inheritdoc />
		bool IDictionary<string, object>.ContainsKey(string key)
		{
			return _innerDictionary.ContainsKey(key);
		}

		/// <inheritdoc />
		void IDictionary<string, object>.Add(string key, object value)
		{
			_innerDictionary.Add(key, value);
		}

		/// <inheritdoc />
		bool IDictionary<string, object>.Remove(string key)
		{
			return _innerDictionary.Remove(key);
		}

		/// <inheritdoc />
		bool IDictionary<string, object>.TryGetValue(string key, out object value)
		{
			return _innerDictionary.TryGetValue(key, out value);
		}

		/// <inheritdoc />
		object IDictionary<string, object>.this[string key]
		{
			get => _innerDictionary[key];
			set => _innerDictionary[key] = value;
		}

		/// <inheritdoc />
		ICollection<string> IDictionary<string, object>.Keys => _innerDictionary.Keys;

		/// <inheritdoc />
		ICollection<object> IDictionary<string, object>.Values => _innerDictionary.Values;

		#endregion
	}
}