using System.Collections.Generic;

namespace SqlFirst.Codegen.Trees
{
	/// <summary>
	/// Описание типа сгенерированного элемента
	/// </summary>
	public interface IGeneratedType
	{
		/// <summary>
		/// Имя типа
		/// </summary>
		string Name { get; }

		/// <summary>
		/// Признак интерфейса
		/// </summary>
		bool IsInterface { get; }

		/// <summary>
		/// Признак обобщенного типа
		/// </summary>
		bool IsGeneric { get; }

		/// <summary>
		/// Список обобщенных параметров
		/// </summary>
		IEnumerable<IGenericArgument> GenericArguments { get; }
	}
}