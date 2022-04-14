using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace SymbolicExecution.Analysis.ExpressionSyntaxHandling;

public interface IExpressionSyntaxHandler
{
	bool CanHandle(ExpressionSyntax expressionSyntax);
	SymbolicAnalysisContext Handle(ExpressionSyntax expressionSyntax, SymbolicAnalysisContext analysisContext, CodeBlockAnalysisContext codeBlockContext);
}