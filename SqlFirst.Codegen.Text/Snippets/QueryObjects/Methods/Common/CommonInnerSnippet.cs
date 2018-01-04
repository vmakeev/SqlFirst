namespace SqlFirst.Codegen.Text.Snippets.QueryObjects.Methods.Common
{
	internal class CommonInnerSnippet : QueryObjectsSnippetBase
	{
		public CommonInnerSnippet()
			: base("Methods.Common.Snippets")
		{
		}

		public string MapField => GetSnippetText();
	}
}