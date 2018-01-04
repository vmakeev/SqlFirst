namespace SqlFirst.Codegen.Text.Snippets.QueryObjects.Methods.Get
{
	internal class GetInnerSnippet : QueryObjectsSnippetBase
	{
		public GetInnerSnippet()
			: base("Methods.Get.Snippets")
		{
		}

		public string AddParameter => GetSnippetText();
		public string MethodParameter => GetSnippetText();
		public string XmlParam => GetSnippetText();
	}
}