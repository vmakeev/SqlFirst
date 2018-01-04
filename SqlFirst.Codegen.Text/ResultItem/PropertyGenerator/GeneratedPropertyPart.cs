namespace SqlFirst.Codegen.Text.ResultItem.PropertyGenerator
{
	/// <summary>
	/// Фрагмент кода, сгенерированного для свойства
	/// </summary>
	internal class GeneratedPropertyPart
	{
		/// <summary>
		/// Является ли фрагмент описанием поля для записи и чтения значений свойства
		/// </summary>
		public bool IsBackingField { get; }

		/// <summary>
		/// Содержимое сгенерированного фрагмента
		/// </summary>
		public string Content { get; }

		/// <summary>
		/// Инициализирует новый экземпляр класса <see cref="GeneratedPropertyPart"/>
		/// </summary>
		/// <param name="content">Сгенерированный код</param>
		/// <param name="isBackingField">Является ли фрагмент описанием поля для записи и чтения значений свойства</param>
		public GeneratedPropertyPart(string content, bool isBackingField)
		{
			IsBackingField = isBackingField;
			Content = content;
		}
	}
}