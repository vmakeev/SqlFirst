using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using SqlFirst.Codegen.Impl;
using SqlFirst.Codegen.Text.QueryObject.Data;
using SqlFirst.Codegen.Text.QueryObject.Factories;
using SqlFirst.Codegen.Text.QueryObject.Factories.Options;
using SqlFirst.Codegen.Text.Snippets;
using SqlFirst.Core;

namespace SqlFirst.Codegen.Text.QueryObject
{
	public class QueryObjectGenerator
	{
		public IGeneratedQueryObject GenerateQueryObject(ICodeGenerationContext context, IQueryGenerationOptions options)
		{
			var result = new GeneratedQueryObject
			{
				Name = context.GetQueryName(),
				Namespace = context.GetNamespace(),
				Modifiers = new[] { "public", "partial" }
			};

			IQueryObjectData data = GenerateQueryObjectData(context, options);

			string item = Snippet.Query.QueryObject.Render(new
			{
				Nested = data.Nested,
				Consts = data.Constants,
				Fields = data.Fields,
				Properties = data.Properties,
				Methods = data.Methods
			});

			result.Content = item;
			result.Usings = data.Usings.Distinct().OrderBy(@using => @using);
			result.ObjectType = ObjectTypes.Class;
			result.BeforeItemData = new[] { @"// ReSharper disable once PartialTypeWithSinglePart" };

			return result;
		}

		private static IQueryObjectData GenerateQueryObjectData(ICodeGenerationContext context, IQueryGenerationOptions options)
		{
			QueryObjectTemplate template = GetQueryTemplate(context, options);
			return template.GenerateData(context);
		}

		[SuppressMessage("ReSharper", "SwitchStatementMissingSomeCases")]
		private static QueryObjectTemplate GetQueryTemplate(ICodeGenerationContext context, IQueryGenerationOptions options)
		{
			switch (options.QueryType)
			{
				case QueryType.Create:
					var insertDefaults = GetOptionDefaults(options.OptionDefaults, "insert");
					var insertOptions = new InsertQueryObjectOptions(options.SqlFirstOptions, insertDefaults);
					return InsertTemplateFactory.Build(context, insertOptions);

				case QueryType.Read:
					var selectDefaults = GetOptionDefaults(options.OptionDefaults, "select");
					var selectOptions = new SelectQueryObjectOptions(options.SqlFirstOptions, selectDefaults);
					return SelectTemplateFactory.Build(context, selectOptions);

				case QueryType.Update:
					var updateDefaults = GetOptionDefaults(options.OptionDefaults, "update");
					var updateOptions = new UpdateQueryObjectOptions(options.SqlFirstOptions, updateDefaults);
					return UpdateTemplateFactory.Build(context, updateOptions);

				case QueryType.Delete:
					var deleteDefaults = GetOptionDefaults(options.OptionDefaults, "delete");
					var deleteOptions = new DeleteQueryObjectOptions(options.SqlFirstOptions, deleteDefaults);
					return DeleteTemplateFactory.Build(context, deleteOptions);

				case QueryType.Merge:
					throw new NotImplementedException("MERGE queries are not currently supported.");

				default:
					throw new CodeGenerationException($"Unsupported QueryType: [{options.QueryType:G}] ({options.QueryType:D})");
			}
		}

		private static IReadOnlyDictionary<string, bool> GetOptionDefaults(IOptionDefaults optionDefaults, string section)
		{
			if (optionDefaults == null || !optionDefaults.TryGetValue(section, out IReadOnlyDictionary<string, bool> sectionData))
			{
				return new Dictionary<string, bool>();
			}

			return sectionData;
		}
	}
}