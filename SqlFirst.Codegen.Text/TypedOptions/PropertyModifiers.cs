using System;

namespace SqlFirst.Codegen.Text.TypedOptions
{
	/// <summary>
	/// Модификаторы свойства
	/// </summary>
	[Flags]
	public enum PropertyModifiers
	{
		/// <summary>
		/// Модификаторы отсутствуют
		/// </summary>
		None = 0,

		/// <summary>
		/// Доступно только на чтение
		/// </summary>
		ReadOnly = 1,

		/// <summary>
		/// Виртуальное свойство
		/// </summary>
		Virtual = 2
	}
}