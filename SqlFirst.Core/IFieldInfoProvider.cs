using System.Data;

namespace SqlFirst.Core
{
	/// <summary>
	/// Поставщик информации о столбце БД
	/// </summary>
	public interface IFieldInfoProvider
	{
		/// <summary>
		/// Возвращает данные о поле из строки с его метаданными
		/// </summary>
		/// <param name="fieldMetadata">Метаданные поля</param>
		/// <returns>Данные о поле</returns>
		IFieldDetails GetFieldDetails(DataRow fieldMetadata);
	}
}