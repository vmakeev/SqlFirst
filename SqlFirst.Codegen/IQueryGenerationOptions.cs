using System.Collections.Generic;
using SqlFirst.Core;

namespace SqlFirst.Codegen
{
	/// <summary>
	/// Параметры генерации объекта запроса
	/// </summary>
	public interface IQueryGenerationOptions
	{
		QueryType QueryType { get; }

		IEnumerable<ISqlFirstOption> SqlFirstOptions { get; }
		
		/// <inheritdoc />
		IOptionDefaults OptionDefaults { get; }
	}
}