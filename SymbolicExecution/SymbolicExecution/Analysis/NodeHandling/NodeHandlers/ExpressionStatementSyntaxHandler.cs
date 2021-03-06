using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using SymbolicExecution.Analysis.Context;
using SymbolicExecution.Analysis.ExpressionSyntaxHandling;

namespace SymbolicExecution.Analysis.NodeHandling.NodeHandlers;

public class ExpressionStatementSyntaxHandler : NodeHandlerBase<ExpressionStatementSyntax>
{
	protected override SymbolicAnalysisContext ProcessNode(
		ExpressionStatementSyntax node,
		SymbolicAnalysisContext analysisContext,
		CodeBlockAnalysisContext codeBlockContext
		)
	{
		return ExpressionSyntaxHandlerMediator.Instance.Handle(node.Expression, analysisContext, codeBlockContext);
	}
}