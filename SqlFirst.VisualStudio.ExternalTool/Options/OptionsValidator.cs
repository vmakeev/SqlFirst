using System.IO;
using SqlFirst.Codegen.Helpers;

namespace SqlFirst.VisualStudio.ExternalTool.Options
{
	internal static class OptionsValidator
	{
		public static (bool isValid, string error) Validate(this GenerationOptions generationOptions)
		{
			if (generationOptions == null)
			{
				return (false, "Отсутствует набор параметров");
			}

			if (string.IsNullOrEmpty(generationOptions.QueryFile))
			{
				return (false, "Не указан путь к файлу SQL");
			}

			if (!File.Exists(generationOptions.QueryFile))
			{
				return (false, "Файл SQL не найден");
			}

			if (!string.IsNullOrEmpty(generationOptions.ResultItemName) && CSharpCodeHelper.GetValidIdentifierName(generationOptions.ResultItemName) != generationOptions.ResultItemName)
			{
				return (false, "Некорректное имя генерируемого результата");
			}

			if (!string.IsNullOrEmpty(generationOptions.ParameterItemName) &&
				CSharpCodeHelper.GetValidIdentifierName(generationOptions.ParameterItemName) != generationOptions.ParameterItemName)
			{
				return (false, "Некорректное имя параметра запроса");
			}

			if (generationOptions.Dialect == null)
			{
				return (false, "Не указан диалект SQL");
			}

			return (true, null);
		}
	}
}