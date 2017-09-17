using System.Collections.Generic;

namespace SqlFirst.Codegen.Trees
{
	/// <summary>
	/// Сгенерированный элемент
	/// </summary>
	public interface IGeneratedItem
	{
		/// <summary>
		/// Перечень требуемых using'ов
		/// </summary>
		IEnumerable<string> Usings { get; }

		/// <summary>
		/// Пространство имен
		/// </summary>
		string Namespace { get; }

		/// <summary>
		/// Имя сгенерированного элемента
		/// </summary>
		string ItemName { get; }

		/// <summary>
		/// Модификаторы сгенерированного элемента
		/// </summary>
		IEnumerable<string> ItemModifiers { get; }

		/// <summary>
		/// Перечень базовых типов для сгенерированного
		/// </summary>
		IEnumerable<IGeneratedType> BaseTypes { get; set; }

		/// <summary>
		/// Полный текст сгенерированного элемента
		/// </summary>
		string Item { get; }
	}
}