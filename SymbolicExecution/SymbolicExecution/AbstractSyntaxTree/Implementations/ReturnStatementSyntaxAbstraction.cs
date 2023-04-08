namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public class ReturnStatementSyntaxAbstraction : StatementSyntaxAbstraction
{
    public ReturnStatementSyntaxAbstraction(ImmutableArray<ISyntaxNodeAbstraction> children, ISymbol? symbol, Location location) : base(children, symbol, location)
    {
    }

    public override TaggedUnion<IEnumerable<IAnalysisState>, AnalysisFailure> AnalyzeNode(IAnalysisState previous)
    {
        if (Children.Length == 0)
        {
            var modifiedState = previous.SetReturnValue(null, Location);
            if (!modifiedState.IsT1)
                return modifiedState.T2Value;
            
            return new TaggedUnion<IEnumerable<IAnalysisState>, AnalysisFailure>(new[] { modifiedState.T1Value });
        }
        
        if (Children.Length > 1)
            return new AnalysisFailure("Return statements can only have one expression", Location);
        
        var expressionResultsOrFailure = Children[0].GetExpressionResults(previous);
        if (!expressionResultsOrFailure.IsT1)
            return expressionResultsOrFailure.T2Value;
        
        var expressionResults = expressionResultsOrFailure.T1Value;
        
        var modifiedStates = new List<IAnalysisState>();
        foreach (var (expressionResult, stateAfterExpression) in expressionResults)
        {
            var modifiedState = stateAfterExpression.SetReturnValue(expressionResult, Location);
            if (!modifiedState.IsT1)
                return modifiedState.T2Value;
            
            modifiedStates.Add(modifiedState.T1Value);
        }

        return new TaggedUnion<IEnumerable<IAnalysisState>, AnalysisFailure>(modifiedStates);
    }

    public override TaggedUnion<ImmutableArray<(IObjectInstance, IAnalysisState)>, AnalysisFailure> GetExpressionResults(IAnalysisState state)
    {
        return new AnalysisFailure("Cannot get the result of a return statement", Location);
    }
}