using System.Collections.Generic;
using SqlFirst.Codegen.Trees;

namespace SqlFirst.Codegen.Impl
{
	public class GenericArgument : IGenericArgument
	{
		/// <summary>
		/// Имя шаблона обобщенного параметра
		/// </summary>
		public string GenericTemplateName { get; set; }

		/// <summary>
		/// Признак аргумента обобщенного типа
		/// </summary>
		public bool IsGeneric { get; set; }

		/// <summary>
		/// Список обобщенных параметров
		/// </summary>
		public IEnumerable<IGenericArgument> GenericArguments { get; set; } = new IGenericArgument[0];
	}
}
