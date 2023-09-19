using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace SqlFirst.Core;

/// <summary>
/// Фабрика логгеров
/// </summary>
public static class LogManager
{
	private static readonly ILoggerFactory _loggerFactory;

	static LogManager()
	{
		IConfiguration configuration = new ConfigurationBuilder()
										.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
										.Build();

		_loggerFactory = LoggerFactory.Create(
			builder =>
			{
				builder
					.AddConfiguration(configuration)
					.AddConsole();
			});
	}

	/// <summary>
	/// Возвращает логгер для указанного типа
	/// </summary>
	/// <param name="type"></param>
	/// <returns></returns>
	public static ILogger GetLogger(Type type)
	{
		return _loggerFactory.CreateLogger(type);
	}

	/// <summary>
	/// Возвращает логгер для указанного типа
	/// </summary>
	public static ILogger GetLogger<T>()
	{
		return GetLogger(typeof(T));
	}
}