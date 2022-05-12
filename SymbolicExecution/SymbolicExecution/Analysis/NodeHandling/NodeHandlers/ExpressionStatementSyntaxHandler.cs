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