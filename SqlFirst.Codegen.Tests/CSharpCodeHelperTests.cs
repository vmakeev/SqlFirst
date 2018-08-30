using System;
using System.Collections.Generic;
using Shouldly;
using SqlFirst.Codegen.Helpers;
using Xunit;

namespace SqlFirst.Codegen.Tests
{
	public class CSharpCodeHelperTests
	{
		[Theory]
		[InlineData(null, "_")]
		[InlineData("", "_")]
		[InlineData("_", "_")]
		[InlineData("__", "__")]
		[InlineData("3", "_3")]
		[InlineData("!", "_")]
		[InlineData("%$#&###@@%^$%", "_")]
		[InlineData("variable", "variable")]
		[InlineData("this", "@this")]
		[InlineData("!! nu-l=l :)", "@null")]
		[InlineData("int", "@int")]
		[InlineData("@int", "@int")]
		[InlineData("@variable", "variable")]
		[InlineData("кириллица", "кириллица")]
		[InlineData("2x2", "_2x2")]
		[InlineData("2x2=4", "_2x24")]
		[InlineData("tooMany Words с пробелами", "tooManyWordsспробелами")]
		[InlineData("in;valid#symbols_every-where", "invalidsymbols_everywhere")]
		public void GetValidIdentifierNameTests(string input, string output)
		{
			CSharpCodeHelper.GetValidIdentifierName(input).ShouldBe(output);
		}

		[Theory]

		#region A lot of test cases

		#region CamelCase

		[InlineData(NamingPolicy.CamelCase, null, "_")]
		[InlineData(NamingPolicy.CamelCase, "", "_")]
		[InlineData(NamingPolicy.CamelCase, "_", "_")]
		[InlineData(NamingPolicy.CamelCase, "__", "_")]
		[InlineData(NamingPolicy.CamelCase, "3", "_3")]
		[InlineData(NamingPolicy.CamelCase, "!", "_")]
		[InlineData(NamingPolicy.CamelCase, "%$#&###@@%^$%", "_")]
		[InlineData(NamingPolicy.CamelCase, "variable", "variable")]
		[InlineData(NamingPolicy.CamelCase, "this", "@this")]
		[InlineData(NamingPolicy.CamelCase, "!! nu-l=l :)", "nuLL")]
		[InlineData(NamingPolicy.CamelCase, "!! null :)", "@null")]
		[InlineData(NamingPolicy.CamelCase, "int", "@int")]
		[InlineData(NamingPolicy.CamelCase, "@int", "@int")]
		[InlineData(NamingPolicy.CamelCase, "@variable", "variable")]
		[InlineData(NamingPolicy.CamelCase, "кириллица", "кириллица")]
		[InlineData(NamingPolicy.CamelCase, "2x2", "_2x2")]
		[InlineData(NamingPolicy.CamelCase, "2x2=4", "_2x24")]
		[InlineData(NamingPolicy.CamelCase, "tooMany Words с пробелами", "tooManyWordsСПробелами")]
		[InlineData(NamingPolicy.CamelCase, "in;valid#symbols_every-where", "inValidSymbolsEveryWhere")]

		#endregion

		#region CamelCaseWithUnderscope

		[InlineData(NamingPolicy.CamelCaseWithUnderscope, null, "_")]
		[InlineData(NamingPolicy.CamelCaseWithUnderscope, "", "_")]
		[InlineData(NamingPolicy.CamelCaseWithUnderscope, "_", "_")]
		[InlineData(NamingPolicy.CamelCaseWithUnderscope, "__", "_")]
		[InlineData(NamingPolicy.CamelCaseWithUnderscope, "3", "_3")]
		[InlineData(NamingPolicy.CamelCaseWithUnderscope, "!", "_")]
		[InlineData(NamingPolicy.CamelCaseWithUnderscope, "%$#&###@@%^$%", "_")]
		[InlineData(NamingPolicy.CamelCaseWithUnderscope, "variable", "_variable")]
		[InlineData(NamingPolicy.CamelCaseWithUnderscope, "this", "_this")]
		[InlineData(NamingPolicy.CamelCaseWithUnderscope, "!! nu-l=l :)", "_nuLL")]
		[InlineData(NamingPolicy.CamelCaseWithUnderscope, "!! null :)", "_null")]
		[InlineData(NamingPolicy.CamelCaseWithUnderscope, "int", "_int")]
		[InlineData(NamingPolicy.CamelCaseWithUnderscope, "@int", "_int")]
		[InlineData(NamingPolicy.CamelCaseWithUnderscope, "@variable", "_variable")]
		[InlineData(NamingPolicy.CamelCaseWithUnderscope, "кириллица", "_кириллица")]
		[InlineData(NamingPolicy.CamelCaseWithUnderscope, "2x2", "_2x2")]
		[InlineData(NamingPolicy.CamelCaseWithUnderscope, "2x2=4", "_2x24")]
		[InlineData(NamingPolicy.CamelCaseWithUnderscope, "tooMany Words с пробелами", "_tooManyWordsСПробелами")]
		[InlineData(NamingPolicy.CamelCaseWithUnderscope, "in;valid#symbols_every-where", "_inValidSymbolsEveryWhere")]

