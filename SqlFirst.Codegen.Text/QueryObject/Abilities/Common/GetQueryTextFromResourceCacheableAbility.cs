using System.Collections.Generic;
using System.Linq;
using System.Text;
using SqlFirst.Codegen.Helpers;
using SqlFirst.Codegen.Text.QueryObject.Data;
using SqlFirst.Codegen.Text.Snippets;

namespace SqlFirst.Codegen.Text.QueryObject.Abilities.Common
{
	internal class GetQueryTextFromResourceCacheableAbility : IQueryObjectAbility
	{
		/// <inheritdoc />
		public IQueryObjectData Apply(ICodeGenerationContext context, IQueryObjectData data)
		{
			var methodBuilder = new StringBuilder(QuerySnippet.Methods.Common.GetQueryFromResourceCacheable);

			string cacheFieldName = CSharpCodeHelper.GetValidIdentifierName("cachedSql", NamingPolicy.CamelCaseWithUnderscope);
			string cacheFieldType = CSharpCodeHelper.GetTypeBuiltInName(typeof(string));
			string cacheField = new StringBuilder(FieldSnippet.BackingField)
				.Replace("$Type$", cacheFieldType)
				.Replace("$Name$", cacheFieldName)
				.ToString();

			string lockerFieldName = CSharpCodeHelper.GetValidIdentifierName("cachedSqlLocker", NamingPolicy.CamelCaseWithUnderscope);
			string lockerFieldType = CSharpCodeHelper.GetTypeBuiltInName(typeof(object));
			string lockerFieldValue = $"new {lockerFieldType}()";
			string lockerField = new StringBuilder(FieldSnippet.ReadOnlyField)
				.Replace("$Type$", lockerFieldType)
				.Replace("$Name$", lockerFieldName)
				.Replace("$Value$", lockerFieldValue)
				.ToString();

			methodBuilder.Replace("$QueryName$", context.GetQueryName());
			methodBuilder.Replace("$QuerySqlFullPath$", context.GetResourcePath());
			methodBuilder.Replace("$LockerName$", lockerFieldName);
			methodBuilder.Replace("$CacheName$", cacheFieldName);

			QueryObjectData result = QueryObjectData.CreateFrom(data);
			result.Fields = result.Fields.Append(cacheField, lockerField);
			result.Methods = result.Methods.Append(methodBuilder.ToString());
			result.Usings = result.Usings.Append(
				"System",
				"System.IO",
				"System.Text.RegularExpressions");
			return result;
		}

		/// <inheritdoc />
		public IEnumerable<string> GetDependencies() => Enumerable.Empty<string>();

		/// <inheritdoc />
		public string Name { get; } = KnownAbilityName.GetQueryText;
	}
}