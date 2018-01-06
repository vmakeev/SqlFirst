using System.Collections.Generic;
using SqlFirst.Codegen.Helpers;
using SqlFirst.Codegen.Text.Snippets;
using SqlFirst.Codegen.Text.Snippets.Properties;

namespace SqlFirst.Codegen.Text.Common.PropertyGenerator.Impl
{
	/// <summary>
	/// Генератор свойств c отдельным полем для чтения/записи данных
	/// </summary>
	internal class BackingFieldPropertiesGenerator : PropertiesGeneratorBase
	{
		/// <summary>
		/// Инициализирует новый экземпляр класса <see cref="BackingFieldPropertiesGenerator" />
		/// </summary>
		/// <param name="options">Параметры генерации свойств</param>
		public BackingFieldPropertiesGenerator(PropertyGenerationOptions options)
			: base(options)
		{
		}

		/// <inheritdoc />
		public override IEnumerable<GeneratedPropertyInfo> GenerateProperties(IEnumerable<ICodeMemberInfo> properties)
		{
			// ignore default value
			string propertyTemplate = GetPropertyTemplate(_options, false);

			foreach (ICodeMemberInfo memberInfo in properties)
			{
				// default value is here
				string backingFieldTemplate = GetBackingFieldTemplate(memberInfo.HasDefaultValue);

				string usingString = memberInfo.Type.Namespace;
				string csTypeString = CSharpCodeHelper.GetTypeBuiltInName(memberInfo.Type);
				string csPropertyNameString = CSharpCodeHelper.GetValidIdentifierName(memberInfo.Name, NamingPolicy.Pascal);
				string csBackingFieldNameString = CSharpCodeHelper.GetValidIdentifierName(memberInfo.Name, NamingPolicy.CamelCaseWithUnderscope);
				string defaultValueString = CSharpCodeHelper.GetValidValue(memberInfo.Type, memberInfo.DefaultValue);

				string backingField = backingFieldTemplate
					.Replace("$Type$", csTypeString)
					.Replace("$Name$", csBackingFieldNameString)
					.Replace("$Value$", defaultValueString);

				string property = propertyTemplate
					.Replace("$Type$", csTypeString)
					.Replace("$BackingFieldName$", csBackingFieldNameString)
					.Replace("$Name$", csPropertyNameString);

				var backingFieldPart = new GeneratedPropertyPart(backingField, true);
				var propertyPart = new GeneratedPropertyPart(property, false);
				yield return new GeneratedPropertyInfo(new[] { usingString }, new[] { backingFieldPart, propertyPart });
			}
		}

		private string GetBackingFieldTemplate(bool hasDefaultValue)
		{
			return hasDefaultValue
				? FieldSnippet.BackingFieldWithValue
				: FieldSnippet.BackingField;
		}

		/// <inheritdoc />
		protected override string GetPropertyTemplate(PropertyGenerationOptions options, bool hasDefaultValue)
		{
			if (options.IsReadOnly)
			{
				return _options.IsVirtual
					? PropertySnippet.BackingField.ReadOnly.ReadOnlyBackingFieldPropertyVirtual
					: PropertySnippet.BackingField.ReadOnly.ReadOnlyBackingFieldProperty;
			}

			return options.IsVirtual
				? PropertySnippet.BackingField.BackingFieldPropertyVirtual
				: PropertySnippet.BackingField.BackingFieldProperty;
		}
	}
}