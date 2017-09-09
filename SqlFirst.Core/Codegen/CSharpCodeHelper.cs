﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace SqlFirst.Core.Codegen
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
				string genericArguments = string.Join(", ", underlyingTypes.Select(p => GetTypeName(p, directTypeNameResolver, simplifyNullable)));
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
		public static string GetValidVariableName(string name)
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
		public static string GetValidVariableName(string name, VariableNamingPolicy namingPolicy)
		{
			if (string.IsNullOrEmpty(name))
			{
				return "_";
			}

			string preparedName = RemoveInvalidSymbols(name, '_');

			string formattedName;

			switch (namingPolicy)
			{
				case VariableNamingPolicy.CamelCase:
					formattedName = TextCaseFormatter.ToCamelCase(preparedName);
					break;

				case VariableNamingPolicy.CamelCaseWithUnderscope:
					formattedName = '_' + TextCaseFormatter.ToCamelCase(preparedName).TrimStart('_', '@');
					break;

				case VariableNamingPolicy.Pascal:
					formattedName = TextCaseFormatter.ToPascal(preparedName);
					break;

				case VariableNamingPolicy.Underscope:
					formattedName = TextCaseFormatter.ToUnderscopes(preparedName);
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

			if (_reservedWords.Any(p => p == result))
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
				? new string(name.Select(c => IsValidSymbol(c) ? c : replacement.Value).ToArray())
				: new string(name.Where(IsValidSymbol).ToArray());
		}

		#region Long arrays

		private static readonly string[] _reservedWords =
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
	}
}