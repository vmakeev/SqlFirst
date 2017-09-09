using SqlFirst.Core.Codegen;
using Xunit;
using Xunit.Should;

namespace SqlFirst.Core.Tests
{
	public class CodeFormatterTests
	{
		[Theory]
		[InlineData(null, "")]
		[InlineData("", "")]
		[InlineData("_", "_")]
		[InlineData("__", "_")]
		[InlineData("_variable", "variable")]
		[InlineData("__variable", "variable")]
		[InlineData("_variable_", "variable")]
		[InlineData("_variable123var", "variable123var")]
		[InlineData("_variable123_var", "variable123Var")]
		[InlineData("_variable_123_var", "variable123Var")]
		[InlineData("_Variable", "variable")]
		[InlineData("_VariAble", "variAble")]
		[InlineData("_VARIABLE", "variable")]
		[InlineData("VARIABLE_LONG", "variableLong")]
		[InlineData("variAble_LONG", "variAbleLong")]
		[InlineData("_variAble_LONG", "variAbleLong")]
		[InlineData("some_long_TEST_VARIaBLE_wiThSurprise_s", "someLongTestVARIaBLEWiThSurpriseS")]
		public void CamelCaseTests(string input, string output)
		{
			TextCaseFormatter.ToCamelCase(input).ShouldBe(output);
		}

		[Theory]
		[InlineData(null, "")]
		[InlineData("", "")]
		[InlineData("_", "_")]
		[InlineData("__", "_")]
		[InlineData("_variable", "Variable")]
		[InlineData("__variable", "Variable")]
		[InlineData("_variable_", "Variable")]
		[InlineData("_variable123var", "Variable123var")]
		[InlineData("_variable123_var", "Variable123Var")]
		[InlineData("_variable_123_var", "Variable123Var")]
		[InlineData("_Variable", "Variable")]
		[InlineData("_VariAble", "VariAble")]
		[InlineData("_VARIABLE", "Variable")]
		[InlineData("VARIABLE_LONG", "VariableLong")]
		[InlineData("variAble_LONG", "VariAbleLong")]
		[InlineData("_variAble_LONG", "VariAbleLong")]
		[InlineData("some_long_TEST_VARIaBLE_wiThSurprise_s", "SomeLongTestVARIaBLEWiThSurpriseS")]
		public void PascalTests(string input, string output)
		{
			TextCaseFormatter.ToPascal(input).ShouldBe(output);
		}

		[Theory]
		[InlineData(null, "")]
		[InlineData("", "")]
		[InlineData("_", "_")]
		[InlineData("__", "_")]
		[InlineData("variable", "VARIABLE")]
		[InlineData("_variable", "VARIABLE")]
		[InlineData("__variable", "VARIABLE")]
		[InlineData("_variable_", "VARIABLE")]
		[InlineData("_variable123var", "VARIABLE123VAR")]
		[InlineData("_variable123_var", "VARIABLE123_VAR")]
		[InlineData("_variable_123_var", "VARIABLE_123_VAR")]
		[InlineData("_Variable", "VARIABLE")]
		[InlineData("_VariAble", "VARI_ABLE")]
		[InlineData("_VARIABLE", "VARIABLE")]
		[InlineData("VARIABLE_LONG", "VARIABLE_LONG")]
		[InlineData("variAble_LONG", "VARI_ABLE_LONG")]
		[InlineData("_variAble_LONG", "VARI_ABLE_LONG")]
		[InlineData("some_long_TEST_VARIaBLE_wiThSurprise_s", "SOME_LONG_TEST_VARIA_BLE_WI_TH_SURPRISE_S")]
		public void UnderscopeTests(string input, string output)
		{
			TextCaseFormatter.ToUnderscopes(input).ShouldBe(output);
		}
	}
}