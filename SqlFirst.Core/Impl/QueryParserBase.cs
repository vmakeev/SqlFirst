using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.RegularExpressions;

namespace SqlFirst.Core.Impl
{
	/// <inheritdoc />
	public abstract class QueryParserBase : IQueryParser
	{
		/// <summary>
		/// Регулярное выражение для поиска именованных секций запроса
		/// </summary>
		protected const string SectionRegexPattern = @"--\s*begin\s+(?<sectionName>[a-zA-Z0-9_]*)\s*\r?\n(?<content>.*?)\s*\r?\n\s*--\s*end\s*\r?\n";

		/// <inheritdoc />
		public virtual IEnumerable<IQueryParamInfo> GetDeclaredParameters(string queryText, string connectionString)
		{
			IEnumerable<string> allDeclarations = GetQuerySections(queryText, QuerySectionType.Declarations).Select(querySection => querySection.Content);
			string declarations = string.Join(Environment.NewLine, allDeclarations);
			if (string.IsNullOrEmpty(declarations))
			{
				yield break;
			}

			foreach (IQueryParamInfo info in GetDeclaredParametersInternal(declarations, connectionString))
			{
				yield return info;
			}
		}

		/// <inheritdoc />
		public virtual IEnumerable<IQueryParamInfo> GetUndeclaredParameters(string queryText, string connectionString)
		{
			IEnumerable<IQueryParamInfo> declared = GetDeclaredParameters(queryText, connectionString);
			foreach (IQueryParamInfo info in GetUndeclaredParametersInternal(declared, queryText, connectionString))
			{
				yield return info;
			}
		}

		/// <inheritdoc />
		public abstract IEnumerable<IFieldDetails> GetResultDetails(string queryText, string connectionString);

		/// <inheritdoc />
		public abstract IQueryBaseInfo GetQueryBaseInfo(string queryText);

		/// <inheritdoc />
		public abstract IQueryInfo GetQueryInfo(string queryText, string connectionString);

		/// <inheritdoc />
		[SuppressMessage("ReSharper", "SuggestVarOrType_SimpleTypes")]
		public virtual IEnumerable<IQuerySection> GetQuerySections(string query)
		{
			if (query == null)
			{
				throw new ArgumentNullException(nameof(query));
			}

			query = query.Trim();
			var sectionsInfo = new List<(int Start, int End, IQuerySection Section)>();

			// Все именованные секции - сюда
			Regex sectionsRegex = GetSectionsRegex();
			MatchCollection matches = sectionsRegex.Matches(query);
			foreach (Match match in matches)
			{
				if (match.Success)
				{
					string sectionName = match.Groups["sectionName"]?.Value;
					string content = match.Groups["content"]?.Value;
					QuerySectionType sectionType = GetSectionType(sectionName);

					var section = new QuerySection(sectionType, sectionName, content);
					var info = (Start: match.Index, End: match.Index + match.Length, Section: section);

					sectionsInfo.Add(info);
				}
			}

			if (sectionsInfo.Count > 0)
			{
				// Если первый раздел не в начале файла, то все, что до него, возвращаем как раздел неизвестного типа
				if (sectionsInfo[0].Start > 0)
				{
					string content = query.Substring(0, sectionsInfo[0].Start);
					yield return new QuerySection(QuerySectionType.Unknown, null, content);
				}

				// Теперь обрабатываем все, что между найденными разделами
				for (int i = 0; i < sectionsInfo.Count - 1; i++)
				{
					var current = sectionsInfo[i];
					var next = sectionsInfo[i + 1];

					// Сначала вернем текущий, чтобы не нарушать порядок
					yield return current.Section;

					// Если между разделами что-то есть
					if (current.End != next.Start)
					{
						string content = query.Substring(current.End, next.Start - current.End);
						yield return new QuerySection(QuerySectionType.Unknown, null, content);
					}
				}
			}

			int lastSectionEnds = sectionsInfo.LastOrDefault().End;

			if (lastSectionEnds > 0)
			{
				yield return sectionsInfo.Last().Section;
			}

			string bodyContent = query.Substring(lastSectionEnds, query.Length - lastSectionEnds);
			yield return new QuerySection(QuerySectionType.Body, null, bodyContent);
		}

