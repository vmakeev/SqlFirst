using System;
using SqlFirst.Core;

namespace SqlFirst.Codegen.Text.Common.PropertyGenerator
{
	/// <inheritdoc />
	internal class CodeMemberInfo : ICodeMemberInfo
	{
		/// <inheritdoc />
		public CodeMemberInfo(Type type, string name)
		{
			Type = type;
			Name = name;
			HasDefaultValue = false;
		}

		/// <inheritdoc />
		public CodeMemberInfo(Type type, string name, object defaultValue)
		{
			Type = type;
			Name = name;
			DefaultValue = defaultValue;
			HasDefaultValue = true;
		}

		/// <inheritdoc />
		public Type Type { get; }

		/// <inheritdoc />
		public string Name { get; }

		/// <summary>
		/// Имеет ли член класса значение по умолчанию
		/// </summary>
		public bool HasDefaultValue { get; }

		/// <summary>
		/// Значение по умолчанию
		/// </summary>
		public object DefaultValue { get; }

		public static CodeMemberInfo FromFieldDetails(IFieldDetails details, IDatabaseTypeMapper typeMapper)
		{
			if (details == null)
			{
				throw new ArgumentNullException(paramName: nameof(details));
			}

			if (typeMapper == null)
			{
				throw new ArgumentNullException(paramName: nameof(typeMapper));
			}

			Type type = typeMapper.MapToClrType(details.DbType, details.AllowDbNull);
			var result = new CodeMemberInfo(type, details.ColumnName);
			return result;
		}

		public static CodeMemberInfo FromQueryParamInfo(IQueryParamInfo info, IDatabaseTypeMapper typeMapper)
		{
			if (info == null)
			{
				throw new ArgumentNullException(paramName: nameof(info));
			}

			if (typeMapper == null)
			{
				throw new ArgumentNullException(paramName: nameof(typeMapper));
			}

			Type type = typeMapper.MapToClrType(info.DbType, true, info.DbTypeMetadata);

			CodeMemberInfo result = info.DefaultValue != null
				? new CodeMemberInfo(type, info.SemanticName, info.DefaultValue)
				: new CodeMemberInfo(type, info.SemanticName);

			return result;
		}
	}
}