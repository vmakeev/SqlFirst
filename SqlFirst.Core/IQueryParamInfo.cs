using System;

namespace SqlFirst.Core
{
	/// <summary>
	/// Информация о параметре запроса
	/// </summary>
	public interface IQueryParamInfo
	{
		/// <summary>
		/// Имя параметра в C#
		/// </summary>
		string CsName { get; set; }

		/// <summary>
		/// Тип данных в CLR
		/// </summary>
		Type ClrType { get; set; }

		/// <summary>
		/// Длина
		/// </summary>
		int Length { get; set; }

		/// <summary>
		/// Численная точность
		/// </summary>
		int Precision { get; set; }

		/// <summary>
		/// Точность дробной части
		/// </summary>
		int Scale { get; set; }

		/// <summary>
		/// Имя параметра в запроса
		/// </summary>
		string DbName { get; set; }

		/// <summary>
		/// Тип параметра в БД
		/// </summary>
		string DbType { get; set; }
	}
}