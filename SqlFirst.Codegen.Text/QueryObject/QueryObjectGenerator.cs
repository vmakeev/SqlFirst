using System;
using System.Collections.Generic;
using System.Text;
using SqlFirst.Codegen.Impl;
using SqlFirst.Codegen.Text.QueryObject.Data;
using SqlFirst.Codegen.Text.Snippets;
using SqlFirst.Core;

namespace SqlFirst.Codegen.Text.QueryObject
{
	public class QueryObjectGenerator
	{
		private readonly string _doubleBreak = Environment.NewLine + Environment.NewLine;

		private string EmitCodeBlock(IEnumerable<string> codeItems, bool doubleSpacing = false)
		{
			string codeBlock = string.Join(doubleSpacing ? _doubleBreak : Environment.NewLine, codeItems);
			if (!string.IsNullOrEmpty(codeBlock))
			{
				codeBlock += _doubleBreak;
			}

			return codeBlock.Indent("\t");
		}

		public IGeneratedQueryObject GenerateQueryObject(ICodeGenerationContext context, IQueryGenerationOptions options)
		{
			var result = new GeneratedQueryObject
			{
				ItemName = context.GetQueryName(),
				Namespace = context.GetNamespace(),
				ItemModifiers = new[] { "public", "partial" }
			};

			IQueryObjectData data = GenerateQueryObjectData(context, options);

			string nested = EmitCodeBlock(data.Nested, true);
			string constants = EmitCodeBlock(data.Constants);
			string fields = EmitCodeBlock(data.Fields);
			string properties = EmitCodeBlock(data.Properties, true);
			string methods = EmitCodeBlock(data.Methods, true).TrimEnd();

			var itemBuilder = new StringBuilder(QuerySnippet.QueryObject);
			itemBuilder.Replace("$Modificators$", string.Join(" ", result.ItemModifiers));
			itemBuilder.Replace("$QueryName$", context.GetQueryName());
			itemBuilder.Replace("$Nested$", nested);
			itemBuilder.Replace("$Consts$", constants);
			itemBuilder.Replace("$Fields$", fields);
			itemBuilder.Replace("$Properties$", properties);
			itemBuilder.Replace("$Methods$", methods);

			result.Item = itemBuilder.ToString();
			result.Usings = data.Usings;

			return result;
		}

		private IQueryObjectData GenerateQueryObjectData(ICodeGenerationContext context, IQueryGenerationOptions options)
		{
			QueryObjectTemplate template = GetQueryTemplate(context, options);

			ApplyOptions(template, options);

			return template.GenerateData(context);
		}

		// ReSharper disable UnusedParameter.Local
		private void ApplyOptions(QueryObjectTemplate template, IQueryGenerationOptions options)
		{
			// todo: process user generation options here
		}

		private QueryObjectTemplate GetQueryTemplate(ICodeGenerationContext context, IQueryGenerationOptions options)
		{
			switch (options.QueryType)
			{
				case QueryType.Create:
					throw new NotImplementedException();

				case QueryType.Read:
					//todo: use options
					return SelectTemplateBuilder.Build(context);

				case QueryType.Update:
					throw new NotImplementedException();

				case QueryType.Delete:
					throw new NotImplementedException();

				default:
					throw new CodeGenerationException($"Unsupported QueryType: [{options.QueryType:G}] ({options.QueryType:D})");
			}
		}
	}
}