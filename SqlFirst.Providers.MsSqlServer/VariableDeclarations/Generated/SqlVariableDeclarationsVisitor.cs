//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.6.4
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from c:\users\keeper\documents\visual studio 2015\Projects\ClassLibrary1\ClassLibrary1\SqlVariableDeclarations.g4 by ANTLR 4.6.4

// Unreachable code detected

using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;

#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419

namespace SqlFirst.Providers.MsSqlServer.VariableDeclarations.Generated {
	/// <summary>
/// This interface defines a complete generic visitor for a parse tree produced
/// by <see cref="SqlVariableDeclarationsParser"/>.
/// </summary>
/// <typeparam name="Result">The return type of the visit operation.</typeparam>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.6.4")]
[System.CLSCompliant(false)]
public interface ISqlVariableDeclarationsVisitor<Result> : IParseTreeVisitor<Result> {
	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlVariableDeclarationsParser.root"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitRoot([NotNull] SqlVariableDeclarationsParser.RootContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlVariableDeclarationsParser.element"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitElement([NotNull] SqlVariableDeclarationsParser.ElementContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlVariableDeclarationsParser.declaration"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitDeclaration([NotNull] SqlVariableDeclarationsParser.DeclarationContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlVariableDeclarationsParser.commentary"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitCommentary([NotNull] SqlVariableDeclarationsParser.CommentaryContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlVariableDeclarationsParser.spaces"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitSpaces([NotNull] SqlVariableDeclarationsParser.SpacesContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlVariableDeclarationsParser.space"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitSpace([NotNull] SqlVariableDeclarationsParser.SpaceContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlVariableDeclarationsParser.assignment"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitAssignment([NotNull] SqlVariableDeclarationsParser.AssignmentContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlVariableDeclarationsParser.value"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitValue([NotNull] SqlVariableDeclarationsParser.ValueContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlVariableDeclarationsParser.string"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitString([NotNull] SqlVariableDeclarationsParser.StringContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlVariableDeclarationsParser.int"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitInt([NotNull] SqlVariableDeclarationsParser.IntContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlVariableDeclarationsParser.float"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitFloat([NotNull] SqlVariableDeclarationsParser.FloatContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlVariableDeclarationsParser.variable"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitVariable([NotNull] SqlVariableDeclarationsParser.VariableContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlVariableDeclarationsParser.type"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitType([NotNull] SqlVariableDeclarationsParser.TypeContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlVariableDeclarationsParser.typeName"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitTypeName([NotNull] SqlVariableDeclarationsParser.TypeNameContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlVariableDeclarationsParser.size"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitSize([NotNull] SqlVariableDeclarationsParser.SizeContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="SqlVariableDeclarationsParser.identifier"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitIdentifier([NotNull] SqlVariableDeclarationsParser.IdentifierContext context);
}
} // namespace SqlFirst.Providers.MsSqlServer.VariableDeclarations
