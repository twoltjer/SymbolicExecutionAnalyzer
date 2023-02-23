namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public class ExpressionStatementSyntaxAbstraction : StatementSyntaxAbstraction, IExpressionStatementSyntaxAbstraction
{
	public ExpressionStatementSyntaxAbstraction(ImmutableArray<ISyntaxNodeAbstraction> children, ISymbol? symbol, Location location) : base(children, symbol, location)
	{
	}

	public override TaggedUnion<IEnumerable<IAnalysisState>, AnalysisFailure> AnalyzeNode(IAnalysisState previous)
	{
		if (Children.Length != 1)
			return new AnalysisFailure("Expression statement must have exactly one child", Location);

		if (Children[0] is not IAssignmentExpressionSyntaxAbstraction assignment)
			return new AnalysisFailure("Expression statement must have an assignment expression as its child", Location);

		return assignment.AnalyzeNode(previous);
	}
}