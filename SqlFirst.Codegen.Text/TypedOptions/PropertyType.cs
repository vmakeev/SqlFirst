namespace SqlFirst.Codegen.Text.TypedOptions
{
	/// <summary>
	/// Тип свойств
	/// </summary>
	public enum PropertyType
	{
		/// <summary>
		/// Некорректный тип свойств
		/// </summary>
		INVALID,

		/// <summary>
		/// Автосвойство
		/// </summary>
		Auto,

		/// <summary>
		/// Свойство с отдельным полем
		/// </summary>
		BackingField,
	}
}