using System;
using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using Shouldly;
using SqlFirst.Codegen.Text.QueryObject.Abilities;
using SqlFirst.Codegen.Text.QueryObject.Abilities.Common;
using SqlFirst.Codegen.Text.QueryObject.Data;
using SqlFirst.Codegen.Text.Tests.Fixtures;
using SqlFirst.Core;
using Xunit;

namespace SqlFirst.Codegen.Text.Tests.Abilities
{
	public class GetDataTableAbilityTests
	{
		[Fact]
		public void GetDataTableAbility_IsTableType_NonNullable_SingleIntColumn_NonNullable_Test()
		{
			var typeMapper = A.Fake<IDatabaseTypeMapper>(p => p.Strict());
			A.CallTo(() => typeMapper.MapToClrType("int", false, A<IDictionary<string, object>>._)).Returns(typeof(int));

			var context = A.Fake<ICodeGenerationContext>(p => p.Strict());
			A.CallTo(() => context.TypeMapper).Returns(typeMapper);

			var data = A.Dummy<IQueryObjectData>();

			const string dbType = "SomeCustomTableType";

			var complexTypeData = A.Fake<IComplexTypeData>(p => p.Strict());
			A.CallTo(() => complexTypeData.IsTableType).Returns(true);
			A.CallTo(() => complexTypeData.DbTypeDisplayedName).Returns(dbType);
			A.CallTo(() => complexTypeData.AllowNull).Returns(false);

			IEnumerable<IFieldDetails> complexTypeDataFields = new[]
			{
				GetField(
					dbType: "int",
					columnName: "some_int_column",
					allowNull: false,
					ordinal: 0)
			};

			A.CallTo(() => complexTypeData.Fields).Returns(complexTypeDataFields);

			var ability = new GetDataTableAbility(dbType, complexTypeData);
			IQueryObjectData result = ability.Apply(context, data);

			ability.Name.ShouldBe(KnownAbilityName.GetDataTable(dbType));
			ability.GetDependencies(context).ShouldBeEmpty();

			result.Constants.ShouldBeEmpty();
			result.Fields.ShouldBeEmpty();
			result.Nested.ShouldBeEmpty();
			result.Properties.ShouldBeEmpty();

			result.Usings.ShouldNotBeNull();
			result.Usings.Count().ShouldBe(3);
			result.Usings.ShouldContain("System");
			result.Usings.ShouldContain("System.Data");
			result.Usings.ShouldContain("System.Collections.Generic");

			result.Methods.ShouldNotBeNull();
			result.Methods.Count().ShouldBe(1);

			result.Methods.ShouldContain(AbilityArtifacts.GetDataTable.GetDataTableAbility_IsTableType_NonNullable_SingleIntColumn_NonNullable_Test);
		}

		[Fact]
		public void GetDataTableAbility_IsTableType_Nullable_SingleIntColumn_NonNullable_Test()
		{
			var typeMapper = A.Fake<IDatabaseTypeMapper>(p => p.Strict());
			A.CallTo(() => typeMapper.MapToClrType("int", false, A<IDictionary<string, object>>._)).Returns(typeof(int));

			var context = A.Fake<ICodeGenerationContext>(p => p.Strict());
			A.CallTo(() => context.TypeMapper).Returns(typeMapper);

			var data = A.Dummy<IQueryObjectData>();

			const string dbType = "SomeCustomTableType";

			var complexTypeData = A.Fake<IComplexTypeData>(p => p.Strict());
			A.CallTo(() => complexTypeData.IsTableType).Returns(true);
			A.CallTo(() => complexTypeData.DbTypeDisplayedName).Returns(dbType);
			A.CallTo(() => complexTypeData.AllowNull).Returns(true);

			IEnumerable<IFieldDetails> complexTypeDataFields = new[]
			{
				GetField(
					dbType: "int",
					columnName: "some_int_column",
					allowNull: false,
					ordinal: 0)
			};

			A.CallTo(() => complexTypeData.Fields).Returns(complexTypeDataFields);

			var ability = new GetDataTableAbility(dbType, complexTypeData);
			IQueryObjectData result = ability.Apply(context, data);

			ability.Name.ShouldBe(KnownAbilityName.GetDataTable(dbType));
			ability.GetDependencies(context).ShouldBeEmpty();

			result.Constants.ShouldBeEmpty();
			result.Fields.ShouldBeEmpty();
			result.Nested.ShouldBeEmpty();
			result.Properties.ShouldBeEmpty();

			result.Usings.ShouldNotBeNull();
			result.Usings.Count().ShouldBe(3);
			result.Usings.ShouldContain("System");
			result.Usings.ShouldContain("System.Data");
			result.Usings.ShouldContain("System.Collections.Generic");

			result.Methods.ShouldNotBeNull();
			result.Methods.Count().ShouldBe(1);

			result.Methods.ShouldContain(AbilityArtifacts.GetDataTable.GetDataTableAbility_IsTableType_Nullable_SingleIntColumn_NonNullable_Test);
		}

