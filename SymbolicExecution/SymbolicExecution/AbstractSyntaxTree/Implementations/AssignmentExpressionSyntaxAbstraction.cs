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

		if (Children[0] is not IIdentifierNameSyntaxAbstraction identifier)
			return new AnalysisFailure("Assignment expression must have an identifier as its first child", Location);

		if (identifier.Symbol is not ILocalSymbol localSymbol)
			return new AnalysisFailure("Assignment expression must have a local variable as its first child", Location);

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
			var modifiedStateOrFailure = state.SetSymbolValue(localSymbol, value);
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