		#endregion

		#region Pascal

		[InlineData(NamingPolicy.Pascal, null, "_")]
		[InlineData(NamingPolicy.Pascal, "", "_")]
		[InlineData(NamingPolicy.Pascal, "_", "_")]
		[InlineData(NamingPolicy.Pascal, "__", "_")]
		[InlineData(NamingPolicy.Pascal, "3", "_3")]
		[InlineData(NamingPolicy.Pascal, "!", "_")]
		[InlineData(NamingPolicy.Pascal, "%$#&###@@%^$%", "_")]
		[InlineData(NamingPolicy.Pascal, "variable", "Variable")]
		[InlineData(NamingPolicy.Pascal, "this", "This")]
		[InlineData(NamingPolicy.Pascal, "!! nu-l=l :)", "NuLL")]
		[InlineData(NamingPolicy.Pascal, "!! null :)", "Null")]
		[InlineData(NamingPolicy.Pascal, "int", "Int")]
		[InlineData(NamingPolicy.Pascal, "@int", "Int")]
		[InlineData(NamingPolicy.Pascal, "@variable", "Variable")]
		[InlineData(NamingPolicy.Pascal, "кириллица", "Кириллица")]
		[InlineData(NamingPolicy.Pascal, "2x2", "_2x2")]
		[InlineData(NamingPolicy.Pascal, "2x2=4", "_2x24")]
		[InlineData(NamingPolicy.Pascal, "tooMany Words с пробелами", "TooManyWordsСПробелами")]
		[InlineData(NamingPolicy.Pascal, "in;valid#symbols_every-where", "InValidSymbolsEveryWhere")]

		#endregion

		#region Underscope

		[InlineData(NamingPolicy.Underscope, null, "_")]
		[InlineData(NamingPolicy.Underscope, "", "_")]
		[InlineData(NamingPolicy.Underscope, "_", "_")]
		[InlineData(NamingPolicy.Underscope, "__", "_")]
		[InlineData(NamingPolicy.Underscope, "3", "_3")]
		[InlineData(NamingPolicy.Underscope, "!", "_")]
		[InlineData(NamingPolicy.Underscope, "%$#&###@@%^$%", "_")]
		[InlineData(NamingPolicy.Underscope, "variable", "VARIABLE")]
		[InlineData(NamingPolicy.Underscope, "this", "THIS")]
		[InlineData(NamingPolicy.Underscope, "!! nu-l=l :)", "NU_L_L")]
		[InlineData(NamingPolicy.Underscope, "!! null :)", "NULL")]
		[InlineData(NamingPolicy.Underscope, "int", "INT")]
		[InlineData(NamingPolicy.Underscope, "@int", "INT")]
		[InlineData(NamingPolicy.Underscope, "@variable", "VARIABLE")]
		[InlineData(NamingPolicy.Underscope, "кириллица", "КИРИЛЛИЦА")]
		[InlineData(NamingPolicy.Underscope, "2x2", "_2X2")]
		[InlineData(NamingPolicy.Underscope, "2x2=4", "_2X2_4")]
		[InlineData(NamingPolicy.Underscope, "tooMany Words с пробелами", "TOO_MANY_WORDS_С_ПРОБЕЛАМИ")]
		[InlineData(NamingPolicy.Underscope, "in;valid#symbols_every-where", "IN_VALID_SYMBOLS_EVERY_WHERE")]

		#endregion

		#endregion

		public void GetValidIdentifierNameWithFormatTests(NamingPolicy policy, string input, string output)
		{
			CSharpCodeHelper.GetValidIdentifierName(input, policy).ShouldBe(output);
		}

