using System.Collections.Generic;
using SqlFirst.Codegen.Trees;

namespace SqlFirst.Codegen
{
	/// <summary>
	/// Генератор кода
	/// </summary>
	public interface ICodeGenerator
	{
		/// <summary>
		/// Выполняет генерацию объекта, представляющего запрос
		/// </summary>
		/// <param name="context">Контекст генерации кода</param>
		/// <param name="options">Параметры генерации</param>
		/// <returns>Сгенерированный объект</returns>
		IGeneratedQueryObject GenerateQueryObject(ICodeGenerationContext context, IQueryGenerationOptions options);

		/// <summary>
		/// Выполняет генерацию объекта, представляющего результат выполнения запроса
		/// </summary>
		/// <param name="context">Контекст генерации кода</param>
		/// <param name="options">Параметры генерации</param>
		/// <returns>Сгенерированный объект</returns>
		IGeneratedResultItem GenerateResultItem(ICodeGenerationContext context, IResultGenerationOptions options);

		/// <summary>
		/// Выполняет генерацию файла
		/// </summary>
		/// <param name="generatedItems">Набор сгеренированных элементов, которые следует разместить в файле</param>
		/// <returns>Сгенерированный файл</returns>
		string GenerateFile(IEnumerable<IGeneratedItem> generatedItems);
	}
}