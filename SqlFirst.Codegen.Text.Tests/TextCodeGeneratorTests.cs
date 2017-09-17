using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using FakeItEasy;
using SqlFirst.Codegen.Trees;
using SqlFirst.Core;
using SqlFirst.Core.Parsing;
using Xunit;
using Xunit.Should;

namespace SqlFirst.Codegen.Text.Tests
{
	public class TextCodeGeneratorTests
	{
		#region Fixture
		private const string QueryName = "SelectSomeData";
		private const string MyTestNamespace = "MyTestNamespace";

		private static IDatabaseTypeMapper GetDefaultDatabaseTypeMapper()
		{
			var typeMapper = A.Fake<IDatabaseTypeMapper>(p => p.Strict());
			A.CallTo(() => typeMapper.Map("varchar", true)).Returns(typeof(string));
			A.CallTo(() => typeMapper.Map("int", true)).Returns(typeof(int?));
			A.CallTo(() => typeMapper.Map("bit", false)).Returns(typeof(bool));
			return typeMapper;
		}

		private static IResultGenerationOptions GetDefaultResultGenerationOptions(ResultItemType resultItemType, PropertyType propertyType)
		{
			var options = A.Fake<IResultGenerationOptions>(p => p.Strict());
			A.CallTo(() => options.ItemType).Returns(resultItemType);
			A.CallTo(() => options.PropertyType).Returns(propertyType);
			return options;
		}

		private static ICodeGenerationContext GetDefaultCodeGenerationContext()
		{
			var optionsDictionary = new Dictionary<string, object>
			{
				["Namespace"] = MyTestNamespace,
				["QueryName"] = QueryName
			};

			var context = A.Fake<ICodeGenerationContext>(p => p.Strict());
			A.CallTo(() => context.Options).Returns(optionsDictionary);

			var parameter1 = A.Fake<IFieldDetails>(p => p.Strict());
			A.CallTo(() => parameter1.DbType).Returns("varchar");
			A.CallTo(() => parameter1.AllowDbNull).Returns(true);
			A.CallTo(() => parameter1.ColumnName).Returns("OBJECT_Name");

			var parameter2 = A.Fake<IFieldDetails>(p => p.Strict());
			A.CallTo(() => parameter2.DbType).Returns("int");
			A.CallTo(() => parameter2.AllowDbNull).Returns(true);
			A.CallTo(() => parameter2.ColumnName).Returns("currentStage");

			var parameter3 = A.Fake<IFieldDetails>(p => p.Strict());
			A.CallTo(() => parameter3.DbType).Returns("bit");
			A.CallTo(() => parameter3.AllowDbNull).Returns(false);
			A.CallTo(() => parameter3.ColumnName).Returns("Is_Completed");

			var outgoingParameters = new[] { parameter1, parameter2, parameter3 };

			A.CallTo(() => context.OutgoingParameters).Returns(outgoingParameters);
			return context;
		}
		#endregion

