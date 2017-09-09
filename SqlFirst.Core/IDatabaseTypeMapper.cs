namespace SqlFirst.Core
{
    /// <summary>
    /// Interface that for mapping C# types onto SQL types.
    /// </summary>
    public interface IDatabaseTypeMapper
    {
		/// <summary>
		/// Возвращает имя типа CLR, который может быть безопасно использован для представления указанного типа данных в БД
		/// </summary>
		/// <param name="dbType">Название типа данных в БД</param>
		/// <param name="dbTypeNormalized">Нормализованное название типа данных в БД</param>
		/// <param name="nullable">Поддерживается ли значение null</param>
		/// <returns>Имя типа CLR</returns>
		string Map(string dbType, out string dbTypeNormalized, bool nullable = true);
	}
}