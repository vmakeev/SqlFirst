using System.Collections.Generic;

namespace SqlFirst.Core.Parsing
{
	/// <inheritdoc />
	public abstract class QueryParamsParser : IQueryParamsParser
	{
		/// <inheritdoc />
		public virtual IEnumerable<IQueryParamInfo> GetDeclaredParameters(string queryText)
		{
			string section = queryText.GetQuerySection(QuerySectionName.QueryParameters);
			if (string.IsNullOrEmpty(section))
			{
				yield break;
			}

			foreach (IQueryParamInfo info in GetDeclaredParametersInternal(section))
			{
				yield return info;
			}
		}

		/// <inheritdoc />
		public IEnumerable<IQueryParamInfo> GetUndeclaredParameters(string queryText, string connectionString)
		{
			IEnumerable<IQueryParamInfo> declared = GetDeclaredParameters(queryText);
			foreach (IQueryParamInfo info in GetUndeclaredParametersInternal(declared, queryText, connectionString))
			{
				yield return info;
			}
		}

		/// <summary>
		/// Возвращает информацию о явно объявленных в секции "queryParameters" параметров запроса
		/// </summary>
		/// <param name="parametersDeclaration">Строка с объявленными переменными</param>
		/// <returns>Информация о параметрах</returns>
		protected abstract IEnumerable<IQueryParamInfo> GetDeclaredParametersInternal(string parametersDeclaration);

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