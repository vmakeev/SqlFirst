using SqlFirst.Codegen.Text.Snippets.QueryObjects.Methods.Get;

namespace SqlFirst.Codegen.Text.Snippets.QueryObjects.Methods
{
	internal class GetSnippet : QueryObjectsSnippetBase
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Object"></see> class.</summary>
		public GetSnippet()
			: base("Methods.Get")
		{
		}

		public GetInnerSnippet Snippets { get; } = new GetInnerSnippet();

		public string GetFirst => GetSnippetText();
		public string GetIEnumerable => GetSnippetText();
		public string GetIEnumerableAsync => GetSnippetText();

		public string GetScalar => GetSnippetText();
		public string GetScalarAsync => GetSnippetText();
		public string GetScalars => GetSnippetText();
		public string GetScalarsAsync => GetSnippetText();
	}
}