using System;
using System.Collections.Generic;
using SqlFirst.Codegen.Helpers;
using SqlFirst.Codegen.Text.Snippets;
using SqlFirst.Core;

namespace SqlFirst.Codegen.Text.PropertyGenerator.Impl
{
	/// <summary>
	/// Генератор автосвойств
	/// </summary>
	internal class AutoPropertiesGenerator : PropertiesGeneratorBase
	{
		/// <summary>
		/// Инициализирует новый экземпляр класса <see cref="AutoPropertiesGenerator" />
		/// </summary>
		/// <param name="typeMapper">Преобразователь типов данных БД в типы CLR</param>
		/// <param name="options">Параметры генерации свойств</param>
		public AutoPropertiesGenerator(IDatabaseTypeMapper typeMapper, PropertyGenerationOptions options)
			: base(typeMapper, options)
		{
		}

		/// <inheritdoc />
		public override IEnumerable<GeneratedPropertyInfo> GenerateProperties(IEnumerable<IFieldDetails> results)
		{
			string propertyTemplate = GetPropertyTemplate(_options);

			foreach (IFieldDetails fieldDetails in results)
			{
				Type csType = _typeMapper.Map(fieldDetails.DbType, fieldDetails.AllowDbNull);
				if (csType == null)
				{
					throw new CodeGenerationException($"Can not map dbType [{fieldDetails.DbType}] to CLR type");
				}

				string usingString = csType.Namespace;
				string csTypeString = CSharpCodeHelper.GetTypeBuiltInName(csType);
				string csPropertyNameString = CSharpCodeHelper.GetValidIdentifierName(fieldDetails.ColumnName, NamingPolicy.Pascal);

				string property = propertyTemplate
					.Replace("$Type$", csTypeString)
					.Replace("$Name$", csPropertyNameString);

				var propertyPart = new GeneratedPropertyPart(property, false);
				yield return new GeneratedPropertyInfo(new[] { usingString }, new[] { propertyPart });
			}
		}

		/// <inheritdoc />
		protected override string GetPropertyTemplate(PropertyGenerationOptions options)
		{
			if (_options.IsReadOnly)
			{
				return _options.IsVirtual
					? PropertySnippet.ReadOnlyAutoPropertyVirtual
					: PropertySnippet.ReadOnlyAutoProperty;
			}

			return _options.IsVirtual
				? PropertySnippet.AutoPropertyVirtual
				: PropertySnippet.AutoProperty;
		}
	}
}