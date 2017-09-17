namespace SqlFirst.Codegen
{
	/// <summary>
	/// Параметры генерации объекта выходных данных запроса
	/// </summary>
	public interface IResultGenerationOptions
	{
		/// <summary>
		/// Тип объекта
		/// </summary>
		ResultItemType ItemType { get; }

		/// <summary>
		/// Тип свойств
		/// </summary>
		PropertyType PropertyType { get; }

		/// <summary>
		/// Модификаторы свойств
		/// </summary>
		PropertyModifiers PropertyModifiers { get; }
	}
}