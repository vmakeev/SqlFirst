using System.IO;

namespace SqlFirst.Intelligence.Options
{
	public static class OptionsValidator
	{
		public static (bool isValid, string error) Validate(this GenerationOptions generationOptions)
		{
			if (generationOptions == null)
			{
				return (false, "Отсутствует набор параметров");
			}

			if (string.IsNullOrEmpty(generationOptions.Target))
			{
				return (false, "Не указан путь к файлу SQL");
			}

			if (!File.Exists(generationOptions.Target))
			{
				return (false, "Файл SQL не найден");
			}

			if (generationOptions.Dialect == null)
			{
				return (false, "Не указан диалект SQL");
			}

			return (true, null);
		}
	}
}