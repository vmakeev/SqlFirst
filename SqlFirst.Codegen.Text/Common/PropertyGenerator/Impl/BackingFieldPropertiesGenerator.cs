using System.Collections.Generic;
using SqlFirst.Codegen.Helpers;
using SqlFirst.Codegen.Text.Snippets;
using SqlFirst.Codegen.Text.Templating;

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
			IRenderableTemplate propertyTemplate = GetPropertyTemplate(_options, false);

			foreach (ICodeMemberInfo memberInfo in properties)
			{
				// default value is here
				IRenderableTemplate backingFieldTemplate = GetBackingFieldTemplate(memberInfo.HasDefaultValue);

				string usingString = memberInfo.Type.Namespace;
				string csTypeString = CSharpCodeHelper.GetTypeBuiltInName(memberInfo.Type);
				string csPropertyNameString = CSharpCodeHelper.GetValidIdentifierName(memberInfo.Name, NamingPolicy.Pascal);
				string csBackingFieldNameString = CSharpCodeHelper.GetValidIdentifierName(memberInfo.Name, NamingPolicy.CamelCaseWithUnderscope);
				string defaultValueString = CSharpCodeHelper.GetValidValue(memberInfo.Type, memberInfo.DefaultValue);

				string backingField = backingFieldTemplate.Render(new
				{
					Type = csTypeString,
					Name = csBackingFieldNameString,
					Value = defaultValueString
				});

				string property = propertyTemplate.Render(new
				{
					Type = csTypeString,
					BackingFieldName = csBackingFieldNameString,
					Name = csPropertyNameString
				});

				var backingFieldPart = new GeneratedPropertyPart(backingField, true);
				var propertyPart = new GeneratedPropertyPart(property, false);
				yield return new GeneratedPropertyInfo(new[] { usingString }, new[] { backingFieldPart, propertyPart });
			}
		}

		/// <inheritdoc />
		protected override IRenderableTemplate GetPropertyTemplate(PropertyGenerationOptions options, bool hasDefaultValue)
		{
			if (options.IsReadOnly)
			{
				return _options.IsVirtual
					? Snippet.Property.BackingField.ReadOnly.ReadOnlyBackingFieldPropertyVirtual
					: Snippet.Property.BackingField.ReadOnly.ReadOnlyBackingFieldProperty;
			}

			return options.IsVirtual
				? Snippet.Property.BackingField.BackingFieldPropertyVirtual
				: Snippet.Property.BackingField.BackingFieldProperty;
		}

		private IRenderableTemplate GetBackingFieldTemplate(bool hasDefaultValue)
		{
			return hasDefaultValue
				? Snippet.Field.BackingFieldWithValue
				: Snippet.Field.BackingField;
		}
	}
}