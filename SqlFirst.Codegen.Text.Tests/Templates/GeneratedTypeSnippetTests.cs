using FakeItEasy;
using Shouldly;
using SqlFirst.Codegen.Text.Snippets;
using SqlFirst.Codegen.Text.Templating;
using SqlFirst.Codegen.Trees;
using Xunit;

namespace SqlFirst.Codegen.Text.Tests.Templates
{
	public class GeneratedTypeSnippetTests
	{
		[Fact]
		public void Test_1()
		{
			var argument1 = A.Fake<IGenericArgument>(p => p.Strict());
			A.CallTo(() => argument1.Type).Returns("string");
			A.CallTo(() => argument1.IsGeneric).Returns(false);

			var argument2 = A.Fake<IGenericArgument>(p => p.Strict());
			A.CallTo(() => argument2.Type).Returns("object");
			A.CallTo(() => argument2.IsGeneric).Returns(false);

			var model = A.Fake<IGeneratedType>(p => p.Strict());
			A.CallTo(() => model.Name).Returns("Dictionary");
			A.CallTo(() => model.IsGeneric).Returns(true);
			A.CallTo(() => model.IsInterface).Returns(false);

			A.CallTo(() => model.GenericArguments).Returns(new[] { argument1, argument2 });

			IRenderableTemplate<IGeneratedType> template = Snippet.Item.Type.GeneratedType;

			string result = template.Render(model);
			result.ShouldBe("Dictionary<string, object>");
		}

		[Fact]
		public void Test_2()
		{
			var argument1 = A.Fake<IGenericArgument>(p => p.Strict());
			A.CallTo(() => argument1.Type).Returns("string");
			A.CallTo(() => argument1.IsGeneric).Returns(false);

			var argument2Argument1 = A.Fake<IGenericArgument>(p => p.Strict());
			A.CallTo(() => argument2Argument1.Type).Returns("int");
			A.CallTo(() => argument2Argument1.IsGeneric).Returns(false);

			var argument2 = A.Fake<IGenericArgument>(p => p.Strict());
			A.CallTo(() => argument2.Type).Returns("IEnumerable");
			A.CallTo(() => argument2.IsGeneric).Returns(true);
			A.CallTo(() => argument2.GenericArguments).Returns(new[] { argument2Argument1 });

			var model = A.Fake<IGeneratedType>(p => p.Strict());
			A.CallTo(() => model.Name).Returns("Dictionary");
			A.CallTo(() => model.IsGeneric).Returns(true);
			A.CallTo(() => model.IsInterface).Returns(false);

			A.CallTo(() => model.GenericArguments).Returns(new[] { argument1, argument2 });

			IRenderableTemplate<IGeneratedType> template = Snippet.Item.Type.GeneratedType;

			string result = template.Render(model);
			result.ShouldBe("Dictionary<string, IEnumerable<int>>");
		}

		[Fact]
		public void Test_3()
		{
			var model = A.Fake<IGeneratedType>(p => p.Strict());
			A.CallTo(() => model.Name).Returns("bool");
			A.CallTo(() => model.IsGeneric).Returns(false);
			A.CallTo(() => model.IsInterface).Returns(false);

			IRenderableTemplate<IGeneratedType> template = Snippet.Item.Type.GeneratedType;

			string result = template.Render(model);
			result.ShouldBe("bool");
		}

	}
}
