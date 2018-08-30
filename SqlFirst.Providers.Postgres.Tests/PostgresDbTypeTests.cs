using Shouldly;
using Xunit;

namespace SqlFirst.Providers.Postgres.Tests
{
	public class PostgresDbTypeTests
	{
		[Theory]
		[InlineData(null, null)]
		[InlineData("", "")]
		[InlineData(" ", "")]
		[InlineData("test", "test")]
		[InlineData("test ", "test")]
		[InlineData(" test ", "test")]
		[InlineData(" test", "test")]
		[InlineData("TesT", "test")]
		[InlineData("two woRds", "two words")]
		[InlineData("two woRds (max)", "two words")]
		[InlineData("two woRds(max)", "two words")]
		[InlineData("test(123)", "test")]
		[InlineData(" test(123)", "test")]
		[InlineData(" test(123) ", "test")]
		[InlineData("test(123) ", "test")]
		[InlineData("test (123) ", "test")]
		[InlineData("test (123 ) ", "test")]
		[InlineData("test ( 123 ) ", "test")]
		[InlineData("test ( 123) ", "test")]
		[InlineData("timestamp ( 123)   without     time zone", "timestamp without time zone")]
		public void NormalizeTest(string input, string output)
		{
			PostgresDbType.Normalize(input).ShouldBe(output);
		}

		[Theory]
		[InlineData(null, null)]
		[InlineData("", null)]
		[InlineData(" ", null)]
		[InlineData("test", null)]
		[InlineData("test ", null)]
		[InlineData(" test ", null)]
		[InlineData(" test", null)]
		[InlineData("TesT", null)]
		[InlineData("two woRds", null)]
		[InlineData("two woRds (max)", "max")]
		[InlineData("two woRds(max)", "max")]
		[InlineData("test(123)", "123")]
		[InlineData(" test(123)", "123")]
		[InlineData(" test(123) ", "123")]
		[InlineData("test(123) ", "123")]
		[InlineData("test (123) ", "123")]
		[InlineData("test (123 ) ", "123")]
		[InlineData("test ( 123 ) ", "123")]
		[InlineData("test ( 123) ", "123")]
		[InlineData("timestamp ( 123)   without     time zone", "123")]
		public void GetLengthTest(string input, string output)
		{
			PostgresDbType.GetLength(input).ShouldBe(output);
		}
	}
}