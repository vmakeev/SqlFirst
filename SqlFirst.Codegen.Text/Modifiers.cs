namespace SqlFirst.Codegen.Text
{
	/// <summary>
	/// Модификаторы класса
	/// </summary>
	internal static class Modifiers
	{
		/// <summary>
		/// Публичный класс
		/// </summary>
		public static string Public => @"public";

		/// <summary>
		/// Разделенный класс
		/// </summary>
		public static string Partial => @"partial";

		/// <summary>
		/// Статический класс
		/// </summary>
		public static string Static => @"static";
	}
}