using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace SqlFirst.Codegen.Helpers
{
	internal class CacheableEnumerable<T> : IEnumerable<T>
	{
		private readonly Lazy<List<T>> _valueFactory;

		/// <inheritdoc />
		public CacheableEnumerable(IEnumerable<T> source)
		{
			if (source == null)
			{
				throw new ArgumentNullException(nameof(source));
			}

			_valueFactory = new Lazy<List<T>>(source.ToList, LazyThreadSafetyMode.ExecutionAndPublication);
		}

		/// <inheritdoc />
		public IEnumerator<T> GetEnumerator()
		{
			return _valueFactory.Value.GetEnumerator();
		}

		/// <inheritdoc />
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable)_valueFactory.Value).GetEnumerator();
		}
	}
}