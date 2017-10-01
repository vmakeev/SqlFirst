using System.Collections.Generic;
using SqlFirst.Core;

namespace SqlFirst.Codegen
{
	/// <summary>
	/// Параметры генерации объекта запроса
	/// </summary>
	public interface IQueryGenerationOptions
	{
		IEnumerable<ISqlFirstOption> SqlFirstOptions { get; }
	}
}