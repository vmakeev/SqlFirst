using System;
using System.Collections.Generic;
using SqlFirst.Codegen.Text.Snippets;
using SqlFirst.Core;
using SqlFirst.Core.Codegen;
using SqlFirst.Core.Parsing;

namespace SqlFirst.Codegen.Text.PropertyGenerator.Impl
{
	internal class BackingFieldPropertiesGenerator : PropertiesGeneratorBase
	{
		private readonly IDatabaseTypeMapper _typeMapper;
		private readonly bool _isReadOnly;

		public BackingFieldPropertiesGenerator(IDatabaseTypeMapper typeMapper, bool isReadOnly)
		{
			_typeMapper = typeMapper;
			_isReadOnly = isReadOnly;
		}

		public override IEnumerable<GeneratedPropertyInfo> GenerateProperties(IEnumerable<IFieldDetails> results, PropertyType propertyType)
		{
			string propertyTemplate = GetPropertyTemplate(_isReadOnly);

			string backingFieldTemplate = Snippet.BackingField;

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

		protected virtual string GetPropertyTemplate(bool isReadOnly)
		{
			return isReadOnly
				? Snippet.ReadOnlyBackingFieldProperty
				: Snippet.BackingFieldProperty;
		}
	}
}