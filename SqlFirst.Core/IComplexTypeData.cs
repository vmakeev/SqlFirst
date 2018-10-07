using System.Collections.Generic;

namespace SqlFirst.Core
{
	/// <summary>
	/// Данные составного типа
	/// </summary>
	public interface IComplexTypeData
	{
		/// <summary>
		/// Имя элемента составного типа
		/// </summary>
		string Name { get; }

		/// <summary>
		/// Является ли составной тип табличным
		/// </summary>
		bool IsTableType { get; }

		/// <summary>
		/// Разрешается ли значение null
		/// </summary>
		bool AllowNull { get; }

		/// <summary>
		/// Отображаемое имя типа в БД
		/// </summary>
		string DbTypeDisplayedName { get; }

		/// <summary>
		/// Перечень полей типа
		/// </summary>
		IEnumerable<IFieldDetails> Fields { get; }
	}
}