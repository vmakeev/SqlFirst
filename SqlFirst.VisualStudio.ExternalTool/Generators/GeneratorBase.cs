using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Common.Logging;
using Newtonsoft.Json;
using SqlFirst.Codegen;
using SqlFirst.Codegen.Helpers;
using SqlFirst.Codegen.Impl;
using SqlFirst.Codegen.Trees;
using SqlFirst.Core;
using SqlFirst.Core.Impl;
using SqlFirst.VisualStudio.ExternalTool.Options;

namespace SqlFirst.VisualStudio.ExternalTool.Generators
{
	internal abstract class GeneratorBase
	{
		private ILog _log;

		protected ILog Log => _log ?? (_log = LogManager.GetLogger(GetType().Name));

		public abstract IQueryParser QueryParser { get; }

		public abstract ICodeGenerator CodeGenerator { get; }

		public abstract IDatabaseTypeMapper TypeMapper { get; }

		public abstract IDatabaseProvider DatabaseProvider { get; }

		public IGeneratedItem GenerateParameterItem(string query, GenerationOptions parameters)
		{
			IQueryInfo info = GetQueryInfo(query, parameters.ConnectionString);

			if (!info.Parameters.Any(paramInfo => paramInfo.IsNumbered))
			{
				Log.Debug("No numbered parameters found, parameter item won't be generated.");
				return null;
			}

			Log.Info("Preparing to generate parameter item");
			ICodeGenerationContext context = GetCodeGenerationContext(info, parameters, query);

			IParameterGenerationOptions itemOptions = new ParameterGenerationOptions(info.SqlFirstOptions);
			Log.Info("Parameter item generating is in progress");
			IGeneratedItem generatedItem = CodeGenerator.GenerateParameterItem(context, itemOptions);
			Log.Info("Parameter item generating is completed");

			return generatedItem;
		}

		public IGeneratedItem GenerateResultItem(string query, GenerationOptions parameters)
		{
			IQueryInfo info = GetQueryInfo(query, parameters.ConnectionString);

			if (info.Results.Count() <= 1)
			{
				Log.Debug("No such output columns found, result item won't be generated.");
				return null;
			}

			Log.Info("Preparing to generate result item");
			ICodeGenerationContext context = GetCodeGenerationContext(info, parameters, query);

			IResultGenerationOptions itemOptions = new ResultGenerationOptions(info.SqlFirstOptions);
			Log.Info("Result item generating is in progress");
			IGeneratedItem generatedItem = CodeGenerator.GenerateResultItem(context, itemOptions);
			Log.Info("Result item generating is completed");

			return generatedItem;
		}

		public string GenerateResultItemCode(string query, GenerationOptions parameters)
		{
			IGeneratedItem generatedItem = GenerateResultItem(query, parameters);

			if (generatedItem == null)
			{
				return null;
			}

			Log.Info("Preparing to compose .cs file with result item");
			string itemCode = CodeGenerator.GenerateFile(new[] { generatedItem });
			Log.Info("File with result item was successfully composed");

			return itemCode;
		}

		public string GenerateParameterItemCode(string query, GenerationOptions parameters)
		{
			IGeneratedItem generatedItem = GenerateParameterItem(query, parameters);

			if (generatedItem == null)
			{
				return null;
			}

			Log.Info("Preparing to compose .cs file with parameter item");
			string itemCode = CodeGenerator.GenerateFile(new[] { generatedItem });
			Log.Info("File with parameter item was successfully composed");

			return itemCode;
		}

		public IGeneratedItem GenerateQueryObject(string query, GenerationOptions parameters)
		{
			IQueryInfo info = GetQueryInfo(query, parameters.ConnectionString);

			Log.Info("Preparing to generate query object");
			ICodeGenerationContext context = GetCodeGenerationContext(info, parameters, query);

			IQueryGenerationOptions queryOptions = new QueryGenerationOptions(info.Type, info.SqlFirstOptions);
			Log.Info("Query object generating is in progress");
			IGeneratedItem generatedItem = CodeGenerator.GenerateQueryObject(context, queryOptions);
			Log.Info("Query object generating is completed");

			return generatedItem;
		}

		public string GenerateQueryObjectCode(string query, GenerationOptions parameters)
		{
			IGeneratedItem generatedItem = GenerateQueryObject(query, parameters);

			if (generatedItem == null)
			{
				return null;
			}

			Log.Info("Preparing to compose .cs file with query object");
			string itemCode = CodeGenerator.GenerateFile(new[] { generatedItem });
			Log.Info("File with query object was successfully composed");

			return itemCode;
		}

		private IQueryInfo GetQueryInfo(string query, string connectionString)
		{
			ISqlFirstOption csOption = QueryParser.GetOptions(query).SingleOrDefault(option => string.Equals(option.Name, "ConnectionString", StringComparison.OrdinalIgnoreCase));
			if (csOption != null)
			{
				connectionString = string.Join(" ", csOption.Parameters).TrimStart('=').Trim();
				Log.Debug($"Connection string option found. [{connectionString}] will be used as primary connection string.");
			}

			IQueryInfo baseIinfo = QueryParser.GetQueryInfo(query, connectionString);

			return new QueryInfoWrapper(baseIinfo);
		}

		private ICodeGenerationContext GetCodeGenerationContext(IQueryInfo info, GenerationOptions parameters, string rawSql)
		{
			string queryName = Path.GetFileNameWithoutExtension(parameters.QueryFile);
			string queryItemName = CSharpCodeHelper.GetValidIdentifierName(queryName, NamingPolicy.Pascal);

			IReadOnlyDictionary<string, object> contextOptions = new Dictionary<string, object>
			{
				["Namespace"] = parameters.Namespace,
				["QueryName"] = queryItemName,
				["QueryResultItemName"] = parameters.ResultItemName,
				["QueryParameterItemName"] = parameters.ParameterItemName,
				["QueryText"] = info.Sections.Single(p => p.Type == QuerySectionType.Body).Content,
				["QueryTextRaw"] = rawSql,
				["ResourcePath"] = $"{parameters.Namespace}.{Path.GetFileName(parameters.QueryFile)}"
			};

			Log.Trace(p => p("CodeGenerationContext options:\n" + JsonConvert.SerializeObject(contextOptions, Formatting.Indented)));

			var context = new CodeGenerationContext(info.Parameters, info.Results, contextOptions, TypeMapper, DatabaseProvider);

			return context;
		}
	}
}