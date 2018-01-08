using System;
using System.Data;

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
		Type MapToClrType(string dbType, bool nullable);

		/// <summary>
		/// Возвращает <see cref="DbType"/>, который может быть безопасно использован для представления указанного типа данных в БД
		/// </summary>
		/// <param name="dbType">Название типа данных в БД</param>
		/// <returns><see cref="DbType"/></returns>
		DbType MapToDbType(string dbType);

		/// <summary>
		/// Возвращает <see cref="IProviderSpecificType"/>, который может быть безопасно использован для представления указанного типа данных в БД
		/// </summary>
		/// <param name="dbType">Название типа данных в БД</param>
		/// <returns><see cref="IProviderSpecificType"/></returns>
		IProviderSpecificType MapToProviderSpecificType(string dbType);

	}
}