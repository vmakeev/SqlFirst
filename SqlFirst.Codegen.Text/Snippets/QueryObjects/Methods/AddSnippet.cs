
namespace SqlFirst.Codegen.Text.Snippets.QueryObjects.Methods
{
	internal class AddSnippet : QueryObjectsSnippetBase
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Object"></see> class.</summary>
		public AddSnippet()
			: base("Methods.Add")
		{
		}

		public string AddSingle => GetSnippetText();

		public string AddMultiple => GetSnippetText();
		public string AddMultipleAsync => GetSnippetText();

		public string AddSingleAsync => GetSnippetText();
	}
}