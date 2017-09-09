namespace SqlFirst.Core
{
	public class QueryParamInfo : IQueryParamInfo
	{
		/// <summary>
		/// Имя параметра в C#
		/// </summary>
		public string CsName { get; set; }

		/// <summary>
		/// Тип параметра в C#
		/// </summary>
		public string CsType { get; set; }

		/// <summary>
		/// Имя параметра в запросе
		/// </summary>
		public string DbName { get; set; }

		/// <summary>
		/// Тип параметра в запросе
		/// </summary>
		public string DbType { get; set; }

		/// <summary>
		/// Длина параметра
		/// </summary>
		public int Length { get; set; }

		/// <summary>
		/// Числовая точность
		/// </summary>
		public int Precision { get; set; }

		/// <summary>
		/// Точность после запятой
		/// </summary>
		public int Scale { get; set; }
	}
}