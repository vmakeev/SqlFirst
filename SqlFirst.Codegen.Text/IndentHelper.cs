using System;

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
		/// <param name="indent">Отступ</param>
		/// <returns>Фрагмент кода с указанным отступом</returns>
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