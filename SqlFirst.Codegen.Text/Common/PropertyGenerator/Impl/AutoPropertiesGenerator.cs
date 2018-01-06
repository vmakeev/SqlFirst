using System.Collections.Generic;
using SqlFirst.Codegen.Helpers;
using SqlFirst.Codegen.Text.Snippets.Properties;

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
				string propertyTemplate = GetPropertyTemplate(_options, memberInfo.HasDefaultValue);

				string usingString = memberInfo.Type.Namespace;
				string csTypeString = CSharpCodeHelper.GetTypeBuiltInName(memberInfo.Type);
				string csPropertyNameString = CSharpCodeHelper.GetValidIdentifierName(memberInfo.Name, NamingPolicy.Pascal);
				string defaultValueString = CSharpCodeHelper.GetValidValue(memberInfo.Type, memberInfo.DefaultValue);

				string property = propertyTemplate
					.Replace("$Type$", csTypeString)
					.Replace("$Name$", csPropertyNameString)
					.Replace("$Value$", defaultValueString);

				var propertyPart = new GeneratedPropertyPart(property, false);
				yield return new GeneratedPropertyInfo(new[] { usingString }, new[] { propertyPart });
			}
		}

		/// <inheritdoc />
		protected override string GetPropertyTemplate(PropertyGenerationOptions options, bool hasDefaultValue)
		{
			if (hasDefaultValue)
			{
				if (_options.IsReadOnly)
				{
					return _options.IsVirtual
						? PropertySnippet.Auto.ReadOnly.ReadOnlyAutoPropertyVirtualWithDefault
						: PropertySnippet.Auto.ReadOnly.ReadOnlyAutoPropertyWithDefault;
				}

				return _options.IsVirtual
					? PropertySnippet.Auto.AutoPropertyVirtualWithDefault
					: PropertySnippet.Auto.AutoPropertyWithDefault;
			}
			else
			{
				if (_options.IsReadOnly)
				{
					return _options.IsVirtual
						? PropertySnippet.Auto.ReadOnly.ReadOnlyAutoPropertyVirtual
						: PropertySnippet.Auto.ReadOnly.ReadOnlyAutoProperty;
				}

				return _options.IsVirtual
					? PropertySnippet.Auto.AutoPropertyVirtual
					: PropertySnippet.Auto.AutoProperty;
			}
		}
	}
}