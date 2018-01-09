using System;
using System.Diagnostics.CodeAnalysis;
using SqlFirst.Codegen.Impl;
using SqlFirst.Codegen.Text.QueryObject.Data;
using SqlFirst.Codegen.Text.QueryObject.Options;
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
				ItemName = context.GetQueryName(),
				Namespace = context.GetNamespace(),
				ItemModifiers = new[] { "public", "partial" }
			};

			IQueryObjectData data = GenerateQueryObjectData(context, options);

			string item = Snippet.Query.QueryObject.Render(new
			{
				Modificators = result.ItemModifiers,
				QueryName = context.GetQueryName(),
				Nested = data.Nested,
				Consts = data.Constants,
				Fields = data.Fields,
				Properties = data.Properties,
				Methods = data.Methods
			});

			result.Item = item;
			result.Usings = data.Usings;

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
					var insertOptions = new InsertQueryObjectOptions(options.SqlFirstOptions);
					return InsertTemplateFactory.Build(context, insertOptions);

				case QueryType.Read:
					var selectOptions = new SelectQueryObjectOptions(options.SqlFirstOptions);
					return SelectTemplateFactory.Build(context, selectOptions);

				case QueryType.Update:
					throw new NotImplementedException();

				case QueryType.Delete:
					throw new NotImplementedException();

				case QueryType.Merge:
					throw new NotImplementedException();

				default:
					throw new CodeGenerationException($"Unsupported QueryType: [{options.QueryType:G}] ({options.QueryType:D})");
			}
		}
	}
}