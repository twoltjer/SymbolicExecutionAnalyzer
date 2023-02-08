namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public class ExpressionStatementSyntaxAbstraction : StatementSyntaxAbstraction, IExpressionStatementSyntaxAbstraction
{
	public ExpressionStatementSyntaxAbstraction(ImmutableArray<ISyntaxNodeAbstraction> children, ISymbol? symbol, Location location) : base(children, symbol, location)
	{
	}

	public override TaggedUnion<IEnumerable<IAnalysisState>, AnalysisFailure> AnalyzeNode(IAnalysisState previous)
	{
		if (Children.Length != 1)
			return new AnalysisFailure("Expression statement must have exactly one child", Location.None);

		if (Children[0] is not IAssignmentExpressionSyntaxAbstraction assignment)
			return new AnalysisFailure("Expression statement must have an assignment expression as its child", Location.None);

		return assignment.AnalyzeNode(previous);
	}

	public override TaggedUnion<ImmutableArray<(IObjectInstance, IAnalysisState)>, AnalysisFailure> GetExpressionResults(IAnalysisState state)
	{
		return new AnalysisFailure("Cannot get the result of an expression statement", Location.None);
	}
}