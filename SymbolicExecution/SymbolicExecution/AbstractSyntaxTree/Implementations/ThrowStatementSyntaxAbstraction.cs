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

		if (Children[0] is not IExpressionSyntaxAbstraction expressionSyntaxAbstraction)
			return new AnalysisFailure("Throw statement child expected to be an expression", Location);
		var expressionResultOrFailure = expressionSyntaxAbstraction.GetResults(previous);
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
}