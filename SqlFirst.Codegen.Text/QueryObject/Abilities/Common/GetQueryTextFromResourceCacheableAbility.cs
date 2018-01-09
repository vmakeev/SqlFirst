using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using SqlFirst.Codegen.Helpers;
using SqlFirst.Codegen.Text.QueryObject.Data;
using SqlFirst.Codegen.Text.Snippets;
using SqlFirst.Codegen.Text.Templating;

namespace SqlFirst.Codegen.Text.QueryObject.Abilities.Common
{
	internal class GetQueryTextFromResourceCacheableAbility : IQueryObjectAbility
	{
		/// <inheritdoc />
		public IQueryObjectData Apply(ICodeGenerationContext context, IQueryObjectData data)
		{
			(string checksumFieldName, IRenderable checksumField) = GetChecksumField(context.GetQueryText());
			(string cacheFieldName, IRenderable cacheField) = GetCacheField();
			(string lockerFieldName, IRenderable lockerField) = GetLockerField();

			IRenderable primaryMethod = GetPrimaryMethod(context, checksumFieldName, cacheFieldName, lockerFieldName);
			IRenderable calculateChecksumMethod = GetCalculateChecksumMethod();

			QueryObjectData result = QueryObjectData.CreateFrom(data);
			result.Fields = result.Fields.AppendItems(cacheField.Render(), lockerField.Render(), checksumField.Render());
			result.Methods = result.Methods.AppendItems(calculateChecksumMethod.Render(), primaryMethod.Render());
			result.Usings = result.Usings.AppendItems(
				"System",
				"System.IO",
				"System.Text",
				"System.Text.RegularExpressions");
			return result;
		}

		/// <inheritdoc />
		public IEnumerable<string> GetDependencies() => Enumerable.Empty<string>();

		/// <inheritdoc />
		public string Name { get; } = KnownAbilityName.GetQueryText;

		private int CalculateChecksum(string data)
		{
			if (string.IsNullOrEmpty(data))
			{
				return 0;
			}

			byte[] bytes = Encoding.UTF8.GetBytes(data);

			const ushort poly = 4129;
			var table = new ushort[256];
			const ushort initialValue = 0xffff;
			ushort crc = initialValue;
			for (int i = 0; i < table.Length; ++i)
			{
				ushort temp = 0;
				ushort a = (ushort)(i << 8);
				for (int j = 0; j < 8; ++j)
				{
					if (((temp ^ a) & 0x8000) != 0)
					{
						temp = (ushort)((temp << 1) ^ poly);
					}
					else
					{
						temp <<= 1;
					}

					a <<= 1;
				}

				table[i] = temp;
			}

			foreach (byte b in bytes)
			{
				crc = (ushort)((crc << 8) ^ table[(crc >> 8) ^ (0xff & b)]);
			}

			return crc;
		}

		private IRenderable GetCalculateChecksumMethod()
		{
			return Snippet.Query.Methods.Common.Snippets.CalculateChecksum;
		}

		private static IRenderable GetPrimaryMethod(ICodeGenerationContext context, string checksumFieldName, string cacheFieldName, string lockerFieldName)
		{
			IRenderableTemplate template = Snippet.Query.Methods.Common.GetQueryFromResourceCacheable;
			var model = new
			{
				QueryName = context.GetQueryName(),
				QuerySqlFullPath = context.GetResourcePath(),
				ChecksumName = checksumFieldName,
				LockerName = lockerFieldName,
				CacheName = cacheFieldName
			};
			return Renderable.Create(template, model);
		}

		private (string checksumFieldName, IRenderable checksumField) GetChecksumField(string queryText)
		{
			int queryTextHash = CalculateChecksum(queryText);

			string checksumFieldName = CSharpCodeHelper.GetValidIdentifierName("queryTextChecksum", NamingPolicy.CamelCaseWithUnderscope);
			string hashCodeFieldType = CSharpCodeHelper.GetTypeBuiltInName(typeof(int));

			IRenderableTemplate template = Snippet.Field.ReadOnlyField;
			var model = new
			{
				Type = hashCodeFieldType,
				Name = checksumFieldName,
				Value = queryTextHash.ToString(CultureInfo.InvariantCulture)
			};

			IRenderable hashCodeField = Renderable.Create(template, model);

			return (checksumFieldName, hashCodeField);
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