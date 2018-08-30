using System;
using System.Globalization;
using System.IO;

namespace SqlFirst.Intelligence.Helpers
{
	public static class PathHelper
	{
		/// <summary>
		/// Сравнивает два пути на эквивалентность
		/// </summary>
		/// <param name="path1">Первый путь</param>
		/// <param name="path2">Второй путь</param>
		/// <returns>True, если пути ведут к одному и тому же объекту</returns>
		public static bool ArePathEquals(string path1, string path2)
		{
			string s1 = NormalizePath(path1);
			string s2 = NormalizePath(path2);
			return string.Equals(s1, s2, StringComparison.OrdinalIgnoreCase);
		}

		/// <summary>
		/// Выполняет нормализацию пути к файлу или папке
		/// </summary>
		/// <param name="path">Путь к файлу или папке</param>
		/// <returns>Нормализованный путь к файлу или папке</returns>
		public static string NormalizePath(string path)
		{
			return Path.GetFullPath(path).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
		}

		/// <summary>
		/// Проверяет, является ли <paramref name="possibleSubpath" /> частью пути к <paramref name="path" />
		/// </summary>
		/// <param name="path">Путь</param>
		/// <param name="possibleSubpath">Проверяемая часть пути</param>
		/// <returns>True, если <paramref name="possibleSubpath" /> является частью пути к <paramref name="path" /></returns>
		public static bool IsSubpathOf(string path, string possibleSubpath)
		{
			string s1 = NormalizePath(path).ToUpperInvariant();
			string s2 = NormalizePath(possibleSubpath).ToUpperInvariant();

			if (s1.Length < s2.Length)
			{
				return false;
			}

			if (!s1.StartsWith(s2, StringComparison.OrdinalIgnoreCase))
			{
				return false;
			}

			string s3 = s1.Substring(s2.Length);
			if (string.IsNullOrEmpty(s3))
			{
				return true;
			}

			return s3.StartsWith(Path.DirectorySeparatorChar.ToString(CultureInfo.InvariantCulture), StringComparison.OrdinalIgnoreCase) ||
					s3.StartsWith(Path.AltDirectorySeparatorChar.ToString(CultureInfo.InvariantCulture), StringComparison.OrdinalIgnoreCase);
		}

		public static string GetRelativePath(string from, string to)
		{
			if (ArePathEquals(from, to))
			{
				return string.Empty;
			}

			char separator = Path.DirectorySeparatorChar;
			char[] separators = { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar };

			var projectFileUri = new Uri(from.TrimEnd(separators) + separator, UriKind.Absolute);
			var queryFileUri = new Uri(to.TrimEnd(separators), UriKind.Absolute);

			string relativeUri = projectFileUri.MakeRelativeUri(queryFileUri).ToString();

			return relativeUri.Replace('/', '\\');
		}
	}
}