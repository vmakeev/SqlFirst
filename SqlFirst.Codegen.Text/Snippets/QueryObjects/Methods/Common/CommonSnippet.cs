using SqlFirst.Codegen.Text.Snippets.QueryObjects.Methods.Common.Snippets;

namespace SqlFirst.Codegen.Text.Snippets.QueryObjects.Methods.Common
{
	internal class CommonSnippet : QueryObjectsSnippetBase
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Object"></see> class.</summary>
		public CommonSnippet()
			: base("Methods.Common")
		{
		}

		public CommonInnerSnippet Snippets { get; } = new CommonInnerSnippet();

		public string AddParameter => GetSnippetText();
		public string GetItemFromRecord => GetSnippetText();
		public string GetScalarFromRecord => GetSnippetText();
		public string GetScalarFromValue => GetSnippetText();
		public string GetQueryFromResourceCacheable => GetSnippetText();
		public string GetQueryFromString => GetSnippetText();
		public string GetQueryFromStringMultipleInsert => GetSnippetText();
		public string GetQueryRuntimeGeneratedMultipleInsert => GetSnippetText();
	}
}