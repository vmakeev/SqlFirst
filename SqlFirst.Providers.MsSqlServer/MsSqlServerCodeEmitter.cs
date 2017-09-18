using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using SqlFirst.Core;
using SqlFirst.Core.Impl;

namespace SqlFirst.Providers.MsSqlServer
{
	/// <summary>
	/// Генерирует корректный SQL
	/// </summary>
	public class MsSqlServerCodeEmitter
	{
		public string EmitQuery(IEnumerable<IQuerySection> sections)
		{
			var stringBuilder = new StringBuilder();
			string space = Environment.NewLine + Environment.NewLine;

			IEnumerable<IQuerySection> mergedSections = GetMergedSections(sections.ToArray());
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

		/// <summary>
		/// Сортирует разделы запроса. Разделы должны быть предварительно объединены
		/// </summary>
		/// <param name="sections">Разделы запроса</param>
		/// <returns>Отсортированные разделы</returns>
		private IEnumerable<IQuerySection> GetSortedSections(IQuerySection[] sections)
		{
			var result = new List<IQuerySection>(sections);

			// Тело запроса - в конец файла
			IQuerySection body = sections.SingleOrDefault(section => section.Type == QuerySectionType.Body) 
				?? new QuerySection(QuerySectionType.Body, string.Empty);

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
					throw new QueryParsingException($"Unexpected querySectionType: {section.Type:G}({section.Type:D})");
			}

			string result = sectionStart + section.Content.Trim() + sectionEnd;

			return result;
		}

		/// <summary>
		/// Возвращает корректное константное значение, которое может быть использовано в SQL
		/// </summary>
		/// <param name="value">Значение</param>
		/// <param name="dbType">Тип значения в БД</param>
		/// <returns>Константное значение, которое может быть использовано в SQL</returns>
		public string EmitValue(object value, string dbType = null)
		{
			bool IsUnicodeString(string type)
			{
				return type == MsSqlDbType.NVarChar ||
						type == MsSqlDbType.NText ||
						type == MsSqlDbType.NChar;
			}

			switch (value)
			{
				case null:
					return null;

				case string stringValue when IsUnicodeString(dbType):
					return $"N'{stringValue}'";

				case string stringValue:
					return $"'{stringValue}'";

				case int intValue:
					return intValue.ToString(CultureInfo.InvariantCulture);

				case float floatValue:
					return floatValue.ToString(CultureInfo.InvariantCulture);

				default:
					throw new QueryParsingException($"Valid defaultValue type expected, actual is [{value.GetType().Name}]");
			}
		}

		/// <summary>
		/// Возвращает текст SQL для объявления указанного параметра
		/// </summary>
		/// <param name="info">Информация о параметре</param>
		/// <returns>Текст SQL для объявления указанного параметра</returns>
		public string EmitDeclaration(IQueryParamInfo info)
		{
			if (info == null)
			{
				throw new ArgumentNullException(nameof(info));
			}

			if (string.IsNullOrWhiteSpace(info.DbName))
			{
				throw new ArgumentException($"{nameof(info.DbName)} can not be empty");
			}

			if (string.IsNullOrWhiteSpace(info.DbType))
			{
				throw new ArgumentException($"{nameof(info.DbType)} can not be empty");
			}

			string fullTypeName = string.IsNullOrEmpty(info.Length)
				? info.DbType
				: $"{info.DbType}({info.Length.ToUpperInvariant()})";

			string defaultValue = EmitValue(info.DefaultValue, info.DbType);

			string result = string.IsNullOrEmpty(defaultValue)
				? $@"DECLARE @{info.DbName} {fullTypeName};"
				: $@"DECLARE @{info.DbName} {fullTypeName} = {defaultValue};";

			return result;
		}

		/// <summary>
		/// Возвращает текст SQL для объявления указанных параметров
		/// </summary>
		/// <param name="infos">Информация о параметрах</param>
		/// <returns>Текст SQL для объявления указанных параметров</returns>
		public string EmitDeclarations(IEnumerable<IQueryParamInfo> infos)
		{
			if (infos == null)
			{
				throw new ArgumentNullException(nameof(infos));
			}

			IEnumerable<string> declarations = infos.Select(EmitDeclaration);
			string result = string.Join(Environment.NewLine, declarations);
			return result;
		}
	}
}
