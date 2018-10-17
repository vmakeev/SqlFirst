using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using FakeItEasy;
using SqlFirst.Core;

namespace SqlFirst.Codegen.Text.Tests.Fixtures
{
	internal class CodeGenerationContextBuilder
	{
		internal class ComplexTypeDataBuilder
		{
			private readonly CodeGenerationContextBuilder _contextBuilder;
			private readonly IComplexTypeData _complexTypeData;
			private readonly List<IFieldDetails> _fields = new List<IFieldDetails>();

			/// <inheritdoc />
			public ComplexTypeDataBuilder(CodeGenerationContextBuilder contextBuilder)
			{
				_contextBuilder = contextBuilder ?? throw new ArgumentNullException(nameof(contextBuilder));
				_complexTypeData = A.Fake<IComplexTypeData>();
			}

			public ComplexTypeDataBuilder WithBaseInfo(string itemName, string dbTypeDisplayedName, bool nullable)
			{
				A.CallTo(() => _complexTypeData.Name).Returns(itemName);
				A.CallTo(() => _complexTypeData.DbTypeDisplayedName).Returns(dbTypeDisplayedName);
				A.CallTo(() => _complexTypeData.AllowNull).Returns(nullable);
				return this;
			}

			public ComplexTypeDataBuilder WithField(string columnName,
													string dbType,
													bool isNullable,
													Type clrType = null,
													string providerSpecificTypeElementName = null)
			{
				var result = A.Fake<IFieldDetails>(p => p.Strict());
				A.CallTo(() => result.ColumnName).Returns(columnName);
				A.CallTo(() => result.DbType).Returns(dbType);
				A.CallTo(() => result.AllowDbNull).Returns(isNullable);
				A.CallTo(() => result.ColumnOrdinal).Returns(_fields.Count);
				A.CallTo(() => result.DbTypeMetadata).Returns(null);

				_fields.Add(result);

				_contextBuilder.WithClrTypeMapping(
									dbType: dbType,
									clrType: clrType,
									isNullable: isNullable)
								.WithProviderSpecificTypeMapping(
									dbType: dbType,
									name: providerSpecificTypeElementName);

				return this;
			}

			public IComplexTypeData Build()
			{
				if (_fields.Any())
				{
					A.CallTo(() => _complexTypeData.Fields).Returns(new List<IFieldDetails>(_fields));
				}

				return _complexTypeData;
			}
		}

		private readonly ICodeGenerationContext _context;
		private readonly Type _providerSpecificType;

		private readonly List<IQueryParamInfo> _incomingParameters = new List<IQueryParamInfo>();
		private readonly List<IFieldDetails> _outgoingParameters = new List<IFieldDetails>();

		/// <inheritdoc />
		private CodeGenerationContextBuilder(ICodeGenerationContext context, Type providerSpecificType)
		{
			_context = context ?? throw new ArgumentNullException(nameof(context));
			_providerSpecificType = providerSpecificType ?? throw new ArgumentNullException(nameof(providerSpecificType));
		}

		public static CodeGenerationContextBuilder Create(Type providerSpecificType, string queryResultItemName = "QueryItemTestName")
		{
			var providerTypesInfo = A.Fake<IProviderTypesInfo>(p => p.Strict());

			var provider = A.Fake<IDatabaseProvider>(p => p.Strict());
			A.CallTo(() => provider.ProviderTypesInfo).Returns(providerTypesInfo);

			var mapper = A.Fake<IDatabaseTypeMapper>(p => p.Strict());
			var options = A.Fake<IReadOnlyDictionary<string, object>>(p => p.Strict());

			object _;
			A.CallTo(() => options.TryGetValue("QueryResultItemName", out _))
			.Returns(true)
			.AssignsOutAndRefParametersLazily((string a, object b) => new object[] { queryResultItemName });

			var context = A.Fake<ICodeGenerationContext>(p => p.Strict());
			A.CallTo(() => context.TypeMapper).Returns(mapper);
			A.CallTo(() => context.Options).Returns(options);
			A.CallTo(() => context.DatabaseProvider).Returns(provider);
			A.CallTo(() => context.IncomingParameters).Returns(Enumerable.Empty<IQueryParamInfo>());
			A.CallTo(() => context.OutgoingParameters).Returns(Enumerable.Empty<IFieldDetails>());

			return new CodeGenerationContextBuilder(context, providerSpecificType);
		}

