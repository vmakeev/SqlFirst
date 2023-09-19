using System;
using System.Runtime.CompilerServices;
using System.Text;
using Microsoft.Extensions.Logging;
using Mono.Options;
using SqlFirst.Core;
using SqlFirst.Intelligence.Options;

[assembly: InternalsVisibleTo("SqlFirst.ExternalTool.Tests")]

namespace SqlFirst.ExternalTool
{
	public static class Cli
	{
		private static readonly ILogger _log = LogManager.GetLogger(typeof(Cli));

		public static int Main(params string[] args)
		{
			Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
			Console.OutputEncoding = Encoding.GetEncoding(1251);
			var options = new GenerationOptions();

			bool helpOnly = false;

			OptionSet optionsSet = new OptionSet()
									.Add("f|folder|file=", "Путь к файлу SQL или папке с файлами", s => options.Target = s)
									.Add("ns|namespace=", "Namespace генерируемых классов", s => options.Namespace = s)
									.Add("cs|connectionstring=", "Строка подключения к БД", s => options.ConnectionString = s)
									.Add("p|project=", "Путь к файлу проекта", s => options.ProjectFile = s)
									.Add("s|solution=", "Путь к файлу решения", s => options.SolutionFile = s)
									.Add("i|in=", "Имя генерируемого аргумента запроса", s => options.ParameterItemName = s)
									.Add("o|out=", "Имя генерируемого результата запроса", s => options.ResultItemName = s)
									.Add("csproj=", "Добавить сгенерированные файлы в проект (выключено по умолчанию)", s => options.UpdateCsproj = bool.Parse(s))
									.Add("b|beautify", "Форматировать текст запроса", s => options.BeautifyFile = true)
									.Add("d|dialect=", "Диалект SQL: MsSqlServer или Postgres", s => options.Dialect = (Dialect)Enum.Parse(typeof(Dialect), s, true))
									.Add("?|help", "Справка", s => helpOnly = true);

			optionsSet.Parse(args);

			if (helpOnly || args.Length < 2)
			{
				optionsSet.WriteOptionDescriptions(Console.Out);
				return 0;
			}

			try
			{
				IPerformer generator = PerformerSelector.Select(options);
				generator.Perform(options);
			}
			catch (Exception ex)
			{
				_log.LogCritical(ex, "Query generation error");
				return -1;
			}

			return 0;
		}
	}
}