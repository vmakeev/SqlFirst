using System.Collections.Generic;
using SqlFirst.Codegen.Helpers;
using SqlFirst.Codegen.Text.Snippets;
using SqlFirst.Codegen.Text.Templating;

namespace SqlFirst.Codegen.Text.Common.PropertyGenerator.Impl
{
	/// <summary>
	/// Генератор автосвойств
	/// </summary>
	internal class AutoPropertiesGenerator : PropertiesGeneratorBase
	{
		/// <summary>
		/// Инициализирует новый экземпляр класса <see cref="AutoPropertiesGenerator" />
		/// </summary>
		/// <param name="options">Параметры генерации свойств</param>
		public AutoPropertiesGenerator(PropertyGenerationOptions options)
			: base(options)
		{
		}

		/// <inheritdoc />
		public override IEnumerable<GeneratedPropertyInfo> GenerateProperties(IEnumerable<ICodeMemberInfo> properties)
		{
			foreach (ICodeMemberInfo memberInfo in properties)
			{
				IRenderableTemplate propertyTemplate = GetPropertyTemplate(_options, memberInfo.HasDefaultValue);

				string usingString = memberInfo.Type.Namespace;
				string csTypeString = CSharpCodeHelper.GetTypeBuiltInName(memberInfo.Type);
				string csPropertyNameString = CSharpCodeHelper.GetValidIdentifierName(memberInfo.Name, NamingPolicy.Pascal);
				string defaultValueString = CSharpCodeHelper.GetValidValue(memberInfo.Type, memberInfo.DefaultValue);

				string property = propertyTemplate.Render(new
				{
					Type = csTypeString,
					Name = csPropertyNameString,
					Value = defaultValueString
				});

				var propertyPart = new GeneratedPropertyPart(property, false);
				yield return new GeneratedPropertyInfo(new[] { usingString }, new[] { propertyPart });
			}
		}

		/// <inheritdoc />
		protected override IRenderableTemplate GetPropertyTemplate(PropertyGenerationOptions options, bool hasDefaultValue)
		{
			if (hasDefaultValue)
			{
				if (_options.IsReadOnly)
				{
					return _options.IsVirtual
						? Snippet.Property.Auto.ReadOnly.ReadOnlyAutoPropertyVirtualWithDefault
						: Snippet.Property.Auto.ReadOnly.ReadOnlyAutoPropertyWithDefault;
				}

				return _options.IsVirtual
					? Snippet.Property.Auto.AutoPropertyVirtualWithDefault
					: Snippet.Property.Auto.AutoPropertyWithDefault;
			}
			else
			{
				if (_options.IsReadOnly)
				{
					return _options.IsVirtual
						? Snippet.Property.Auto.ReadOnly.ReadOnlyAutoPropertyVirtual
						: Snippet.Property.Auto.ReadOnly.ReadOnlyAutoProperty;
				}

				return _options.IsVirtual
					? Snippet.Property.Auto.AutoPropertyVirtual
					: Snippet.Property.Auto.AutoProperty;
			}
		}
	}
}