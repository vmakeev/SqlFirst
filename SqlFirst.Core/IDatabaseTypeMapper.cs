using System;
using System.Collections.Generic;
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
		/// <param name="metadata">Метаданные типа</param>
		/// <returns>Имя типа CLR</returns>
		Type MapToClrType(string dbType, bool nullable, IDictionary<string, object> metadata = null);

		/// <summary>
		/// Возвращает <see cref="DbType" />, который может быть безопасно использован для представления указанного типа данных в
		/// БД
		/// </summary>
		/// <param name="dbType">Название типа данных в БД</param>
		/// <param name="metadata">Метаданные типа</param>
		/// <returns>
		/// <see cref="DbType" />
		/// </returns>
		DbType MapToDbType(string dbType, IDictionary<string, object> metadata = null);

		/// <summary>
		/// Возвращает <see cref="IProviderSpecificType" />, который может быть безопасно использован для представления указанного
		/// типа данных в БД
		/// </summary>
		/// <param name="dbType">Название типа данных в БД</param>
		/// <param name="metadata">Метаданные типа</param>
		/// <returns>
		/// <see cref="IProviderSpecificType" />
		/// </returns>
		IProviderSpecificType MapToProviderSpecificType(string dbType, IDictionary<string, object> metadata = null);
	}
}