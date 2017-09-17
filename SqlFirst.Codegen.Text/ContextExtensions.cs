using System;

namespace SqlFirst.Codegen.Text
{
	internal static class ContextExtensions
	{
		public static string GetNamespace(this ICodeGenerationContext context)
		{
			return context.GetOption<string>("Namespace");
		}

		public static string GetQueryName(this ICodeGenerationContext context)
		{
			return context.GetOption<string>("QueryName");
		}

		private static T GetOption<T>(this ICodeGenerationContext context, string property) where T : class
		{
			if (context?.Options == null)
			{
				return null;
			}

			if (!context.Options.TryGetValue(property, out object result))
			{
				return null;
			}

			if (result is T typedResult)
			{
				return typedResult;
			}

			throw new InvalidCastException();
		}
	}
}