namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public class ThrowStatementSyntaxAbstraction : StatementSyntaxAbstraction, IThrowStatementSyntaxAbstraction
{
	public ThrowStatementSyntaxAbstraction(
		ImmutableArray<ISyntaxNodeAbstraction> children,
		ISymbol? symbol,
		Location location
		) : base(children, symbol, location)
	{
	}

	public override TaggedUnion<IEnumerable<IAnalysisState>, AnalysisFailure> AnalyzeNode(IAnalysisState previous)
	{
		if (Children.Length != 1)
			return new AnalysisFailure("Throw statement expected to have exactly one child", Location);

		var expressionResultOrFailure = Children[0].GetExpressionResult(previous);
		if (!expressionResultOrFailure.IsT1)
			return expressionResultOrFailure.T2Value;

		var thrownState = previous.ThrowException(expressionResultOrFailure.T1Value, Location);
		return new[] { thrownState };
	}

	public override TaggedUnion<IObjectInstance, AnalysisFailure> GetExpressionResult(IAnalysisState state)
	{
		return new AnalysisFailure("Cannot analyze throw statements", Location);
	}
}