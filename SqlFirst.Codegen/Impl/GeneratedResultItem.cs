using System.Collections.Generic;
using SqlFirst.Codegen.Trees;

namespace SqlFirst.Codegen.Impl
{
	public class GeneratedResultItem : IGeneratedResultItem
	{
		/// <summary>
		/// Перечень требуемых using'ов
		/// </summary>
		public IEnumerable<string> Usings { get; set; } = new string[0];

		/// <summary>
		/// Пространство имен
		/// </summary>
		public string Namespace { get; set; }

		/// <summary>
		/// Имя сгенерированного элемента
		/// </summary>
		public string ItemName { get; set; }

		/// <summary>
		/// Модификаторы сгенерированного элемента
		/// </summary>
		public IEnumerable<string> ItemModifiers { get; set; } = new string[0];

		/// <summary>
		/// Перечень базовых для 
		/// </summary>
		public IEnumerable<IGeneratedType> BaseTypes { get; set; } = new IGeneratedType[0];

		/// <summary>
		/// Полный текст сгенерированного элемента
		/// </summary>
		public string Item { get; set; }
	}
}