using System;

namespace SqlFirst.Codegen.Text.TypedOptions
{
	/// <summary>
	/// Особые умения класса результата запроса
	/// </summary>
	[Flags]
	public enum ResultItemAbilities
	{
		/// <summary>
		/// Отсутствуют
		/// </summary>
		None = 0,

		/// <summary>
		/// Уведомление об изменении свойств
		/// </summary>
		NotifyPropertyChanged = 1
	}
}