		[Fact]
		public void GenerateResultItemTest_Poco_AutoProperty()
		{
			IDatabaseTypeMapper typeMapper = GetDefaultDatabaseTypeMapper();
			ICodeGenerationContext context = GetDefaultCodeGenerationContext();
			IResultGenerationOptions options = GetDefaultResultGenerationOptions(ResultItemType.Poco, PropertyType.Auto);

			var generator = new TextCodeGenerator(typeMapper);

			IGeneratedResultItem resultItem = generator.GenerateResultItem(context, options);

			resultItem.ItemName.ShouldBe("SelectSomeDataItem");

			resultItem.ItemModifiers.Count().ShouldBe(2);
			resultItem.ItemModifiers.ShouldContain("public");
			resultItem.ItemModifiers.ShouldContain("partial");

			resultItem.Usings.Count().ShouldBe(1);
			resultItem.Usings.ShouldContain("System");

			resultItem.BaseTypes.ShouldBeEmpty();

			resultItem.Item.ShouldBe(
@"public partial class SelectSomeDataItem
{
	public string ObjectName { get; set; }

	public int? CurrentStage { get; set; }

	public bool IsCompleted { get; set; }

	internal void AfterLoad()
	{
		AfterLoadInternal();
	}

	partial void AfterLoadInternal();
}");
		}

		[Fact]
		public void GenerateResultItemTest_Poco_ReadOnlyAutoProperty()
		{
			IDatabaseTypeMapper typeMapper = GetDefaultDatabaseTypeMapper();
			ICodeGenerationContext context = GetDefaultCodeGenerationContext();
			IResultGenerationOptions options = GetDefaultResultGenerationOptions(ResultItemType.Poco, PropertyType.AutoReadOnly);

			var generator = new TextCodeGenerator(typeMapper);

			IGeneratedResultItem resultItem = generator.GenerateResultItem(context, options);

			resultItem.ItemName.ShouldBe("SelectSomeDataItem");

			resultItem.ItemModifiers.Count().ShouldBe(2);
			resultItem.ItemModifiers.ShouldContain("public");
			resultItem.ItemModifiers.ShouldContain("partial");

			resultItem.Usings.Count().ShouldBe(1);
			resultItem.Usings.ShouldContain("System");

			resultItem.BaseTypes.ShouldBeEmpty();

			resultItem.Item.ShouldBe(
@"public partial class SelectSomeDataItem
{
	public string ObjectName { get; internal set; }

	public int? CurrentStage { get; internal set; }

	public bool IsCompleted { get; internal set; }

	internal void AfterLoad()
	{
		AfterLoadInternal();
	}

	partial void AfterLoadInternal();
}");
		}

		[Fact]
		public void GenerateResultItemTest_Poco_BackingFieldProperty()
		{
			IDatabaseTypeMapper typeMapper = GetDefaultDatabaseTypeMapper();
			ICodeGenerationContext context = GetDefaultCodeGenerationContext();
			IResultGenerationOptions options = GetDefaultResultGenerationOptions(ResultItemType.Poco, PropertyType.BackingField);

			var generator = new TextCodeGenerator(typeMapper);

			IGeneratedResultItem resultItem = generator.GenerateResultItem(context, options);

			resultItem.ItemName.ShouldBe("SelectSomeDataItem");

			resultItem.ItemModifiers.Count().ShouldBe(2);
			resultItem.ItemModifiers.ShouldContain("public");
			resultItem.ItemModifiers.ShouldContain("partial");

			resultItem.Usings.Count().ShouldBe(1);
			resultItem.Usings.ShouldContain("System");

			resultItem.BaseTypes.ShouldBeEmpty();

			resultItem.Item.ShouldBe(
@"public partial class SelectSomeDataItem
{
	private string _objectName;
	private int? _currentStage;
	private bool _isCompleted;

	public string ObjectName
	{
		get => _objectName;
		set => _objectName = value;
	}

	public int? CurrentStage
	{
		get => _currentStage;
		set => _currentStage = value;
	}

	public bool IsCompleted
	{
		get => _isCompleted;
		set => _isCompleted = value;
	}

	internal void AfterLoad()
	{
		AfterLoadInternal();
	}

	partial void AfterLoadInternal();
}");
		}

		[Fact]
		public void GenerateResultItemTest_Poco_ReadOnlyBackingFieldProperty()
		{
			IDatabaseTypeMapper typeMapper = GetDefaultDatabaseTypeMapper();
			ICodeGenerationContext context = GetDefaultCodeGenerationContext();
			IResultGenerationOptions options = GetDefaultResultGenerationOptions(ResultItemType.Poco, PropertyType.BackingFieldReadOnly);

			var generator = new TextCodeGenerator(typeMapper);

			IGeneratedResultItem resultItem = generator.GenerateResultItem(context, options);

			resultItem.ItemName.ShouldBe("SelectSomeDataItem");

			resultItem.ItemModifiers.Count().ShouldBe(2);
			resultItem.ItemModifiers.ShouldContain("public");
			resultItem.ItemModifiers.ShouldContain("partial");

			resultItem.Usings.Count().ShouldBe(1);
			resultItem.Usings.ShouldContain("System");

			resultItem.BaseTypes.ShouldBeEmpty();

			resultItem.Item.ShouldBe(
@"public partial class SelectSomeDataItem
{
	private string _objectName;
	private int? _currentStage;
	private bool _isCompleted;

	public string ObjectName
	{
		get => _objectName;
		internal set => _objectName = value;
	}

	public int? CurrentStage
	{
		get => _currentStage;
		internal set => _currentStage = value;
	}

	public bool IsCompleted
	{
		get => _isCompleted;
		internal set => _isCompleted = value;
	}

	internal void AfterLoad()
	{
		AfterLoadInternal();
	}

	partial void AfterLoadInternal();
}");
		}

		[Fact]
		public void GenerateResultItemTest_INPC_BackingFieldProperty()
		{
			IDatabaseTypeMapper typeMapper = GetDefaultDatabaseTypeMapper();
			ICodeGenerationContext context = GetDefaultCodeGenerationContext();
			IResultGenerationOptions options = GetDefaultResultGenerationOptions(ResultItemType.NotifyPropertyChanged, PropertyType.BackingField);

			var generator = new TextCodeGenerator(typeMapper);

			IGeneratedResultItem resultItem = generator.GenerateResultItem(context, options);

			resultItem.ItemName.ShouldBe("SelectSomeDataItem");

			resultItem.ItemModifiers.Count().ShouldBe(2);
			resultItem.ItemModifiers.ShouldContain("public");
			resultItem.ItemModifiers.ShouldContain("partial");

			resultItem.Usings.Count().ShouldBe(2);
			resultItem.Usings.ShouldContain("System");
			resultItem.Usings.ShouldContain("System.ComponentModel");

			resultItem.BaseTypes.Count().ShouldBe(1);
			IGeneratedType type = resultItem.BaseTypes.Single();
			type.GenericArguments.ShouldBeEmpty();
			type.GenericConditions.ShouldBeEmpty();
			type.IsGeneric.ShouldBeFalse();
			type.IsInterface.ShouldBeTrue();
			type.TypeName.ShouldBe(nameof(INotifyPropertyChanged));

			resultItem.Item.ShouldBe(
			#region Too long result
@"public partial class SelectSomeDataItem : INotifyPropertyChanged
{
	private string _objectName;
	private int? _currentStage;
	private bool _isCompleted;

	public string ObjectName
	{
		get => _objectName;
		set
		{
			if (value == _objectName)
			{
				return;
			}
	
			_objectName = value;
			OnPropertyChanged(nameof(ObjectName));
		}
	}

	public int? CurrentStage
	{
		get => _currentStage;
		set
		{
			if (value == _currentStage)
			{
				return;
			}
	
			_currentStage = value;
			OnPropertyChanged(nameof(CurrentStage));
		}
	}

	public bool IsCompleted
	{
		get => _isCompleted;
		set
		{
			if (value == _isCompleted)
			{
				return;
			}
	
			_isCompleted = value;
			OnPropertyChanged(nameof(IsCompleted));
		}
	}

	internal void AfterLoad()
	{
		AfterLoadInternal();
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected virtual void OnPropertyChanged(string propertyName)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}

	partial void AfterLoadInternal();
}"
			#endregion
			);
		}
	}
}
