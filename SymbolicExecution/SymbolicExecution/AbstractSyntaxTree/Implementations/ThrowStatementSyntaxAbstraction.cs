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

		var expressionResultOrFailure = Children[0].GetExpressionResults(previous);
		if (!expressionResultOrFailure.IsT1)
			return expressionResultOrFailure.T2Value;

		var thrownStates = new IAnalysisState[expressionResultOrFailure.T1Value.Length];
		for (var i = 0; i < thrownStates.Length; i++)
		{
			var (resultObject, resultState) = expressionResultOrFailure.T1Value[i];
			var thrownState = resultState.ThrowException(resultObject, Location);
			thrownStates[i] = thrownState;
		}

		return thrownStates;
	}

	public override TaggedUnion<ImmutableArray<(IObjectInstance, IAnalysisState)>, AnalysisFailure> GetExpressionResults(IAnalysisState state)
	{
		return new AnalysisFailure("Cannot analyze throw statements", Location);
	}
}