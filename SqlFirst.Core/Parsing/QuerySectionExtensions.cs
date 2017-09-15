using System.Collections.Concurrent;
using System.Text.RegularExpressions;

namespace SqlFirst.Core.Parsing
{
	/// <summary>
	/// Расширения для работы с секциями SQL файла
	/// </summary>
	public static class QuerySectionExtensions
	{
		private const string GetSectionTemplate = "--\\s*begin\\s*{0}\\s*(?<content>.*)\\s*--\\s*end";

		// (.*\r\n?\s*--\s*end.*?\r\n?)*\s*(?<body>.*)\s*
		private const string GetBodyPattern = "(.*\\r\\n?\\s*--\\s*end.*?\\r\\n?)*\\s*(?<body>.*)\\s*";

		private static readonly Regex _getBodyRegex = new Regex(GetBodyPattern, RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.Compiled);

		private static readonly ConcurrentDictionary<string, Regex> _sectionsRegexCache = new ConcurrentDictionary<string, Regex>
		{
			[QuerySectionName.QueryParameters] = CreateSectionRegex(QuerySectionName.QueryParameters)
		};

		/// <summary>
		/// Возвращает содержимое указанной секции
		/// </summary>
		/// <param name="query">Полный текст файла SQL</param>
		/// <param name="section">Имя секции</param>
		/// <returns>Содержимое секции, или пустая строка</returns>
		public static string GetQuerySection(this string query, string section)
		{
			if (query == null)
			{
				throw new System.ArgumentNullException(nameof(query));
			}

			if (!_sectionsRegexCache.TryGetValue(section, out Regex regex))
			{
				regex = CreateSectionRegex(section);
				_sectionsRegexCache.TryAdd(section, regex);
			}

			string content = regex.Match(query).Groups["content"]?.Value;
			return content ?? string.Empty;
		}

		/// <summary>
		/// Возвращает содержимое тела запроса
		/// </summary>
		/// <param name="query">Полный текст файла SQL</param>
		/// <returns>Содержимое тела запроса, или пустая строка</returns>
		public static string GetQueryBody(this string query)
		{
			if (query == null)
			{
				throw new System.ArgumentNullException(nameof(query));
			}

			string content = _getBodyRegex.Match(query).Groups["body"]?.Value;
			return content ?? string.Empty;
		}

		private static Regex CreateSectionRegex(string section)
		{
			string text = string.Format(GetSectionTemplate, section);
			const RegexOptions options = RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.Compiled;
			return new Regex(text, options);
		}
	}
}