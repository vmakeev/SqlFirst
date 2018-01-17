using SqlFirst.Intelligence.Options;

namespace SqlFirst.VisualStudio.ExternalTool
{
	public interface IPerformer
	{
		void Perform(GenerationOptions options);
	}
}