using System;
using System.Collections.Generic;
using SqlFirst.Core.Codegen;
using Xunit;
using Xunit.Should;

namespace SqlFirst.Core.Tests
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
		public void GetValidVariableNameTests(string input, string output)
		{
			CSharpCodeHelper.GetValidVariableName(input).ShouldBe(output);
		}

		[Theory]

		#region A lot of test cases

		#region CamelCase

		[InlineData(VariableNamingPolicy.CamelCase, null, "_")]
		[InlineData(VariableNamingPolicy.CamelCase, "", "_")]
		[InlineData(VariableNamingPolicy.CamelCase, "_", "_")]
		[InlineData(VariableNamingPolicy.CamelCase, "__", "_")]
		[InlineData(VariableNamingPolicy.CamelCase, "3", "_3")]
		[InlineData(VariableNamingPolicy.CamelCase, "!", "_")]
		[InlineData(VariableNamingPolicy.CamelCase, "%$#&###@@%^$%", "_")]
		[InlineData(VariableNamingPolicy.CamelCase, "variable", "variable")]
		[InlineData(VariableNamingPolicy.CamelCase, "this", "@this")]
		[InlineData(VariableNamingPolicy.CamelCase, "!! nu-l=l :)", "nuLL")]
		[InlineData(VariableNamingPolicy.CamelCase, "!! null :)", "@null")]
		[InlineData(VariableNamingPolicy.CamelCase, "int", "@int")]
		[InlineData(VariableNamingPolicy.CamelCase, "@int", "@int")]
		[InlineData(VariableNamingPolicy.CamelCase, "@variable", "variable")]
		[InlineData(VariableNamingPolicy.CamelCase, "кириллица", "кириллица")]
		[InlineData(VariableNamingPolicy.CamelCase, "2x2", "_2x2")]
		[InlineData(VariableNamingPolicy.CamelCase, "2x2=4", "_2x24")]
		[InlineData(VariableNamingPolicy.CamelCase, "tooMany Words с пробелами", "tooManyWordsСПробелами")]
		[InlineData(VariableNamingPolicy.CamelCase, "in;valid#symbols_every-where", "inValidSymbolsEveryWhere")]

		#endregion

		#region CamelCaseWithUnderscope

		[InlineData(VariableNamingPolicy.CamelCaseWithUnderscope, null, "_")]
		[InlineData(VariableNamingPolicy.CamelCaseWithUnderscope, "", "_")]
		[InlineData(VariableNamingPolicy.CamelCaseWithUnderscope, "_", "_")]
		[InlineData(VariableNamingPolicy.CamelCaseWithUnderscope, "__", "_")]
		[InlineData(VariableNamingPolicy.CamelCaseWithUnderscope, "3", "_3")]
		[InlineData(VariableNamingPolicy.CamelCaseWithUnderscope, "!", "_")]
		[InlineData(VariableNamingPolicy.CamelCaseWithUnderscope, "%$#&###@@%^$%", "_")]
		[InlineData(VariableNamingPolicy.CamelCaseWithUnderscope, "variable", "_variable")]
		[InlineData(VariableNamingPolicy.CamelCaseWithUnderscope, "this", "_this")]
		[InlineData(VariableNamingPolicy.CamelCaseWithUnderscope, "!! nu-l=l :)", "_nuLL")]
		[InlineData(VariableNamingPolicy.CamelCaseWithUnderscope, "!! null :)", "_null")]
		[InlineData(VariableNamingPolicy.CamelCaseWithUnderscope, "int", "_int")]
		[InlineData(VariableNamingPolicy.CamelCaseWithUnderscope, "@int", "_int")]
		[InlineData(VariableNamingPolicy.CamelCaseWithUnderscope, "@variable", "_variable")]
		[InlineData(VariableNamingPolicy.CamelCaseWithUnderscope, "кириллица", "_кириллица")]
		[InlineData(VariableNamingPolicy.CamelCaseWithUnderscope, "2x2", "_2x2")]
		[InlineData(VariableNamingPolicy.CamelCaseWithUnderscope, "2x2=4", "_2x24")]
		[InlineData(VariableNamingPolicy.CamelCaseWithUnderscope, "tooMany Words с пробелами", "_tooManyWordsСПробелами")]
		[InlineData(VariableNamingPolicy.CamelCaseWithUnderscope, "in;valid#symbols_every-where", "_inValidSymbolsEveryWhere")]

		#endregion

		#region Pascal

		[InlineData(VariableNamingPolicy.Pascal, null, "_")]
		[InlineData(VariableNamingPolicy.Pascal, "", "_")]
		[InlineData(VariableNamingPolicy.Pascal, "_", "_")]
		[InlineData(VariableNamingPolicy.Pascal, "__", "_")]
		[InlineData(VariableNamingPolicy.Pascal, "3", "_3")]
		[InlineData(VariableNamingPolicy.Pascal, "!", "_")]
		[InlineData(VariableNamingPolicy.Pascal, "%$#&###@@%^$%", "_")]
		[InlineData(VariableNamingPolicy.Pascal, "variable", "Variable")]
		[InlineData(VariableNamingPolicy.Pascal, "this", "This")]
		[InlineData(VariableNamingPolicy.Pascal, "!! nu-l=l :)", "NuLL")]
		[InlineData(VariableNamingPolicy.Pascal, "!! null :)", "Null")]
		[InlineData(VariableNamingPolicy.Pascal, "int", "Int")]
		[InlineData(VariableNamingPolicy.Pascal, "@int", "Int")]
		[InlineData(VariableNamingPolicy.Pascal, "@variable", "Variable")]
		[InlineData(VariableNamingPolicy.Pascal, "кириллица", "Кириллица")]
		[InlineData(VariableNamingPolicy.Pascal, "2x2", "_2x2")]
		[InlineData(VariableNamingPolicy.Pascal, "2x2=4", "_2x24")]
		[InlineData(VariableNamingPolicy.Pascal, "tooMany Words с пробелами", "TooManyWordsСПробелами")]
		[InlineData(VariableNamingPolicy.Pascal, "in;valid#symbols_every-where", "InValidSymbolsEveryWhere")]

		#endregion

		#region Underscope

		[InlineData(VariableNamingPolicy.Underscope, null, "_")]
		[InlineData(VariableNamingPolicy.Underscope, "", "_")]
		[InlineData(VariableNamingPolicy.Underscope, "_", "_")]
		[InlineData(VariableNamingPolicy.Underscope, "__", "_")]
		[InlineData(VariableNamingPolicy.Underscope, "3", "_3")]
		[InlineData(VariableNamingPolicy.Underscope, "!", "_")]
		[InlineData(VariableNamingPolicy.Underscope, "%$#&###@@%^$%", "_")]
		[InlineData(VariableNamingPolicy.Underscope, "variable", "VARIABLE")]
		[InlineData(VariableNamingPolicy.Underscope, "this", "THIS")]
		[InlineData(VariableNamingPolicy.Underscope, "!! nu-l=l :)", "NU_L_L")]
		[InlineData(VariableNamingPolicy.Underscope, "!! null :)", "NULL")]
		[InlineData(VariableNamingPolicy.Underscope, "int", "INT")]
		[InlineData(VariableNamingPolicy.Underscope, "@int", "INT")]
		[InlineData(VariableNamingPolicy.Underscope, "@variable", "VARIABLE")]
		[InlineData(VariableNamingPolicy.Underscope, "кириллица", "КИРИЛЛИЦА")]
		[InlineData(VariableNamingPolicy.Underscope, "2x2", "_2X2")]
		[InlineData(VariableNamingPolicy.Underscope, "2x2=4", "_2X2_4")]
		[InlineData(VariableNamingPolicy.Underscope, "tooMany Words с пробелами", "TOO_MANY_WORDS_С_ПРОБЕЛАМИ")]
		[InlineData(VariableNamingPolicy.Underscope, "in;valid#symbols_every-where", "IN_VALID_SYMBOLS_EVERY_WHERE")]

		#endregion

		#endregion

		public void GetValidVariableNameWithFormatTests(VariableNamingPolicy policy, string input, string output)
		{
			CSharpCodeHelper.GetValidVariableName(input, policy).ShouldBe(output);
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