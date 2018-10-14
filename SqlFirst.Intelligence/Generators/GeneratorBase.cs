using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Common.Logging;
using SqlFirst.Codegen;
using SqlFirst.Codegen.Helpers;
using SqlFirst.Codegen.Impl;
using SqlFirst.Codegen.Trees;
using SqlFirst.Core;
using SqlFirst.Core.Impl;
using SqlFirst.Intelligence.Options;

namespace SqlFirst.Intelligence.Generators
{
	public abstract class GeneratorBase
	{
		private ILog _log;

		protected ILog Log => _log ?? (_log = LogManager.GetLogger(GetType()));

		public abstract ISqlEmitter SqlEmitter { get; }

		public abstract IQueryParser QueryParser { get; }

		public abstract ICodeGenerator CodeGenerator { get; }

		public abstract IDatabaseTypeMapper TypeMapper { get; }

		public abstract IDatabaseProvider DatabaseProvider { get; }

		public IEnumerable<IGeneratedParameterItem> GenerateParameterItems(string query, GenerationOptions parameters)
		{
			IQueryInfo info = GetQueryInfo(query, parameters.ConnectionString);

			return GenerateParameterItemsInternal(query: query, parameters: parameters, info: info);
		}

		public IGeneratedItem GenerateResultItem(string query, GenerationOptions parameters)
		{
			IQueryInfo info = GetQueryInfo(query, parameters.ConnectionString);

			return GenerateResultItemInternal(query: query, parameters: parameters, info: info);
		}

		public string GenerateResultItemCode(string query, GenerationOptions parameters)
		{
			IGeneratedItem generatedItem = GenerateResultItem(query, parameters);

			return GenerateResultItemCodeInternal(generatedItem: generatedItem);
		}

		public IEnumerable<(string text, string name)> GenerateParameterItemsCode(string query, GenerationOptions parameters)
		{
			IEnumerable<IGeneratedParameterItem> generatedItems = GenerateParameterItems(query, parameters);

			return generatedItems.Select(p => (GenerateParameterItemCodeInternal(p), p.Name));
		}

		public IGeneratedItem GenerateQueryObject(string query, GenerationOptions parameters)
		{
			IQueryInfo info = GetQueryInfo(query, parameters.ConnectionString);
			return GenerateQueryObjectInternal(query, parameters, info);
		}

		public string GenerateQueryObjectCode(string query, GenerationOptions parameters)
		{
			IGeneratedItem generatedItem = GenerateQueryObject(query, parameters);

			return GenerateQueryObjectCodeInternal(generatedItem: generatedItem);
		}

		public (string queryObject, string resultItem, (string data, string name)[] parameterItems) GenerateAll(string query, GenerationOptions parameters)
		{
			IQueryInfo info = GetQueryInfo(query, parameters.ConnectionString);

			IGeneratedItem queryObjectItem = GenerateQueryObjectInternal(query, parameters, info);
			IGeneratedItem resultObjectItem = GenerateResultItemInternal(query, parameters, info);
			IEnumerable<IGeneratedParameterItem> parameterObjectItems = GenerateParameterItemsInternal(query, parameters, info);

			string queryObject = GenerateQueryObjectCodeInternal(queryObjectItem);
			string resultObject = GenerateResultItemCodeInternal(resultObjectItem);
			(string data, string name)[] parameterObjects = parameterObjectItems.Select(item => (GenerateParameterItemCodeInternal(item), item.Name)).ToArray();

			return (queryObject, resultObject, parameterObjects);
		}

		private IEnumerable<IGeneratedParameterItem> GenerateParameterItemsInternal(string query, GenerationOptions parameters, IQueryInfo info)
		{
			if (!info.Parameters.Any(paramInfo => paramInfo.IsNumbered || paramInfo.IsComplexType))
			{
				Log.Debug("No numbered or complex parameters found, parameter items won't be generated.");
				return null;
			}

			Log.Info("Preparing to generate parameter item");
			ICodeGenerationContext context = GetCodeGenerationContext(info, parameters, query);

			IParameterGenerationOptions itemOptions = new ParameterGenerationOptions(info.SqlFirstOptions);
			Log.Info("Parameter items generating is in progress");
			IEnumerable<IGeneratedParameterItem> generatedItems = CodeGenerator.GenerateParameterItems(context, itemOptions);
			Log.Info("Parameter items generating is completed");

			return generatedItems;
		}

