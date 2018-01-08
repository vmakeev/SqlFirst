using System;
using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using Shouldly;
using SqlFirst.Codegen.Text.QueryObject.Abilities;
using SqlFirst.Codegen.Text.QueryObject.Abilities.Insert;
using SqlFirst.Codegen.Text.QueryObject.Data;
using SqlFirst.Core;
using Xunit;

namespace SqlFirst.Codegen.Text.Tests.Abilities
{
	public class InsertSingleAbilitiesTests
	{
		private static ICodeGenerationContext GetDefaultCodeGenerationContext()
		{
			var mapper = A.Fake<IDatabaseTypeMapper>(p => p.Strict());
			A.CallTo(() => mapper.MapToClrType("uniqueidentifier", true)).Returns(typeof(Guid?));
			A.CallTo(() => mapper.MapToClrType("int", true)).Returns(typeof(int?));
			A.CallTo(() => mapper.MapToClrType("date", false)).Returns(typeof(DateTime));

			var firstParameter = A.Fake<IQueryParamInfo>(p => p.Strict());
			A.CallTo(() => firstParameter.DbName).Returns("FirstParam");
			A.CallTo(() => firstParameter.SemanticName).Returns("FirstParam");
			A.CallTo(() => firstParameter.DbType).Returns("uniqueidentifier");

			var secondParameter = A.Fake<IQueryParamInfo>(p => p.Strict());
			A.CallTo(() => secondParameter.DbName).Returns("SECOND_Param");
			A.CallTo(() => secondParameter.SemanticName).Returns("SECOND_Param");
			A.CallTo(() => secondParameter.DbType).Returns("int");

			var firstResult = A.Fake<IFieldDetails>(p => p.Strict());
			A.CallTo(() => firstResult.ColumnName).Returns("FirstResult");
			A.CallTo(() => firstResult.DbType).Returns("date");
			A.CallTo(() => firstResult.AllowDbNull).Returns(false);

			var context = A.Fake<ICodeGenerationContext>(p => p.Strict());
			A.CallTo(() => context.TypeMapper).Returns(mapper);
			A.CallTo(() => context.IncomingParameters).Returns(new[] { firstParameter, secondParameter });
			A.CallTo(() => context.OutgoingParameters).Returns(new[] { firstResult });

			var options = A.Fake<IReadOnlyDictionary<string, object>>(p => p.Strict());

			object _;
			A.CallTo(() => options.TryGetValue("QueryResultItemName", out _))
			.Returns(true)
			.AssignsOutAndRefParametersLazily((string a, object b) => new object[] { "QueryItemTestName" });

			A.CallTo(() => context.Options).Returns(options);
			return context;
		}

