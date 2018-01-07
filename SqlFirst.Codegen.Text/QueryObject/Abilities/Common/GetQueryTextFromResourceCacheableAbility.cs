using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using SqlFirst.Codegen.Helpers;
using SqlFirst.Codegen.Text.QueryObject.Data;
using SqlFirst.Codegen.Text.Snippets;

namespace SqlFirst.Codegen.Text.QueryObject.Abilities.Common
{
	internal class GetQueryTextFromResourceCacheableAbility : IQueryObjectAbility
	{
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

		/// <inheritdoc />
		public IQueryObjectData Apply(ICodeGenerationContext context, IQueryObjectData data)
		{
			var methodBuilder = new StringBuilder(QuerySnippet.Methods.Common.GetQueryFromResourceCacheable);

			string queryText = context.GetQueryText();
			int queryTextHash = CalculateChecksum(queryText);

			string checksumFieldName = CSharpCodeHelper.GetValidIdentifierName("queryTextChecksum", NamingPolicy.CamelCaseWithUnderscope);
			string hashCodeFieldType = CSharpCodeHelper.GetTypeBuiltInName(typeof(int));
			string hashCodeField = new StringBuilder(FieldSnippet.ReadOnlyField)
				.Replace("$Type$", hashCodeFieldType)
				.Replace("$Name$", checksumFieldName)
				.Replace("$Value$", queryTextHash.ToString(CultureInfo.InvariantCulture))
				.ToString();

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
			methodBuilder.Replace("$ChecksumName$", checksumFieldName);
			methodBuilder.Replace("$LockerName$", lockerFieldName);
			methodBuilder.Replace("$CacheName$", cacheFieldName);

			QueryObjectData result = QueryObjectData.CreateFrom(data);
			result.Fields = result.Fields.Append(cacheField, lockerField, hashCodeField);
			result.Methods = result.Methods.Append(QuerySnippet.Methods.Common.Snippets.CalculateChecksum, methodBuilder.ToString());
			result.Usings = result.Usings.Append(
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
	}
}