using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Xml;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using SqlFirst.Codegen;
using SqlFirst.Codegen.Helpers;
using SqlFirst.Codegen.Impl;
using SqlFirst.Codegen.Text;
using SqlFirst.Codegen.Trees;
using SqlFirst.Core;
using SqlFirst.Core.Impl;
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
			RegisterHighlight(ParameterItemEditor, Highlight.CSharp);
			RegisterHighlight(QueryObjectEditor, Highlight.CSharp);
		}

		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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

		private void Build(object sender, RoutedEventArgs e)
		{
			try
			{
				FormattedSql = null;
				ResultItem = null;
				ParameterItem = null;
				QueryObject = null;

				FormattedSql = FormatSql(SourceSql);
				ResultItem = GenerateResultItemCode(SourceSql);
				ParameterItem = GenerateParameterItemCode(SourceSql);
				QueryObject = GenerateQueryObjectCode(SourceSql);
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

		private void BuildAtClipboard(object sender, RoutedEventArgs e)
		{
			try
			{
				var generator = new TextCodeGenerator();

				IGeneratedItem resultItem = GenerateResultItem(SourceSql, generator);
				IGeneratedItem parameterItem = GenerateParameterItem(SourceSql, generator);
				IGeneratedItem queryObject = GenerateQueryObject(SourceSql, generator);

				string file = generator.GenerateFile(new[] { resultItem, parameterItem, queryObject }.Where(p => p != null));
				Clipboard.SetText(file);

				MessageBox.Show("Все объекты помещены в буфер обмена", "Генерация", MessageBoxButton.OK, MessageBoxImage.Information);
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

		private string FormatSql(string query)
		{
			var parser = new MsSqlServerQueryParser();
			var emitter = new MsSqlServerCodeEmitter();

			IQueryInfo info = parser.GetQueryInfo(query, ConnectionString);
			List<IQuerySection> sections = parser.GetQuerySections(query).ToList();

			IQuerySection[] allDeclarations = sections.Where(p => p.Type == QuerySectionType.Declarations).ToArray();
			sections.RemoveAll(allDeclarations.Contains);

			string declarations = emitter.EmitDeclarations(info.Parameters);

			IQuerySection declarationsSection = new QuerySection(QuerySectionType.Declarations, declarations);
			sections.Add(declarationsSection);

			if (sections.All(p => p.Type != QuerySectionType.Options))
			{
				IQuerySection optionsSection = new QuerySection(QuerySectionType.Options, @"/* add SqlFirst options here */");
				sections.Add(optionsSection);
			}

			string result = emitter.EmitQuery(sections);
			return result;
		}

		private IGeneratedItem GenerateParameterItem(string query, ICodeGenerator codeGenerator)
		{
			var parser = new MsSqlServerQueryParser();

			IQueryInfo info = parser.GetQueryInfo(query, ConnectionString);

			if (!info.Parameters.Any())
			{
				return null;
			}

			var typeMapper = new MsSqlServerTypeMapper();

			IReadOnlyDictionary<string, object> contextOptions = new Dictionary<string, object>
			{
				["Namespace"] = Namespace,
				["QueryName"] = CSharpCodeHelper.GetValidIdentifierName(QueryName, NamingPolicy.Pascal),
				["QueryParameterItemName"] = CSharpCodeHelper.GetValidIdentifierName(QueryName, NamingPolicy.Pascal) + "Parameter"
			};
			var context = new CodeGenerationContext(info.Parameters, info.Results, contextOptions, typeMapper);

			IParameterGenerationOptions itemOptions = new ParameterGenerationOptions(info.SqlFirstOptions);
			IGeneratedItem generatedItem = codeGenerator.GenerateParameterItem(context, itemOptions);

			return generatedItem;
		}

		private IGeneratedItem GenerateResultItem(string query, ICodeGenerator codeGenerator)
		{
			var parser = new MsSqlServerQueryParser();

			IQueryInfo info = parser.GetQueryInfo(query, ConnectionString);

			if (info.Results.Count() <= 1)
			{
				return null;
			}

			var typeMapper = new MsSqlServerTypeMapper();

			IReadOnlyDictionary<string, object> contextOptions = new Dictionary<string, object>
			{
				["Namespace"] = Namespace,
				["QueryName"] = CSharpCodeHelper.GetValidIdentifierName(QueryName, NamingPolicy.Pascal),
				["QueryResultItemName"] = CSharpCodeHelper.GetValidIdentifierName(QueryName, NamingPolicy.Pascal) + "Result"
			};
			var context = new CodeGenerationContext(info.Parameters, info.Results, contextOptions, typeMapper);

			IResultGenerationOptions itemOptions = new ResultGenerationOptions(info.SqlFirstOptions);
			IGeneratedItem generatedItem = codeGenerator.GenerateResultItem(context, itemOptions);

			return generatedItem;
		}

		private string GenerateResultItemCode(string query)
		{
			ICodeGenerator codeGenerator = new TextCodeGenerator();
			IGeneratedItem generatedItem = GenerateResultItem(query, codeGenerator);

			if (generatedItem == null)
			{
				return null;
			}

			string itemCode = codeGenerator.GenerateFile(new[] { generatedItem });

			return itemCode;
		}

		private string GenerateParameterItemCode(string query)
		{
			ICodeGenerator codeGenerator = new TextCodeGenerator();
			IGeneratedItem generatedItem = GenerateParameterItem(query, codeGenerator);

			if (generatedItem == null)
			{
				return null;
			}

			string itemCode = codeGenerator.GenerateFile(new[] { generatedItem });

			return itemCode;
		}

		private IGeneratedItem GenerateQueryObject(string query, ICodeGenerator codeGenerator)
		{
			var parser = new MsSqlServerQueryParser();

			IQueryInfo info = parser.GetQueryInfo(query, ConnectionString);

			var typeMapper = new MsSqlServerTypeMapper();

			IReadOnlyDictionary<string, object> contextOptions = new Dictionary<string, object>
			{
				["Namespace"] = Namespace,
				["QueryName"] = CSharpCodeHelper.GetValidIdentifierName(QueryName, NamingPolicy.Pascal),
				["QueryResultItemName"] = CSharpCodeHelper.GetValidIdentifierName(QueryName, NamingPolicy.Pascal) + "Result",
				["QueryParameterItemName"] = CSharpCodeHelper.GetValidIdentifierName(QueryName, NamingPolicy.Pascal) + "Parameter",
				["QueryText"] = info.Sections.Single(p => p.Type == QuerySectionType.Body).Content
			};

			var context = new CodeGenerationContext(info.Parameters, info.Results, contextOptions, typeMapper);

			IQueryGenerationOptions queryOptions = new QueryGenerationOptions(info.Type, info.SqlFirstOptions);
			IGeneratedItem generatedItem = codeGenerator.GenerateQueryObject(context, queryOptions);

			return generatedItem;
		}

		private string GenerateQueryObjectCode(string query)
		{
			ICodeGenerator codeGenerator = new TextCodeGenerator();
			IGeneratedItem generatedItem = GenerateQueryObject(query, codeGenerator);

			if (generatedItem == null)
			{
				return null;
			}

			string itemCode = codeGenerator.GenerateFile(new[] { generatedItem });

			return itemCode;
		}

		#region Public

		private string _namespace = "SqlFirst.Test.Namespace";

		private string _queryName = "My_Test_Query";

		private static readonly IDictionary<QueryType, string> _samples = new Dictionary<QueryType, string>
		{
			[QueryType.Read] = @"-- begin sqlFirstOptions

-- generate result class properties auto virtual

-- end

-- begin variables 

declare @userKey varchar(MAX) ='test'; 

-- end

select *
from CaseSubscriptions with(nolock)
where UserKey = @userKey
order by CreateDateUtc desc
offset @skip rows
fetch next @take rows only",

			[QueryType.Create] = @"-- begin sqlFirstOptions

-- generate result class properties auto virtual
-- generate parameter class

-- end

-- begin variables 

declare @userKey_N varchar(MAX) ='test'; 

-- end

insert into caseevents (userKey, inn, ogrn, caseid, shardname, finddateutc) 
output inserted.id, inserted.userKey
values (@userKey_N, @inn_N, @ogrn_N, @caseId_N, @shardName_N, @findDateUtc)"
		};

		private string _sourceSql = _samples[QueryType.Create];

		private string _formattedSql;

		private string _resultItem;

		private string _parameterItem;

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

		public string ParameterItem
		{
			get => _parameterItem;
			set
			{
				_parameterItem = value;
				OnPropertyChanged(nameof(ParameterItem));
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