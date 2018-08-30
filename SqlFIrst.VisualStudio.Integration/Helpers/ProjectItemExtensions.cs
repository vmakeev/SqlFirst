using System.Diagnostics.CodeAnalysis;
using System.IO;
using Common.Logging;
using EnvDTE;
using SqlFirst.Intelligence.Helpers;

namespace SqlFIrst.VisualStudio.Integration.Helpers
{
	internal static class ProjectItemExtensions
	{
		[SuppressMessage("ReSharper", "PossibleNullReferenceException")]
		public static string GetNamespace(this ProjectItem item)
		{
			string rootNamespace = item.ContainingProject.Properties.Item("DefaultNamespace").Value.ToString();

			string projectFolder = Path.GetDirectoryName(item.ContainingProject.FullName);
			string itemFolder = Path.GetDirectoryName(item.GetFullPath());

			string relativePath = PathHelper.GetRelativePath(projectFolder, itemFolder);
			string relativeNamespace = relativePath
										.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
										.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar)
										.Replace(Path.DirectorySeparatorChar, '.');

			string itemNamespace = string.Join(".", rootNamespace, relativeNamespace);
			return itemNamespace;
		}

		public static string GetText(this ProjectItem item)
		{
			if (item.IsDirty)
			{
				item.Save();
			}

			string itemPath = item.GetFullPath();
			return File.ReadAllText(itemPath);
		}

		public static string GetFullPath(this ProjectItem item)
		{
			return item.Properties.Item("FullPath").Value.ToString();
		}

		public static void RemoveDependentObject(this ProjectItem item, string fileName)
		{
			foreach (ProjectItem projectItem in item.ProjectItems)
			{
				if (projectItem.Name == fileName)
				{
					string filePath = projectItem.GetFullPath();

					projectItem.Delete();

					if (File.Exists(filePath))
					{
						File.Delete(filePath);
					}

					return;
				}
			}
		}

		[SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
		public static void AddDependentObject(this ProjectItem item, string fileName, string content)
		{
			string targetFolder = Path.GetDirectoryName(item.GetFullPath());
			string objectFileName = Path.Combine(targetFolder, fileName);
			File.WriteAllText(objectFileName, content);
			// todo: netstandard projects doesn't make dependentupon https://github.com/dotnet/project-system/issues/1870
			item.ProjectItems.AddFromFile(objectFileName);
		}

		public static void EnsureIsEmbeddedResource(this ProjectItem item, ILog log = null)
		{
			const string embeddedResource = "EmbeddedResource";

			string itemType = item.Properties.Item("ItemType").Value.ToString();
			if (itemType != embeddedResource)
			{
				log?.Warn($"Query SQL file \"{item.Name}\" is not an {embeddedResource}. Trying to fix it");
				item.Properties.Item("ItemType").Value = embeddedResource;
			}
		}
	}
}