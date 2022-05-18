using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SymbolicExecution.Analysis.ExpressionSyntaxHandling;

public abstract class ExpressionSyntaxHandlerBase<T> : IHandler<ExpressionSyntax> where T : ExpressionSyntax
{
	public bool CanHandle(ExpressionSyntax expressionSyntax) => expressionSyntax is T;

	public SymbolicAnalysisContext Handle(
		ExpressionSyntax expressionSyntax,
		SymbolicAnalysisContext analysisContext,
		CodeBlockAnalysisContext codeBlockContext
		)
	{
		return ProcessExpressionSyntax((T)expressionSyntax, analysisContext, codeBlockContext);
	}

	protected abstract SymbolicAnalysisContext ProcessExpressionSyntax(T node, SymbolicAnalysisContext analysisContext, CodeBlockAnalysisContext codeBlockContext);
}