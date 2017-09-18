﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Xml;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using SqlFirst.Codegen;
using SqlFirst.Codegen.Impl;
using SqlFirst.Codegen.Text;
using SqlFirst.Core;
using SqlFirst.Demo.Wpf.Annotations;
using SqlFirst.Demo.Wpf.HighlightSchemes;
using SqlFirst.Providers.MsSqlServer;

namespace SqlFirst.Demo.Wpf
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : INotifyPropertyChanged
	{
		private string _connectionString = @"Server=api-dev;Database=CasebookApi.Arbitrage.Tracking_dev;Integrated Security=SSPI;";

		public MainWindow()
		{
			InitializeComponent();
			DataContext = this;
			RegisterHighlight(SqlEditor, Highlight.Sql);
			RegisterHighlight(FormattedSqlEditor, Highlight.Sql);

			RegisterHighlight(ResultItemEditor, Highlight.CSharp);
			RegisterHighlight(QueryObjectEditor, Highlight.CSharp);
		}

		private void RegisterHighlight(TextEditor editor, string schema)
		{
			using (TextReader textReader = new StringReader(schema))
			using (XmlReader xmlReader = new XmlTextReader(textReader))
			{
				IHighlightingDefinition customHighlighting = HighlightingLoader.Load(xmlReader, HighlightingManager.Instance);
				editor.SyntaxHighlighting = customHighlighting;
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				FormattedSql = null;
				ResultItem = null;
				QueryObject = null;

				string query = SourceSql;

				var parser = new MsSqlServerQueryParser();

				IQueryInfo info = parser.GetQueryInfo(query, ConnectionString);

				var typeMapper = new MsSqlServerTypeMapper();

				var codeGenerator = new TextCodeGenerator(typeMapper);

				IReadOnlyDictionary<string, object> contextOptions = new Dictionary<string, object>
				{
					["Namespace"] = Namespace,
					["QueryName"] = QueryName
				};
				var context = new CodeGenerationContext(info.Parameters, info.Results, contextOptions);

				ResultItemAbilities abilities = IsNotifyPropertyChanged ? ResultItemAbilities.NotifyPropertyChanged : ResultItemAbilities.None;
				var modifiers = PropertyModifiers.None;
				if (IsVirtualProperties)
				{
					modifiers |= PropertyModifiers.Virtual;
				}
				if (IsReadOnlyProperties)
				{
					modifiers |= PropertyModifiers.ReadOnly;
				}

				IResultGenerationOptions options = new ResultGenerationOptions(ResultItemType, abilities, PropertyType, modifiers);
				IGeneratedResultItem item = codeGenerator.GenerateResultItem(context, options);

				string code = codeGenerator.GenerateFile(new[] { item });

				ResultItem = code;
			}
			catch (Exception ex)
			{
				string message = $"{ex.GetType().Name} occurred:";

				while (ex != null)
				{
					message += $"\n-----\n{ex.Message}";
					ex = ex.InnerException;
				}

				MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		#region Public

		private ResultItemType _resultItemType = ResultItemType.Class;

		private bool _isNotifyPropertyChanged;

		private PropertyType _propertyType = PropertyType.Auto;

		private bool _isVirtualProperties;

		private bool _isReadOnlyProperties;

		private string _namespace = "SqlFirst.Test.Namespace";

		private string _queryName = "My_Test_Query";

		private string _sourceSql =
			@"declare @userKey varchar(MAX) ='test'; 
declare @skip int = 42;

select *
from CaseSubscriptions with(nolock)
where UserKey = @userKey
order by CreateDateUtc desc
offset @skip rows
fetch next @take rows only";

		private string _formattedSql;

		private string _resultItem;

		private string _queryObject;

		public string ConnectionString
		{
			get => _connectionString;
			set
			{
				_connectionString = value;
				OnPropertyChanged(nameof(ConnectionString));
			}
		}

		public string Namespace
		{
			get => _namespace;
			set
			{
				_namespace = value;
				OnPropertyChanged(nameof(Namespace));
			}
		}

		public string QueryName
		{
			get => _queryName;
			set
			{
				_queryName = value;
				OnPropertyChanged(nameof(QueryName));
			}
		}

		public ResultItemType ResultItemType
		{
			get => _resultItemType;
			set
			{
				_resultItemType = value;
				OnPropertyChanged(nameof(ResultItemType));
			}
		}

		public bool IsNotifyPropertyChanged
		{
			get => _isNotifyPropertyChanged;
			set
			{
				_isNotifyPropertyChanged = value;
				OnPropertyChanged(nameof(IsNotifyPropertyChanged));
			}
		}

		public PropertyType PropertyType
		{
			get => _propertyType;
			set
			{
				_propertyType = value;
				OnPropertyChanged(nameof(PropertyType));
			}
		}

		public bool IsVirtualProperties
		{
			get => _isVirtualProperties;
			set
			{
				_isVirtualProperties = value;
				OnPropertyChanged(nameof(IsVirtualProperties));
			}
		}

		public bool IsReadOnlyProperties
		{
			get => _isReadOnlyProperties;
			set
			{
				_isReadOnlyProperties = value;
				OnPropertyChanged(nameof(IsReadOnlyProperties));
			}
		}

		public string SourceSql
		{
			get => _sourceSql;
			set
			{
				_sourceSql = value;
				OnPropertyChanged(nameof(SourceSql));
			}
		}

		public string FormattedSql
		{
			get => _formattedSql;
			set
			{
				_formattedSql = value;
				OnPropertyChanged(nameof(FormattedSql));
			}
		}

		public string ResultItem
		{
			get => _resultItem;
			set
			{
				_resultItem = value;
				OnPropertyChanged(nameof(ResultItem));
			}
		}

		public string QueryObject
		{
			get => _queryObject;
			set
			{
				_queryObject = value;
				OnPropertyChanged(nameof(QueryObject));
			}
		}

		#endregion
	}
}