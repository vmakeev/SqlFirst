using System.Collections.Generic;
using System.Linq;
using SqlFirst.Codegen;
using SqlFirst.Codegen.Helpers;
using SqlFirst.Codegen.Impl;
using SqlFirst.Codegen.Text;
using SqlFirst.Codegen.Trees;
using SqlFirst.Core;
using SqlFirst.Core.Impl;

namespace SqlFirst.Demo.Wpf.Logic
{
	public abstract class GeneratorBase
	{
		public abstract SamplesBase Samples { get; }

		public abstract IQueryParser QueryParser { get; }

		public abstract ICodeGenerator CodeGenerator { get; }

		public abstract IDatabaseTypeMapper TypeMapper { get; }

		public abstract IDatabaseProvider DatabaseProvider { get; }

		public IGeneratedItem GenerateParameterItem(string query, GeneratorParameters parameters)
		{
			IQueryInfo info = GetQueryInfo(query, parameters.ConnectionString);

			if (!info.Parameters.Any(paramInfo => paramInfo.IsNumbered))
			{
				return null;
			}

			ICodeGenerationContext context = GetCodeGenerationContext(info, parameters);

			IParameterGenerationOptions itemOptions = new ParameterGenerationOptions(info.SqlFirstOptions);
			IGeneratedItem generatedItem = CodeGenerator.GenerateParameterItem(context, itemOptions);

			return generatedItem;
		}

		public IGeneratedItem GenerateResultItem(string query, GeneratorParameters parameters)
		{
			IQueryInfo info = GetQueryInfo(query, parameters.ConnectionString);

			if (info.Results.Count() <= 1)
			{
				return null;
			}

			ICodeGenerationContext context = GetCodeGenerationContext(info, parameters);

			IResultGenerationOptions itemOptions = new ResultGenerationOptions(info.SqlFirstOptions);
			IGeneratedItem generatedItem = CodeGenerator.GenerateResultItem(context, itemOptions);

			return generatedItem;
		}

		public string GenerateResultItemCode(string query, GeneratorParameters parameters)
		{
			ICodeGenerator codeGenerator = new TextCodeGenerator();
			IGeneratedItem generatedItem = GenerateResultItem(query, parameters);

			if (generatedItem == null)
			{
				return null;
			}

			string itemCode = codeGenerator.GenerateFile(new[] { generatedItem });

			return itemCode;
		}

		public string GenerateParameterItemCode(string query, GeneratorParameters parameters)
		{
			ICodeGenerator codeGenerator = new TextCodeGenerator();
			IGeneratedItem generatedItem = GenerateParameterItem(query, parameters);

			if (generatedItem == null)
			{
				return null;
			}

			string itemCode = codeGenerator.GenerateFile(new[] { generatedItem });

			return itemCode;
		}

		public IGeneratedItem GenerateQueryObject(string query, GeneratorParameters parameters)
		{
			IQueryInfo info = GetQueryInfo(query, parameters.ConnectionString);

			ICodeGenerationContext context = GetCodeGenerationContext(info, parameters);

			IQueryGenerationOptions queryOptions = new QueryGenerationOptions(info.Type, info.SqlFirstOptions);
			IGeneratedItem generatedItem = CodeGenerator.GenerateQueryObject(context, queryOptions);

			return generatedItem;
		}

		public string GenerateQueryObjectCode(string query, GeneratorParameters parameters)
		{
			IGeneratedItem generatedItem = GenerateQueryObject(query, parameters);

			if (generatedItem == null)
			{
				return null;
			}

			string itemCode = CodeGenerator.GenerateFile(new[] { generatedItem });

			return itemCode;
		}

		private IQueryInfo GetQueryInfo(string query, string connectionString)
		{
			IQueryInfo info = QueryParser.GetQueryInfo(query, connectionString);
			return info;
		}

		private ICodeGenerationContext GetCodeGenerationContext(IQueryInfo info, GeneratorParameters parameters)
		{
			IReadOnlyDictionary<string, object> contextOptions = new Dictionary<string, object>
			{
				["Namespace"] = parameters.Namespace,
				["QueryName"] = CSharpCodeHelper.GetValidIdentifierName(parameters.QueryName, NamingPolicy.Pascal),
				["QueryResultItemName"] = CSharpCodeHelper.GetValidIdentifierName(parameters.QueryName, NamingPolicy.Pascal) + "Result",
				["QueryParameterItemName"] = CSharpCodeHelper.GetValidIdentifierName(parameters.QueryName, NamingPolicy.Pascal) + "Parameter",
				["QueryText"] = info.Sections.Single(p => p.Type == QuerySectionType.Body).Content
			};
			var context = new CodeGenerationContext(info.Parameters, info.Results, contextOptions, TypeMapper, DatabaseProvider);

			return context;
		}
	}
}