namespace SqlFirst.Codegen.Text.Snippets.QueryObjects.Methods.Get
{
	internal class GetInnerSnippet : QueryObjectsSnippetBase
	{
		public GetInnerSnippet()
			: base("Methods.Get.Snippets")
		{
		}

		public string CallAddParameter => GetSnippetText();
		public string CallAddParameterNumbered => GetSnippetText();
		public string MethodParameter => GetSnippetText();
		public string XmlParam => GetSnippetText();
	}
}