		[Fact]
		public void GetValidIdentifierNameWithInvalidFormatTests()
		{
			Assert.Throws<ArgumentOutOfRangeException>(() => CSharpCodeHelper.GetValidIdentifierName("variable", (NamingPolicy)42));
		}

		[Theory]
		[InlineData(typeof(int), "System.Int32")]
		[InlineData(typeof(DateTime?), "System.Nullable<System.DateTime>")]
		[InlineData(typeof(string[]), "System.String[]")]
		[InlineData(typeof(List<string>), "System.Collections.Generic.List<System.String>")]
		[InlineData(typeof(List<string[]>), "System.Collections.Generic.List<System.String[]>")]
		[InlineData(typeof(IEnumerable<List<string>>), "System.Collections.Generic.IEnumerable<System.Collections.Generic.List<System.String>>")]
		[InlineData(typeof(Dictionary<int, decimal>), "System.Collections.Generic.Dictionary<System.Int32, System.Decimal>")]
		[InlineData(typeof(Dictionary<int, decimal>[]), "System.Collections.Generic.Dictionary<System.Int32, System.Decimal>[]")]
		[InlineData(typeof(List<Dictionary<int, decimal>[]>), "System.Collections.Generic.List<System.Collections.Generic.Dictionary<System.Int32, System.Decimal>[]>")]
		[InlineData(typeof(List<Dictionary<int, decimal>[]>[]), "System.Collections.Generic.List<System.Collections.Generic.Dictionary<System.Int32, System.Decimal>[]>[]")]
		[InlineData(typeof(List<>), "System.Collections.Generic.List")]
		[InlineData(typeof(object), "System.Object")]
		[InlineData(typeof(long?[]), "System.Nullable<System.Int64>[]")]
		public void GetTypeFullNameTests(Type input, string output)
		{
			CSharpCodeHelper.GetTypeFullName(input).ShouldBe(output);
		}

		[Theory]
		[InlineData(typeof(int), "Int32")]
		[InlineData(typeof(DateTime?), "Nullable<DateTime>")]
		[InlineData(typeof(string[]), "String[]")]
		[InlineData(typeof(List<string>), "List<String>")]
		[InlineData(typeof(List<string[]>), "List<String[]>")]
		[InlineData(typeof(IEnumerable<List<string>>), "IEnumerable<List<String>>")]
		[InlineData(typeof(Dictionary<int, decimal>), "Dictionary<Int32, Decimal>")]
		[InlineData(typeof(Dictionary<int, decimal>[]), "Dictionary<Int32, Decimal>[]")]
		[InlineData(typeof(List<Dictionary<int, decimal>[]>), "List<Dictionary<Int32, Decimal>[]>")]
		[InlineData(typeof(List<Dictionary<int, decimal>[]>[]), "List<Dictionary<Int32, Decimal>[]>[]")]
		[InlineData(typeof(List<>), "List")]
		[InlineData(typeof(object), "Object")]
		[InlineData(typeof(long?[]), "Nullable<Int64>[]")]
		public void GetTypeShortNameTests(Type input, string output)
		{
			CSharpCodeHelper.GetTypeShortName(input).ShouldBe(output);
		}

		[Theory]
		[InlineData(typeof(int), "int")]
		[InlineData(typeof(DateTime?), "DateTime?")]
		[InlineData(typeof(string[]), "string[]")]
		[InlineData(typeof(List<string>), "List<string>")]
		[InlineData(typeof(List<string[]>), "List<string[]>")]
		[InlineData(typeof(IEnumerable<List<string>>), "IEnumerable<List<string>>")]
		[InlineData(typeof(Dictionary<int, decimal>), "Dictionary<int, decimal>")]
		[InlineData(typeof(Dictionary<int, decimal>[]), "Dictionary<int, decimal>[]")]
		[InlineData(typeof(List<Dictionary<int, decimal>[]>), "List<Dictionary<int, decimal>[]>")]
		[InlineData(typeof(List<Dictionary<int, decimal>[]>[]), "List<Dictionary<int, decimal>[]>[]")]
		[InlineData(typeof(List<>), "List")]
		[InlineData(typeof(object), "object")]
		[InlineData(typeof(long?[]), "long?[]")]
		public void GetTypeBuiltInNameTests(Type input, string output)
		{
			CSharpCodeHelper.GetTypeBuiltInName(input).ShouldBe(output);
		}
	}
}