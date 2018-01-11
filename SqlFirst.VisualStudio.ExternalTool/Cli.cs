using System;
using System.Text;
using Common.Logging;
using Mono.Options;
using SqlFirst.VisualStudio.ExternalTool.Options;

namespace SqlFirst.VisualStudio.ExternalTool
{
	public static class Cli
	{
		private static readonly ILog _log = LogManager.GetLogger("Main");

		public static int Main(params string[] args)
		{
			Console.OutputEncoding = Encoding.GetEncoding(1251);
			var options = new GenerationOptions();

			bool helpOnly = false;

			OptionSet optionsSet = new OptionSet()
									.Add("f|file=", "Путь к файлу SQL", s => options.QueryFile = s)
									.Add("ns|namespace=", "Namespace генерируемых классов", s => options.Namespace = s)
									.Add("cs|connectionstring=", "Строка подключения к БД", s => options.ConnectionString = s)
									.Add("p|project=", "Путь к файлу проекта", s => options.ProjectFile = s)
									.Add("s|solution=", "Путь к файлу решения", s => options.SolutionFile = s)
									.Add("i|in=", "Имя генерируемого аргумента запроса", s => options.ParameterItemName = s)
									.Add("o|out=", "Имя генерируемого результата запроса", s => options.ResultItemName = s)
									.Add("b|beautify", "Форматировать текст запроса", s => options.BeautifyFile = true)
									.Add("d|dialect=", "Диалект SQL: MsSqlServer или Postgres", s => options.Dialect = (Dialect)Enum.Parse(typeof(Dialect), s, true))
									.Add("?|help", "Справка", s => helpOnly = true);

			optionsSet.Parse(args);

			if (helpOnly || args.Length < 2)
			{
				optionsSet.WriteOptionDescriptions(Console.Out);
				return 0;
			}

			var generator = new Performer();

			try
			{
				generator.Perform(options);
			}
			catch (Exception ex)
			{
				_log.Fatal("Query generation error", ex);
				return -1;
			}

			return 0;
		}
	}
}