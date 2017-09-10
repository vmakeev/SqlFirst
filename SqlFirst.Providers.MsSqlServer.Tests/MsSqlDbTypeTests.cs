using Xunit;
using Xunit.Should;

namespace SqlFirst.Providers.MsSqlServer.Tests
{
	public class MsSqlDbTypeTests
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
		public void NormalizeTest(string input, string output)
		{
			MsSqlDbType.Normalize(input).ShouldBe(output);
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
		public void GetLengthTest(string input, string output)
		{
			MsSqlDbType.GetLength(input).ShouldBe(output);
		}
	}
}