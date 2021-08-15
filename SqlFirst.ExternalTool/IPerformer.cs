using SqlFirst.Intelligence.Options;

namespace SqlFirst.ExternalTool
{
	public interface IPerformer
	{
		void Perform(GenerationOptions options);
	}
}