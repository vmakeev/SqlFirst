using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace SqlFirst.Codegen.Text.Snippets
{
	internal static class Snippet
	{
		public static string NotifyPropertyChangedBackingFieldProperty => GetSnippetText();

		public static string NotifyPropertyChangedReadOnlyBackingFieldProperty => GetSnippetText();

		public static string NotifyPropertyChangedBackingFieldPropertyVirtual => GetSnippetText();

		public static string NotifyPropertyChangedReadOnlyBackingFieldPropertyVirtual => GetSnippetText();

		public static string NotifyPropertyChangedResultItem => GetSnippetText();

		public static string PocoResultItem => GetSnippetText();

		public static string StructResultItem => GetSnippetText();

		public static string AutoPropertyVirtual => GetSnippetText();

		public static string AutoProperty => GetSnippetText();

		public static string ReadOnlyAutoPropertyVirtual => GetSnippetText();

		public static string ReadOnlyAutoProperty => GetSnippetText();

		public static string BackingField => GetSnippetText();

		public static string BackingFieldPropertyVirtual => GetSnippetText();

		public static string BackingFieldProperty => GetSnippetText();

		public static string ReadOnlyBackingFieldPropertyVirtual => GetSnippetText();

		public static string ReadOnlyBackingFieldProperty => GetSnippetText();

		private static string GetSnippetText([CallerMemberName] string name = null)
		{
			string resourceName = $"SqlFirst.Codegen.Text.Snippets.{name}.txt";
			Stream stream = typeof(Snippet).Assembly.GetManifestResourceStream(resourceName);
			string queryText = new StreamReader(stream ?? throw new CodeGenerationException($"Resource not found: {resourceName}")).ReadToEnd();
			return queryText;
		}
	}
}