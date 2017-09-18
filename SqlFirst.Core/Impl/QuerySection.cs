using System;
using System.Diagnostics;

namespace SqlFirst.Core.Impl
{
	/// <inheritdoc />
	[DebuggerDisplay("{Type} [{Name}] => {Content}")]
	public class QuerySection : IQuerySection
	{
		private static string GetKnownName(QuerySectionType type)
		{
			switch (type)
			{
				case QuerySectionType.Options:
					return QuerySectionName.Options;

				case QuerySectionType.Declarations:
					return QuerySectionName.Declarations;

				case QuerySectionType.Body:
				case QuerySectionType.Unknown:
					return null;

				case QuerySectionType.Custom:
					throw new ArgumentException("Custom query section must have an explicit name");

				default:
					return null;
			}
		}

		/// <summary>
		/// Инициализирует новый экземпляр класса <see cref="QuerySection"/>
		/// </summary>
		/// <param name="type">Тип раздела</param>
		/// <param name="name">Имя раздела</param>
		/// <param name="content">Содержимое раздела</param>
		public QuerySection(QuerySectionType type, string name, string content)
		{
			Type = type;
			Name = name;
			Content = content;
		}

		/// <summary>
		/// Инициализирует новый экземпляр класса <see cref="QuerySection"/>
		/// </summary>
		/// <param name="type">Тип раздела</param>
		/// <param name="content">Содержимое раздела</param>
		public QuerySection(QuerySectionType type, string content)
			: this(type, GetKnownName(type), content)
		{
		}

		/// <inheritdoc />
		public QuerySectionType Type { get; }

		/// <inheritdoc />
		public string Name { get; }

		/// <inheritdoc />
		public string Content { get; }
	}
}