		private IGeneratedItem GenerateResultItemInternal(string query, GenerationOptions parameters, IQueryInfo info)
		{
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

		private string GenerateResultItemCodeInternal(IGeneratedItem generatedItem)
		{
			if (generatedItem == null)
			{
				return null;
			}

			Log.Info("Preparing to compose .cs file with result item");
			string itemCode = CodeGenerator.GenerateFile(new[] { generatedItem });
			Log.Info("File with result item was successfully composed");

			return itemCode;
		}

		private string GenerateParameterItemCodeInternal(IGeneratedItem generatedItem)
		{
			if (generatedItem == null)
			{
				return null;
			}

			Log.Info("Preparing to compose .cs file with parameter item");
			string itemCode = CodeGenerator.GenerateFile(new[] { generatedItem });
			Log.Info("File with parameter item was successfully composed");

			return itemCode;
		}

		private IGeneratedItem GenerateQueryObjectInternal(string query, GenerationOptions parameters, IQueryInfo info)
		{
			Log.Info("Preparing to generate query object");
			ICodeGenerationContext context = GetCodeGenerationContext(info, parameters, query);

			IQueryGenerationOptions queryOptions = new QueryGenerationOptions(info.Type, info.SqlFirstOptions);
			Log.Info("Query object generating is in progress");
			IGeneratedItem generatedItem = CodeGenerator.GenerateQueryObject(context, queryOptions);
			Log.Info("Query object generating is completed");

			return generatedItem;
		}

		private string GenerateQueryObjectCodeInternal(IGeneratedItem generatedItem)
		{
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

			IQueryInfo baseInfo = QueryParser.GetQueryInfo(query, connectionString);

			return QueryInfoWrapper.Create(baseInfo);
		}

		private ICodeGenerationContext GetCodeGenerationContext(IQueryInfo info, GenerationOptions parameters, string rawSql)
		{
			string queryName = Path.GetFileNameWithoutExtension(parameters.Target);
			string resultName = parameters.ResultItemName;
			string parameterName = parameters.ParameterItemName;

			if (!CSharpCodeHelper.IsValidTypeName(name: queryName, allowBuiltInTypes: false))
			{
				string oldName = queryName;
				queryName = CSharpCodeHelper.GetValidTypeName(name: oldName, namingPolicy: NamingPolicy.Pascal, allowBuiltInTypes: false);
				Log.Warn($"Invalid Query Object name [{oldName}]. Name [{queryName}] will be used");
			}

			if (!string.IsNullOrEmpty(resultName) && !CSharpCodeHelper.IsValidTypeName(name: resultName, allowBuiltInTypes: false))
			{
				string oldName = resultName;
				resultName = CSharpCodeHelper.GetValidTypeName(name: oldName, namingPolicy: NamingPolicy.Pascal, allowBuiltInTypes: false);
				Log.Warn($"Invalid Query Result Item name [{oldName}]. Name [{resultName}] will be used");
			}

			if (!string.IsNullOrEmpty(parameterName) && !CSharpCodeHelper.IsValidTypeName(name: parameterName, allowBuiltInTypes: false))
			{
				string oldName = parameterName;
				parameterName = CSharpCodeHelper.GetValidTypeName(name: oldName, namingPolicy: NamingPolicy.Pascal, allowBuiltInTypes: false);
				Log.Warn($"Invalid Query Parameter Item name [{oldName}]. Name [{parameterName}] will be used");
			}

			IReadOnlyDictionary<string, object> contextOptions = new Dictionary<string, object>
			{
				["Namespace"] = parameters.Namespace,
				["QueryName"] = queryName,
				["QueryResultItemName"] = resultName,
				["QueryParameterItemName"] = parameterName,
				["QueryText"] = info.Sections.Single(p => p.Type == QuerySectionType.Body).Content,
				["QueryTextRaw"] = rawSql,
				["ResourcePath"] = $"{parameters.Namespace}.{Path.GetFileName(parameters.Target)}"
			};

			Log.Trace(p => p("CodeGenerationContext options:\n" + string.Join("\n", contextOptions.Select(pair => $"\t{pair.Key}: {pair.Value}"))));

			var context = new CodeGenerationContext(info.Parameters, info.Results, contextOptions, TypeMapper, DatabaseProvider);

			return context;
		}

		public string FormatQuery(string query, GenerationOptions parameters)
		{
			IQueryParser parser = QueryParser;

			IQueryInfo info = parser.GetQueryInfo(query, parameters.ConnectionString);
			List<IQuerySection> sections = parser.GetQuerySections(query).ToList();

			if (SqlEmitter.CanEmitDeclarations)
			{
				IQuerySection[] allDeclarations = sections.Where(p => p.Type == QuerySectionType.Declarations).ToArray();
				sections.RemoveAll(allDeclarations.Contains);

				string declarations = SqlEmitter.EmitDeclarations(info.Parameters);

				IQuerySection declarationsSection = new QuerySection(QuerySectionType.Declarations, declarations);
				sections.Add(declarationsSection);
			}

			if (sections.All(p => p.Type != QuerySectionType.Options))
			{
				IQuerySection optionsSection = new QuerySection(QuerySectionType.Options, @"/* add SqlFirst options here */");
				sections.Add(optionsSection);
			}

			string result = SqlEmitter.EmitQuery(sections);
			return result;
		}
	}
}