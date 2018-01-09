using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SqlFirst.Codegen.Helpers
{
	/// <summary>
	/// Помощник форматирования текста
	/// </summary>
	internal static class AdjustTextCase
	{
		/// <summary>
		/// Преобразует строку <paramref name="name" /> в стиль CamelCase
		/// </summary>
		/// <param name="name">Искомая строка</param>
		/// <returns>Преобразованная строка</returns>
		public static string ToCamelCase(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				return string.Empty;
			}

			if (name.All(p => p == '_'))
			{
				return "_";
			}

			if (name.Contains("_"))
			{
				string[] nameParts = name.Split(new[] { '_' }, StringSplitOptions.RemoveEmptyEntries);
				IEnumerable<string> casedParts = nameParts.Select((item, index) => index == 0 ? ToCamelCase(item) : ToPascal(item));
				return string.Join(string.Empty, casedParts);
			}

			if (name.All(char.IsUpper))
			{
				return name.ToLowerInvariant();
			}

			return char.ToLowerInvariant(name[0]) + name.Substring(1);
		}

		/// <summary>
		/// Преобразует строку <paramref name="name" /> в стиль Pascal
		/// </summary>
		/// <param name="name">Искомая строка</param>
		/// <returns>Преобразованная строка</returns>
		public static string ToPascal(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				return string.Empty;
			}

			if (name.All(p => p == '_'))
			{
				return "_";
			}

			if (name.Contains("_"))
			{
				string[] nameParts = name.Split(new[] { '_' }, StringSplitOptions.RemoveEmptyEntries);
				IEnumerable<string> casedParts = nameParts.Select(ToPascal);
				return string.Join(string.Empty, casedParts);
			}

			if (name.All(char.IsUpper))
			{
				return char.ToUpperInvariant(name[0]) + name.Substring(1).ToLowerInvariant();
			}

			return char.ToUpperInvariant(name[0]) + name.Substring(1);
		}

		/// <summary>
		/// Преобразует строку <paramref name="name" /> в верхний регистр с разделением значимых частей нижним подчеркиванием
		/// </summary>
		/// <param name="name">Искомая строка</param>
		/// <returns>Преобразованная строка</returns>
		public static string ToUnderscopes(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				return string.Empty;
			}

			if (name.All(p => p == '_'))
			{
				return "_";
			}

			if (name.All(char.IsLower))
			{
				return name.ToUpperInvariant();
			}

			var sb = new StringBuilder(name.Length + 10);

			bool isPrevCharIsLower = false;
			foreach (char c in name)
			{
				if (char.IsUpper(c) && isPrevCharIsLower)
				{
					sb.Append("_");
				}

				isPrevCharIsLower = char.IsLower(c);

				sb.Append(char.ToUpperInvariant(c));
			}

			return sb.ToString().Trim('_');
		}
	}
}