namespace SqlFirst.Core.Parsing
{
	/// <summary>
	/// Известные имена разделов
	/// </summary>
	public static class QuerySectionName
	{
		/// <summary>
		/// Объявление переменных
		/// </summary>
		public static string Declarations => "variables";

		/// <summary>
		/// Настройки SqlFirst
		/// </summary>
		public static string Options => "sqlFirstOptions";
	}
}