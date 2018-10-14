using System.Collections.Generic;

namespace SqlFirst.Codegen.Trees
{
	/// <summary>
	/// Сгенерированный элемент
	/// </summary>
	public interface IGeneratedItem
	{
		/// <summary>
		/// Перечень требуемых usings
		/// </summary>
		IEnumerable<string> Usings { get; }

		/// <summary>
		/// Пространство имен
		/// </summary>
		string Namespace { get; }

		/// <summary>
		/// Имя сгенерированного элемента
		/// </summary>
		string Name { get; }

		/// <summary>
		/// Модификаторы сгенерированного элемента
		/// </summary>
		IEnumerable<string> Modifiers { get; }

		/// <summary>
		/// Перечень базовых типов для сгенерированного
		/// </summary>
		IEnumerable<IGeneratedType> BaseTypes { get; }

		/// <summary>
		/// Содержимое сгенерированного элемента
		/// </summary>
		string Content { get; }

		/// <summary>
		/// Тип объекта (класс, перечисление, структура)
		/// </summary>
		string ObjectType { get; }

		/// <summary>
		/// Произвольные данные, располагающиеся перед началом описания элемента
		/// </summary>
		IEnumerable<string> BeforeItemData { get; }
	}
}