using System;
using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using Shouldly;
using SqlFirst.Codegen.Text.QueryObject.Abilities;
using SqlFirst.Codegen.Text.QueryObject.Abilities.Common;
using SqlFirst.Codegen.Text.QueryObject.Abilities.Select;
using SqlFirst.Codegen.Text.QueryObject.Data;
using SqlFirst.Core;
using Xunit;

namespace SqlFirst.Codegen.Text.Tests
{
	public class ItemAbilitiesTests
	{
		[Fact]
		public void AddSqlConnectionParameterAbility_Test()
		{
			var context = A.Fake<ICodeGenerationContext>(p => p.Strict());
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
			result.Usings.ShouldContain("System.Data.SqlClient");

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
protected virtual void AddParameter(IDbCommand command, SqlDbType parameterType, string parameterName, object value, int? length = null)
{
	SqlParameter sqlParameter;
	if (length.HasValue && length.Value > 0)
	{
		sqlParameter = new SqlParameter(parameterName, parameterType, length.Value);
	}
	else
	{
		sqlParameter = new SqlParameter(parameterName, parameterType);
	}
	sqlParameter.Value = value ?? DBNull.Value;
	command.Parameters.Add(sqlParameter);
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

			A.CallTo(() => options.TryGetValue("ResourcePath", out _))
				.Returns(true)
				.AssignsOutAndRefParametersLazily((string a, object b) => new object[] { "TestQueryResourcePath" });

			A.CallTo(() => context.Options).Returns(options);

			var data = A.Dummy<IQueryObjectData>();

			var ability = new GetQueryTextFromResourceCacheableAbility();
			IQueryObjectData result = ability.Apply(context, data);

			ability.Name.ShouldBe(KnownAbilityName.GetQueryText);
			ability.GetDependencies().ShouldBeEmpty();

			result.Constants.ShouldBeEmpty();
			result.Nested.ShouldBeEmpty();
			result.Properties.ShouldBeEmpty();

			result.Fields.ShouldNotBeNull();
			result.Fields.Count().ShouldBe(2);
			result.Fields.ShouldContain("private string _cachedSql;");
			result.Fields.ShouldContain("private readonly object _cachedSqlLocker = new object();");

			result.Usings.ShouldNotBeNull();
			result.Usings.Count().ShouldBe(3);
			result.Usings.ShouldContain("System");
			result.Usings.ShouldContain("System.IO");
			result.Usings.ShouldContain("System.Text.RegularExpressions");

			result.Methods.ShouldNotBeNull();
			result.Methods.Count().ShouldBe(1);
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
			if (_cachedSql == null)
			{
				using (Stream stream = typeof(TestQueryName).Assembly.GetManifestResourceStream(""TestQueryResourcePath""))
				{
					string sql = new StreamReader(stream ?? throw new InvalidOperationException(""Can not get manifest resource stream."")).ReadToEnd();

					const string sectionRegexPattern = @""--\s*begin\s+[a-zA-Z0-9_]*\s*\r?\n.*?\s*\r?\n\s*--\s*end\s*\r?\n"";
					const RegexOptions regexOptions = RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.Compiled;
					sql = Regex.Replace(sql, sectionRegexPattern, String.Empty, regexOptions);

					_cachedSql = sql;
				}
			}
		}
	}

