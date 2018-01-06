using System.Collections.Generic;
using SqlFirst.Core;

namespace SqlFirst.Codegen
{
	/// <summary>
	/// Параметры генерации объекта входных параметров запроса
	/// </summary>
	public interface IParameterGenerationOptions
	{
		IEnumerable<ISqlFirstOption> SqlFirstOptions { get; }
	}
}