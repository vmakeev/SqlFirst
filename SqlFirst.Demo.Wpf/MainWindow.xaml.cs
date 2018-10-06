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
using SqlFirst.Codegen.Trees;
using SqlFirst.Core;
using SqlFirst.Core.Impl;
using SqlFirst.Demo.Wpf.Annotations;
using SqlFirst.Demo.Wpf.HighlightSchemes;
using SqlFirst.Demo.Wpf.Logic;
using SqlFirst.Providers.MsSqlServer;
using SqlFirst.Providers.Postgres;

namespace SqlFirst.Demo.Wpf
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : INotifyPropertyChanged
	{
		public MainWindow()
		{
			InitializeComponent();

			_sourceSql = Generator.Samples.GetSample(QueryType.StoredProcedure);

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

				string sql = SourceSql;
				GeneratorParameters parameters = GetGeneratorParameters();

				FormattedSql = FormatSql(SourceSql);
				ResultItem = Generator.GenerateResultItemCode(sql, parameters);
				ParameterItem = Generator.GenerateParameterItemCode(sql, parameters);
				QueryObject = Generator.GenerateQueryObjectCode(sql, parameters);
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
				string sql = SourceSql;
				GeneratorParameters parameters = GetGeneratorParameters();

				IGeneratedItem resultItem = Generator.GenerateResultItem(sql, parameters);
				IGeneratedItem parameterItem = Generator.GenerateParameterItem(sql, parameters);
				IGeneratedItem queryObject = Generator.GenerateQueryObject(sql, parameters);

				string file = Generator.CodeGenerator.GenerateFile(new[] { resultItem, parameterItem, queryObject }.Where(p => p != null));
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
			IQueryParser parser = Generator.QueryParser;

			if (_providerType == ProviderType.MsSqlServer)
			{
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
			else
			{
				var emitter = new PostgresCodeEmitter();

				List<IQuerySection> sections = parser.GetQuerySections(query).ToList();

				IQuerySection[] allDeclarations = sections.Where(p => p.Type == QuerySectionType.Declarations).ToArray();
				sections.RemoveAll(allDeclarations.Contains);

				if (sections.All(p => p.Type != QuerySectionType.Options))
				{
					IQuerySection optionsSection = new QuerySection(QuerySectionType.Options, @"/* add SqlFirst options here */");
					sections.Add(optionsSection);
				}

				string result = emitter.EmitQuery(sections);
				return result;
			}
		}

		private GeneratorParameters GetGeneratorParameters()
		{
			return new GeneratorParameters
			{
				ConnectionString = ConnectionString,
				Namespace = Namespace,
				QueryName = QueryName
			};
		}

		private void ShowSelectSample(object sender, RoutedEventArgs e)
		{
			SourceSql = Generator.Samples.GetSample(QueryType.Read);
		}

		private void ShowInsertSample(object sender, RoutedEventArgs e)
		{
			SourceSql = Generator.Samples.GetSample(QueryType.Create);
		}

		#region Private

		private ProviderType _providerType = ProviderType.MsSqlServer;

		private string _connectionStringMsSqlServer = @"Server=vmcbapi.kadlab.local;Initial Catalog=CBAPI_SvcQueries_dev;integrated security=True;MultipleActiveResultSets=True";
		private string _connectionStringPostgres = @"Server = 127.0.0.1; Port = 5432; Database = CasebookApi.Arbitrage.Tracking_dev; User Id = postgres;Password = postgres;";

		private string _namespace = "SqlFirst.Test.Namespace";

		private string _queryName = "My_Test_Query";

		private readonly GeneratorBase _generatorMsSqlServer = new MsSqlServerGenerator();
		private readonly GeneratorBase _generatorPostgres = new PostgresGenerator();

		private string _sourceSql;

		private string _formattedSql;

		private string _resultItem;

		private string _parameterItem;

		private string _queryObject;

		#endregion

		#region Public

		public ProviderType ProviderType
		{
			get => _providerType;
			set
			{
				if (_providerType != value)
				{
					_providerType = value;
					OnPropertyChanged(nameof(ProviderType));
					OnPropertyChanged(nameof(ConnectionString));
				}
			}
		}

		public GeneratorBase Generator
		{
			get
			{
				switch (_providerType)
				{
					case ProviderType.MsSqlServer:
						return _generatorMsSqlServer;

					case ProviderType.Postgres:
						return _generatorPostgres;

					default:
						throw new ArgumentOutOfRangeException();
				}
			}
		}

		public string ConnectionString
		{
			get
			{
				switch (_providerType)
				{
					case ProviderType.MsSqlServer:
						return _connectionStringMsSqlServer;

					case ProviderType.Postgres:
						return _connectionStringPostgres;

					default:
						throw new ArgumentOutOfRangeException();
				}
			}
			set
			{
				switch (_providerType)
				{
					case ProviderType.MsSqlServer:
						_connectionStringMsSqlServer = value;
						break;

					case ProviderType.Postgres:
						_connectionStringPostgres = value;
						break;

					default:
						throw new ArgumentOutOfRangeException();
				}

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