	return _cachedSql;
}");
		}

		[Fact]
		public void MapDataRecordToItemAbility_Test()
		{
			var mapper = A.Fake<IDatabaseTypeMapper>(p => p.Strict());
			A.CallTo(() => mapper.Map("dummyDbType1", false)).Returns(typeof(Guid));
			A.CallTo(() => mapper.Map("dummyDbType2", true)).Returns(typeof(int?));

			var firstParameter = A.Fake<IFieldDetails>(p=>p.Strict());
			A.CallTo(() => firstParameter.ColumnName).Returns("FirstParam");
			A.CallTo(() => firstParameter.DbType).Returns("dummyDbType1");
			A.CallTo(() => firstParameter.AllowDbNull).Returns(false);

			var secondParameter = A.Fake<IFieldDetails>(p=>p.Strict());
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
		public void SelectFirstItemAbility_Test()
		{
			var mapper = A.Fake<IDatabaseTypeMapper>(p => p.Strict());
			A.CallTo(() => mapper.Map("uniqueidentifier", true)).Returns(typeof(Guid?));
			A.CallTo(() => mapper.Map("int", true)).Returns(typeof(int?));

			var firstParameter = A.Fake<IQueryParamInfo>(p=>p.Strict());
			A.CallTo(() => firstParameter.DbName).Returns("FirstParam");
			A.CallTo(() => firstParameter.DbType).Returns("uniqueidentifier");

			var secondParameter = A.Fake<IQueryParamInfo>(p=>p.Strict());
			A.CallTo(() => secondParameter.DbName).Returns("SECOND_Param");
			A.CallTo(() => secondParameter.DbType).Returns("int");

			var context = A.Fake<ICodeGenerationContext>(p => p.Strict());
			A.CallTo(() => context.TypeMapper).Returns(mapper);
			A.CallTo(() => context.IncomingParameters).Returns(new[] { firstParameter, secondParameter });

			var options = A.Fake<IReadOnlyDictionary<string, object>>(p => p.Strict());

			object _;
			A.CallTo(() => options.TryGetValue("QueryResultItemName", out _))
				.Returns(true)
				.AssignsOutAndRefParametersLazily((string a, object b) => new object[] { "QueryItemTestName" });

			A.CallTo(() => context.Options).Returns(options);

			var data = A.Dummy<IQueryObjectData>();

			var ability = new SelectFirstItemAbility();
			IQueryObjectData result = ability.Apply(context, data);

			ability.Name.ShouldBe("GetFirstItem");

			result.Constants.ShouldBeEmpty();
			result.Nested.ShouldBeEmpty();
			result.Properties.ShouldBeEmpty();
			result.Fields.ShouldBeEmpty();

			ability.GetDependencies().ShouldNotBeNull();
			ability.GetDependencies().Count().ShouldBe(3);
			ability.GetDependencies().ShouldContain(KnownAbilityName.GetQueryText);
			ability.GetDependencies().ShouldContain(KnownAbilityName.AddParameter);
			ability.GetDependencies().ShouldContain(KnownAbilityName.GetItemFromRecord);

			result.Usings.ShouldNotBeNull();
			result.Usings.Count().ShouldBe(2);
			result.Usings.ShouldContain("System");
			result.Usings.ShouldContain("System.Data");

			result.Methods.ShouldNotBeNull();
			result.Methods.Count().ShouldBe(1);
			result.Methods.ShouldContain(@"/// <summary>
/// Выполняет загрузку первого элемента типа <see cref=""QueryItemTestName""/>
/// </summary>
/// <param name=""connection"">Подключение к БД</param>
/// <param name=""firstParam"">FirstParam</param>
/// <param name=""secondParam"">SECOND_Param</param>
/// <returns>Первый элемент типа <see cref=""QueryItemTestName""/></returns>
public virtual QueryItemTestName GetFirst(IDbConnection connection, Guid? firstParam, int? secondParam)
{
	IDbCommand cmd = connection.CreateCommand();
	cmd.CommandText = GetQueryText();
	AddParameter(cmd, SqlDbType.UniqueIdentifier, ""@FirstParam"", firstParam);
	AddParameter(cmd, SqlDbType.Int, ""@SECOND_Param"", secondParam);

	using (IDataReader reader = cmd.ExecuteReader())
	{
		if (!reader.Read())
		{
			return null;
		}

		return GetItemFromRecord(reader);
	}
}");
		}

		[Fact]
		public void SelectItemsIEnumerableAsyncNestedEnumerableAbility_Test()
		{
			var mapper = A.Fake<IDatabaseTypeMapper>(p => p.Strict());
			A.CallTo(() => mapper.Map("uniqueidentifier", true)).Returns(typeof(Guid?));
			A.CallTo(() => mapper.Map("int", true)).Returns(typeof(int?));

			var firstParameter = A.Fake<IQueryParamInfo>(p=>p.Strict());
			A.CallTo(() => firstParameter.DbName).Returns("FirstParam");
			A.CallTo(() => firstParameter.DbType).Returns("uniqueidentifier");

			var secondParameter = A.Fake<IQueryParamInfo>(p=>p.Strict());
			A.CallTo(() => secondParameter.DbName).Returns("SECOND_Param");
			A.CallTo(() => secondParameter.DbType).Returns("int");

			var context = A.Fake<ICodeGenerationContext>(p => p.Strict());
			A.CallTo(() => context.TypeMapper).Returns(mapper);
			A.CallTo(() => context.IncomingParameters).Returns(new[] { firstParameter, secondParameter });

			var options = A.Fake<IReadOnlyDictionary<string, object>>(p => p.Strict());

			object _;
			A.CallTo(() => options.TryGetValue("QueryResultItemName", out _))
				.Returns(true)
				.AssignsOutAndRefParametersLazily((string a, object b) => new object[] { "QueryItemTestName" });

			A.CallTo(() => context.Options).Returns(options);

			var data = A.Dummy<IQueryObjectData>();

			var ability = new SelectItemsIEnumerableAsyncNestedEnumerableAbility();
			IQueryObjectData result = ability.Apply(context, data);

			ability.Name.ShouldBe("GetItemsIEnumerableAsync");

			result.Constants.ShouldBeEmpty();
			result.Nested.ShouldBeEmpty();
			result.Properties.ShouldBeEmpty();
			result.Fields.ShouldBeEmpty();

			ability.GetDependencies().ShouldNotBeNull();
			ability.GetDependencies().Count().ShouldBe(4);
			ability.GetDependencies().ShouldContain(KnownAbilityName.GetQueryText);
			ability.GetDependencies().ShouldContain(KnownAbilityName.AddParameter);
			ability.GetDependencies().ShouldContain(KnownAbilityName.GetItemFromRecord);
			ability.GetDependencies().ShouldContain(KnownAbilityName.AsyncEnumerable);

			result.Usings.ShouldNotBeNull();
			result.Usings.Count().ShouldBe(6);
			result.Usings.ShouldContain("System");
			result.Usings.ShouldContain("System.Data");
			result.Usings.ShouldContain("System.Data.Common");
			result.Usings.ShouldContain("System.Threading");
			result.Usings.ShouldContain("System.Threading.Tasks");
			result.Usings.ShouldContain("System.Collections.Generic");

			result.Methods.ShouldNotBeNull();
			result.Methods.Count().ShouldBe(1);
			result.Methods.ShouldContain(@"/// <summary>
/// Выполняет ленивую загрузку списка элементов типа <see cref=""QueryItemTestName""/>
/// </summary>
/// <param name=""connection"">Подключение к БД</param>
/// <param name=""firstParam"">FirstParam</param>
/// <param name=""secondParam"">SECOND_Param</param>
/// <param name=""cancellationToken"">Токен отмены</param>
/// <returns>Список элементов типа <see cref=""QueryItemTestName""/></returns>
public virtual Task<IEnumerable<QueryItemTestName>> GetAsync(DbConnection connection, Guid? firstParam, int? secondParam, CancellationToken cancellationToken)
{
	DbCommand cmd = connection.CreateCommand();
	cmd.CommandText = GetQueryText();
	AddParameter(cmd, SqlDbType.UniqueIdentifier, ""@FirstParam"", firstParam);
	AddParameter(cmd, SqlDbType.Int, ""@SECOND_Param"", secondParam);

	async Task<IEnumerator<QueryItemTestName>> CreateEnumerator(DbCommand dbCommand)
	{
		DbDataReader reader = await dbCommand.ExecuteReaderAsync(cancellationToken);
		return new DbAsyncEnumerator<QueryItemTestName>(reader, GetItemFromRecord, cancellationToken);
	}

	IEnumerable<QueryItemTestName> enumerable = new Enumerable<QueryItemTestName>(async () => await CreateEnumerator(cmd));
	return Task.FromResult(enumerable);
}");
		}
	}
}
