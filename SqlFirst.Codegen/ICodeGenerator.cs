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
	}
}