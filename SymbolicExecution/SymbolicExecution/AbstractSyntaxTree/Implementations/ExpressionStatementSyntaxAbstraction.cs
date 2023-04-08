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

		return Children[0] switch
		{
			IAssignmentExpressionSyntaxAbstraction assignment => assignment.AnalyzeNode(previous),
			InvocationExpressionSyntaxAbstraction invocation => invocation.AnalyzeNode(previous),
			_ => new AnalysisFailure("Expression statement must have an assignment expression or invocation as its child", Location)
		};
	}

	public override TaggedUnion<ImmutableArray<(IObjectInstance, IAnalysisState)>, AnalysisFailure> GetExpressionResults(IAnalysisState state)
	{
		return new AnalysisFailure("Cannot get the result of an expression statement", Location);
	}
}