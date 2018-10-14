using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FakeItEasy;
using Shouldly;
using SqlFirst.Codegen.Helpers;
using Xunit;

namespace SqlFirst.Codegen.Tests
{
	[SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
	public class CacheableEnumerableTests
	{
		[Fact]
		public void SmokeTest()
		{
			IEnumerable<string> source = new[] { "test" };

			IEnumerable<string> cacheable = new CacheableEnumerable<string>(source);
			cacheable.ShouldNotBeSameAs(source);
			cacheable.Single().ShouldBe("test");
		}

		[Fact]
		public void SourceGenericEnumerableShouldNotBeCalledTwice()
		{
			var sourceEnumerator = new List<object>
			{
				new object(),
				new object(),
				new object()
			};

			var source = A.Fake<IEnumerable<object>>(options => options.Strict());
			A.CallTo(() => source.GetEnumerator()).ReturnsLazily(() => sourceEnumerator.GetEnumerator());

			var cacheableEnumerable = new CacheableEnumerable<object>(source);

			// GetEnumerator() not called 
			A.CallTo(() => source.GetEnumerator()).MustNotHaveHappened();

			int index = 0;
			// 1st enumeration
			foreach (object o in cacheableEnumerable)
			{
				o.ShouldBeSameAs(sourceEnumerator[index++]);
			}

			// GetEnumerator() called
			A.CallTo(() => source.GetEnumerator()).MustHaveHappened(Repeated.Like(count => count == 1));

			// 2nd enumeration
			index = 0;
			foreach (object o in cacheableEnumerable)
			{
				o.ShouldBeSameAs(sourceEnumerator[index++]);
			}

			// GetEnumerator() is called only once
			A.CallTo(() => source.GetEnumerator()).MustHaveHappened(Repeated.Like(count => count == 1));
		}

		[Fact]
		public void SourceGenericEnumerableShouldNotBeCalledTwice_ExtensionMethod()
		{
			var sourceEnumerator = new List<object>
			{
				new object(),
				new object(),
				new object()
			};

			var source = A.Fake<IEnumerable<object>>(options => options.Strict());
			A.CallTo(() => source.GetEnumerator()).ReturnsLazily(() => sourceEnumerator.GetEnumerator());

			IEnumerable<object> cacheableEnumerable = source.AsCacheable();

			// GetEnumerator() not called 
			A.CallTo(() => source.GetEnumerator()).MustNotHaveHappened();

			int index = 0;
			// 1st enumeration
			foreach (object o in cacheableEnumerable)
			{
				o.ShouldBeSameAs(sourceEnumerator[index++]);
			}

			// GetEnumerator() called
			A.CallTo(() => source.GetEnumerator()).MustHaveHappened(Repeated.Like(count => count == 1));

			// 2nd enumeration
			index = 0;
			foreach (object o in cacheableEnumerable)
			{
				o.ShouldBeSameAs(sourceEnumerator[index++]);
			}

			// GetEnumerator() is called only once
			A.CallTo(() => source.GetEnumerator()).MustHaveHappened(Repeated.Like(count => count == 1));
		}

		[Fact]
		public void SourceEnumerableAsNonGenericShouldNotBeCalledTwice()
		{
			var sourceEnumerator = new List<object>
			{
				new object(),
				new object(),
				new object()
			};

			var source = A.Fake<IEnumerable<object>>(options => options.Strict());

			// ((IEnumerable)source).GetEnumerator() won't be used
			A.CallTo(() => source.GetEnumerator()).ReturnsLazily(() => sourceEnumerator.GetEnumerator());

			IEnumerable<object> cacheableEnumerable = new CacheableEnumerable<object>(source);

			// GetEnumerator() not called 
			A.CallTo(() => source.GetEnumerator()).MustNotHaveHappened();

			int index = 0;
			// 1st enumeration
			foreach (object o in (IEnumerable)cacheableEnumerable)
			{
				o.ShouldBeSameAs(sourceEnumerator[index++]);
			}

			// GetEnumerator() called
			A.CallTo(() => source.GetEnumerator()).MustHaveHappened(Repeated.Like(count => count == 1));

			// 2nd enumeration
			index = 0;
			foreach (object o in (IEnumerable)cacheableEnumerable)
			{
				o.ShouldBeSameAs(sourceEnumerator[index++]);
			}

			// GetEnumerator() is called only once
			A.CallTo(() => source.GetEnumerator()).MustHaveHappened(Repeated.Like(count => count == 1));
		}

		[Fact]
		public void SourceEnumerableAsNonGenericShouldNotBeCalledTwice_ExtensionMethod()
		{
			var sourceEnumerator = new List<object>
			{
				new object(),
				new object(),
				new object()
			};

			var source = A.Fake<IEnumerable<object>>(options => options.Strict());

			// ((IEnumerable)source).GetEnumerator() won't be used
			A.CallTo(() => source.GetEnumerator()).ReturnsLazily(() => sourceEnumerator.GetEnumerator());

			IEnumerable<object> cacheableEnumerable = source.AsCacheable();

			// GetEnumerator() not called 
			A.CallTo(() => source.GetEnumerator()).MustNotHaveHappened();

			int index = 0;
			// 1st enumeration
			foreach (object o in (IEnumerable)cacheableEnumerable)
			{
				o.ShouldBeSameAs(sourceEnumerator[index++]);
			}

			// GetEnumerator() called
			A.CallTo(() => source.GetEnumerator()).MustHaveHappened(Repeated.Like(count => count == 1));

			// 2nd enumeration
			index = 0;
			foreach (object o in (IEnumerable)cacheableEnumerable)
			{
				o.ShouldBeSameAs(sourceEnumerator[index++]);
			}

			// GetEnumerator() is called only once
			A.CallTo(() => source.GetEnumerator()).MustHaveHappened(Repeated.Like(count => count == 1));
		}

		[Fact]
		[SuppressMessage("ReSharper", "ExpressionIsAlwaysNull")]
		public void SourceEnumerableIsNullIsNotAllowed()
		{
			IEnumerable<object> source = null;

			Assert.Throws<ArgumentNullException>(() => new CacheableEnumerable<object>(source)).ParamName.ShouldBe("source");
		}

		[Fact]
		[SuppressMessage("ReSharper", "ExpressionIsAlwaysNull")]
		public void SourceEnumerableIsNullMustBeNull_ExtensionMethod()
		{
			IEnumerable<object> source = null;

			IEnumerable<object> cacheable = source.AsCacheable();

			cacheable.ShouldBeNull();
		}
	}
}
