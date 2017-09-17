using System;

namespace SqlFirst.Codegen.Text
{
	internal static class IndentExtensions
	{
		public static string Indent(this string target, string indent)
		{
			if (string.IsNullOrEmpty(target))
			{
				return indent;
			}

			string result = indent + target.Replace(Environment.NewLine, Environment.NewLine + indent);

			return result.EndsWith(indent)
				? result.Substring(0, result.Length - indent.Length)
				: result;
		}
	}
}