using System;
using System.Linq;

namespace SqlFirst.Codegen.Text
{
	/// <summary>
	/// Помогает форматировать отступы в коде
	/// </summary>
	internal static class IndentExtensions
	{
		/// <summary>
		/// Увеличить отступ
		/// </summary>
		/// <param name="target">Целевой фрагмент кода</param>
		/// <param name="indent">Символ отступа</param>
		/// <param name="size">Размер отступа</param>
		/// <returns>Фрагмент кода с указанным отступом</returns>
		public static string Indent(this string target, string indent, byte size)
		{
			if (string.IsNullOrEmpty(target))
			{
				return string.Empty;
			}

			string indentString;

			switch (size)
			{
				case 0:
					return target;
				case 1:
					indentString = indent;
					break;

				default:
					indentString = string.Concat(Enumerable.Range(0, size).Select(_ => indent));
					break;
			}

			string result = indentString + target.Replace(Environment.NewLine, Environment.NewLine + indentString);

			while (result.EndsWith(indent))
			{
				result = result.Substring(0, result.Length - indent.Length);
			}

			return result;
		}
	}
}