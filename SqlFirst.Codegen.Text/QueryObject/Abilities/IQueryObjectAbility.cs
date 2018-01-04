using System.Collections.Generic;
using SqlFirst.Codegen.Text.QueryObject.Data;

namespace SqlFirst.Codegen.Text.QueryObject.Abilities
{
	internal interface IQueryObjectAbility
	{
		/// <summary>
		/// Имя умения
		/// </summary>
		string Name { get; }

		/// <summary>
		/// Добавляет умение к данным запроса
		/// </summary>
		/// <param name="context">Контекст кодогенерации</param>
		/// <param name="data">Данные объекта запроса</param>
		/// <returns>Обновленные данные объекта запроса</returns>
		IQueryObjectData Apply(ICodeGenerationContext context, IQueryObjectData data);

		/// <summary>
		/// Возвращает имена умений, от которых зависит текущее
		/// </summary>
		/// <returns>Имена умений, от которых зависит текущее</returns>
		IEnumerable<string> GetDependencies();
	}
}