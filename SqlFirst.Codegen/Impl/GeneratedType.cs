using System.Collections.Generic;
using SqlFirst.Codegen.Trees;

namespace SqlFirst.Codegen.Impl
{
	public class GeneratedType : IGeneratedType
	{
		/// <summary>
		/// Имя типа
		/// </summary>
		public string TypeName { get; set; }

		/// <summary>
		/// Признак интерфейса
		/// </summary>
		public bool IsInterface { get; set; }

		/// <summary>
		/// Признак обобщенного типа
		/// </summary>
		public bool IsGeneric { get; set; }

		/// <summary>
		/// Список обобщенных параметров
		/// </summary>
		public IEnumerable<IGenericArgument> GenericArguments { get; set; } = new IGenericArgument[0];

		/// <summary>
		/// Условия, накладываемые на Generic параметры
		/// </summary>
		public IEnumerable<string> GenericConditions { get; set; } = new string[0];
	}
}