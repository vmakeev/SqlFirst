namespace SqlFirst.Codegen.Impl
{
	/// <inheritdoc />
	public class ResultGenerationOptions : IResultGenerationOptions
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Object"></see> class.</summary>
		public ResultGenerationOptions(ResultItemType itemType, ResultItemAbilities itemAbilities, PropertyType propertyType, PropertyModifiers propertyModifiers)
		{
			ItemType = itemType;
			ItemAbilities = itemAbilities;
			PropertyType = propertyType;
			PropertyModifiers = propertyModifiers;
		}

		/// <inheritdoc />
		public ResultItemType ItemType { get; }

		/// <inheritdoc />
		public ResultItemAbilities ItemAbilities { get; }

		/// <inheritdoc />
		public PropertyType PropertyType { get; }

		/// <inheritdoc />
		public PropertyModifiers PropertyModifiers { get; }
	}
}