		[Fact]
		public void GetDataTableAbility_IsTableType_Nullable_SingleIntColumn_Nullable_Test()
		{
			var typeMapper = A.Fake<IDatabaseTypeMapper>(p => p.Strict());
			A.CallTo(() => typeMapper.MapToClrType("int", true, A<IDictionary<string, object>>._)).Returns(typeof(int?));

			var context = A.Fake<ICodeGenerationContext>(p => p.Strict());
			A.CallTo(() => context.TypeMapper).Returns(typeMapper);

			var data = A.Dummy<IQueryObjectData>();

			const string dbType = "SomeCustomTableType";

			var complexTypeData = A.Fake<IComplexTypeData>(p => p.Strict());
			A.CallTo(() => complexTypeData.IsTableType).Returns(true);
			A.CallTo(() => complexTypeData.DbTypeDisplayedName).Returns(dbType);
			A.CallTo(() => complexTypeData.AllowNull).Returns(true);

			IEnumerable<IFieldDetails> complexTypeDataFields = new[]
			{
				GetField(
					dbType: "int",
					columnName: "some_int_column",
					allowNull: true,
					ordinal: 0)
			};

			A.CallTo(() => complexTypeData.Fields).Returns(complexTypeDataFields);

			var ability = new GetDataTableAbility(dbType, complexTypeData);
			IQueryObjectData result = ability.Apply(context, data);

			ability.Name.ShouldBe(KnownAbilityName.GetDataTable(dbType));
			ability.GetDependencies(context).ShouldBeEmpty();

			result.Constants.ShouldBeEmpty();
			result.Fields.ShouldBeEmpty();
			result.Nested.ShouldBeEmpty();
			result.Properties.ShouldBeEmpty();

			result.Usings.ShouldNotBeNull();
			result.Usings.Count().ShouldBe(3);
			result.Usings.ShouldContain("System");
			result.Usings.ShouldContain("System.Data");
			result.Usings.ShouldContain("System.Collections.Generic");

			result.Methods.ShouldNotBeNull();
			result.Methods.Count().ShouldBe(1);

			result.Methods.ShouldContain(AbilityArtifacts.GetDataTable.GetDataTableAbility_IsTableType_Nullable_SingleIntColumn_Nullable_Test);
		}

		[Fact]
		public void GetDataTableAbility_IsTableType_Nullable_SingleStringColumn_Nullable_Test()
		{
			var typeMapper = A.Fake<IDatabaseTypeMapper>(p => p.Strict());
			A.CallTo(() => typeMapper.MapToClrType("nvarchar", true, A<IDictionary<string, object>>._)).Returns(typeof(string));

			var context = A.Fake<ICodeGenerationContext>(p => p.Strict());
			A.CallTo(() => context.TypeMapper).Returns(typeMapper);

			var data = A.Dummy<IQueryObjectData>();

			const string dbType = "SomeCustomTableType";

			var complexTypeData = A.Fake<IComplexTypeData>(p => p.Strict());
			A.CallTo(() => complexTypeData.IsTableType).Returns(true);
			A.CallTo(() => complexTypeData.DbTypeDisplayedName).Returns(dbType);
			A.CallTo(() => complexTypeData.AllowNull).Returns(true);

			IEnumerable<IFieldDetails> complexTypeDataFields = new[]
			{
				GetField(
					dbType: "nvarchar",
					columnName: "some_nvarchar_column",
					allowNull: true,
					ordinal: 0)
			};

			A.CallTo(() => complexTypeData.Fields).Returns(complexTypeDataFields);

			var ability = new GetDataTableAbility(dbType, complexTypeData);
			IQueryObjectData result = ability.Apply(context, data);

			ability.Name.ShouldBe(KnownAbilityName.GetDataTable(dbType));
			ability.GetDependencies(context).ShouldBeEmpty();

			result.Constants.ShouldBeEmpty();
			result.Fields.ShouldBeEmpty();
			result.Nested.ShouldBeEmpty();
			result.Properties.ShouldBeEmpty();

			result.Usings.ShouldNotBeNull();
			result.Usings.Count().ShouldBe(3);
			result.Usings.ShouldContain("System");
			result.Usings.ShouldContain("System.Data");
			result.Usings.ShouldContain("System.Collections.Generic");

			result.Methods.ShouldNotBeNull();
			result.Methods.Count().ShouldBe(1);

			result.Methods.ShouldContain(AbilityArtifacts.GetDataTable.GetDataTableAbility_IsTableType_Nullable_SingleStringColumn_Nullable_Test);
		}

