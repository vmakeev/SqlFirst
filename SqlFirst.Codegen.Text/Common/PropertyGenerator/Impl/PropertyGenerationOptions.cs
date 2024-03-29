﻿namespace SqlFirst.Codegen.Text.Common.PropertyGenerator.Impl
{
	/// <summary>
	/// Параметры генерации свойств
	/// </summary>
	internal class PropertyGenerationOptions
	{
		/// <summary>
		/// Генерировать свойства, доступные только на чтение
		/// </summary>
		public bool IsReadOnly { get; }

		/// <summary>
		/// Генерировать виртуальные свойства
		/// </summary>
		public bool IsVirtual { get; }

		/// <summary>
		/// Инициализирует новый экземпляр класса <see cref="PropertyGenerationOptions" />
		/// </summary>
		/// <param name="isReadOnly">Генерировать свойства, доступные только на чтение</param>
		/// <param name="isVirtual">Генерировать виртуальные свойства</param>
		public PropertyGenerationOptions(bool isReadOnly, bool isVirtual)
		{
			IsReadOnly = isReadOnly;
			IsVirtual = isVirtual;
		}
	}
}