		[Fact]
		public void InsertSingleValuePlainAbility_Test()
		{
			ICodeGenerationContext context = GetDefaultCodeGenerationContext();

			var data = A.Dummy<IQueryObjectData>();

			var ability = new InsertSingleValuePlainAbility();
			IQueryObjectData result = ability.Apply(context, data);

			ability.Name.ShouldBe("InsertSingleValuePlain");

			result.Constants.ShouldBeEmpty();
			result.Nested.ShouldBeEmpty();
			result.Properties.ShouldBeEmpty();
			result.Fields.ShouldBeEmpty();

			ability.GetDependencies().ShouldNotBeNull();
			ability.GetDependencies().Count().ShouldBe(2);
			ability.GetDependencies().ShouldContain(KnownAbilityName.GetQueryText);
			ability.GetDependencies().ShouldContain(KnownAbilityName.AddParameter);

			result.Usings.ShouldNotBeNull();
			result.Usings.Count().ShouldBe(2);
			result.Usings.ShouldContain("System");
			result.Usings.ShouldContain("System.Data");

			result.Methods.ShouldNotBeNull();
			result.Methods.Count().ShouldBe(1);
			result.Methods.ShouldContain(@"/// <summary>
/// Выполняет добавление строки в таблицу
/// </summary>
/// <param name=""connection"">Подключение к БД</param>
/// <param name=""firstParam"">FirstParam</param>
/// <param name=""secondParam"">SECOND_Param</param>
/// <returns>Количество обновленных строк</returns>
public virtual int Add(IDbConnection connection, Guid? firstParam, int? secondParam)
{
	using(IDbCommand cmd = connection.CreateCommand())
	{
		cmd.CommandText = GetQueryText();
		AddParameter(cmd, SqlDbType.UniqueIdentifier, ""@FirstParam"", firstParam);
		AddParameter(cmd, SqlDbType.Int, ""@SECOND_Param"", secondParam);

		return cmd.ExecuteNonQuery();
	}
}");
		}

		[Fact]
		public void InsertSingleValuePlainAsyncAbility_Test()
		{
			ICodeGenerationContext context = GetDefaultCodeGenerationContext();

			var data = A.Dummy<IQueryObjectData>();

			var ability = new InsertSingleValuePlainAsyncAbility();
			IQueryObjectData result = ability.Apply(context, data);

			ability.Name.ShouldBe("InsertSingleValuePlainAsync");

			result.Constants.ShouldBeEmpty();
			result.Nested.ShouldBeEmpty();
			result.Properties.ShouldBeEmpty();
			result.Fields.ShouldBeEmpty();

			ability.GetDependencies().ShouldNotBeNull();
			ability.GetDependencies().Count().ShouldBe(2);
			ability.GetDependencies().ShouldContain(KnownAbilityName.GetQueryText);
			ability.GetDependencies().ShouldContain(KnownAbilityName.AddParameter);

			result.Usings.ShouldNotBeNull();
			result.Usings.Count().ShouldBe(5);
			result.Usings.ShouldContain("System");
			result.Usings.ShouldContain("System.Data");
			result.Usings.ShouldContain("System.Data.Common");
			result.Usings.ShouldContain("System.Threading");
			result.Usings.ShouldContain("System.Threading.Tasks");

			result.Methods.ShouldNotBeNull();
			result.Methods.Count().ShouldBe(1);
			result.Methods.ShouldContain(@"/// <summary>
/// Выполняет добавление строки в таблицу
/// </summary>
/// <param name=""connection"">Подключение к БД</param>
/// <param name=""firstParam"">FirstParam</param>
/// <param name=""secondParam"">SECOND_Param</param>
/// <param name=""cancellationToken"">Токен отмены</param>
/// <returns>Количество обновленных строк</returns>
public virtual async Task<int> AddAsync(DbConnection connection, Guid? firstParam, int? secondParam, CancellationToken cancellationToken)
{
	using (DbCommand cmd = connection.CreateCommand())
	{
		cmd.CommandText = GetQueryText();
		AddParameter(cmd, SqlDbType.UniqueIdentifier, ""@FirstParam"", firstParam);
		AddParameter(cmd, SqlDbType.Int, ""@SECOND_Param"", secondParam);

		return await cmd.ExecuteNonQueryAsync(cancellationToken);
	}
}");
		}

		[Fact]
		public void InsertSingleValuePlainWithResultAbility_Test()
		{
			ICodeGenerationContext context = GetDefaultCodeGenerationContext();

			var data = A.Dummy<IQueryObjectData>();

			var ability = new InsertSingleValuePlainWithResultAbility();
			IQueryObjectData result = ability.Apply(context, data);

			ability.Name.ShouldBe("InsertSingleValuePlain");

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
/// Выполняет добавление строки в таблицу и возвращает дополнительные данные
/// </summary>
/// <param name=""connection"">Подключение к БД</param>
/// <param name=""firstParam"">FirstParam</param>
/// <param name=""secondParam"">SECOND_Param</param>
/// <returns>Результат выполнения запроса</returns>
public virtual QueryItemTestName Add(IDbConnection connection, Guid? firstParam, int? secondParam)
{
	using(IDbCommand cmd = connection.CreateCommand())
	{
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
	}
}");
		}

		[Fact]
		public void InsertSingleValuePlainWithResultAsyncAbility_Test()
		{
			ICodeGenerationContext context = GetDefaultCodeGenerationContext();

			var data = A.Dummy<IQueryObjectData>();

			var ability = new InsertSingleValuePlainWithResultAsyncAbility();
			IQueryObjectData result = ability.Apply(context, data);

			ability.Name.ShouldBe("InsertSingleValuePlainAsync");

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
			result.Usings.Count().ShouldBe(5);
			result.Usings.ShouldContain("System");
			result.Usings.ShouldContain("System.Data");
			result.Usings.ShouldContain("System.Data.Common");
			result.Usings.ShouldContain("System.Threading");
			result.Usings.ShouldContain("System.Threading.Tasks");

			result.Methods.ShouldNotBeNull();
			result.Methods.Count().ShouldBe(1);
			result.Methods.ShouldContain(@"/// <summary>
/// Выполняет добавление строки в таблицу и возвращает дополнительные данные
/// </summary>
/// <param name=""connection"">Подключение к БД</param>
/// <param name=""firstParam"">FirstParam</param>
/// <param name=""secondParam"">SECOND_Param</param>
/// <param name=""cancellationToken"">Токен отмены</param>
/// <returns>Результат выполнения запроса</returns>
public virtual async Task<QueryItemTestName> AddAsync(DbConnection connection, Guid? firstParam, int? secondParam, CancellationToken cancellationToken)
{
	using(DbCommand cmd = connection.CreateCommand())
	{
		cmd.CommandText = GetQueryText();
		AddParameter(cmd, SqlDbType.UniqueIdentifier, ""@FirstParam"", firstParam);
		AddParameter(cmd, SqlDbType.Int, ""@SECOND_Param"", secondParam);

		using (DbDataReader reader = await cmd.ExecuteReaderAsync(cancellationToken))
		{
			if (await reader.ReadAsync(cancellationToken) != true)
			{
				return null;
			}
			
			return GetItemFromRecord(reader);			
		}
	}
}");
		}

		[Fact]
		public void InsertSingleValuePlainWithScalarResultAbility_Test()
		{
			ICodeGenerationContext context = GetDefaultCodeGenerationContext();

			var data = A.Dummy<IQueryObjectData>();

			var ability = new InsertSingleValuePlainWithScalarResultAbility();
			IQueryObjectData result = ability.Apply(context, data);

			ability.Name.ShouldBe("InsertSingleValuePlain");

			result.Constants.ShouldBeEmpty();
			result.Nested.ShouldBeEmpty();
			result.Properties.ShouldBeEmpty();
			result.Fields.ShouldBeEmpty();

			ability.GetDependencies().ShouldNotBeNull();
			ability.GetDependencies().Count().ShouldBe(3);
			ability.GetDependencies().ShouldContain(KnownAbilityName.GetQueryText);
			ability.GetDependencies().ShouldContain(KnownAbilityName.AddParameter);
			ability.GetDependencies().ShouldContain(KnownAbilityName.GetScalarFromValue);

			result.Usings.ShouldNotBeNull();
			result.Usings.Count().ShouldBe(2);
			result.Usings.ShouldContain("System");
			result.Usings.ShouldContain("System.Data");

			result.Methods.ShouldNotBeNull();
			result.Methods.Count().ShouldBe(1);
			result.Methods.ShouldContain(@"/// <summary>
/// Выполняет добавление строки в таблицу и возвращает дополнительные данные
/// </summary>
/// <param name=""connection"">Подключение к БД</param>
/// <param name=""firstParam"">FirstParam</param>
/// <param name=""secondParam"">SECOND_Param</param>
/// <returns>FirstResult</returns>
public virtual DateTime Add(IDbConnection connection, Guid? firstParam, int? secondParam)
{
	using(IDbCommand cmd = connection.CreateCommand())
	{
		cmd.CommandText = GetQueryText();
		AddParameter(cmd, SqlDbType.UniqueIdentifier, ""@FirstParam"", firstParam);
		AddParameter(cmd, SqlDbType.Int, ""@SECOND_Param"", secondParam);
		
		object value = cmd.ExecuteScalar();
		return GetScalarFromValue<DateTime>(value);
	}
}");
		}

		[Fact]
		public void InsertSingleValuePlainWithScalarResultAsyncAbility_Test()
		{
			ICodeGenerationContext context = GetDefaultCodeGenerationContext();

			var data = A.Dummy<IQueryObjectData>();

			var ability = new InsertSingleValuePlainWithScalarResultAsyncAbility();
			IQueryObjectData result = ability.Apply(context, data);

			ability.Name.ShouldBe("InsertSingleValuePlainAsync");

			result.Constants.ShouldBeEmpty();
			result.Nested.ShouldBeEmpty();
			result.Properties.ShouldBeEmpty();
			result.Fields.ShouldBeEmpty();

			ability.GetDependencies().ShouldNotBeNull();
			ability.GetDependencies().Count().ShouldBe(3);
			ability.GetDependencies().ShouldContain(KnownAbilityName.GetQueryText);
			ability.GetDependencies().ShouldContain(KnownAbilityName.AddParameter);
			ability.GetDependencies().ShouldContain(KnownAbilityName.GetScalarFromValue);

			result.Usings.ShouldNotBeNull();
			result.Usings.Count().ShouldBe(5);
			result.Usings.ShouldContain("System");
			result.Usings.ShouldContain("System.Data");
			result.Usings.ShouldContain("System.Data.Common");
			result.Usings.ShouldContain("System.Threading");
			result.Usings.ShouldContain("System.Threading.Tasks");

			result.Methods.ShouldNotBeNull();
			result.Methods.Count().ShouldBe(1);
			result.Methods.ShouldContain(@"/// <summary>
/// Выполняет добавление строки в таблицу и возвращает дополнительные данные
/// </summary>
/// <param name=""connection"">Подключение к БД</param>
/// <param name=""firstParam"">FirstParam</param>
/// <param name=""secondParam"">SECOND_Param</param>
/// <param name=""cancellationToken"">Токен отмены</param>
/// <returns>FirstResult</returns>
public virtual async Task<DateTime> AddAsync(DbConnection connection, Guid? firstParam, int? secondParam, CancellationToken cancellationToken)
{
	using(DbCommand cmd = connection.CreateCommand())
	{
		cmd.CommandText = GetQueryText();
		AddParameter(cmd, SqlDbType.UniqueIdentifier, ""@FirstParam"", firstParam);
		AddParameter(cmd, SqlDbType.Int, ""@SECOND_Param"", secondParam);
		
		object value = await cmd.ExecuteScalarAsync(cancellationToken);
		return GetScalarFromValue<DateTime>(value);
	}
}");
		}
	}
}
