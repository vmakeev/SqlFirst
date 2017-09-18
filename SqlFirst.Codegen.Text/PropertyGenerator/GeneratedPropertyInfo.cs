namespace SqlFirst.Codegen.Text.PropertyGenerator
{
	/// <summary>
	/// Информация о сгенерированном свойстве
	/// </summary>
	internal class GeneratedPropertyInfo
	{
		/// <summary>
		/// Перечень требуемых импортов в коде
		/// </summary>
		public string[] Usings { get; }

		/// <summary>
		/// Фрагменты кода, сгенерированного для свойства
		/// </summary>
		public GeneratedPropertyPart[] Properties { get; }

		/// <summary>
		/// Инициализирует новый экземпляр класса <see cref="GeneratedPropertyInfo"/>
		/// </summary>
		/// <param name="usings">Перечень требуемых импортов в коде</param>
		/// <param name="properties">Фрагменты кода, сгенерированного для свойства</param>
		public GeneratedPropertyInfo(string[] usings, GeneratedPropertyPart[] properties)
		{
			Usings = usings;
			Properties = properties;
		}
	}
}