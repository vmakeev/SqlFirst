using System.Collections.Generic;
using System.Linq;
using SqlFirst.Codegen.Helpers;
using SqlFirst.Codegen.Text.QueryObject.Data;
using SqlFirst.Codegen.Text.Snippets;
using SqlFirst.Codegen.Text.Templating;

namespace SqlFirst.Codegen.Text.QueryObject.Abilities.Common
{
	internal class GetQueryTextFromResourceCacheableNoCheckAbility : IQueryObjectAbility
	{
		/// <inheritdoc />
		public IQueryObjectData Apply(ICodeGenerationContext context, IQueryObjectData data)
		{
			(string cacheFieldName, IRenderable cacheField) = GetCacheField();
			(string lockerFieldName, IRenderable lockerField) = GetLockerField();

			IRenderable primaryMethod = GetPrimaryMethod(context, cacheFieldName, lockerFieldName);

			QueryObjectData result = QueryObjectData.CreateFrom(data);
			result.Fields = result.Fields.AppendItems(cacheField.Render(), lockerField.Render());
			result.Methods = result.Methods.AppendItems(primaryMethod.Render());
			result.Usings = result.Usings.AppendItems(
				"System",
				"System.IO",
				"System.Text",
				"System.Text.RegularExpressions");
			return result;
		}

		/// <inheritdoc />
		public IEnumerable<string> GetDependencies() => new[]
		{
			KnownAbilityName.ProcessCachedSql
		};

		/// <inheritdoc />
		public string Name { get; } = KnownAbilityName.GetQueryText;

		private static IRenderable GetPrimaryMethod(ICodeGenerationContext context, string cacheFieldName, string lockerFieldName)
		{
			IRenderableTemplate template = Snippet.Query.Methods.Common.GetQueryFromResourceCacheableNoCheck;
			var model = new
			{
				QueryName = context.GetQueryName(),
				QuerySqlFullPath = context.GetResourcePath(),
				LockerName = lockerFieldName,
				CacheName = cacheFieldName
			};
			return Renderable.Create(template, model);
		}

		private (string cacheFieldName, IRenderable cacheField) GetCacheField()
		{
			string cacheFieldName = CSharpCodeHelper.GetValidIdentifierName("cachedSql", NamingPolicy.CamelCaseWithUnderscope);
			string cacheFieldType = CSharpCodeHelper.GetTypeBuiltInName(typeof(string));

			IRenderableTemplate template = Snippet.Field.BackingField;
			var model = new
			{
				Type = cacheFieldType,
				Name = cacheFieldName
			};

			IRenderable cacheField = Renderable.Create(template, model);

			return (cacheFieldName, cacheField);
		}

		private (string lockerFieldName, IRenderable lockerField) GetLockerField()
		{
			string lockerFieldName = CSharpCodeHelper.GetValidIdentifierName("cachedSqlLocker", NamingPolicy.CamelCaseWithUnderscope);
			string lockerFieldType = CSharpCodeHelper.GetTypeBuiltInName(typeof(object));
			string lockerFieldValue = $"new {lockerFieldType}()";

			IRenderableTemplate template = Snippet.Field.ReadOnlyField;

			var model = new
			{
				Type = lockerFieldType,
				Name = lockerFieldName,
				Value = lockerFieldValue
			};

			IRenderable lockerField = Renderable.Create(template, model);

			return (lockerFieldName, lockerField);
		}
	}
}