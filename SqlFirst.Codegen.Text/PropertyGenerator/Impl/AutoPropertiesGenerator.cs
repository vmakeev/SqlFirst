using System;
using System.Collections.Generic;
using SqlFirst.Codegen.Text.Snippets;
using SqlFirst.Core;
using SqlFirst.Core.Codegen;
using SqlFirst.Core.Parsing;

namespace SqlFirst.Codegen.Text.PropertyGenerator.Impl
{
	internal class AutoPropertiesGenerator : PropertiesGeneratorBase
	{
		private readonly IDatabaseTypeMapper _typeMapper;
		private readonly bool _isReadOnly;

		public AutoPropertiesGenerator(IDatabaseTypeMapper typeMapper, bool isReadOnly)
		{
			_typeMapper = typeMapper;
			_isReadOnly = isReadOnly;
		}

		public override IEnumerable<GeneratedPropertyInfo> GenerateProperties(IEnumerable<IFieldDetails> results, PropertyType propertyType)
		{
			string propertyTemplate = _isReadOnly
				? Snippet.ReadOnlyAutoProperty
				: Snippet.AutoProperty;

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

				string property = propertyTemplate
					.Replace("$Type$", csTypeString)
					.Replace("$Name$", csPropertyNameString);

				var propertyPart = new GeneratedPropertyPart(property, false);
				yield return new GeneratedPropertyInfo(new[] { usingString }, new[] { propertyPart });
			}
		}
	}
}