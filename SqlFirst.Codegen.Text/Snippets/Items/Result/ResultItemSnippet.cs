using SqlFirst.Codegen.Text.Templating;

namespace SqlFirst.Codegen.Text.Snippets.Items.Result
{
	internal class ResultItemSnippet : SqlFirstSnippet
	{
		public IRenderableTemplate NotifyPropertyChangedClassResultItem => GetRenderableTemplate();

		public IRenderableTemplate NotifyPropertyChangedStructResultItem => GetRenderableTemplate();

		public IRenderableTemplate PocoResultItem => GetRenderableTemplate();

		public IRenderableTemplate StructResultItem => GetRenderableTemplate();

		public ResultItemSnippet()
			: base("Items.Result")
		{
		}
	}
}