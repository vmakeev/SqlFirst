using System;
using System.Collections.Generic;
using SqlFirst.Core;

namespace SqlFirst.Intelligence.Options
{
	public static class OptionDefaultsMapper
	{
		public static IOptionDefaults ToOptionDefaults(this SqlFirstDefaults source)
		{
			SqlFirstDefaultsSection common = source.Common ?? SqlFirstDefaultsSection.Empty;

			SqlFirstDefaultsSection select = source.Select ?? SqlFirstDefaultsSection.Empty;
			SqlFirstDefaultsSection insert = source.Insert ?? SqlFirstDefaultsSection.Empty;
			SqlFirstDefaultsSection update = source.Update ?? SqlFirstDefaultsSection.Empty;
			SqlFirstDefaultsSection delete = source.Delete ?? SqlFirstDefaultsSection.Empty;

			var target = new OptionDefaults
			{
				["select"] = MergeWithCommon(@select, common),
				["insert"] = MergeWithCommon(insert, common),
				["update"] = MergeWithCommon(update, common),
				["delete"] = MergeWithCommon(delete, common)
			};

			return target;
		}

		private static IReadOnlyDictionary<string, bool> MergeWithCommon(SqlFirstDefaultsSection specific, SqlFirstDefaultsSection common)
		{
			var target = new Dictionary<string, bool>(specific, StringComparer.InvariantCultureIgnoreCase);

			foreach (KeyValuePair<string, bool> pair in common)
			{
				if (!target.ContainsKey(pair.Key))
				{
					target.Add(pair.Key, pair.Value);
				}
			}

			return target;
		}
	}
}