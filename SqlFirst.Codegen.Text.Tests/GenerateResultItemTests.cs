using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using FakeItEasy;
using Shouldly;
using SqlFirst.Codegen.Trees;
using SqlFirst.Core;
using SqlFirst.Core.Impl;
using Xunit;

namespace SqlFirst.Codegen.Text.Tests
{
	public class GenerateResultItemTests
	{
		[Theory]
		[InlineData("generate result class properties auto virtual")]
		[InlineData("generate result class properties virtual")]
		[InlineData("generate result properties virtual")]
		public void GenerateResultItemTest_Poco_AutoProperty_Virtual(string optionsString)
		{
			IDatabaseTypeMapper typeMapper = GetDefaultDatabaseTypeMapper();
			ICodeGenerationContext context = GetDefaultCodeGenerationContext(typeMapper);
			IResultGenerationOptions options = GetDefaultResultGenerationOptions(optionsString);

			var generator = new TextCodeGenerator();

			IGeneratedResultItem resultItem = generator.GenerateResultItem(context, options);

			resultItem.Name.ShouldBe("SelectSomeDataItem");

			resultItem.Modifiers.Count().ShouldBe(2);
			resultItem.Modifiers.ShouldContain("public");
			resultItem.Modifiers.ShouldContain("partial");

			resultItem.Usings.Count().ShouldBe(1);
			resultItem.Usings.ShouldContain("System");

			resultItem.BaseTypes.ShouldBeEmpty();

			resultItem.Content.ShouldBe(
				@"public virtual string ObjectName { get; set; }

public virtual int? CurrentStage { get; set; }

public virtual bool IsCompleted { get; set; }

internal void AfterLoad()
{
	AfterLoadInternal();
}

partial void AfterLoadInternal();");
		}

		[Theory]
		[InlineData("generate result class properties auto virtual readonly")]
		[InlineData("generate result class immutable properties auto virtual")]
		[InlineData("generate result class properties virtual readonly")]
		[InlineData("generate result properties virtual readonly")]
		[InlineData("generate result immutable properties virtual readonly")]
		[InlineData("generate result immutable properties virtual")]
		[InlineData("generate result properties auto virtual readonly")]
		[InlineData("generate result immutable properties auto virtual readonly")]
		[InlineData("generate result immutable properties auto virtual")]
		public void GenerateResultItemTest_Poco_AutoProperty_ReadOnly_Virtual(string optionsString)
		{
			IDatabaseTypeMapper typeMapper = GetDefaultDatabaseTypeMapper();
			ICodeGenerationContext context = GetDefaultCodeGenerationContext(typeMapper);
			IResultGenerationOptions options = GetDefaultResultGenerationOptions(optionsString);

			var generator = new TextCodeGenerator();

			IGeneratedResultItem resultItem = generator.GenerateResultItem(context, options);

			resultItem.Name.ShouldBe("SelectSomeDataItem");

			resultItem.Modifiers.Count().ShouldBe(2);
			resultItem.Modifiers.ShouldContain("public");
			resultItem.Modifiers.ShouldContain("partial");

			resultItem.Usings.Count().ShouldBe(1);
			resultItem.Usings.ShouldContain("System");

			resultItem.BaseTypes.ShouldBeEmpty();

			resultItem.Content.ShouldBe(
				@"public virtual string ObjectName { get; internal set; }

public virtual int? CurrentStage { get; internal set; }

public virtual bool IsCompleted { get; internal set; }

internal void AfterLoad()
{
	AfterLoadInternal();
}

partial void AfterLoadInternal();");
		}

		[Theory]
		[InlineData("generate result class properties backingField virtual")]
		[InlineData("generate result properties backingField virtual")]
		[InlineData("generate result properties virtual backingField")]
		public void GenerateResultItemTest_Poco_BackingFieldProperty_Virtual(string optionsString)
		{
			IDatabaseTypeMapper typeMapper = GetDefaultDatabaseTypeMapper();
			ICodeGenerationContext context = GetDefaultCodeGenerationContext(typeMapper);
			IResultGenerationOptions options = GetDefaultResultGenerationOptions(optionsString);

			var generator = new TextCodeGenerator();

			IGeneratedResultItem resultItem = generator.GenerateResultItem(context, options);

			resultItem.Name.ShouldBe("SelectSomeDataItem");

			resultItem.Modifiers.Count().ShouldBe(2);
			resultItem.Modifiers.ShouldContain("public");
			resultItem.Modifiers.ShouldContain("partial");

			resultItem.Usings.Count().ShouldBe(1);
			resultItem.Usings.ShouldContain("System");

			resultItem.BaseTypes.ShouldBeEmpty();

			resultItem.Content.ShouldBe(
				@"private string _objectName;
private int? _currentStage;
private bool _isCompleted;

public virtual string ObjectName
{
	get => _objectName;
	set => _objectName = value;
}

public virtual int? CurrentStage
{
	get => _currentStage;
	set => _currentStage = value;
}

public virtual bool IsCompleted
{
	get => _isCompleted;
	set => _isCompleted = value;
}

internal void AfterLoad()
{
	AfterLoadInternal();
}

partial void AfterLoadInternal();");
		}

		[Theory]
		[InlineData("generate result class properties backingField virtual readonly")]
		[InlineData("generate result properties backingField virtual readonly")]
		[InlineData("generate result properties readonly backingField virtual ")]
		public void GenerateResultItemTest_Poco_BackingFieldProperty_ReadOnly_Virtual(string optionsString)
		{
			IDatabaseTypeMapper typeMapper = GetDefaultDatabaseTypeMapper();
			ICodeGenerationContext context = GetDefaultCodeGenerationContext(typeMapper);
			IResultGenerationOptions options = GetDefaultResultGenerationOptions(optionsString);

			var generator = new TextCodeGenerator();

			IGeneratedResultItem resultItem = generator.GenerateResultItem(context, options);

			resultItem.Name.ShouldBe("SelectSomeDataItem");

			resultItem.Modifiers.Count().ShouldBe(2);
			resultItem.Modifiers.ShouldContain("public");
			resultItem.Modifiers.ShouldContain("partial");

			resultItem.Usings.Count().ShouldBe(1);
			resultItem.Usings.ShouldContain("System");

			resultItem.BaseTypes.ShouldBeEmpty();

			resultItem.Content.ShouldBe(
				@"private string _objectName;
private int? _currentStage;
private bool _isCompleted;

public virtual string ObjectName
{
	get => _objectName;
	internal set => _objectName = value;
}

public virtual int? CurrentStage
{
	get => _currentStage;
	internal set => _currentStage = value;
}

public virtual bool IsCompleted
{
	get => _isCompleted;
	internal set => _isCompleted = value;
}

internal void AfterLoad()
{
	AfterLoadInternal();
}

partial void AfterLoadInternal();");
		}

		//------------------------------------------------------------------

		[Theory]
		[InlineData("generate result class properties auto")]
		[InlineData("generate result")]
		[InlineData("generate")]
		[InlineData("")]
		[InlineData("generate result properties auto")]
		[InlineData("generate result properties")]
		public void GenerateResultItemTest_Poco_AutoProperty(string optionsString)
		{
			IDatabaseTypeMapper typeMapper = GetDefaultDatabaseTypeMapper();
			ICodeGenerationContext context = GetDefaultCodeGenerationContext(typeMapper);
			IResultGenerationOptions options = GetDefaultResultGenerationOptions(optionsString);

			var generator = new TextCodeGenerator();

			IGeneratedResultItem resultItem = generator.GenerateResultItem(context, options);

			resultItem.Name.ShouldBe("SelectSomeDataItem");

			resultItem.Modifiers.Count().ShouldBe(2);
			resultItem.Modifiers.ShouldContain("public");
			resultItem.Modifiers.ShouldContain("partial");

			resultItem.Usings.Count().ShouldBe(1);
			resultItem.Usings.ShouldContain("System");

			resultItem.BaseTypes.ShouldBeEmpty();

			resultItem.Content.ShouldBe(
				@"public string ObjectName { get; set; }

public int? CurrentStage { get; set; }

public bool IsCompleted { get; set; }

internal void AfterLoad()
{
	AfterLoadInternal();
}

partial void AfterLoadInternal();");
		}

		[Theory]
		[InlineData("generate result class properties readonly auto")]
		[InlineData("generate result class properties readonly")]
		[InlineData("generate result properties readonly auto")]
		[InlineData("generate result properties readonly")]
		public void GenerateResultItemTest_Poco_AutoProperty_ReadOnly(string optionsString)
		{
			IDatabaseTypeMapper typeMapper = GetDefaultDatabaseTypeMapper();
			ICodeGenerationContext context = GetDefaultCodeGenerationContext(typeMapper);
			IResultGenerationOptions options = GetDefaultResultGenerationOptions(optionsString);

			var generator = new TextCodeGenerator();

			IGeneratedResultItem resultItem = generator.GenerateResultItem(context, options);

			resultItem.Name.ShouldBe("SelectSomeDataItem");

			resultItem.Modifiers.Count().ShouldBe(2);
			resultItem.Modifiers.ShouldContain("public");
			resultItem.Modifiers.ShouldContain("partial");

			resultItem.Usings.Count().ShouldBe(1);
			resultItem.Usings.ShouldContain("System");

			resultItem.BaseTypes.ShouldBeEmpty();

			resultItem.Content.ShouldBe(
				@"public string ObjectName { get; internal set; }

public int? CurrentStage { get; internal set; }

public bool IsCompleted { get; internal set; }

internal void AfterLoad()
{
	AfterLoadInternal();
}

partial void AfterLoadInternal();");
		}

		[Theory]
		[InlineData("generate result class properties BackingField")]
		[InlineData("generate result properties backingfield")]
		public void GenerateResultItemTest_Poco_BackingFieldProperty(string optionsString)
		{
			IDatabaseTypeMapper typeMapper = GetDefaultDatabaseTypeMapper();
			ICodeGenerationContext context = GetDefaultCodeGenerationContext(typeMapper);
			IResultGenerationOptions options = GetDefaultResultGenerationOptions(optionsString);

			var generator = new TextCodeGenerator();

			IGeneratedResultItem resultItem = generator.GenerateResultItem(context, options);

			resultItem.Name.ShouldBe("SelectSomeDataItem");

			resultItem.Modifiers.Count().ShouldBe(2);
			resultItem.Modifiers.ShouldContain("public");
			resultItem.Modifiers.ShouldContain("partial");

			resultItem.Usings.Count().ShouldBe(1);
			resultItem.Usings.ShouldContain("System");

			resultItem.BaseTypes.ShouldBeEmpty();

			resultItem.Content.ShouldBe(
				@"private string _objectName;
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

partial void AfterLoadInternal();");
		}

		[Theory]
		[InlineData("generate result class properties BackingField ReadOnly")]
		[InlineData("generate result properties BackingField ReadOnly")]
		public void GenerateResultItemTest_Poco_BackingFieldProperty_ReadOnly(string optionsString)
		{
			IDatabaseTypeMapper typeMapper = GetDefaultDatabaseTypeMapper();
			ICodeGenerationContext context = GetDefaultCodeGenerationContext(typeMapper);
			IResultGenerationOptions options = GetDefaultResultGenerationOptions(optionsString);

			var generator = new TextCodeGenerator();

			IGeneratedResultItem resultItem = generator.GenerateResultItem(context, options);

			resultItem.Name.ShouldBe("SelectSomeDataItem");

			resultItem.Modifiers.Count().ShouldBe(2);
			resultItem.Modifiers.ShouldContain("public");
			resultItem.Modifiers.ShouldContain("partial");

			resultItem.Usings.Count().ShouldBe(1);
			resultItem.Usings.ShouldContain("System");

			resultItem.BaseTypes.ShouldBeEmpty();

			resultItem.Content.ShouldBe(
				@"private string _objectName;
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

partial void AfterLoadInternal();");
		}

		//------------------------------------------------------------------

		[Theory]
		[InlineData("generate result class inpc properties BackingField Virtual")]
		[InlineData("generate result inpc properties BackingField Virtual")]
		[InlineData("generate result  properties BackingField inpc Virtual")]
		public void GenerateResultItemTest_INPC_BackingFieldProperty_Virtual(string optionsString)
		{
			IDatabaseTypeMapper typeMapper = GetDefaultDatabaseTypeMapper();
			ICodeGenerationContext context = GetDefaultCodeGenerationContext(typeMapper);
			IResultGenerationOptions options = GetDefaultResultGenerationOptions(optionsString);

			var generator = new TextCodeGenerator();

			IGeneratedResultItem resultItem = generator.GenerateResultItem(context, options);

			resultItem.Name.ShouldBe("SelectSomeDataItem");

			resultItem.Modifiers.Count().ShouldBe(2);
			resultItem.Modifiers.ShouldContain("public");
			resultItem.Modifiers.ShouldContain("partial");

			resultItem.Usings.Count().ShouldBe(2);
			resultItem.Usings.ShouldContain("System");
			resultItem.Usings.ShouldContain("System.ComponentModel");

			resultItem.BaseTypes.Count().ShouldBe(1);
			IGeneratedType type = resultItem.BaseTypes.Single();
			type.GenericArguments.ShouldBeEmpty();
			type.IsGeneric.ShouldBeFalse();
			type.IsInterface.ShouldBeTrue();
			type.Name.ShouldBe(nameof(INotifyPropertyChanged));

			resultItem.Content.ShouldBe(

				#region Too long result

				@"private string _objectName;
private int? _currentStage;
private bool _isCompleted;

public virtual string ObjectName
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

public virtual int? CurrentStage
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

public virtual bool IsCompleted
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

protected void OnPropertyChanged(string propertyName)
{
	PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}

partial void AfterLoadInternal();"

				#endregion

			);
		}

		[Theory]
		[InlineData("generate result class inpc properties readonly backingfield virtual")]
		[InlineData("generate result class  properties inpc readonly backingfield virtual")]
		[InlineData("generate result inpc properties readonly backingfield virtual")]
		[InlineData("generate result  properties readonly inpc backingfield virtual")]
		public void GenerateResultItemTest_INPC_BackingFieldProperty_ReadOnly_Virtual(string optionsString)
		{
			IDatabaseTypeMapper typeMapper = GetDefaultDatabaseTypeMapper();
			ICodeGenerationContext context = GetDefaultCodeGenerationContext(typeMapper);
			IResultGenerationOptions options = GetDefaultResultGenerationOptions(optionsString);

			var generator = new TextCodeGenerator();

			IGeneratedResultItem resultItem = generator.GenerateResultItem(context, options);

			resultItem.Name.ShouldBe("SelectSomeDataItem");

			resultItem.Modifiers.Count().ShouldBe(2);
			resultItem.Modifiers.ShouldContain("public");
			resultItem.Modifiers.ShouldContain("partial");

			resultItem.Usings.Count().ShouldBe(2);
			resultItem.Usings.ShouldContain("System");
			resultItem.Usings.ShouldContain("System.ComponentModel");

			resultItem.BaseTypes.Count().ShouldBe(1);
			IGeneratedType type = resultItem.BaseTypes.Single();
			type.GenericArguments.ShouldBeEmpty();
			type.IsGeneric.ShouldBeFalse();
			type.IsInterface.ShouldBeTrue();
			type.Name.ShouldBe(nameof(INotifyPropertyChanged));

			resultItem.Content.ShouldBe(

				#region Too long result

				@"private string _objectName;
private int? _currentStage;
private bool _isCompleted;

public virtual string ObjectName
{
	get => _objectName;
	internal set
	{
		if (value == _objectName)
		{
			return;
		}

		_objectName = value;
		OnPropertyChanged(nameof(ObjectName));
	}
}

public virtual int? CurrentStage
{
	get => _currentStage;
	internal set
	{
		if (value == _currentStage)
		{
			return;
		}

		_currentStage = value;
		OnPropertyChanged(nameof(CurrentStage));
	}
}

public virtual bool IsCompleted
{
	get => _isCompleted;
	internal set
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

protected void OnPropertyChanged(string propertyName)
{
	PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}

partial void AfterLoadInternal();"

				#endregion

			);
		}

		[Theory]
		[InlineData("generate result class inpc properties BackingField")]
		[InlineData("generate result class properties BackingField inpc")]
		[InlineData("generate result inpc properties BackingField")]
		[InlineData("generate result properties inpc BackingField")]
		public void GenerateResultItemTest_INPC_BackingFieldProperty(string optionsString)
		{
			IDatabaseTypeMapper typeMapper = GetDefaultDatabaseTypeMapper();
			ICodeGenerationContext context = GetDefaultCodeGenerationContext(typeMapper);
			IResultGenerationOptions options = GetDefaultResultGenerationOptions(optionsString);

			var generator = new TextCodeGenerator();

			IGeneratedResultItem resultItem = generator.GenerateResultItem(context, options);

			resultItem.Name.ShouldBe("SelectSomeDataItem");

			resultItem.Modifiers.Count().ShouldBe(2);
			resultItem.Modifiers.ShouldContain("public");
			resultItem.Modifiers.ShouldContain("partial");

			resultItem.Usings.Count().ShouldBe(2);
			resultItem.Usings.ShouldContain("System");
			resultItem.Usings.ShouldContain("System.ComponentModel");

			resultItem.BaseTypes.Count().ShouldBe(1);
			IGeneratedType type = resultItem.BaseTypes.Single();
			type.GenericArguments.ShouldBeEmpty();
			type.IsGeneric.ShouldBeFalse();
			type.IsInterface.ShouldBeTrue();
			type.Name.ShouldBe(nameof(INotifyPropertyChanged));

			resultItem.Content.ShouldBe(

				#region Too long result

				@"private string _objectName;
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

protected void OnPropertyChanged(string propertyName)
{
	PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}

partial void AfterLoadInternal();"

				#endregion

			);
		}

		[Theory]
		[InlineData("generate result class inpc properties ReadOnly BackingField")]
		[InlineData("generate result class  properties ReadOnly inpc BackingField")]
		[InlineData("generate result inpc properties ReadOnly BackingField")]
		[InlineData("generate result properties inpc ReadOnly BackingField")]
		public void GenerateResultItemTest_INPC_BackingFieldProperty_ReadOnly(string optionsString)
		{
			IDatabaseTypeMapper typeMapper = GetDefaultDatabaseTypeMapper();
			ICodeGenerationContext context = GetDefaultCodeGenerationContext(typeMapper);
			IResultGenerationOptions options = GetDefaultResultGenerationOptions(optionsString);

			var generator = new TextCodeGenerator();

			IGeneratedResultItem resultItem = generator.GenerateResultItem(context, options);

			resultItem.Name.ShouldBe("SelectSomeDataItem");

			resultItem.Modifiers.Count().ShouldBe(2);
			resultItem.Modifiers.ShouldContain("public");
			resultItem.Modifiers.ShouldContain("partial");

			resultItem.Usings.Count().ShouldBe(2);
			resultItem.Usings.ShouldContain("System");
			resultItem.Usings.ShouldContain("System.ComponentModel");

			resultItem.BaseTypes.Count().ShouldBe(1);
			IGeneratedType type = resultItem.BaseTypes.Single();
			type.GenericArguments.ShouldBeEmpty();
			type.IsGeneric.ShouldBeFalse();
			type.IsInterface.ShouldBeTrue();
			type.Name.ShouldBe(nameof(INotifyPropertyChanged));

			resultItem.Content.ShouldBe(

				#region Too long result

				@"private string _objectName;
private int? _currentStage;
private bool _isCompleted;

public string ObjectName
{
	get => _objectName;
	internal set
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
	internal set
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
	internal set
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

protected void OnPropertyChanged(string propertyName)
{
	PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}

partial void AfterLoadInternal();"

				#endregion

			);
		}

		//------------------------------------------------------------------

		[Theory]
		[InlineData("generate result struct properties auto")]
		[InlineData("generate result struct properties")]
		[InlineData("generate result struct")]
		public void GenerateResultItemTest_Struct_AutoProperty(string optionsString)
		{
			IDatabaseTypeMapper typeMapper = GetDefaultDatabaseTypeMapper();
			ICodeGenerationContext context = GetDefaultCodeGenerationContext(typeMapper);
			IResultGenerationOptions options = GetDefaultResultGenerationOptions(optionsString);

			var generator = new TextCodeGenerator();

			IGeneratedResultItem resultItem = generator.GenerateResultItem(context, options);

			resultItem.Name.ShouldBe("SelectSomeDataItem");

			resultItem.Modifiers.Count().ShouldBe(2);
			resultItem.Modifiers.ShouldContain("public");
			resultItem.Modifiers.ShouldContain("partial");

			resultItem.Usings.Count().ShouldBe(1);
			resultItem.Usings.ShouldContain("System");

			resultItem.BaseTypes.ShouldBeEmpty();

			resultItem.Content.ShouldBe(
				@"public string ObjectName { get; set; }

public int? CurrentStage { get; set; }

public bool IsCompleted { get; set; }

internal void AfterLoad()
{
	AfterLoadInternal();
}

partial void AfterLoadInternal();");
		}

		[Theory]
		[InlineData("generate result struct properties auto ReadOnly")]
		[InlineData("generate result struct properties ReadOnly")]
		[InlineData("generate result struct immutable")]
		public void GenerateResultItemTest_Struct_AutoProperty_ReadOnly(string optionsString)
		{
			IDatabaseTypeMapper typeMapper = GetDefaultDatabaseTypeMapper();
			ICodeGenerationContext context = GetDefaultCodeGenerationContext(typeMapper);
			IResultGenerationOptions options = GetDefaultResultGenerationOptions(optionsString);

			var generator = new TextCodeGenerator();

			IGeneratedResultItem resultItem = generator.GenerateResultItem(context, options);

			resultItem.Name.ShouldBe("SelectSomeDataItem");

			resultItem.Modifiers.Count().ShouldBe(2);
			resultItem.Modifiers.ShouldContain("public");
			resultItem.Modifiers.ShouldContain("partial");

			resultItem.Usings.Count().ShouldBe(1);
			resultItem.Usings.ShouldContain("System");

			resultItem.BaseTypes.ShouldBeEmpty();

			resultItem.Content.ShouldBe(
				@"public string ObjectName { get; internal set; }

public int? CurrentStage { get; internal set; }

public bool IsCompleted { get; internal set; }

internal void AfterLoad()
{
	AfterLoadInternal();
}

partial void AfterLoadInternal();");
		}

		[Theory]
		[InlineData("generate result struct properties BackingField")]
		public void GenerateResultItemTest_Struct_BackingFieldProperty(string optionsString)
		{
			IDatabaseTypeMapper typeMapper = GetDefaultDatabaseTypeMapper();
			ICodeGenerationContext context = GetDefaultCodeGenerationContext(typeMapper);
			IResultGenerationOptions options = GetDefaultResultGenerationOptions(optionsString);

			var generator = new TextCodeGenerator();

			IGeneratedResultItem resultItem = generator.GenerateResultItem(context, options);

			resultItem.Name.ShouldBe("SelectSomeDataItem");

			resultItem.Modifiers.Count().ShouldBe(2);
			resultItem.Modifiers.ShouldContain("public");
			resultItem.Modifiers.ShouldContain("partial");

			resultItem.Usings.Count().ShouldBe(1);
			resultItem.Usings.ShouldContain("System");

			resultItem.BaseTypes.ShouldBeEmpty();

			resultItem.Content.ShouldBe(
				@"private string _objectName;
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

partial void AfterLoadInternal();");
		}

		[Theory]
		[InlineData("generate result struct properties ReadOnly BackingField")]
		[InlineData("generate result struct properties BackingField ReadOnly ")]
		public void GenerateResultItemTest_Struct_BackingFieldProperty_ReadOnly(string optionsString)
		{
			IDatabaseTypeMapper typeMapper = GetDefaultDatabaseTypeMapper();
			ICodeGenerationContext context = GetDefaultCodeGenerationContext(typeMapper);
			IResultGenerationOptions options = GetDefaultResultGenerationOptions(optionsString);

			var generator = new TextCodeGenerator();

			IGeneratedResultItem resultItem = generator.GenerateResultItem(context, options);

			resultItem.Name.ShouldBe("SelectSomeDataItem");

			resultItem.Modifiers.Count().ShouldBe(2);
			resultItem.Modifiers.ShouldContain("public");
			resultItem.Modifiers.ShouldContain("partial");

			resultItem.Usings.Count().ShouldBe(1);
			resultItem.Usings.ShouldContain("System");

			resultItem.BaseTypes.ShouldBeEmpty();

			resultItem.Content.ShouldBe(
				@"private string _objectName;
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

partial void AfterLoadInternal();");
		}

		//------------------------------------------------------------------

		[Theory]
		[InlineData("generate result struct properties inpc BackingField")]
		[InlineData("generate result struct inpc properties BackingField")]
		[InlineData("generate result inpc struct properties BackingField")]
		public void GenerateResultItemTest_Struct_INPC_BackingFieldProperty(string optionsString)
		{
			IDatabaseTypeMapper typeMapper = GetDefaultDatabaseTypeMapper();
			ICodeGenerationContext context = GetDefaultCodeGenerationContext(typeMapper);
			IResultGenerationOptions options = GetDefaultResultGenerationOptions(optionsString);

			var generator = new TextCodeGenerator();

			IGeneratedResultItem resultItem = generator.GenerateResultItem(context, options);

			resultItem.Name.ShouldBe("SelectSomeDataItem");

			resultItem.Modifiers.Count().ShouldBe(2);
			resultItem.Modifiers.ShouldContain("public");
			resultItem.Modifiers.ShouldContain("partial");

			resultItem.Usings.Count().ShouldBe(2);
			resultItem.Usings.ShouldContain("System");
			resultItem.Usings.ShouldContain("System.ComponentModel");

			resultItem.BaseTypes.Count().ShouldBe(1);
			IGeneratedType type = resultItem.BaseTypes.Single();
			type.GenericArguments.ShouldBeEmpty();
			type.IsGeneric.ShouldBeFalse();
			type.IsInterface.ShouldBeTrue();
			type.Name.ShouldBe(nameof(INotifyPropertyChanged));

			resultItem.Content.ShouldBe(
				@"private string _objectName;
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

protected void OnPropertyChanged(string propertyName)
{
	PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}

partial void AfterLoadInternal();");
		}

		[Theory]
		[InlineData("generate result struct properties ReadOnly inpc BackingField")]
		[InlineData("generate result struct inpc properties ReadOnly  BackingField")]
		[InlineData("generate result inpc struct  properties ReadOnly  BackingField")]
		public void GenerateResultItemTest_Struct_INPC_BackingFieldProperty_ReadOnly(string optionsString)
		{
			IDatabaseTypeMapper typeMapper = GetDefaultDatabaseTypeMapper();
			ICodeGenerationContext context = GetDefaultCodeGenerationContext(typeMapper);
			IResultGenerationOptions options = GetDefaultResultGenerationOptions(optionsString);

			var generator = new TextCodeGenerator();

			IGeneratedResultItem resultItem = generator.GenerateResultItem(context, options);

			resultItem.Name.ShouldBe("SelectSomeDataItem");

			resultItem.Modifiers.Count().ShouldBe(2);
			resultItem.Modifiers.ShouldContain("public");
			resultItem.Modifiers.ShouldContain("partial");

			resultItem.Usings.Count().ShouldBe(2);
			resultItem.Usings.ShouldContain("System");
			resultItem.Usings.ShouldContain("System.ComponentModel");

			resultItem.BaseTypes.Count().ShouldBe(1);
			IGeneratedType type = resultItem.BaseTypes.Single();
			type.GenericArguments.ShouldBeEmpty();
			type.IsGeneric.ShouldBeFalse();
			type.IsInterface.ShouldBeTrue();
			type.Name.ShouldBe(nameof(INotifyPropertyChanged));

			resultItem.Content.ShouldBe(
				@"private string _objectName;
private int? _currentStage;
private bool _isCompleted;

public string ObjectName
{
	get => _objectName;
	internal set
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
	internal set
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
	internal set
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

protected void OnPropertyChanged(string propertyName)
{
	PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}

partial void AfterLoadInternal();");
		}

		#region Fixture

		private const string QueryItemName = "SelectSomeDataItem";
		private const string QueryName = "SelectSomeData";
		private const string MyTestNamespace = "MyTestNamespace";

		private static IDatabaseTypeMapper GetDefaultDatabaseTypeMapper()
		{
			var typeMapper = A.Fake<IDatabaseTypeMapper>(p => p.Strict());
			A.CallTo(() => typeMapper.MapToClrType("varchar", true)).Returns(typeof(string));
			A.CallTo(() => typeMapper.MapToClrType("int", true)).Returns(typeof(int?));
			A.CallTo(() => typeMapper.MapToClrType("bit", false)).Returns(typeof(bool));
			return typeMapper;
		}

		private static IResultGenerationOptions GetDefaultResultGenerationOptions(params string[] options)
		{
			var result = A.Fake<IResultGenerationOptions>(p => p.Strict());

			var defaultOptions = new List<ISqlFirstOption>();

			foreach (string option in options)
			{
				if (!string.IsNullOrEmpty(option))
				{
					string[] array = option.Split(' ').Where(p => !string.IsNullOrEmpty(p)).ToArray();
					defaultOptions.Add(new SqlFirstOption(array[0], array.Skip(1)));
				}
			}

			A.CallTo(() => result.SqlFirstOptions).Returns(defaultOptions);
			return result;
		}

		private static ICodeGenerationContext GetDefaultCodeGenerationContext(IDatabaseTypeMapper typeMapper)
		{
			var optionsDictionary = new Dictionary<string, object>
			{
				["Namespace"] = MyTestNamespace,
				["QueryName"] = QueryName,
				["QueryResultItemName"] = QueryItemName,
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

			A.CallTo(() => context.TypeMapper).Returns(typeMapper);
			return context;
		}

		#endregion
	}
}