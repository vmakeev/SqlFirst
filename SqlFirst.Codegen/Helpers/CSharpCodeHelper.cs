using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace SqlFirst.Codegen.Helpers
{
	/// <summary>
	/// Помощник генерации кода C#
	/// </summary>
	public static class CSharpCodeHelper
	{
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
			if (string.IsNullOrEmpty(name))
			{
				return "_";
			}

			string preparedName = RemoveInvalidSymbols(name);
			string result = FixNameStartSymbolIssues(preparedName);

			return result;
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
			if (string.IsNullOrEmpty(name))
			{
				return "_";
			}

			string preparedName = RemoveInvalidSymbols(name, '_');

			string formattedName;

			switch (namingPolicy)
			{
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

			string result = FixNameStartSymbolIssues(formattedName);

			return result;
		}

		/// <summary>
		/// Выполняет корректировку первого символа имени переменной, если имя не является корректным идентификатором C# по этой
		/// причине
		/// </summary>
		/// <param name="name">Предполагаемое имя переменной</param>
		/// <returns>Скорректированное имя переменной</returns>
		private static string FixNameStartSymbolIssues(string name)
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

			if (_reservedWords.Any(reservedWord => result == reservedWord))
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
		private static string RemoveInvalidSymbols(string name, char? replacement = null)
		{
			bool IsValidSymbol(char c)
			{
				return char.IsLetterOrDigit(c) || c == '_';
			}

			return replacement != null
				? new string(name.Select(symbol => IsValidSymbol(symbol) ? symbol : replacement.Value).ToArray())
				: new string(name.Where(IsValidSymbol).ToArray());
		}

		#region Long arrays

		private static readonly HashSet<string> _reservedWords = new HashSet<string>
		{
			"abstract",
			"as",
			"base",
			"bool",
			"break",
			"byte",
			"case",
			"catch",
			"char",
			"checked",
			"class",
			"const",
			"continue",
			"decimal",
			"default",
			"delegate",
			"do",
			"double",
			"else",
			"enum",
			"event",
			"explicit",
			"extern",
			"false",
			"finally",
			"fixed",
			"float",
			"for",
			"foreach",
			"goto",
			"if",
			"implicit",
			"in",
			"int",
			"interface",
			"internal",
			"is",
			"lock",
			"long",
			"namespace",
			"new",
			"null",
			"object",
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
			"sbyte",
			"sealed",
			"short",
			"sizeof",
			"stackalloc",
			"static",
			"string",
			"struct",
			"switch",
			"this",
			"throw",
			"true",
			"try",
			"typeof",
			"uint",
			"ulong",
			"unchecked",
			"unsafe",
			"ushort",
			"using",
			"static",
			"virtual",
			"void",
			"volatile",
			"while"
		};

		private static readonly Dictionary<Type, string> _typeAliases = new Dictionary<Type, string>
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

		#endregion

		public static string GetValidValue(Type targetType, object value)
		{
			if (value == null)
			{
				return "\"null\"";
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

			if (targetType == typeof(int) || targetType == typeof(int?) ||
				targetType == typeof(uint) || targetType == typeof(uint?) ||
				targetType == typeof(long) || targetType == typeof(long?) ||
				targetType == typeof(ulong) || targetType == typeof(ulong?) ||
				targetType == typeof(byte) || targetType == typeof(byte?) ||
				targetType == typeof(sbyte) || targetType == typeof(sbyte?) ||
				targetType == typeof(decimal) || targetType == typeof(decimal?) ||
				targetType == typeof(double) || targetType == typeof(double?) ||
				targetType == typeof(float) || targetType == typeof(float?))
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
					case '\'': literal.Append(@"\'"); break;
					case '\"': literal.Append("\\\""); break;
					case '\\': literal.Append(@"\\"); break;
					case '\0': literal.Append(@"\0"); break;
					case '\a': literal.Append(@"\a"); break;
					case '\b': literal.Append(@"\b"); break;
					case '\f': literal.Append(@"\f"); break;
					case '\n': literal.Append(@"\n"); break;
					case '\r': literal.Append(@"\r"); break;
					case '\t': literal.Append(@"\t"); break;
					case '\v': literal.Append(@"\v"); break;
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

		public static string EscapeStringVerbatium(string input)
		{
			return input.Replace("\"", "\"\"");
		}
	}
}