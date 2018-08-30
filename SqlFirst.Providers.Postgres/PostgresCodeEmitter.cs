using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SqlFirst.Core;
using SqlFirst.Core.Impl;

namespace SqlFirst.Providers.Postgres
{
	/// <summary>
	/// Генерирует корректный SQL
	/// </summary>
	public class PostgresCodeEmitter : ISqlEmitter
	{
		public string EmitQuery(IEnumerable<IQuerySection> sections)
		{
			IQuerySection[] sectionsArray = sections.ToArray();
			if (sectionsArray.Any(p => p.Type == QuerySectionType.Declarations))
			{
				throw new QueryEmitException($"{nameof(PostgresCodeEmitter)} does not support variable declarations.");
			}

			var stringBuilder = new StringBuilder();
			string space = Environment.NewLine + Environment.NewLine;

			IEnumerable<IQuerySection> mergedSections = GetMergedSections(sectionsArray);
			IEnumerable<IQuerySection> sortedSections = GetSortedSections(mergedSections.ToArray());

			foreach (IQuerySection section in sortedSections)
			{
				string sectionText = EmitSection(section);
				if (!string.IsNullOrWhiteSpace(sectionText))
				{
					stringBuilder.Append(sectionText);
					stringBuilder.Append(space);
				}
			}

			return stringBuilder.ToString().Trim();
		}

		public string EmitSection(IQuerySection section)
		{
			if (string.IsNullOrWhiteSpace(section.Content))
			{
				return string.Empty;
			}

			string space = Environment.NewLine + Environment.NewLine;

			string sectionStart;
			string sectionEnd;
			switch (section.Type)
			{
				case QuerySectionType.Options:
				case QuerySectionType.Declarations:
				case QuerySectionType.Custom:
					sectionStart = $"-- begin {section.Name?.Trim()}{space}";
					sectionEnd = $"{space}-- end";
					break;

				case QuerySectionType.Body:
				case QuerySectionType.Unknown:
					sectionStart = string.Empty;
					sectionEnd = string.Empty;
					break;
				default:
					throw new QueryEmitException($"Unexpected querySectionType: {section.Type:G}({section.Type:D})");
			}

			string result = sectionStart + section.Content.Trim() + sectionEnd;

			return result;
		}
		
		/// <inheritdoc />
		public string EmitDeclarations(IEnumerable<IQueryParamInfo> infos)
		{
			throw new NotSupportedException("Postgres does not support parameter declarations");
		}

		/// <inheritdoc />
		public bool CanEmitDeclarations { get; } = false;

		public string EmitOption(ISqlFirstOption option)
		{
			return $"-- {option.Name} " + string.Join(" ", option.Parameters);
		}

		public string EmitOptions(IEnumerable<ISqlFirstOption> options)
		{
			IEnumerable<string> results = options.Select(EmitOption);
			string result = string.Join(Environment.NewLine, results);
			return result;
		}

		/// <summary>
		/// Сортирует разделы запроса. Разделы должны быть предварительно объединены
		/// </summary>
		/// <param name="sections">Разделы запроса</param>
		/// <returns>Отсортированные разделы</returns>
		private IEnumerable<IQuerySection> GetSortedSections(IQuerySection[] sections)
		{
			var result = new List<IQuerySection>(sections);

			// Тело запроса - в конец файла
			IQuerySection body = sections.SingleOrDefault(section => section.Type == QuerySectionType.Body) ?? new QuerySection(QuerySectionType.Body, string.Empty);

			if (result.IndexOf(body) != result.Count - 1)
			{
				result.Remove(body);
				result.Insert(result.Count, body);
			}

			// Объявления переменных - перед телом запроса
			IQuerySection declaration = sections.SingleOrDefault(section => section.Type == QuerySectionType.Declarations);
			if (declaration != null)
			{
				if (result.IndexOf(declaration) != result.Count - 2)
				{
					result.Remove(declaration);
					result.Insert(result.Count - 1, declaration);
				}
			}

			// Опции запроса следует разместить выше всех, кроме первой неопознанной
			IQuerySection options = sections.SingleOrDefault(section => section.Type == QuerySectionType.Options);
			if (options != null)
			{
				int firstUnknownIndex = result.FindIndex(p => p.Type == QuerySectionType.Unknown);
				int optionsIndex = firstUnknownIndex == 0 ? 1 : 0;

				if (result.IndexOf(options) != optionsIndex)
				{
					result.Remove(options);
					result.Insert(optionsIndex, options);
				}
			}

			return result;
		}

		private IEnumerable<IQuerySection> GetMergedSections(IQuerySection[] sections)
		{
			var result = new List<IQuerySection>(sections);

			MergeSections(result, QuerySectionType.Body);
			MergeSections(result, QuerySectionType.Options);
			MergeSections(result, QuerySectionType.Declarations);

			string[] customSectionNames = result
										.Where(section => section.Type == QuerySectionType.Custom)
										.Select(p => p.Name)
										.Distinct(StringComparer.InvariantCultureIgnoreCase).ToArray();

			foreach (string customSectionName in customSectionNames)
			{
				MergeSections(result, QuerySectionType.Custom, customSectionName);
			}

			return result;
		}

		private static void MergeSections(List<IQuerySection> sections, QuerySectionType sectionType, string sectionName = null)
		{
			IQuerySection[] targetSections = sectionName == null
				? sections.Where(section => section.Type == sectionType).ToArray()
				: sections.Where(section => section.Type == sectionType && string.Equals(section.Name, sectionName, StringComparison.InvariantCultureIgnoreCase)).ToArray();

			if (targetSections.Length > 1)
			{
				IQuerySection primaryTarget = targetSections[0];
				string contents = string.Join(Environment.NewLine, targetSections.Select(section => section.Content));
				var merged = new QuerySection(sectionType, sectionName ?? primaryTarget.Name, contents);
				int firstOptionIndex = sections.IndexOf(primaryTarget);
				sections.RemoveAll(targetSections.Contains);
				sections.Insert(firstOptionIndex, merged);
			}
		}
	}
}