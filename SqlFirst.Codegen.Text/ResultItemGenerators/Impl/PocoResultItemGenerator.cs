using System;
using System.Collections.Generic;
using System.Linq;
using SqlFirst.Codegen.Impl;
using SqlFirst.Codegen.Text.PropertyGenerator;
using SqlFirst.Codegen.Text.Snippets;
using SqlFirst.Codegen.Trees;

namespace SqlFirst.Codegen.Text.ResultItemGenerators.Impl
{
	internal class PocoResultItemGenerator : ResultItemGeneratorBase
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Object"></see> class.</summary>
		public PocoResultItemGenerator(PropertiesGeneratorBase propertiesGenerator)
			: base(propertiesGenerator)
		{
		}

		public override IGeneratedResultItem GenerateResultItem(ICodeGenerationContext context, IResultGenerationOptions options)
		{
			return GenerateResultItemInternal(context, options);
		}

		protected virtual IEnumerable<string> GetCommonUsings()
		{
			yield break;
		}

		protected virtual GeneratedResultItem GenerateResultItemInternal(ICodeGenerationContext context, IResultGenerationOptions options)
		{
			string targetNamespace = context.GetNamespace();

			string template = GetTemplate();

			var result = new GeneratedResultItem
			{
				Namespace = targetNamespace,
				ItemModifiers = new[] { Modifiers.Public, Modifiers.Partial },
				ItemName = context.GetQueryName() + "Item",
				BaseTypes = new IGeneratedType[0]
			};

			GeneratedPropertyInfo[] propertiesInfo = _propertiesGenerator.GenerateProperties(context.OutgoingParameters, options.PropertyType).ToArray();

			IEnumerable<string> allUsings = GetCommonUsings().Concat(propertiesInfo.SelectMany(p => p.Usings));
			result.Usings = allUsings.Distinct().OrderBy(@using => @using);

			string space = Environment.NewLine + Environment.NewLine;
			const string indent = "\t";

			IEnumerable<string> backingFields = propertiesInfo
				.SelectMany(info => info.Properties)
				.Where(propertyPart => propertyPart.IsBackingField)
				.Select(propertyPart => propertyPart.Content);
			string backingFieldsText = string.Join(Environment.NewLine, backingFields.Select(field => field.Indent(indent)));

			IEnumerable<string> properties = propertiesInfo
				.SelectMany(info => info.Properties)
				.Where(propertyPart => !propertyPart.IsBackingField)
				.Select(p => p.Content);
			string propertiesText = string.Join(space, properties.Select(property => property.Indent(indent)));

			string fullPropertiesText = string.IsNullOrEmpty(backingFieldsText)
				? propertiesText
				: backingFieldsText + space + propertiesText;

			string itemText = template
				.Replace("$ItemName$", result.ItemName)
				.Replace("$Modificators$", string.Join(" ", result.ItemModifiers))
				.Replace("$Properties$", fullPropertiesText);

			result.Item = itemText;

			return result;
		}

		protected virtual string GetTemplate() => Snippet.PocoResultItem;
	}
}