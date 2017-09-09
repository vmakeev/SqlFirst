namespace SqlFirst.Core.Codegen
{
	/// <summary>
	/// Политика именования переменных
	/// </summary>
	public enum VariableNamingPolicy
	{
		/// <summary>
		/// Camel case (myVariableName)
		/// </summary>
		CamelCase,

		/// <summary>
		/// Camel case, начинающийся с подчеркивания (_myVariableName)
		/// </summary>
		CamelCaseWithUnderscope,

		/// <summary>
		/// Pascal (MyVariableName)
		/// </summary>
		Pascal,

		/// <summary>
		/// Underscope (MY_VARIABLE_NAME)
		/// </summary>
		Underscope
	}
}