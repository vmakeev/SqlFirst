
namespace SqlFirst.Codegen.Text.Snippets.QueryObjects.Methods.Add
{
	internal class AddSnippet : QueryObjectsSnippetBase
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Object"></see> class.</summary>
		public AddSnippet()
			: base("Methods.Add")
		{
		}

		public string AddSingle => GetSnippetText();
		public string AddSingleAsync => GetSnippetText();

		public string AddSingleWithResult => GetSnippetText();
		public string AddSingleWithResultAsync => GetSnippetText();

		public string AddSingleWithScalarResult => GetSnippetText();
		public string AddSingleWithScalarResultAsync => GetSnippetText();

		public string AddMultiple => GetSnippetText();
		public string AddMultipleAsync => GetSnippetText();

		public string AddMultipleWithResult => GetSnippetText();
		public string AddMultipleWithResultAsync => GetSnippetText();

		public string AddMultipleWithScalarResult => GetSnippetText();
		public string AddMultipleWithScalarResultAsync => GetSnippetText();

	}
}