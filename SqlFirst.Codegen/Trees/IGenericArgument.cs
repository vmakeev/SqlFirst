using System.Collections.Generic;

namespace SqlFirst.Codegen.Trees
{
	/// <summary>
	/// Описание обобщенного параметра
	/// </summary>
	public interface IGenericArgument
	{
		/// <summary>
		/// Имя конкретного типа обобщенного параметра
		/// </summary>
		string Type { get; }

		/// <summary>
		/// Признак аргумента обобщенного типа
		/// </summary>
		bool IsGeneric { get; }

		/// <summary>
		/// Список обобщенных параметров
		/// </summary>
		IEnumerable<IGenericArgument> GenericArguments { get; }
	}
}