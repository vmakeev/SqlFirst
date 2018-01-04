using System;

namespace SqlFirst.Codegen.Text.ResultItem.TypedOptions
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