namespace SqlFirst.Codegen.Text.Snippets.QueryObjects.Methods.Common.Snippets
{
	internal class CommonInnerSnippet : QueryObjectsSnippetBase
	{
		public CommonInnerSnippet()
			: base("Methods.Common.Snippets")
		{
		}

		public string MapField => GetSnippetText();

		public string GetQueryTemplates => GetSnippetText();

		public string GetInsertedValuesSection => GetSnippetText();

		public string GetNumberedParameters => GetSnippetText();

		public string NumberedParameterInfo => GetSnippetText();
		public string CalculateChecksum => GetSnippetText();
	}
}