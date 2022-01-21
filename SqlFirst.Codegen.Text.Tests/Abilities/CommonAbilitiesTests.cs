using System;
using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using MySpecificDatabaseTypes;
using Shouldly;
using SqlFirst.Codegen.Text.QueryObject.Abilities;
using SqlFirst.Codegen.Text.QueryObject.Abilities.Common;
using SqlFirst.Codegen.Text.QueryObject.Data;
using SqlFirst.Core;
using Xunit;

namespace SqlFirst.Codegen.Text.Tests.Abilities
{
	public class CommonAbilitiesTests
	{
		[Fact]
		public void AddSqlConnectionParameterAbility_Test()
		{
			var context = A.Fake<ICodeGenerationContext>(p => p.Strict());

			var providerTypesInfo = A.Fake<IProviderTypesInfo>(p => p.Strict());
			A.CallTo(() => providerTypesInfo.CommandParameterSpecificDbTypePropertyType).Returns(typeof(MySpecificDbType));
			A.CallTo(() => providerTypesInfo.CommandParameterType).Returns(typeof(MySpecificParameterType));
			A.CallTo(() => providerTypesInfo.CommandParameterSpecificDbTypePropertyName).Returns("MySpecificDbTypePropertyName");

			var provider = A.Fake<IDatabaseProvider>(p => p.Strict());
			A.CallTo(() => provider.ProviderTypesInfo).Returns(providerTypesInfo);

			A.CallTo(() => context.DatabaseProvider).Returns(provider);

			var data = A.Dummy<IQueryObjectData>();

			var ability = new AddSqlConnectionParameterAbility();
			IQueryObjectData result = ability.Apply(context, data);

			ability.Name.ShouldBe(KnownAbilityName.AddParameter);
			ability.GetDependencies().ShouldBeEmpty();

			result.Constants.ShouldBeEmpty();
			result.Fields.ShouldBeEmpty();
			result.Nested.ShouldBeEmpty();
			result.Properties.ShouldBeEmpty();

			result.Usings.ShouldNotBeNull();
			result.Usings.Count().ShouldBe(3);
			result.Usings.ShouldContain("System");
			result.Usings.ShouldContain("System.Data");
			result.Usings.ShouldContain("MySpecificDatabaseTypes");

			result.Methods.ShouldNotBeNull();
			result.Methods.Count().ShouldBe(1);
			result.Methods.ShouldContain(@"/// <summary>
/// Добавляет параметр к команде
/// </summary>
/// <param name=""command"">Команда SQL</param>
/// <param name=""parameterType"">Тип параметра</param>
/// <param name=""parameterName"">Имя параметра</param>
/// <param name=""value"">Значение параметра</param>
/// <param name=""length"">Длина параметра</param>
protected virtual void AddParameter(IDbCommand command, MySpecificDbType parameterType, string parameterName, object value, int? length = null)
{
	var parameter = new MySpecificParameterType
	{
		ParameterName = parameterName,
		MySpecificDbTypePropertyName = parameterType,
		Value = value ?? DBNull.Value
	};

	if (length.HasValue && length.Value > 0)
	{
		parameter.Size = length.Value;
	}
	
	command.Parameters.Add(parameter);
}");
		}

		[Fact]
		public void GetQueryTextFromResourceCacheableAbility_Test()
		{
			var context = A.Fake<ICodeGenerationContext>(p => p.Strict());

			var options = A.Fake<IReadOnlyDictionary<string, object>>(p => p.Strict());

			object _;
			A.CallTo(() => options.TryGetValue("QueryName", out _))
			.Returns(true)
			.AssignsOutAndRefParametersLazily((string a, object b) => new object[] { "TestQueryName" });

			A.CallTo(() => options.TryGetValue("QueryText", out _))
			.Returns(true)
			.AssignsOutAndRefParametersLazily((string a, object b) => new object[] { "SomeQueryText" });

			A.CallTo(() => options.TryGetValue("QueryTextRaw", out _))
			.Returns(true)
			.AssignsOutAndRefParametersLazily((string a, object b) => new object[] { "SomeQueryTextRaw" });

			A.CallTo(() => options.TryGetValue("ResourcePath", out _))
			.Returns(true)
			.AssignsOutAndRefParametersLazily((string a, object b) => new object[] { "TestQueryResourcePath" });

			A.CallTo(() => context.Options).Returns(options);

			var data = A.Dummy<IQueryObjectData>();

			var ability = new GetQueryTextFromResourceCacheableWithCheckAbility();
			IQueryObjectData result = ability.Apply(context, data);

			ability.Name.ShouldBe(KnownAbilityName.GetQueryText);
			ability.GetDependencies().ShouldNotBeNull();
			ability.GetDependencies().Count().ShouldBe(1);
			ability.GetDependencies().ShouldContain(KnownAbilityName.ProcessCachedSql);

			result.Constants.ShouldBeEmpty();
			result.Nested.ShouldBeEmpty();
			result.Properties.ShouldBeEmpty();

			result.Fields.ShouldNotBeNull();
			result.Fields.Count().ShouldBe(3);
			result.Fields.ShouldContain("private const int QueryTextChecksum = 22918;");
			result.Fields.ShouldContain("private string _cachedSql;");
			result.Fields.ShouldContain("private readonly object _cachedSqlLocker = new object();");

			result.Usings.ShouldNotBeNull();
			result.Usings.Count().ShouldBe(4);
			result.Usings.ShouldContain("System");
			result.Usings.ShouldContain("System.IO");
			result.Usings.ShouldContain("System.Text");
			result.Usings.ShouldContain("System.Text.RegularExpressions");

			result.Methods.ShouldNotBeNull();
			result.Methods.Count().ShouldBe(2);
			result.Methods.ShouldContain(@"/// <summary>
/// Вычисляет контрольную сумму строки по алгоритму CRC 8
/// </summary>
/// <param name=""data"">Искомая строка</param>
/// <returns>Контрольная сумма строки</returns>
private int CalculateChecksum(string data)
{
	if (string.IsNullOrEmpty(data))
	{
		return 0;
	}

	data = data.Replace(""\r\n"", ""\n"");

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
}");

			result.Methods.ShouldContain(@"/// <summary>
/// Возвращает текст запроса
/// </summary>
/// <returns>Текст запроса</returns>
protected virtual string GetQueryText()
{
	if (_cachedSql == null)
	{
		lock (_cachedSqlLocker)
		{
			using (Stream stream = typeof(TestQueryName).Assembly.GetManifestResourceStream(""TestQueryResourcePath""))
			{
				string sql = new StreamReader(stream ?? throw new InvalidOperationException(""Can not get manifest resource stream."")).ReadToEnd();
				
				if (CalculateChecksum(sql) != QueryTextChecksum)
				{
					throw new Exception($""{GetType().FullName}: query text was changed. Query object must be re-generated."");
				}

				const string sectionRegexPattern = @""--\s*begin\s+[a-zA-Z0-9_]*\s*\r?\n.*?\s*\r?\n\s*--\s*end\s*\r?\n"";
				const RegexOptions regexOptions = RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.Compiled;
				sql = Regex.Replace(sql, sectionRegexPattern, string.Empty, regexOptions);

				_cachedSql = sql;

				ProcessCachedSql(ref _cachedSql);
			}
		}
	}

	return _cachedSql;
}");
		}

		[Fact]
		public void GetQueryTextFromStringAbility_Test()
		{
			var context = A.Fake<ICodeGenerationContext>(p => p.Strict());

			var options = A.Fake<IReadOnlyDictionary<string, object>>(p => p.Strict());

			object _;
			A.CallTo(() => options.TryGetValue("QueryName", out _))
			.Returns(true)
			.AssignsOutAndRefParametersLazily((string a, object b) => new object[] { "TestQueryName" });

			A.CallTo(() => options.TryGetValue("QueryText", out _))
			.Returns(true)
			.AssignsOutAndRefParametersLazily((string a, object b) => new object[] { "TestQueryText" });

			A.CallTo(() => context.Options).Returns(options);

			var data = A.Dummy<IQueryObjectData>();

			var ability = new GetQueryTextFromStringAbility();
			IQueryObjectData result = ability.Apply(context, data);

			ability.Name.ShouldBe(KnownAbilityName.GetQueryText);
			ability.GetDependencies().ShouldBeEmpty();

			result.Constants.ShouldBeEmpty();
			result.Nested.ShouldBeEmpty();
			result.Properties.ShouldBeEmpty();

			result.Fields.ShouldNotBeNull();
			result.Fields.Count().ShouldBe(0);

			result.Usings.ShouldNotBeNull();
			result.Usings.Count().ShouldBe(0);

			result.Methods.ShouldNotBeNull();
			result.Methods.Count().ShouldBe(1);
			result.Methods.ShouldContain(@"/// <summary>
/// Возвращает текст запроса
/// </summary>
/// <returns>Текст запроса</returns>
protected virtual string GetQueryText()
{
	return @""TestQueryText"";
}");
		}

		[Fact]
		public void MapDataRecordToItemAbility_Test()
		{
			var mapper = A.Fake<IDatabaseTypeMapper>(p => p.Strict());
			A.CallTo(() => mapper.MapToClrType("dummyDbType1", false)).Returns(typeof(Guid));
			A.CallTo(() => mapper.MapToClrType("dummyDbType2", true)).Returns(typeof(int?));

			var firstParameter = A.Fake<IFieldDetails>(p => p.Strict());
			A.CallTo(() => firstParameter.ColumnName).Returns("FirstParam");
			A.CallTo(() => firstParameter.DbType).Returns("dummyDbType1");
			A.CallTo(() => firstParameter.AllowDbNull).Returns(false);

			var secondParameter = A.Fake<IFieldDetails>(p => p.Strict());
			A.CallTo(() => secondParameter.ColumnName).Returns("SECOND_Param");
			A.CallTo(() => secondParameter.DbType).Returns("dummyDbType2");
			A.CallTo(() => secondParameter.AllowDbNull).Returns(true);

			var context = A.Fake<ICodeGenerationContext>(p => p.Strict());
			A.CallTo(() => context.TypeMapper).Returns(mapper);
			A.CallTo(() => context.OutgoingParameters).Returns(new[] { firstParameter, secondParameter });

			var options = A.Fake<IReadOnlyDictionary<string, object>>(p => p.Strict());

			object _;
			A.CallTo(() => options.TryGetValue("QueryResultItemName", out _))
			.Returns(true)
			.AssignsOutAndRefParametersLazily((string a, object b) => new object[] { "QueryItemTestName" });

			A.CallTo(() => context.Options).Returns(options);

			var data = A.Dummy<IQueryObjectData>();

			var ability = new MapDataRecordToItemAbility();
			IQueryObjectData result = ability.Apply(context, data);

			ability.Name.ShouldBe(KnownAbilityName.GetItemFromRecord);
			ability.GetDependencies().ShouldBeEmpty();

			result.Constants.ShouldBeEmpty();
			result.Nested.ShouldBeEmpty();
			result.Properties.ShouldBeEmpty();
			result.Fields.ShouldBeEmpty();

			result.Usings.ShouldNotBeNull();
			result.Usings.Count().ShouldBe(2);
			result.Usings.ShouldContain("System");
			result.Usings.ShouldContain("System.Data");

			result.Methods.ShouldNotBeNull();
			result.Methods.Count().ShouldBe(1);
			result.Methods.ShouldContain(@"/// <summary>
/// Возвращает новый элемент, заполненный данными из <paramref name=""record""/>
/// </summary>
/// <param name=""record"">Строка БД</param>
/// <returns>Новый элемент, заполненный данными из <paramref name=""record""/></returns>
protected virtual QueryItemTestName GetItemFromRecord(IDataRecord record)
{
	var result = new QueryItemTestName();

	if (record[0] != null && record[0] != DBNull.Value)
	{
		result.FirstParam = (Guid)record[0];
	}
	if (record[1] != null && record[1] != DBNull.Value)
	{
		result.SecondParam = (int?)record[1];
	}

	result.AfterLoad();
	return result;
}");
		}

		[Fact]
		public void MapDataRecordToScalarAbility_Test()
		{
			var context = A.Fake<ICodeGenerationContext>(p => p.Strict());

			var data = A.Dummy<IQueryObjectData>();

			var ability = new MapDataRecordToScalarAbility();
			IQueryObjectData result = ability.Apply(context, data);

			ability.Name.ShouldBe(KnownAbilityName.GetScalarFromRecord);
			ability.GetDependencies().ShouldNotBeNull();
			ability.GetDependencies().Count().ShouldBe(1);
			ability.GetDependencies().ShouldContain(KnownAbilityName.GetScalarFromValue);

			result.Constants.ShouldBeEmpty();
			result.Nested.ShouldBeEmpty();
			result.Properties.ShouldBeEmpty();
			result.Fields.ShouldBeEmpty();

			result.Usings.ShouldNotBeNull();
			result.Usings.Count().ShouldBe(2);
			result.Usings.ShouldContain("System");
			result.Usings.ShouldContain("System.Data");

			result.Methods.ShouldNotBeNull();
			result.Methods.Count().ShouldBe(1);
			result.Methods.ShouldContain(@"/// <summary>
/// Возвращает значение из первого столбца <paramref name=""record""/>
/// </summary>
/// <typeparam name=""T"">Тип значения</typeparam>
/// <param name=""record"">Строка БД</param>
/// <returns>Значение из первого столбца <paramref name=""record""/></returns>
protected virtual T GetScalarFromRecord<T>(IDataRecord record)
{
	if (record.FieldCount < 1)
	{
		throw new Exception(""Data record contain no values."");
	}

	object valueObject = record[0];

	return GetScalarFromValue<T>(valueObject);	
}");
		}

		[Fact]
		public void MapValueToScalarAbility_Test()
		{
			var context = A.Fake<ICodeGenerationContext>(p => p.Strict());

			var data = A.Dummy<IQueryObjectData>();

			var ability = new MapValueToScalarAbility();
			IQueryObjectData result = ability.Apply(context, data);

			ability.Name.ShouldBe(KnownAbilityName.GetScalarFromValue);

			ability.GetDependencies().ShouldBeEmpty();
			result.Constants.ShouldBeEmpty();
			result.Nested.ShouldBeEmpty();
			result.Properties.ShouldBeEmpty();
			result.Fields.ShouldBeEmpty();

			result.Usings.ShouldNotBeNull();
			result.Usings.Count().ShouldBe(2);
			result.Usings.ShouldContain("System");
			result.Usings.ShouldContain("System.Data");

			result.Methods.ShouldNotBeNull();
			result.Methods.Count().ShouldBe(1);
			result.Methods.ShouldContain(@"/// <summary>
/// Конвертирует значение <paramref name=""valueObject""/> в <typeparamref name=""T""/>
/// </summary>
/// <typeparam name=""T"">Тип значения</typeparam>
/// <param name=""valueObject"">Строка БД</param>
/// <returns>Значение <paramref name=""valueObject""/>, сконвертированное в <typeparamref name=""T""/></returns>
protected virtual T GetScalarFromValue<T>(object valueObject)
{
	switch (valueObject)
	{
		case null:
		// ReSharper disable once UnusedVariable
		case DBNull dbNull:
			return default(T);

		case T value:
			return value;

		case IConvertible convertible:
			return (T)Convert.ChangeType(convertible, typeof(T));

		default:
			// ReSharper disable once ConstantConditionalAccessQualifier
			throw new InvalidCastException($""Can not convert {valueObject?.GetType().FullName ?? ""null""} to {typeof(T).FullName}"");
	}
}");
		}
	}
}