using System.Collections.Generic;
using SqlFirst.Core;

namespace SqlFirst.Codegen
{
	/// <summary>
	/// Контекст генерации кода
	/// </summary>
	public interface ICodeGenerationContext
	{
		/// <summary>
		/// Входящие параметры запроса
		/// </summary>
		IEnumerable<IQueryParamInfo> IncomingParameters { get; }

		/// <summary>
		/// Результаты запроса
		/// </summary>
		IEnumerable<IFieldDetails> OutgoingParameters { get; }

		/// <summary>
		/// Дополнительные параметры кодогенерации
		/// </summary>
		IReadOnlyDictionary<string, object> Options { get; }
	}
}