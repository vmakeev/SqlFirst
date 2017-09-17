using System;
using System.Collections.Generic;
using SqlFirst.Codegen.Text.Snippets;
using SqlFirst.Core;
using SqlFirst.Core.Codegen;
using SqlFirst.Core.Parsing;

namespace SqlFirst.Codegen.Text.PropertyGenerator.Impl
{
	/// <summary>
	/// Генератор свойств c отдельным полем для чтения/записи данных
	/// </summary>
	internal class BackingFieldPropertiesGenerator : PropertiesGeneratorBase
	{
		/// <summary>
		/// Инициализирует новый экземпляр класса <see cref="BackingFieldPropertiesGenerator" />
		/// </summary>
		/// <param name="typeMapper">Преобразователь типов данных БД в типы CLR</param>
		/// <param name="options">Параметры генерации свойств</param>
		public BackingFieldPropertiesGenerator(IDatabaseTypeMapper typeMapper, PropertyGenerationOptions options)
			: base(typeMapper, options)
		{
		}

		/// <inheritdoc />
		public override IEnumerable<GeneratedPropertyInfo> GenerateProperties(IEnumerable<IFieldDetails> results)
		{
			string propertyTemplate = GetPropertyTemplate(_options);

			string backingFieldTemplate = FieldSnippet.BackingField;

			foreach (IFieldDetails fieldDetails in results)
			{
				Type csType = _typeMapper.Map(fieldDetails.DbType, fieldDetails.AllowDbNull);
				if (csType == null)
				{
					throw new CodeGenerationException($"Can not map dbType [{fieldDetails.DbType}] to CLR type");
				}

				string usingString = csType.Namespace;
				string csTypeString = CSharpCodeHelper.GetTypeBuiltInName(csType);
				string csPropertyNameString = CSharpCodeHelper.GetValidVariableName(fieldDetails.ColumnName, VariableNamingPolicy.Pascal);
				string csBackingFieldNameString = CSharpCodeHelper.GetValidVariableName(fieldDetails.ColumnName, VariableNamingPolicy.CamelCaseWithUnderscope);

				string backingField = backingFieldTemplate
					.Replace("$Type$", csTypeString)
					.Replace("$Name$", csBackingFieldNameString);

				string property = propertyTemplate
					.Replace("$Type$", csTypeString)
					.Replace("$BackingFieldName$", csBackingFieldNameString)
					.Replace("$Name$", csPropertyNameString);

				var backingFieldPart = new GeneratedPropertyPart(backingField, true);
				var propertyPart = new GeneratedPropertyPart(property, false);
				yield return new GeneratedPropertyInfo(new[] { usingString }, new[] { backingFieldPart, propertyPart });
			}
		}

		/// <inheritdoc />
		protected override string GetPropertyTemplate(PropertyGenerationOptions options)
		{
			if (options.IsReadOnly)
			{
				return _options.IsVirtual
					? PropertySnippet.ReadOnlyBackingFieldPropertyVirtual
					: PropertySnippet.ReadOnlyBackingFieldProperty;
			}

			return options.IsVirtual
				? PropertySnippet.BackingFieldPropertyVirtual
				: PropertySnippet.BackingFieldProperty;
		}
	}
}