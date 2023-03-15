namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public class AssignmentExpressionSyntaxAbstraction : ExpressionSyntaxAbstraction, IAssignmentExpressionSyntaxAbstraction
{
	public AssignmentExpressionSyntaxAbstraction(ImmutableArray<ISyntaxNodeAbstraction> children, ISymbol? symbol, Location location, ITypeSymbol? actualTypeSymbol, ITypeSymbol? convertedTypeSymbol) : base(children, symbol, location, actualTypeSymbol, convertedTypeSymbol)
	{
	}

	public override TaggedUnion<IEnumerable<IAnalysisState>, AnalysisFailure> AnalyzeNode(IAnalysisState previous)
	{
		if (Children.Length != 2)
			return new AnalysisFailure("Assignment expression must have exactly two children", Location);

		Func<IAnalysisState, IObjectInstance, TaggedUnion<IAnalysisState, AnalysisFailure>> setValueOnState;
		if (Children[0] is IIdentifierNameSyntaxAbstraction identifier && identifier.Symbol is ILocalSymbol localSymbol)
		{
			setValueOnState = (state, value) => state.SetSymbolValue(localSymbol, value);
		}
		else if (Children[0] is ElementAccessExpressionSyntaxAbstraction)
		{
			return new AnalysisFailure("Cannot assign to an element access expression", Location);
		}
		else
		{
			return new AnalysisFailure("Cannot assign to a non-identifier expression", Location);
		}

		if (Children[1] is not IExpressionSyntaxAbstraction expression)
			return new AnalysisFailure("Assignment expression must have an expression as its second child", Location);

		var resultsOrFailure = expression.GetExpressionResults(previous);
		if (!resultsOrFailure.IsT1)
			return resultsOrFailure.T2Value;

		var results = resultsOrFailure.T1Value;
		var returnStates = new IAnalysisState[results.Length];
		for (var i = 0; i < results.Length; i++)
		{
			var (value, state) = results[i];
			var modifiedStateOrFailure = setValueOnState(state, value);
			if (!modifiedStateOrFailure.IsT1)
				return modifiedStateOrFailure.T2Value;

			returnStates[i] = modifiedStateOrFailure.T1Value;
		}

		return returnStates;
	}

	public override TaggedUnion<ImmutableArray<(IObjectInstance, IAnalysisState)>, AnalysisFailure> GetExpressionResults(IAnalysisState state)
	{
		return new AnalysisFailure("Cannot get the result of an assignment expression", Location);
	}
}