		public CodeGenerationContextBuilder WithParameter(string dbName = null,
														string semanticName = null,
														string dbType = null,
														Type clrType = null,
														string providerSpecificTypeElementName = null)
		{
			return WithParameterInternal(
						dbName: dbName,
						semanticName: semanticName,
						dbType: dbType,
						complexTypeData: null)
					.WithClrTypeMapping(
						dbType: dbType,
						clrType: clrType,
						isNullable: true)
					.WithProviderSpecificTypeMapping(
						dbType: dbType,
						name: providerSpecificTypeElementName);
		}

		public CodeGenerationContextBuilder WithTableParameter(string dbName = null,
																string semanticName = null,
																string dbType = null,
																Action<ComplexTypeDataBuilder> setup = null)
		{
			var complexTypeDataBuilder = new ComplexTypeDataBuilder(this);
			setup?.Invoke(complexTypeDataBuilder);
			IComplexTypeData complexTypeData = complexTypeDataBuilder.Build();

			return WithParameterInternal(
					dbName: dbName,
					semanticName: semanticName,
					dbType: dbType,
					complexTypeData: complexTypeData)
				.WithClrTypeMapping(
					dbType: dbType,
					clrType: typeof(DataTable),
					isNullable: true);
		}

		public ICodeGenerationContext Build()
		{
			if (_incomingParameters.Any())
			{
				A.CallTo(() => _context.IncomingParameters).Returns(new List<IQueryParamInfo>(_incomingParameters));
			}

			if (_outgoingParameters.Any())
			{
				A.CallTo(() => _context.OutgoingParameters).Returns(new List<IFieldDetails>(_outgoingParameters));
			}

			return _context;
		}

		public CodeGenerationContextBuilder WithResultField(string columnName,
															string dbType,
															bool isNullable,
															Type clrType = null,
															string providerSpecificTypeElementName = null)
		{
			var result = A.Fake<IFieldDetails>(p => p.Strict());
			A.CallTo(() => result.ColumnName).Returns(columnName);
			A.CallTo(() => result.DbType).Returns(dbType);
			A.CallTo(() => result.AllowDbNull).Returns(isNullable);
			A.CallTo(() => result.ColumnOrdinal).Returns(_outgoingParameters.Count);
			A.CallTo(() => result.DbTypeMetadata).Returns(null);

			_outgoingParameters.Add(result);

			return WithClrTypeMapping(
					dbType: dbType,
					clrType: clrType,
					isNullable: isNullable)
				.WithProviderSpecificTypeMapping(
					dbType: dbType,
					name: providerSpecificTypeElementName);
		}

		private CodeGenerationContextBuilder WithParameterInternal(string dbName,
																	string semanticName,
																	string dbType,
																	IComplexTypeData complexTypeData)
		{
			var parameter = A.Fake<IQueryParamInfo>(p => p.Strict());
			A.CallTo(() => parameter.DbName).Returns(dbName);
			A.CallTo(() => parameter.SemanticName).Returns(semanticName);
			A.CallTo(() => parameter.DbType).Returns(dbType);
			A.CallTo(() => parameter.DbTypeMetadata).Returns(null);
			A.CallTo(() => parameter.IsComplexType).Returns(complexTypeData != null);
			A.CallTo(() => parameter.ComplexTypeData).Returns(complexTypeData);

			_incomingParameters.Add(parameter);

			return this;
		}

		private CodeGenerationContextBuilder WithProviderSpecificTypeMapping(string dbType, string name)
		{
			var specificType = A.Fake<IProviderSpecificType>(p => p.Strict());
			A.CallTo(() => specificType.TypeName).Returns(_providerSpecificType.Name);
			A.CallTo(() => specificType.ValueName).Returns(name);
			A.CallTo(() => specificType.Usings).Returns(new[] { _providerSpecificType.Namespace });

			A.CallTo(() => _context.TypeMapper.MapToProviderSpecificType(dbType, A<IDictionary<string, object>>._)).Returns(specificType);

			return this;
		}

		private CodeGenerationContextBuilder WithClrTypeMapping(string dbType, Type clrType, bool isNullable)
		{
			A.CallTo(() => _context.TypeMapper.MapToClrType(dbType, isNullable, A<IDictionary<string, object>>._)).Returns(clrType);
			return this;
		}
	}
}