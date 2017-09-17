namespace SqlFirst.Codegen.Text.PropertyGenerator
{
	internal class GeneratedPropertyInfo
	{
		public string[] Usings { get; }

		public GeneratedPropertyPart[] Properties { get; }

		/// <summary>Initializes a new instance of the <see cref="T:System.Object"></see> class.</summary>
		public GeneratedPropertyInfo(string[] usings, GeneratedPropertyPart[] properties)
		{
			Usings = usings;
			Properties = properties;
		}
	}
}