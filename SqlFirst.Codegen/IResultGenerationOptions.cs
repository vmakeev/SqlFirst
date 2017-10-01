using System.Collections.Generic;
using SqlFirst.Core;

namespace SqlFirst.Codegen
{
	/// <summary>
	/// Параметры генерации объекта выходных данных запроса
	/// </summary>
	public interface IResultGenerationOptions
	{
		IEnumerable<ISqlFirstOption> SqlFirstOptions { get; }
	}
}