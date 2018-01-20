using SqlFirst.Core.Impl;

namespace SqlFirst.Core
{
	/// <summary>
	/// Опция кодогенератора SqlFirst
	/// </summary>
	public interface ISqlFirstOption
	{
		/// <summary>
		/// Имя опции
		/// </summary>
		string Name { get; }

		/// <summary>
		/// Параметры
		/// </summary>
		string[] Parameters { get; }
	}
}