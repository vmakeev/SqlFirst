namespace SqlFirst.Codegen.Text.PropertyGenerator
{
	internal class GeneratedPropertyPart
	{
		public bool IsBackingField { get; }

		public string Content { get; }

		/// <summary>Initializes a new instance of the <see cref="T:System.Object"></see> class.</summary>
		public GeneratedPropertyPart(string content, bool isBackingField)
		{
			IsBackingField = isBackingField;
			Content = content;
		}
	}
}