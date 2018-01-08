using System.Collections.Generic;

namespace SqlFirst.Core
{
	/// <summary>
	/// Специфичный для конкретного поставщика данных тип параметра
	/// </summary>
	public interface IProviderSpecificType
	{
		/// <summary>
		/// Имя типа
		/// </summary>
		string TypeName { get; }

		/// <summary>
		/// Имя значения
		/// </summary>
		string ValueName { get; }

		/// <summary>
		/// Требуемые Using'и
		/// </summary>
		IEnumerable<string> Usings { get; }
	}
}