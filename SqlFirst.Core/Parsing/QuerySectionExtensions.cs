using System.Collections.Concurrent;
using System.Text.RegularExpressions;

namespace SqlFirst.Core.Parsing
{
	public static class QuerySectionExtensions
	{
		private const string GET_SECTION_TEMPLATE = "--\\s*begin\\s*{0}\\s*(?<content>.*)\\s*--\\s*end";

		private static readonly ConcurrentDictionary<string, Regex> _sectionsRegexCache = new ConcurrentDictionary<string, Regex>
		{
			[QuerySectionName.QueryParameters] = CreateSectionRegex(QuerySectionName.QueryParameters)
		};

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

		private static Regex CreateSectionRegex(string section)
		{
			string text = string.Format(GET_SECTION_TEMPLATE, section);
			const RegexOptions options = RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.Compiled;
			return new Regex(text, options);
		}
	}
}