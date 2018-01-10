using SqlFirst.Codegen.Text.Common.PropertyGenerator;

namespace SqlFirst.Codegen.Text.ParameterItem.Impl
{
	/// <summary>
	/// Генератор аргумента в виде обычного класса
	/// </summary>
	internal class PocoParameterItemGenerator : ParameterItemGeneratorBase
	{
		/// <summary>
		/// Инициализирует новый экземпляр класса <see cref="ParameterItemGeneratorBase" />
		/// </summary>
		/// <param name="propertiesGenerator">Генератор свойств</param>
		public PocoParameterItemGenerator(PropertiesGeneratorBase propertiesGenerator)
			: base(propertiesGenerator)
		{
		}

		protected override string ObjectType { get; } = ObjectTypes.Class;
	}
}