namespace SqlFirst.Providers.MsSqlServer
{
	internal class MsSqlServerTypeDescription
	{
		/// <summary>
		/// Имя типа данных. Уникально в пределах схемы
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Идентификатор внутреннего системного типа, соответствующего данному типу данных
		/// </summary>
		public byte SystemTypeId { get; set; }

		/// <summary>
		/// Идентификатор типа данных. Уникален в пределах базы данных.
		/// Для системных типов данных <see cref="UserTypeId"/> равен <see cref="SystemTypeId"/>
		/// </summary>
		public int UserTypeId { get; set; }

		/// <summary>
		/// Идентификатор схемы, к которой принадлежит тип данных
		/// </summary>
		public int SchemaId { get; set; }

		/// <summary>
		/// Идентификатор отдельного владельца, если он отличается от владельца схемы.
		/// По умолчанию содержащиеся в схеме объекты принадлежат владельцу схемы.
		/// Однако с помощью инструкции ALTER AUTHORIZATION можно изменить право собственности и указать другого владельца.
		/// Имеет значение NULL, если нет другого владельца
		/// </summary>
		public int? PrincipalId { get; set; }

		/// <summary>
		/// Максимальная длина типа (в байтах):
		/// -1 = тип данных столбца — varchar(max), nvarchar(max), varbinary(max), или xml
		/// </summary>
		public short MaxLength { get; set; }

		/// <summary>
		/// Максимальная точность значений этого типа данных, если он числовой; иначе — значение 0
		/// </summary>
		public byte Precision { get; set; }

		/// <summary>
		/// Максимальный масштаб значений этого типа данных, если он числовой; иначе — значение 0
		/// </summary>
		public byte Scale { get; set; }

		/// <summary>
		/// Имя параметров сортировки значений этого типа данных, если он символьный; иначе — значение NULL
		/// </summary>
		public string CollationName { get; set; }

		/// <summary>
		/// Тип данных допускает значения NULL
		/// </summary>
		public bool? IsNullable { get; set; }

		/// <summary>
		/// True = определяемый пользователем тип.
		/// False = SQL Server тип системных данных
		/// </summary>
		public bool IsUserDefined { get; set; }

		/// <summary>
		/// True = реализация этого типа данных определена в сборке среды CLR.
		/// False = тип данных основан на системном типе данных SQL Server
		/// </summary>
		public bool IsAssemblyType { get; set; }

		/// <summary>
		/// Идентификатор изолированного значения по умолчанию, привязанного к типу данных с помощью sp_bindefault.
		/// 0 = нет значения по умолчанию
		/// </summary>
		public int DefaultObjectId { get; set; }

		/// <summary>
		/// Идентификатор изолированного правила, привязанного к типу данных с помощью sp_bindrule.
		/// 0 = нет правила по умолчанию
		/// </summary>
		public int RuleObjectId { get; set; }

		/// <summary>
		/// Указывает, что тип является табличным
		/// </summary>
		public bool IsTableType { get; set; }
	}
}