		/// <inheritdoc />
		public virtual IEnumerable<IQuerySection> GetQuerySections(string query, string sectionName)
		{
			return GetQuerySections(query).Where(p => string.Equals(p.Name, sectionName, StringComparison.InvariantCultureIgnoreCase));
		}

		/// <inheritdoc />
		public virtual IEnumerable<IQuerySection> GetQuerySections(string query, QuerySectionType sectionType)
		{
			return GetQuerySections(query).Where(p => p.Type == sectionType);
		}

		/// <summary>
		/// Возвращает опции обработки запроса
		/// </summary>
		/// <param name="query">Полный текст файла SQL</param>
		/// <returns>Опции обработки запроса</returns>
		public virtual IEnumerable<ISqlFirstOption> GetOptions(string query)
		{
			IQuerySection[] optionsSections = GetQuerySections(query, QuerySectionType.Options).ToArray();

			string combinedOptions = string.Join(Environment.NewLine, optionsSections.Select(section => section.Content));

			string[] singleLineOptions = combinedOptions
										.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.None)
										.Where(line => !string.IsNullOrWhiteSpace(line))
										.Select(line => line.Trim())
										.ToArray();

			IEnumerable<string[]> splittedOptions = singleLineOptions
													.Where(line => line.StartsWith("--", StringComparison.Ordinal))
													.Select(line => line.Split(' ', '\t').Where(element => element != "--" && !string.IsNullOrWhiteSpace(element)).ToArray());

			foreach (string[] splittedOption in splittedOptions)
			{
				string name = splittedOption.First().TrimStart('-').Trim();
				IEnumerable<string> parameters = splittedOption.Skip(1).Select(parameter => parameter.Trim());
				yield return new SqlFirstOption(name, parameters);
			}
		}

		/// <summary>
		/// Создает регулярное выражение, возвращающее разделы запроса
		/// </summary>
		protected virtual Regex GetSectionsRegex()
		{
			const RegexOptions regexOptions = RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.Compiled;
			return new Regex(SectionRegexPattern, regexOptions);
		}

		/// <summary>
		/// Возвращает тип раздела запроса по его имени
		/// </summary>
		/// <param name="sectionName">Имя раздела</param>
		/// <returns></returns>
		protected virtual QuerySectionType GetSectionType(string sectionName)
		{
			if (string.IsNullOrEmpty(sectionName))
			{
				return QuerySectionType.Unknown;
			}

			if (string.Equals(sectionName, QuerySectionName.Declarations, StringComparison.InvariantCultureIgnoreCase))
			{
				return QuerySectionType.Declarations;
			}

			if (string.Equals(sectionName, QuerySectionName.Options, StringComparison.InvariantCultureIgnoreCase))
			{
				return QuerySectionType.Options;
			}

			return QuerySectionType.Custom;
		}

		/// <summary>
		/// Возвращает информацию о явно объявленных в секции "queryParameters" параметров запроса
		/// </summary>
		/// <param name="parametersDeclaration">Строка с объявленными переменными</param>
		/// <param name="connectionString">Строка подключения к БД</param>
		/// <returns>Информация о параметрах</returns>
		protected abstract IEnumerable<IQueryParamInfo> GetDeclaredParametersInternal(string parametersDeclaration, string connectionString);

		/// <summary>
		/// Возвращает информацию необъявленных в секции "queryParameters" параметров запроса
		/// </summary>
		/// <param name="declared">Уже объявленные параметры</param>
		/// <param name="queryText">Полный текст запроса</param>
		/// <param name="connectionString">Строка подключения к БД</param>
		/// <returns>Информация о параметрах</returns>
		protected abstract IEnumerable<IQueryParamInfo> GetUndeclaredParametersInternal(IEnumerable<IQueryParamInfo> declared, string queryText, string connectionString);
	}
}