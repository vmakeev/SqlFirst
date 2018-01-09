using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SqlFirst.Codegen.Text.Templating
{
	internal static class MetadataCache
	{
		private static readonly ConcurrentDictionary<Type, Dictionary<string, MethodInfo>> _cache = new ConcurrentDictionary<Type, Dictionary<string, MethodInfo>>();

		public static IReadOnlyDictionary<string, MethodInfo> GetTypePropertiesMetadata(Type type)
		{
			if (_cache.TryGetValue(type, out Dictionary<string, MethodInfo> cached))
			{
				return cached;
			}

			Dictionary<string, MethodInfo> propertiesMetadata = type.GetProperties()
																	.Where(propertyInfo => propertyInfo.CanRead)
																	.ToDictionary(info => info.Name, info => info.GetGetMethod());
			_cache.TryAdd(type, propertiesMetadata);
			return propertiesMetadata;
		}
	}
}