		[Fact]
		public void GetDataTableAbility_IsTableType_Nullable_SingleStringColumn_NonNullable_Test()
		{
			var typeMapper = A.Fake<IDatabaseTypeMapper>(p => p.Strict());
			A.CallTo(() => typeMapper.MapToClrType("nvarchar", false, A<IDictionary<string, object>>._)).Returns(typeof(string));

			var context = A.Fake<ICodeGenerationContext>(p => p.Strict());
			A.CallTo(() => context.TypeMapper).Returns(typeMapper);

			var data = A.Dummy<IQueryObjectData>();

			const string dbType = "SomeCustomTableType";

			var complexTypeData = A.Fake<IComplexTypeData>(p => p.Strict());
			A.CallTo(() => complexTypeData.IsTableType).Returns(true);
			A.CallTo(() => complexTypeData.DbTypeDisplayedName).Returns(dbType);
			A.CallTo(() => complexTypeData.AllowNull).Returns(true);

			IEnumerable<IFieldDetails> complexTypeDataFields = new[]
			{
				GetField(
					dbType: "nvarchar",
					columnName: "some_nvarchar_column",
					allowNull: false,
					ordinal: 0)
			};

			A.CallTo(() => complexTypeData.Fields).Returns(complexTypeDataFields);

			var ability = new GetDataTableAbility(dbType, complexTypeData);
			IQueryObjectData result = ability.Apply(context, data);

			ability.Name.ShouldBe(KnownAbilityName.GetDataTable(dbType));
			ability.GetDependencies(context).ShouldBeEmpty();

			result.Constants.ShouldBeEmpty();
			result.Fields.ShouldBeEmpty();
			result.Nested.ShouldBeEmpty();
			result.Properties.ShouldBeEmpty();

			result.Usings.ShouldNotBeNull();
			result.Usings.Count().ShouldBe(3);
			result.Usings.ShouldContain("System");
			result.Usings.ShouldContain("System.Data");
			result.Usings.ShouldContain("System.Collections.Generic");

			result.Methods.ShouldNotBeNull();
			result.Methods.Count().ShouldBe(1);

			result.Methods.ShouldContain(AbilityArtifacts.GetDataTable.GetDataTableAbility_IsTableType_Nullable_SingleStringColumn_NonNullable_Test);
		}

		[Fact]
		public void GetDataTableAbility_IsTableType_Nullable_TwoColumns_BothNonNullable_Test()
		{
			var typeMapper = A.Fake<IDatabaseTypeMapper>(p => p.Strict());
			A.CallTo(() => typeMapper.MapToClrType("nvarchar", false, A<IDictionary<string, object>>._)).Returns(typeof(string));
			A.CallTo(() => typeMapper.MapToClrType("uniqueidentifier", false, A<IDictionary<string, object>>._)).Returns(typeof(Guid));

			var context = A.Fake<ICodeGenerationContext>(p => p.Strict());
			A.CallTo(() => context.TypeMapper).Returns(typeMapper);

			var data = A.Dummy<IQueryObjectData>();

			const string dbType = "SomeCustomTableType";

			var complexTypeData = A.Fake<IComplexTypeData>(p => p.Strict());
			A.CallTo(() => complexTypeData.IsTableType).Returns(true);
			A.CallTo(() => complexTypeData.DbTypeDisplayedName).Returns(dbType);
			A.CallTo(() => complexTypeData.AllowNull).Returns(true);
			A.CallTo(() => complexTypeData.Name).Returns("CustomItem");

			IEnumerable<IFieldDetails> complexTypeDataFields = new[]
			{
				GetField(
					dbType: "nvarchar",
					columnName: "some_nvarchar_column",
					allowNull: false,
					ordinal: 0),

				GetField(
					dbType: "uniqueidentifier",
					columnName: "some_uuid",
					allowNull: false,
					ordinal: 1)
			};

			A.CallTo(() => complexTypeData.Fields).Returns(complexTypeDataFields);

			var ability = new GetDataTableAbility(dbType, complexTypeData);
			IQueryObjectData result = ability.Apply(context, data);

			ability.Name.ShouldBe(KnownAbilityName.GetDataTable(dbType));
			ability.GetDependencies(context).ShouldBeEmpty();

			result.Constants.ShouldBeEmpty();
			result.Fields.ShouldBeEmpty();
			result.Nested.ShouldBeEmpty();
			result.Properties.ShouldBeEmpty();

			result.Usings.ShouldNotBeNull();
			result.Usings.Count().ShouldBe(3);
			result.Usings.ShouldContain("System");
			result.Usings.ShouldContain("System.Data");
			result.Usings.ShouldContain("System.Collections.Generic");

			result.Methods.ShouldNotBeNull();
			result.Methods.Count().ShouldBe(1);

			result.Methods.ShouldContain(AbilityArtifacts.GetDataTable.GetDataTableAbility_IsTableType_Nullable_TwoColumns_BothNonNullable_Test);
		}

