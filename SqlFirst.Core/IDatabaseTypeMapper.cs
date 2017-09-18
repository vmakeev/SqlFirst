using System;

namespace SqlFirst.Core
{
	/// <summary>
	/// Преобразователь типов данных БД в типы CLR
	/// </summary>
	public interface IDatabaseTypeMapper
	{
		/// <summary>
		/// Возвращает имя типа CLR, который может быть безопасно использован для представления указанного типа данных в БД
		/// </summary>
		/// <param name="dbType">Название типа данных в БД</param>
		/// <param name="nullable">Поддерживается ли значение null</param>
		/// <returns>Имя типа CLR</returns>
		Type Map(string dbType, bool nullable);
	}
}