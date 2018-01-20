using System.Collections.Generic;

namespace SqlFirst.Core
{
	/// <summary>
	/// Генератор кода запроса
	/// </summary>
	public interface ISqlEmitter
	{
		/// <summary>
		/// Позволяет построить запрос из секций
		/// </summary>
		/// <param name="sections">Секции запроса</param>
		/// <returns>Текст запроса</returns>
		string EmitQuery(IEnumerable<IQuerySection> sections);
		
		/// <summary>
		/// Позволяет построить текст секции параметров запроса
		/// </summary>
		/// <param name="options">Набор параметров</param>
		/// <returns>Текст секции параметров запроса</returns>
		string EmitOptions(IEnumerable<ISqlFirstOption> options);

		/// <summary>
		/// Позволяет построить текст секции запроса
		/// </summary>
		/// <param name="section">Информация о секцмм</param>
		/// <returns>Текст секции</returns>
		string EmitSection(IQuerySection section);
		
		/// <summary>
		/// Возвращает текст SQL для объявления указанных параметров
		/// </summary>
		/// <param name="infos">Информация о параметрах</param>
		/// <returns>Текст SQL для объявления указанных параметров</returns>
		string EmitDeclarations(IEnumerable<IQueryParamInfo> infos);

		/// <summary>
		/// Поддерживается ли генерация SQL параметров запроса
		/// </summary>
		bool CanEmitDeclarations { get; }
	}
}