		[Fact]
		public void GetDataTableAbility_IsTableType_Nullable_TwoColumns_BothNullable_Test()
		{
			var typeMapper = A.Fake<IDatabaseTypeMapper>(p => p.Strict());
			A.CallTo(() => typeMapper.MapToClrType("nvarchar", true, A<IDictionary<string, object>>._)).Returns(typeof(string));
			A.CallTo(() => typeMapper.MapToClrType("uniqueidentifier", true, A<IDictionary<string, object>>._)).Returns(typeof(Guid?));

			var context = A.Fake<ICodeGenerationContext>(p => p.Strict());
			A.CallTo(() => context.TypeMapper).Returns(typeMapper);

			var data = A.Dummy<IQueryObjectData>();

			const string dbType = "SomeCustomTableType";

			var complexTypeData = A.Fake<IComplexTypeData>(p => p.Strict());
			A.CallTo(() => complexTypeData.IsTableType).Returns(true);
			A.CallTo(() => complexTypeData.DbTypeDisplayedName).Returns(dbType);
			A.CallTo(() => complexTypeData.AllowNull).Returns(true);
			A.CallTo(() => complexTypeData.Name).Returns("CustomItem");

			IEnumerable<IFieldDetails> complexTypeDataFields = new[]
			{
				GetField(
					dbType: "nvarchar",
					columnName: "some_nvarchar_column",
					allowNull: true,
					ordinal: 0),

				GetField(
					dbType: "uniqueidentifier",
					columnName: "some_uuid",
					allowNull: true,
					ordinal: 1)
			};

			A.CallTo(() => complexTypeData.Fields).Returns(complexTypeDataFields);

			var ability = new GetDataTableAbility(dbType, complexTypeData);
			IQueryObjectData result = ability.Apply(context, data);

			ability.Name.ShouldBe(KnownAbilityName.GetDataTable(dbType));
			ability.GetDependencies(context).ShouldBeEmpty();

			result.Constants.ShouldBeEmpty();
			result.Fields.ShouldBeEmpty();
			result.Nested.ShouldBeEmpty();
			result.Properties.ShouldBeEmpty();

			result.Usings.ShouldNotBeNull();
			result.Usings.Count().ShouldBe(3);
			result.Usings.ShouldContain("System");
			result.Usings.ShouldContain("System.Data");
			result.Usings.ShouldContain("System.Collections.Generic");

			result.Methods.ShouldNotBeNull();
			result.Methods.Count().ShouldBe(1);

			result.Methods.ShouldContain(AbilityArtifacts.GetDataTable.GetDataTableAbility_IsTableType_Nullable_TwoColumns_BothNullable_Test);
		}

		[Theory]
		[InlineData(null)]
		[InlineData("")]
		[InlineData(" ")]
		[InlineData("\t")]
		public void GetDataTableAbility_EmptyDbType_Test(string dbType)
		{
			var complexTypeData = A.Fake<IComplexTypeData>(p => p.Strict());

			var exception = Assert.Throws<ArgumentException>(() => new GetDataTableAbility(dbType, complexTypeData));
			exception.ParamName.ShouldBe("dbType");
		}

		[Fact]
		public void GetDataTableAbility_NullComplexType_Test()
		{
			const string dbType = "SomeCustomTableType";

			var exception = Assert.Throws<ArgumentNullException>(() => new GetDataTableAbility(dbType, null));
			exception.ParamName.ShouldBe("complexTypeData");
		}

		[Fact]
		public void GetDataTableAbility_ComplexTypeIsNotTable_Test()
		{
			const string dbType = "SomeCustomTableType";
			var complexTypeData = A.Fake<IComplexTypeData>(p => p.Strict());
			A.CallTo(() => complexTypeData.IsTableType).Returns(false);

			var exception = Assert.Throws<ArgumentException>(() => new GetDataTableAbility(dbType, complexTypeData));
			exception.ParamName.ShouldBe("complexTypeData");
		}

		private static IFieldDetails GetField(string dbType, string columnName, bool allowNull, int ordinal)
		{
			var result = A.Fake<IFieldDetails>(p => p.Strict());
			A.CallTo(() => result.DbType).Returns(dbType);
			A.CallTo(() => result.ColumnName).Returns(columnName);
			A.CallTo(() => result.AllowDbNull).Returns(allowNull);
			A.CallTo(() => result.DbTypeMetadata).Returns(null);
			A.CallTo(() => result.ColumnOrdinal).Returns(ordinal);

			return result;
		}
	}
}