using System.Data;

namespace SqlFirst.Providers.MsSqlServer
{
	internal class UndeclaredParameterDescription
	{
		/// <summary>
		/// Содержит порядковый номер параметра в результирующем наборе.
		/// Позиция первого параметра будет указана как 1
		/// </summary>
		public int ParameterOrdinal { get; private set; }

		/// <summary>
		/// Содержит имя параметра
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		/// Содержит system_type_id типа данных параметра, как указано в sys.types
		/// Для типов CLR несмотря на то что system_type_name столбец вернет значение NULL, будет возвращено значение 240
		/// </summary>
		public int SuggestedSystemTypeId { get; private set; }

		/// <summary>
		/// Содержит имя типа данных. Включает аргументы (длина, точность, масштаб), заданные для типа данных параметра.
		/// Если тип данных является пользовательским псевдонимом, то здесь указывается базовый системный тип данных.
		/// Если это определяемый пользователем тип данных CLR, то возвращается значение NULL.
		/// Если не удается определить тип параметра, возвращается значение NULL
		/// </summary>
		public string SuggestedSystemTypeName { get; private set; }

		/// <summary>
		/// Максимальная длина
		/// </summary>
		public short SuggestedMaxLength { get; private set; }

		/// <summary>
		/// Точность
		/// </summary>
		public byte SuggestedPrecision { get; private set; }

		/// <summary>
		/// Точность дробной части
		/// </summary>
		public byte SuggestedScale { get; private set; }

		/// <summary>
		/// Для типов CLR и псевдонимов содержит user_type_id для типа данных столбца, как указано в sys.types.
		/// В противном случае значение равно NULL
		/// </summary>
		public int? SuggestedUserTypeId { get; private set; }

		/// <summary>
		/// Для типов CLR и псевдонимов содержит имя базы данных, в которой этот тип определен.
		/// В противном случае значение равно NULL
		/// </summary>
		public string SuggestedUserTypeDatabase { get; private set; }

		/// <summary>
		/// Для типов CLR и псевдонимов содержит имя схемы, в которой этот тип определен.
		/// В противном случае значение равно NULL
		/// </summary>
		public string SuggestedUserTypeSchema { get; private set; }

		/// <summary>
		/// Для типов CLR и псевдонимов содержит имя типа.
		/// В противном случае значение равно NULL
		/// </summary>
		public string SuggestedUserTypeName { get; private set; }

		/// <summary>
		/// Для типов CLR возвращает имя сборки и класса, определяющего тип.
		/// В противном случае значение равно NULL
		/// </summary>
		public string SuggestedAssemblyQualifiedTypeName { get; private set; }

		/// <summary>
		/// Содержит xml_collection_id для типа данных параметра, как указано в sys.columns.
		/// Содержит NULL, если возвращаемый тип не связан с коллекцией схем XML
		/// </summary>
		public int? SuggestedXmlCollectionId { get; private set; }

		/// <summary>
		/// Содержит базу данных, в которой определена коллекция схем XML, связанная с этим типом.
		/// Содержит NULL, если возвращаемый тип не связан с коллекцией схем XML
		/// </summary>
		public string SuggestedXmlCollectionDatabase { get; private set; }

		/// <summary>
		/// Содержит схему, в которой определена коллекция схем XML, связанная с этим типом.
		/// Содержит NULL, если возвращаемый тип не связан с коллекцией схем XML
		/// </summary>
		public string SuggestedXmlCollectionSchema { get; private set; }

		/// <summary>
		/// Содержит имя коллекции схем XML, связанной с этим типом.
		/// Содержит NULL, если возвращаемый тип не связан с коллекцией схем XML
		/// </summary>
		public string SuggestedXmlCollectionName { get; private set; }

		/// <summary>
		/// Содержит значение True, если возвращается тип XML и этот тип гарантированно представляет собой XML-документ
		/// В противном случае содержит False.
		/// </summary>
		public bool SuggestedIsXmlDocument { get; private set; }

		/// <summary>
		/// Содержит значение True, если столбец относится к строковому типу с учетом регистра, либо значение False в противном случае
		/// </summary>
		public bool SuggestedIsCaseSensitive { get; private set; }

		/// <summary>
		/// Содержит значение True, если столбец относится к типу CLR с фиксированной длиной, либо значение False в противном случае
		/// </summary>
		public bool SuggestedIsFixedLengthClrType { get; private set; }

		/// <summary>
		/// Содержит значение True, если параметр используется за пределами левой стороны присваивания.
		/// В противном случае возвращается False
		/// </summary>
		public bool SuggestedIsInput { get; private set; }

		/// <summary>
		/// Содержит значение True, если параметр используется в левой стороне присваивания или передается в выходной параметр
		/// хранимой процедуры. В противном случае возвращается False
		/// </summary>
		public bool SuggestedIsOutput { get; private set; }

		/// <summary>
		/// Если параметр служит аргументом хранимой процедуры или определяемой пользователем функции, здесь содержится имя
		/// соответствующего формального параметра. В противном случае содержится NULL.
		/// </summary>
		public string FormalParameterName { get; private set; }

		/// <inheritdoc />
		private UndeclaredParameterDescription()
		{
		}

		public static UndeclaredParameterDescription FromDataRecord(IDataRecord dataRecord)
		{
			int index = 0;

			T Read<T>()
			{
				if (dataRecord.IsDBNull(index))
				{
					index++;
					return default(T);
				}

				return (T)dataRecord.GetValue(index++);
			}

			var result = new UndeclaredParameterDescription
			{
				ParameterOrdinal = Read<int>(),
				Name = Read<string>(),
				SuggestedSystemTypeId = Read<int>(),
				SuggestedSystemTypeName = Read<string>(),
				SuggestedMaxLength = Read<short>(),
				SuggestedPrecision = Read<byte>(),
				SuggestedScale = Read<byte>(),
				SuggestedUserTypeId = Read<int?>(),
				SuggestedUserTypeDatabase = Read<string>(),
				SuggestedUserTypeSchema = Read<string>(),
				SuggestedUserTypeName = Read<string>(),
				SuggestedAssemblyQualifiedTypeName = Read<string>(),
				SuggestedXmlCollectionId = Read<int?>(),
				SuggestedXmlCollectionDatabase = Read<string>(),
				SuggestedXmlCollectionSchema = Read<string>(),
				SuggestedXmlCollectionName = Read<string>(),
				SuggestedIsXmlDocument = Read<bool>(),
				SuggestedIsCaseSensitive = Read<bool>(),
				SuggestedIsFixedLengthClrType = Read<bool>(),
				SuggestedIsInput = Read<bool>(),
				SuggestedIsOutput = Read<bool>(),
				FormalParameterName = Read<string>()
			};

			return result;
		}
	}
}