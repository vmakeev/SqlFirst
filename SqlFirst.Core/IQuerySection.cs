using SqlFirst.Core.Impl;

namespace SqlFirst.Core
{
	/// <summary>
	/// Описание раздела запроса
	/// </summary>
	public interface IQuerySection
	{
		/// <summary>
		/// Тип раздела
		/// </summary>
		QuerySectionType Type { get; }

		/// <summary>
		/// Имя раздела
		/// </summary>
		string Name { get; }

		/// <summary>
		/// Содержимое раздела
		/// </summary>
		string Content { get; }
	}
}