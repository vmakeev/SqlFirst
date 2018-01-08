using System;

namespace SqlFirst.Core
{
	/// <summary>
	/// Информация об истинных типах, используемых поставщиком данных
	/// </summary>
	public interface IProviderTypesInfo
	{
		/// <summary>
		/// Тип подключения к БД
		/// </summary>
		Type ConnectionType { get; }

		/// <summary>
		/// Тип команды
		/// </summary>
		Type CommandType { get; }

		/// <summary>
		/// Тип параметра
		/// </summary>
		Type CommandParameterType { get; }

		/// <summary>
		/// Тип специфичного для параметра типа данных БД
		/// </summary>
		Type CommandParameterSpecificDbTypePropertyType { get; }

		/// <summary>
		/// Имя свойства параметра, содержащего специфичный тип данных БД
		/// </summary>
		string CommandParameterSpecificDbTypePropertyName { get; }

	}
}