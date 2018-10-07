using System;

namespace SqlFirst.Codegen.Text.Common.PropertyGenerator
{
	/// <summary>
	/// Информация о члене класса
	/// </summary>
	internal interface ICodeMemberInfo
	{
		/// <summary>
		/// Тип
		/// </summary>
		Type Type { get; }

		/// <summary>
		/// Имя
		/// </summary>
		string Name { get; }

		/// <summary>
		/// Имеет ли член класса значение по умолчанию
		/// </summary>
		bool HasDefaultValue { get; }

		/// <summary>
		/// Значение по умолчанию
		/// </summary>
		object DefaultValue { get; }
	}
}