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
			setValueOnState = (state, value) => state.SetSymbolValue(localSymbol, value, Location);
		}
		else if (Children[0] is ElementAccessExpressionSyntaxAbstraction elementAccess)
		{
			if (elementAccess.Children.Length != 2)
				return new AnalysisFailure("Element access expression must have exactly two children", Location);
			
			if (elementAccess.Children[0] is not IIdentifierNameSyntaxAbstraction arrayIdentifier)
				return new AnalysisFailure("Element access expression must have an identifier as its first child", Location);
			
			if (arrayIdentifier.Symbol is not ILocalSymbol arrayLocalSymbol)
				return new AnalysisFailure("Element access expression must have a local symbol as its first child", Location);
			
			if (elementAccess.Children[1] is not BracketedArgumentListSyntaxAbstraction bracketedArgumentList)
				return new AnalysisFailure("Element access expression must have a bracketed argument list as its second child", Location);
			
			if (bracketedArgumentList.Children.Length != 1)
				return new AnalysisFailure("Bracketed argument list must have exactly one child", Location);
			
			if (bracketedArgumentList.Children[0] is not IArgumentSyntaxAbstraction argument)
				return new AnalysisFailure("Bracketed argument list must have an argument as its child", Location);
			
			if (argument.Children.Length != 1)
				return new AnalysisFailure("Argument must have exactly one child", Location);
			
			if (argument.Children[0] is not IExpressionSyntaxAbstraction indexExpression)
				return new AnalysisFailure("Argument must have an expression as its child", Location);
			
			var statesAfterIndexEvaluation = indexExpression.GetExpressionResults(previous);
			if (!statesAfterIndexEvaluation.IsT1)
				return statesAfterIndexEvaluation.T2Value;
			
			var statesAfterIndexEvaluationArray = statesAfterIndexEvaluation.T1Value;
			if (statesAfterIndexEvaluationArray.Length != 1)
				return new AnalysisFailure("Element access expression must have exactly one result", Location);
			
			var (indexValue, stateAfterIndexEvaluation) = statesAfterIndexEvaluationArray[0];
			if (indexValue?.Value is not ConstantValueScope constantValueScope)
				return new AnalysisFailure("Element access expression must have a constant value as its index", Location);
			
			if (constantValueScope.Value is not int index)
				return new AnalysisFailure("Element access expression must have an integer as its index", Location);
			
			setValueOnState = (state, value) => state.SetArrayElementValue(arrayLocalSymbol, value, index, Location);
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
			if (state.IsReturning || state.CurrentException != null)
			{
				returnStates[i] = state;
			}
			else
			{
				var modifiedStateOrFailure = setValueOnState(state, value);
				if (!modifiedStateOrFailure.IsT1)
					return modifiedStateOrFailure.T2Value;

				returnStates[i] = modifiedStateOrFailure.T1Value;
			}
		}

		return returnStates;
	}

	public override TaggedUnion<ImmutableArray<(IObjectInstance, IAnalysisState)>, AnalysisFailure> GetExpressionResults(IAnalysisState state)
	{
		return new AnalysisFailure("Cannot get the result of an assignment expression", Location);
	}
}