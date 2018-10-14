using System;
using System.Collections;
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
		[InlineData("too %^&* many     invalid&symbols", "toomanyinvalidsymbols")]
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
		[InlineData(NamingPolicy.CamelCase, "too %^&* many     invalid&symbols", "tooManyInvalidSymbols")]

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
		[InlineData(NamingPolicy.CamelCaseWithUnderscope, "too %^&* many     invalid&symbols", "_tooManyInvalidSymbols")]

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
		[InlineData(NamingPolicy.Pascal, "too %^&* many     invalid&symbols", "TooManyInvalidSymbols")]

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
		[InlineData(NamingPolicy.Underscope, "too %^&* many     invalid&symbols", "TOO_MANY_INVALID_SYMBOLS")]

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

		[Theory]
		[InlineData("someString", "someString")]
		[InlineData("\'\"\\\0\a\b\f\n\r\t\vsomeString\u0001", @"\'\""\\\0\a\b\f\n\r\t\vsomeString\u0001")]
		public void EscapeStringTests(string input, string output)
		{
			CSharpCodeHelper.EscapeString(input).ShouldBe(output);
		}

		[Theory]
		[InlineData(typeof(object), null, "null")]

		[InlineData(typeof(string), "someString", "\"someString\"")]
		[InlineData(typeof(string), 42, "\"42\"")]

		[InlineData(typeof(Guid), 42, "Guid.Parse(\"42\")")]
		[InlineData(typeof(Guid?), 42, "Guid.Parse(\"42\")")]

		[InlineData(typeof(DateTime), 42, "DateTime.Parse(\"42\")")]
		[InlineData(typeof(DateTime?), 42, "DateTime.Parse(\"42\")")]

		[InlineData(typeof(bool?), "true", "true")]
		[InlineData(typeof(bool?), "False", "false")]

		[InlineData(typeof(double), 1.286, "1.286")]
		[InlineData(typeof(double), "1,286", "1.286")]
		[InlineData(typeof(double), "1.286", "1.286")]

		// can't use decimal values at attributes
		[InlineData(typeof(decimal?), 2.72, "2.72")]
		[InlineData(typeof(decimal?), "2,72", "2.72")]
		[InlineData(typeof(decimal?), "2.72", "2.72")]

		[InlineData(typeof(float), 11.14f, "11.14")]
		[InlineData(typeof(float), "11.14", "11.14")]
		[InlineData(typeof(float), "11,14", "11.14")]
		public void GetValidValueTests(Type type, object input, string output)
		{
			CSharpCodeHelper.GetValidValue(type, input).ShouldBe(output);
		}

		[Theory]
		[InlineData(typeof(CSharpCodeHelper), "")]
		[InlineData(typeof(bool), "")]
		[InlineData(typeof(bool), "invalidBool")]
		public void GetValidValueTests_Negative(Type type, object input)
		{
			Assert.Throws<CodeGenerationException>(() => CSharpCodeHelper.GetValidValue(type, input));
		}

		[Theory]
		[InlineData(typeof(IEnumerable<>), "int", "IEnumerable<int>")]
		[InlineData(typeof(IEnumerable<>), "System.String", "IEnumerable<System.String>")]
		[InlineData(typeof(List<>), "Surprise", "List<Surprise>")]
		public void GetGenericTypeTests_1(Type type, string genericArgument, string result)
		{
			CSharpCodeHelper.GetGenericType(type, genericArgument).ShouldBe(result);
		}

		[Theory]
		[InlineData(typeof(IDictionary<,>), "int", "string", "IDictionary<int, string>")]
		[InlineData(typeof(IDictionary<,>), "System.String", "List<Guid>", "IDictionary<System.String, List<Guid>>")]
		[InlineData(typeof(IDictionary<,>), "Surprise", "Omg", "IDictionary<Surprise, Omg>")]
		public void GetGenericTypeTests_2(Type type, string genericArgument1, string genericArgument2, string result)
		{
			CSharpCodeHelper.GetGenericType(type, genericArgument1, genericArgument2).ShouldBe(result);
		}

		[Fact]
		public void GetGenericTypeTests_Negative_TypeCanNotBeNull()
		{
			Assert.Throws<ArgumentNullException>(() => CSharpCodeHelper.GetGenericType(null)).ParamName.ShouldBe("genericType");
			Assert.Throws<ArgumentNullException>(() => CSharpCodeHelper.GetGenericType(null, null)).ParamName.ShouldBe("genericType");
			Assert.Throws<ArgumentNullException>(() => CSharpCodeHelper.GetGenericType(null, string.Empty)).ParamName.ShouldBe("genericType");
			Assert.Throws<ArgumentNullException>(() => CSharpCodeHelper.GetGenericType(null, "SomeValue1")).ParamName.ShouldBe("genericType");
			Assert.Throws<ArgumentNullException>(() => CSharpCodeHelper.GetGenericType(null, "SomeValue1", "SomeValue2")).ParamName.ShouldBe("genericType");
		}

		[Fact]
		public void GetGenericTypeTests_Negative_GenericParametersCanNotBeNull()
		{
			Assert.Throws<ArgumentNullException>(() => CSharpCodeHelper.GetGenericType(typeof(IEnumerable<>), null)).ParamName.ShouldBe("genericTypeArguments");
		}

		[Fact]
		public void GetGenericTypeTests_Negative_TypeMustBeOpenGenericType()
		{
			Assert.Throws<ArgumentException>(() => CSharpCodeHelper.GetGenericType(typeof(object), "SomeValue1")).ParamName.ShouldBe("genericType");
			Assert.Throws<ArgumentException>(() => CSharpCodeHelper.GetGenericType(typeof(IEnumerable<string>), "SomeValue1")).ParamName.ShouldBe("genericType");
			Assert.Throws<ArgumentException>(() => CSharpCodeHelper.GetGenericType(typeof(IList), "SomeValue1")).ParamName.ShouldBe("genericType");
		}

		[Fact]
		public void GetGenericTypeTests_Negative_ParametersShouldNotContainsEmptyValues()
		{
			Assert.Throws<ArgumentException>(() => CSharpCodeHelper.GetGenericType(typeof(IDictionary<,>), null, null)).ParamName.ShouldBe("genericTypeArguments");
			Assert.Throws<ArgumentException>(() => CSharpCodeHelper.GetGenericType(typeof(IDictionary<,>), string.Empty, string.Empty)).ParamName.ShouldBe("genericTypeArguments");
			Assert.Throws<ArgumentException>(() => CSharpCodeHelper.GetGenericType(typeof(IDictionary<,>), " ", " ")).ParamName.ShouldBe("genericTypeArguments");

			Assert.Throws<ArgumentException>(() => CSharpCodeHelper.GetGenericType(typeof(IDictionary<,>), "SomeValue1", null)).ParamName.ShouldBe("genericTypeArguments");
			Assert.Throws<ArgumentException>(() => CSharpCodeHelper.GetGenericType(typeof(IDictionary<,>), "SomeValue1", string.Empty)).ParamName.ShouldBe("genericTypeArguments");

			Assert.Throws<ArgumentException>(() => CSharpCodeHelper.GetGenericType(typeof(IDictionary<,>), null, "SomeValue2")).ParamName.ShouldBe("genericTypeArguments");
			Assert.Throws<ArgumentException>(() => CSharpCodeHelper.GetGenericType(typeof(IDictionary<,>), " ", "SomeValue2")).ParamName.ShouldBe("genericTypeArguments");

			Assert.Throws<ArgumentException>(() => CSharpCodeHelper.GetGenericType(typeof(IDictionary<,>), null, "SomeValue2")).ParamName.ShouldBe("genericTypeArguments");
			Assert.Throws<ArgumentException>(() => CSharpCodeHelper.GetGenericType(typeof(IDictionary<,>), " ", "SomeValue2")).ParamName.ShouldBe("genericTypeArguments");
		}

		[Fact]
		public void GetGenericTypeTests_Negative_ParametersCountShouldMatchWithTypeArgumentsCount()
		{
			Assert.Throws<ArgumentException>(() => CSharpCodeHelper.GetGenericType(typeof(IDictionary<,>))).ParamName.ShouldBe("genericTypeArguments");
			Assert.Throws<ArgumentException>(() => CSharpCodeHelper.GetGenericType(typeof(IDictionary<,>), "SomeValue1")).ParamName.ShouldBe("genericTypeArguments");
			Assert.Throws<ArgumentException>(() => CSharpCodeHelper.GetGenericType(typeof(IDictionary<,>), "SomeValue1", "SomeValue2", "SomeValue3")).ParamName.ShouldBe("genericTypeArguments");

			Assert.Throws<ArgumentException>(() => CSharpCodeHelper.GetGenericType(typeof(IList<>))).ParamName.ShouldBe("genericTypeArguments");
			Assert.Throws<ArgumentException>(() => CSharpCodeHelper.GetGenericType(typeof(IList<>), "SomeValue1", "SomeValue2")).ParamName.ShouldBe("genericTypeArguments");
			Assert.Throws<ArgumentException>(() => CSharpCodeHelper.GetGenericType(typeof(IList<>), "SomeValue1", "SomeValue2", "SomeValue3")).ParamName.ShouldBe("genericTypeArguments");
		}

		[Theory]
		[InlineData(null, false)]
		[InlineData("", false)]
		[InlineData("string", false)]
		[InlineData("char", false)]
		[InlineData("true", false)]
		[InlineData("1tratata", false)]
		[InlineData("some strange value", false)]
		[InlineData("variable*", false)]
		[InlineData("List<string>", false)]
		[InlineData("@", false)]

		[InlineData("@string", true)]
		[InlineData("@true", true)]
		[InlineData("_something", true)]
		[InlineData("v123", true)]
		[InlineData("_", true)]
		public void IsValidIdentifierNameTests(string input, bool output)
		{
			CSharpCodeHelper.IsValidIdentifierName(input).ShouldBe(output);
		}

		[Theory]
		[InlineData(true, null, "_")]
		[InlineData(true, "", "_")]
		[InlineData(true, " ", "_")]
		[InlineData(true, "  ", "_")]
		[InlineData(true, ".", "_")]
		[InlineData(true, "....", "_")]
		[InlineData(true, "System.string", "string")]
		[InlineData(true, "Dictionary<int, string>", "Dictionary")]
		[InlineData(true, "System.Collections.Generic.List<IEnumerable<int>>", "List")]
		[InlineData(true, "System.Colle<ctions.Generic.List<IEnumerable<int>>", "Colle")] // as designed. There's no reason to parse whole world
		[InlineData(true, "SomeType", "SomeType")]
		[InlineData(true, "Some < Type", "Some")]
		[InlineData(true, "char", "char")]
		[InlineData(true, "4char", "_4char")]
		[InlineData(true, "4ch ar", "_4char")]
		[InlineData(true, "[dbo].[IntegerList]", "IntegerList")]
		[InlineData(true, "[dbo].[IntegerList]     ", "IntegerList")]
		[InlineData(true, "[dbsdfg2345o].[I  #$ ^$#%nteg /-erList]", "IntegerList")]
		[InlineData(false, "char", "@char")]
		[InlineData(false, "int?", "@int")]
		public void GetValidTypeNameTests(bool allowBuiltInTypes, string input, string output)
		{
			CSharpCodeHelper.GetValidTypeName(input, allowBuiltInTypes).ShouldBe(output);
		}

		[Theory]
		[InlineData(NamingPolicy.Pascal, true, null, "_")]
		[InlineData(NamingPolicy.Pascal, true, "", "_")]
		[InlineData(NamingPolicy.Pascal, true, " ", "_")]
		[InlineData(NamingPolicy.Pascal, true, "  ", "_")]
		[InlineData(NamingPolicy.Pascal, true, ".", "_")]
		[InlineData(NamingPolicy.Pascal, true, "....", "_")]
		[InlineData(NamingPolicy.Pascal, true, "System.string", "String")]
		[InlineData(NamingPolicy.Pascal, true, "Dictionary<int, string>", "Dictionary")]
		[InlineData(NamingPolicy.Pascal, true, "System.Collections.Generic.List<IEnumerable<int>>", "List")]
		[InlineData(NamingPolicy.Pascal, true, "System.Colle<ctions.Generic.List<IEnumerable<int>>", "Colle")] // as designed. There's no reason to parse whole world
		[InlineData(NamingPolicy.Pascal, true, "SomeType", "SomeType")]
		[InlineData(NamingPolicy.Pascal, true, "Some < Type", "Some")]
		[InlineData(NamingPolicy.Pascal, true, "4char", "_4char")]
		[InlineData(NamingPolicy.Pascal, true, "4ch ar", "_4chAr")]
		[InlineData(NamingPolicy.Pascal, true, "[dbo].[IntegerList]", "IntegerList")]
		[InlineData(NamingPolicy.Pascal, true, "[dbo].[IntegerList]     ", "IntegerList")]
		[InlineData(NamingPolicy.Pascal, true, "[dbsdfg2345o].[I  #$ ^$#%nteg /-erList]", "INtegErList")]
		[InlineData(NamingPolicy.Pascal, true, "char", "Char")]
		[InlineData(NamingPolicy.Pascal, false, "char", "Char")]
		[InlineData(NamingPolicy.Pascal, false, "char?", "Char")]

		[InlineData(NamingPolicy.CamelCase, true, null, "_")]
		[InlineData(NamingPolicy.CamelCase, true, "", "_")]
		[InlineData(NamingPolicy.CamelCase, true, " ", "_")]
		[InlineData(NamingPolicy.CamelCase, true, "  ", "_")]
		[InlineData(NamingPolicy.CamelCase, true, ".", "_")]
		[InlineData(NamingPolicy.CamelCase, true, "....", "_")]
		[InlineData(NamingPolicy.CamelCase, true, "System.string", "string")]
		[InlineData(NamingPolicy.CamelCase, true, "Dictionary<int, string>", "dictionary")]
		[InlineData(NamingPolicy.CamelCase, true, "System.Collections.Generic.List<IEnumerable<int>>", "list")]
		[InlineData(NamingPolicy.CamelCase, true, "System.Colle<ctions.Generic.List<IEnumerable<int>>", "colle")] // as designed. There's no reason to parse whole world
		[InlineData(NamingPolicy.CamelCase, true, "SomeType", "someType")]
		[InlineData(NamingPolicy.CamelCase, true, "Some < Type", "some")]
		[InlineData(NamingPolicy.CamelCase, true, "4char", "_4char")]
		[InlineData(NamingPolicy.CamelCase, true, "4ch ar", "_4chAr")]
		[InlineData(NamingPolicy.CamelCase, true, "[dbo].[IntegerList]", "integerList")]
		[InlineData(NamingPolicy.CamelCase, true, "[dbo].[IntegerList]     ", "integerList")]
		[InlineData(NamingPolicy.CamelCase, true, "[dbsdfg2345o].[I  #$ ^$#%nteg /-erList]", "iNtegErList")]
		[InlineData(NamingPolicy.CamelCase, true, "char", "char")]
		[InlineData(NamingPolicy.CamelCase, false, "char", "@char")]
		[InlineData(NamingPolicy.CamelCase, false, "char?", "@char")]

		[InlineData(NamingPolicy.CamelCaseWithUnderscope, true, null, "_")]
		[InlineData(NamingPolicy.CamelCaseWithUnderscope, true, "", "_")]
		[InlineData(NamingPolicy.CamelCaseWithUnderscope, true, " ", "_")]
		[InlineData(NamingPolicy.CamelCaseWithUnderscope, true, "  ", "_")]
		[InlineData(NamingPolicy.CamelCaseWithUnderscope, true, ".", "_")]
		[InlineData(NamingPolicy.CamelCaseWithUnderscope, true, "....", "_")]
		[InlineData(NamingPolicy.CamelCaseWithUnderscope, true, "System.string", "_string")]
		[InlineData(NamingPolicy.CamelCaseWithUnderscope, true, "Dictionary<int, string>", "_dictionary")]
		[InlineData(NamingPolicy.CamelCaseWithUnderscope, true, "System.Collections.Generic.List<IEnumerable<int>>", "_list")]
		[InlineData(NamingPolicy.CamelCaseWithUnderscope, true, "System.Colle<ctions.Generic.List<IEnumerable<int>>", "_colle")] // as designed. There's no reason to parse whole world
		[InlineData(NamingPolicy.CamelCaseWithUnderscope, true, "SomeType", "_someType")]
		[InlineData(NamingPolicy.CamelCaseWithUnderscope, true, "Some < Type", "_some")]
		[InlineData(NamingPolicy.CamelCaseWithUnderscope, true, "4char", "_4char")]
		[InlineData(NamingPolicy.CamelCaseWithUnderscope, true, "4ch ar", "_4chAr")]
		[InlineData(NamingPolicy.CamelCaseWithUnderscope, true, "[dbo].[IntegerList]", "_integerList")]
		[InlineData(NamingPolicy.CamelCaseWithUnderscope, true, "[dbo].[IntegerList]     ", "_integerList")]
		[InlineData(NamingPolicy.CamelCaseWithUnderscope, true, "[dbsdfg2345o].[I  #$ ^$#%nteg /-erList]", "_iNtegErList")]
		[InlineData(NamingPolicy.CamelCaseWithUnderscope, true, "char", "_char")]
		[InlineData(NamingPolicy.CamelCaseWithUnderscope, false, "char", "_char")]
		[InlineData(NamingPolicy.CamelCaseWithUnderscope, false, "char", "_char")]

		[InlineData(NamingPolicy.Underscope, true, null, "_")]
		[InlineData(NamingPolicy.Underscope, true, "", "_")]
		[InlineData(NamingPolicy.Underscope, true, " ", "_")]
		[InlineData(NamingPolicy.Underscope, true, "  ", "_")]
		[InlineData(NamingPolicy.Underscope, true, ".", "_")]
		[InlineData(NamingPolicy.Underscope, true, "....", "_")]
		[InlineData(NamingPolicy.Underscope, true, "System.string", "STRING")]
		[InlineData(NamingPolicy.Underscope, true, "Dictionary<int, string>", "DICTIONARY")]
		[InlineData(NamingPolicy.Underscope, true, "System.Collections.Generic.List<IEnumerable<int>>", "LIST")]
		[InlineData(NamingPolicy.Underscope, true, "System.Colle<ctions.Generic.List<IEnumerable<int>>", "COLLE")] // as designed. There's no reason to parse whole world
		[InlineData(NamingPolicy.Underscope, true, "SomeType", "SOME_TYPE")]
		[InlineData(NamingPolicy.Underscope, true, "Some < Type", "SOME")]
		[InlineData(NamingPolicy.Underscope, true, "4char", "_4CHAR")]
		[InlineData(NamingPolicy.Underscope, true, "4ch ar", "_4CH_AR")]
		[InlineData(NamingPolicy.Underscope, true, "[dbo].[IntegerList]", "INTEGER_LIST")]
		[InlineData(NamingPolicy.Underscope, true, "[dbo].[IntegerList]     ", "INTEGER_LIST")]
		[InlineData(NamingPolicy.Underscope, true, "[dbsdfg2345o].[I  #$ ^$#%nteg /-erList]", "I_NTEG_ER_LIST")]
		[InlineData(NamingPolicy.Underscope, true, "char", "CHAR")]
		[InlineData(NamingPolicy.Underscope, false, "char", "CHAR")]
		[InlineData(NamingPolicy.Underscope, false, "char?", "CHAR")]
		public void GetValidTypeNameWithFormatTests(NamingPolicy policy, bool allowBuiltInTypes, string input, string output)
		{
			CSharpCodeHelper.GetValidTypeName(input, policy, allowBuiltInTypes).ShouldBe(output);
		}

		[Fact]
		public void GetValidTypeNameWithFormatTests_Negative()
		{
			Assert.Throws<ArgumentOutOfRangeException>(() => CSharpCodeHelper.GetValidTypeName("someValue", (NamingPolicy)42, true)).ParamName.ShouldBe("namingPolicy");
			Assert.Throws<ArgumentOutOfRangeException>(() => CSharpCodeHelper.GetValidTypeName("someValue", (NamingPolicy)42, false)).ParamName.ShouldBe("namingPolicy");
		}

		[Theory]
		[InlineData(true, null, false)]
		[InlineData(true, "", false)]
		[InlineData(true, " ", false)]
		[InlineData(true, "0", false)]
		[InlineData(true, "1typeName", false)]
		[InlineData(true, "char", true)]
		[InlineData(true, "char?", true)]
		[InlineData(true, "decimal", true)]
		[InlineData(true, "decimal?", true)]
		[InlineData(true, "object", true)]
		[InlineData(true, "object?", false)]
		[InlineData(true, "void", false)]
		[InlineData(true, "while", false)]
		[InlineData(true, "ref", false)]
		[InlineData(true, "@", false)]
		[InlineData(true, "@@", false)]
		[InlineData(true, "@char", true)]
		[InlineData(true, "@void", true)]
		[InlineData(true, "_void", true)]
		[InlineData(true, "_", true)]
		[InlineData(true, "__", true)]
		[InlineData(true, "someType", true)]
		[InlineData(true, "some_Type", true)]
		[InlineData(true, "some-Type", false)]
		[InlineData(true, "some Type", false)]
		[InlineData(true, " someType", false)]
		[InlineData(true, "someType ", false)]
		[InlineData(true, "$#E^&%", false)]
		[InlineData(true, "someType?", true)] // no value/reference type info provided. It's safe to allow
		[InlineData(true, "List<string>", false)] // generics are not allowed

		[InlineData(false, null, false)]
		[InlineData(false, "", false)]
		[InlineData(false, " ", false)]
		[InlineData(false, "0", false)]
		[InlineData(false, "1typeName", false)]
		[InlineData(false, "char", false)]
		[InlineData(false, "char?", false)]
		[InlineData(false, "decimal", false)]
		[InlineData(false, "decimal?", false)]
		[InlineData(false, "object", false)]
		[InlineData(false, "object?", false)]
		[InlineData(false, "void", false)]
		[InlineData(false, "while", false)]
		[InlineData(false, "ref", false)]
		[InlineData(false, "@", false)]
		[InlineData(false, "@@", false)]
		[InlineData(false, "@char", true)]
		[InlineData(false, "@void", true)]
		[InlineData(false, "_void", true)]
		[InlineData(false, "_", true)]
		[InlineData(false, "__", true)]
		[InlineData(false, "someType", true)]
		[InlineData(false, "some_Type", true)]
		[InlineData(false, "some-Type", false)]
		[InlineData(false, "some Type", false)]
		[InlineData(false, " someType", false)]
		[InlineData(false, "someType ", false)]
		[InlineData(false, "$#E^&%", false)]
		[InlineData(false, "someType?", true)] // no value/reference type info provided. It's safe to allow
		[InlineData(false, "List<string>", false)] // generics are not allowed
		public void IsValidTypeNameTests(bool allowBuiltInTypes, string input, bool output)
		{
			CSharpCodeHelper.IsValidTypeName(input, allowBuiltInTypes).ShouldBe(output);
		}
	}
}