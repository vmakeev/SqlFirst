namespace SqlFirst.Codegen.Text
{
	/// <summary>
	/// Модификаторы
	/// </summary>
	internal static class Modifiers
	{
		/// <summary>
		/// Публичный
		/// </summary>
		public static string Public => @"public";

		/// <summary>
		/// Разделенный
		/// </summary>
		public static string Partial => @"partial";

		/// <summary>
		/// Статический
		/// </summary>
		public static string Static => @"static";

		/// <summary>
		/// Приватный
		/// </summary>
		public static string Private => @"private";

		/// <summary>
		/// Защищенный
		/// </summary>
		public static string Protected => @"protected";

		/// <summary>
		/// Только для чтения
		/// </summary>
		public static string Readonly => @"readonly";
	}
}