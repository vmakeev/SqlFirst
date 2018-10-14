using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SqlFirst.Codegen.Helpers
{
	/// <summary>
	/// Помощник генерации кода C#
	/// </summary>
	public static class CSharpCodeHelper
	{
		private static readonly HashSet<string> _allReservedWords;

		private static readonly Dictionary<Type, string> _typeAliases;

		private static readonly ISet<string> _allBuiltInTypes;

		private static readonly ISet<string> _builtInReferenceTypes;

		private static readonly ISet<string> _builtInValueTypes;

		private static readonly HashSet<string> _specialWords;

		static CSharpCodeHelper()
		{
			_typeAliases = new Dictionary<Type, string>
			{
				[typeof(bool)] = "bool",
				[typeof(byte)] = "byte",
				[typeof(sbyte)] = "sbyte",
				[typeof(char)] = "char",
				[typeof(decimal)] = "decimal",
				[typeof(double)] = "double",
				[typeof(float)] = "float",
				[typeof(int)] = "int",
				[typeof(uint)] = "uint",
				[typeof(long)] = "long",
				[typeof(ulong)] = "ulong",
				[typeof(object)] = "object",
				[typeof(short)] = "short",
				[typeof(ushort)] = "ushort",
				[typeof(string)] = "string"
			};

			_builtInReferenceTypes = new HashSet<string>
			{
				"object",
				"string"
			};

			_builtInValueTypes = new HashSet<string>
			{
				"bool",
				"byte",
				"sbyte",
				"char",
				"decimal",
				"double",
				"float",
				"int",
				"uint",
				"long",
				"ulong",
				"short",
				"ushort"
			};

			_allBuiltInTypes = new HashSet<string>(_builtInReferenceTypes.Concat(_builtInValueTypes));

			_specialWords = new HashSet<string>
			{
				"abstract",
				"as",
				"base",
				"break",
				"case",
				"catch",
				"checked",
				"class",
				"const",
				"continue",
				"default",
				"delegate",
				"do",
				"else",
				"enum",
				"event",
				"explicit",
				"extern",
				"false",
				"finally",
				"fixed",
				"for",
				"foreach",
				"goto",
				"if",
				"implicit",
				"in",
				"interface",
				"internal",
				"is",
				"lock",
				"namespace",
				"new",
				"null",
				"operator",
				"out",
				"out",
				"override",
				"params",
				"private",
				"protected",
				"public",
				"readonly",
				"ref",
				"return",
				"sealed",
				"sizeof",
				"stackalloc",
				"static",
				"struct",
				"switch",
				"this",
				"throw",
				"true",
				"try",
				"typeof",
				"unchecked",
				"unsafe",
				"using",
				"static",
				"virtual",
				"void",
				"volatile",
				"while"
			};

			_allReservedWords = new HashSet<string>(_specialWords.Concat(_allBuiltInTypes));
		}

		/// <summary>
		/// Возвращает компилируемую строку, содержащую корректное объявление указанного типа
		/// </summary>
		/// <param name="type">Тип</param>
		/// <param name="directTypeNameResolver">Фабрика по получению имени типа без дополнительных преобразований</param>
		/// <param name="simplifyNullable">Использовать ли упрощенный синтаксис для Nullable&lt;&gt;</param>
		/// <returns>Компилируемая строка, содержащая корректное объявление указанного типа</returns>
		public static string GetTypeName(Type type, Func<Type, string> directTypeNameResolver, bool simplifyNullable)
		{
			if (simplifyNullable && TypesHelper.IsNullableValueType(type, out Type underlyingType))
			{
				string underlyingTypeName = GetTypeName(underlyingType, directTypeNameResolver, true);
				return $"{underlyingTypeName}?";
			}

			if (TypesHelper.IsGenericType(type, out Type[] underlyingTypes))
			{
				string outerTypeName = GetTypeName(type.GetGenericTypeDefinition(), directTypeNameResolver, simplifyNullable);
				string genericArguments = string.Join(", ", underlyingTypes.Select(innerType => GetTypeName(innerType, directTypeNameResolver, simplifyNullable)));
				return $"{outerTypeName}<{genericArguments}>";
			}

			if (TypesHelper.IsArrayType(type, out Type itemType))
			{
				string arrayItemTypeName = GetTypeName(itemType, directTypeNameResolver, simplifyNullable);
				return $"{arrayItemTypeName}[]";
			}

			if (type.IsGenericTypeDefinition)
			{
				string typeName = directTypeNameResolver.Invoke(type);
				int genericTypeMarkIndex = typeName.IndexOf('`');
				return typeName.Substring(0, genericTypeMarkIndex);
			}

			return directTypeNameResolver.Invoke(type);
		}

		/// <summary>
		/// Возвращает полное имя типа, включая пространство имен
		/// </summary>
		/// <param name="type">Тип</param>
		/// <returns>Полное имя типа, включая пространство имен</returns>
		public static string GetTypeFullName(Type type)
		{
			string NameResolver(Type t)
			{
				return $"{t.Namespace}.{t.Name}";
			}

			return GetTypeName(
				type: type,
				directTypeNameResolver: NameResolver,
				simplifyNullable: false);
		}

		/// <summary>
		/// Возвращает имя типа
		/// </summary>
		/// <param name="type">Тип</param>
		/// <returns>Имя типа</returns>
		public static string GetTypeShortName(Type type)
		{
			string NameResolver(Type t)
			{
				return t.Name;
			}

			return GetTypeName(
				type: type,
				directTypeNameResolver: NameResolver,
				simplifyNullable: false);
		}

		/// <summary>
		/// Возвращает имя встроенного в C# типа, или просто имя типа, если отдельного ключевого слова не существует
		/// </summary>
		/// <param name="type">Тип</param>
		/// <returns>Наиболее короткое имя для типа</returns>
		public static string GetTypeBuiltInName(Type type)
		{
			string NameResolver(Type t)
			{
				return _typeAliases.TryGetValue(t, out string alias)
					? alias
					: t.Name;
			}

			return GetTypeName(
				type: type,
				directTypeNameResolver: NameResolver,
				simplifyNullable: true);
		}

		/// <summary>
		/// Возвращает корректное название переменной
		/// </summary>
		/// <param name="name">Предполагаемое название переменной</param>
		/// <returns>Корректное название переменной</returns>
		public static string GetValidIdentifierName(string name)
		{
			return GetValidIdentifierNameInternal(name, null);
		}

		/// <summary>
		/// Проверяет, является ли <paramref name="name" /> корректным идентификатором
		/// </summary>
		/// <param name="name">Проверяемый идентификатор</param>
		/// <returns>Является ли идентификатор корректным</returns>
		public static bool IsValidIdentifierName(string name)
		{
			using (CodeDomProvider provider = CodeDomProvider.CreateProvider("C#"))
			{
				return provider.IsValidIdentifier(name);
			}
		}

		/// <summary>
		/// Возвращает корректное название переменной в указанном стиле
		/// </summary>
		/// <param name="name">Предполагаемое название переменной</param>
		/// <param name="namingPolicy">Политика именования переменной</param>
		/// <returns>Корректное название переменной</returns>
		/// <returns></returns>
		public static string GetValidIdentifierName(string name, NamingPolicy namingPolicy)
		{
			return GetValidIdentifierNameInternal(name, namingPolicy);
		}

		/// <summary>
		/// Возвращает корректное название типа без пространства имен и generic параметров
		/// </summary>
		/// <param name="name">Предполагаемое название типа</param>
		/// <param name="allowBuiltInTypes">Разрешить возврат имен, являющихся псевдонимами встроенных типов данных</param>
		/// <returns>Корректное название типа без пространства имен и generic параметров</returns>
		/// <returns></returns>
		public static string GetValidTypeName(string name, bool allowBuiltInTypes)
		{
			return GetValidTypeNameInternal(name, null, allowBuiltInTypes);
		}

		/// <summary>
		/// Возвращает корректное название типа без пространства имен и generic параметров
		/// </summary>
		/// <param name="name">Предполагаемое название типа</param>
		/// <param name="namingPolicy">Политика именования типа</param>
		/// <param name="allowBuiltInTypes">Разрешить возврат имен, являющихся псевдонимами встроенных типов данных</param>
		/// <returns>Корректное название типа без пространства имен и generic параметров</returns>
		/// <returns></returns>
		public static string GetValidTypeName(string name, NamingPolicy namingPolicy, bool allowBuiltInTypes)
		{
			return GetValidTypeNameInternal(name, namingPolicy, allowBuiltInTypes);
		}

		[SuppressMessage("ReSharper", "PossibleNullReferenceException")]
		public static string GetValidValue(Type targetType, object value)
		{
			if (value == null)
			{
				return "null";
			}

			string valueString = Convert.ToString(value, CultureInfo.InvariantCulture);

			if (targetType == typeof(string))
			{
				return $"\"{valueString}\"";
			}

			if (targetType == typeof(DateTime) || targetType == typeof(DateTime?))
			{
				return $"DateTime.Parse(\"{valueString}\")";
			}

			if (targetType == typeof(Guid) || targetType == typeof(Guid?))
			{
				return $"Guid.Parse(\"{valueString}\")";
			}

			if (targetType == typeof(bool) || targetType == typeof(bool?))
			{
				if (!bool.TryParse(valueString, out bool boolValue))
				{
					throw new CodeGenerationException($"Unable to convert [{valueString}] to boolean.");
				}

				return boolValue.ToString().ToLowerInvariant();
			}

			var fractionalTypes = new HashSet<Type>
			{
				typeof(decimal),
				typeof(decimal?),
				typeof(double),
				typeof(double?),
				typeof(float),
				typeof(float?)
			};

			if (fractionalTypes.Contains(targetType))
			{
				return valueString.Replace(",", ".");
			}

			throw new CodeGenerationException($"Unable generate value of type [{targetType.FullName}].");
		}

		public static string EscapeString(string input)
		{
			var literal = new StringBuilder(input.Length);
			foreach (char c in input)
			{
				switch (c)
				{
					case '\'':
						literal.Append(@"\'");
						break;
					case '\"':
						literal.Append("\\\"");
						break;
					case '\\':
						literal.Append(@"\\");
						break;
					case '\0':
						literal.Append(@"\0");
						break;
					case '\a':
						literal.Append(@"\a");
						break;
					case '\b':
						literal.Append(@"\b");
						break;
					case '\f':
						literal.Append(@"\f");
						break;
					case '\n':
						literal.Append(@"\n");
						break;
					case '\r':
						literal.Append(@"\r");
						break;
					case '\t':
						literal.Append(@"\t");
						break;
					case '\v':
						literal.Append(@"\v");
						break;
					default:
						if (char.GetUnicodeCategory(c) != UnicodeCategory.Control)
						{
							literal.Append(c);
						}
						else
						{
							literal.Append(@"\u");
							literal.Append(((ushort)c).ToString("x4"));
						}

						break;
				}
			}

			return literal.ToString();
		}

		/// <summary>
		/// Возвращает строковое представление имени generic-типа с указанными аргументами
		/// </summary>
		/// <param name="genericType">Базовый тип</param>
		/// <param name="genericTypeArguments">Аргументы типа</param>
		/// <returns>Строковое представление имени generic-типа с указанными аргументами</returns>
		public static string GetGenericType(Type genericType, params string[] genericTypeArguments)
		{
			if (genericType == null)
			{
				throw new ArgumentNullException(nameof(genericType));
			}

			if (genericTypeArguments == null)
			{
				throw new ArgumentNullException(nameof(genericTypeArguments));
			}

			if (!genericType.IsGenericTypeDefinition)
			{
				throw new ArgumentException("Provided type must be open generic type.", nameof(genericType));
			}

			if (genericTypeArguments.Any(string.IsNullOrWhiteSpace))
			{
				throw new ArgumentException("Generic type arguments can not contain empty values.", nameof(genericTypeArguments));
			}

			if (genericTypeArguments.Length != genericType.GetGenericArguments().Length)
			{
				throw new ArgumentException("Generic type arguments count must be equals to generic type generic arguments count.", nameof(genericTypeArguments));
			}

			// todo: validate arguments

			string genericTypeBaseName = GetTypeShortName(genericType);
			return $"{genericTypeBaseName}<{string.Join(", ", genericTypeArguments)}>";
		}

		/// <summary>
		/// Проверяет, является ли <paramref name="name" /> корректным именем не generic типа
		/// </summary>
		/// <param name="name">Проверяемое имя типа</param>
		/// <param name="allowBuiltInTypes">Считать ли псевдонимы встроенных типов корректными именами</param>
		/// <returns>True, если <paramref name="name" /> является корректным именем не generic типа</returns>
		public static bool IsValidTypeName(string name, bool allowBuiltInTypes)
		{
			if (string.IsNullOrEmpty(name))
			{
				return false;
			}

			if (char.IsDigit(name[0]))
			{
				return false;
			}

			if (_allBuiltInTypes.Contains(name))
			{
				return allowBuiltInTypes;
			}

			if (_specialWords.Contains(name))
			{
				return false;
			}

			if (name.EndsWith("?"))
			{
				string typeNameWithoutNullableMark = name.Substring(0, name.Length - 1);

				if (_builtInReferenceTypes.Contains(typeNameWithoutNullableMark))
				{
					return false;
				}

				if (_builtInValueTypes.Contains(typeNameWithoutNullableMark))
				{
					return allowBuiltInTypes;
				}
			}

			return Regex.IsMatch(name, @"^@?[a-zA-Z_]\w*\??$");
		}

		/// <summary>
		/// Возвращает корректное название переменной в указанном стиле
		/// </summary>
		/// <param name="name">Предполагаемое название переменной</param>
		/// <param name="namingPolicy">Политика именования переменной</param>
		/// <returns>Корректное название переменной</returns>
		/// <returns></returns>
		private static string GetValidIdentifierNameInternal(string name, NamingPolicy? namingPolicy)
		{
			if (string.IsNullOrEmpty(name))
			{
				return "_";
			}

			string preparedName = RemoveInvalidIdentifierSymbols(name, namingPolicy == null ? (char?)null : '_');

			string formattedName;

			switch (namingPolicy)
			{
				case null:
					formattedName = preparedName;
					break;

				case NamingPolicy.CamelCase:
					formattedName = AdjustTextCase.ToCamelCase(preparedName);
					break;

				case NamingPolicy.CamelCaseWithUnderscope:
					formattedName = '_' + AdjustTextCase.ToCamelCase(preparedName).TrimStart('_', '@');
					break;

				case NamingPolicy.Pascal:
					formattedName = AdjustTextCase.ToPascal(preparedName);
					break;

				case NamingPolicy.Underscope:
					formattedName = AdjustTextCase.ToUnderscopes(preparedName);
					break;

				default:
					throw new ArgumentOutOfRangeException(nameof(namingPolicy), namingPolicy, null);
			}

			string result = FixIdentifierNameStartSymbolIssues(formattedName);

			result = Regex.Replace(result, @"([^_])__+", "$1_");

			return result;
		}

		/// <summary>
		/// Возвращает корректное название типа без пространства имен и generic параметров
		/// </summary>
		/// <param name="name">Предполагаемое название типа</param>
		/// <param name="namingPolicy">Политика именования типа</param>
		/// <param name="allowBuiltInTypes">Разрешить возврат имен, являющихся псевдонимами встроенных типов данных</param>
		/// <returns>Корректное название типа без пространства имен и generic параметров</returns>
		/// <returns></returns>
		private static string GetValidTypeNameInternal(string name, NamingPolicy? namingPolicy, bool allowBuiltInTypes)
		{
			if (name == null)
			{
				return "_";
			}

			const char namespaceDelimiter = '.';
			const char genericStartSymbol = '<';

			int genericStartIndex = name.IndexOf(genericStartSymbol);
			name = genericStartIndex >= 0
				? name.Substring(0, genericStartIndex)
				: name;

			int lastDelimiterIndex = name.LastIndexOf(namespaceDelimiter);
			name = lastDelimiterIndex >= 0
				? name.Substring(lastDelimiterIndex)
				: name;

			name = RemoveInvalidIdentifierSymbols(name, namingPolicy == null ? (char?)null : '_').Trim();

			switch (namingPolicy)
			{
				case null:
					break;

				case NamingPolicy.CamelCase:
					name = AdjustTextCase.ToCamelCase(name);
					break;

				case NamingPolicy.CamelCaseWithUnderscope:
					name = '_' + AdjustTextCase.ToCamelCase(name).TrimStart('_', '@');
					break;

				case NamingPolicy.Pascal:
					name = AdjustTextCase.ToPascal(name);
					break;

				case NamingPolicy.Underscope:
					name = AdjustTextCase.ToUnderscopes(name);
					break;

				default:
					throw new ArgumentOutOfRangeException(nameof(namingPolicy), namingPolicy, null);
			}

			if (name.Length > 0 && char.IsDigit(name[0]))
			{
				name = "_" + name;
			}

			if (string.IsNullOrEmpty(name))
			{
				name = "_";
			}

			name = Regex.Replace(name, @"([^_])__+", "$1_");

			if (_allBuiltInTypes.Contains(name) && !allowBuiltInTypes)
			{
				name = $"@{name}";
			}

			return name;
		}

		/// <summary>
		/// Выполняет корректировку первого символа имени переменной, если имя не является корректным идентификатором C# по этой
		/// причине
		/// </summary>
		/// <param name="name">Предполагаемое имя переменной</param>
		/// <returns>Скорректированное имя переменной</returns>
		private static string FixIdentifierNameStartSymbolIssues(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				return "_";
			}

			string result;

			if (char.IsDigit(name[0]))
			{
				result = '_' + name;
			}
			else
			{
				result = name;
			}

			if (_allReservedWords.Contains(result))
			{
				return '@' + result;
			}

			return result;
		}

		/// <summary>
		/// Удаляет из имени переменной неподдерживаемые языком символы
		/// </summary>
		/// <param name="name">Предполагаемое имя переменной</param>
		/// <param name="replacement">Символ, на который следует заменить некорректные</param>
		/// <returns>Имя переменной, состоящее только из корректных символов</returns>
		private static string RemoveInvalidIdentifierSymbols(string name, char? replacement = null)
		{
			bool IsValidSymbol(char c)
			{
				return char.IsLetterOrDigit(c) || c == '_';
			}

			return replacement != null
				? new string(name.Select(symbol => IsValidSymbol(symbol) ? symbol : replacement.Value).ToArray())
				: new string(name.Where(IsValidSymbol).ToArray());
		}
	}
}