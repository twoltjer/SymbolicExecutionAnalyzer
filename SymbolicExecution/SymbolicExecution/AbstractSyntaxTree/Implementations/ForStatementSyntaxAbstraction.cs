namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public class ForStatementSyntaxAbstraction : StatementSyntaxAbstraction
{
    public ForStatementSyntaxAbstraction(ImmutableArray<ISyntaxNodeAbstraction> children, ISymbol? symbol, Location location) : base(children, symbol, location)
    {
    }

    public override TaggedUnion<IEnumerable<IAnalysisState>, AnalysisFailure> AnalyzeNode(IAnalysisState previous)
    {
        if (Children.Length != 4)
            return new AnalysisFailure("For statements must have four children", Location);
        
        var initializationResultsOrFailure = Children[0].AnalyzeNode(previous);
        if (!initializationResultsOrFailure.IsT1)
            return initializationResultsOrFailure.T2Value;
        
        var initializationResults = initializationResultsOrFailure.T1Value;

        var previousStates = new List<IAnalysisState>(initializationResults);
        var incomplete = new List<IAnalysisState>();
        var complete = new List<IAnalysisState>();
        const int maxIterations = 20_000;
        for (var i = 0; previousStates.Any(); i++)
        {
            if (i >= maxIterations)
                return new AnalysisFailure("For statement has too many iterations", Location);
            foreach (var previousState in previousStates)
            {
                var conditionResultsOrFailure = Children[1].GetExpressionResults(previousState);
                if (!conditionResultsOrFailure.IsT1)
                    return conditionResultsOrFailure.T2Value;
                
                var conditionResults = conditionResultsOrFailure.T1Value;
                foreach (var (conditionResult, stateAfterCondition) in conditionResults)
                {
                    var conditionResultConstantValue = conditionResult?.Value as ConstantValueScope;
                    if (conditionResultConstantValue == null)
                        return new AnalysisFailure("Condition of for statement must be a constant value", Location);
                    
                    if (conditionResultConstantValue.Value is not bool conditionResultValue)
                        return new AnalysisFailure("Condition of for statement must be a boolean", Location);

                    if (conditionResultValue)
                    {
                        var blockResultsOrFailure = Children[3].AnalyzeNode(stateAfterCondition);
                        if (!blockResultsOrFailure.IsT1)
                            return blockResultsOrFailure.T2Value;

                        var blockResults = blockResultsOrFailure.T1Value;
                        foreach (var blockResult in blockResults)
                        {
                            if (blockResult.IsReturning || blockResult.CurrentException != null)
                            {
                                complete.Add(blockResult);
                            }
                            else
                            {
                                var incrementResultsOrFailure = Children[2].AnalyzeNode(blockResult);
                                if (!incrementResultsOrFailure.IsT1)
                                    return incrementResultsOrFailure.T2Value;

                                incomplete.AddRange(incrementResultsOrFailure.T1Value);
                            }
                        }
                    }
                    else
                    {
                        complete.Add(stateAfterCondition);
                    }
                }
            }
            
            previousStates = incomplete;
            incomplete = new List<IAnalysisState>();
        }
        
        return complete;
    }

    public override TaggedUnion<ImmutableArray<(IObjectInstance, IAnalysisState)>, AnalysisFailure> GetExpressionResults(IAnalysisState state)
    {
        return new AnalysisFailure("Cannot get the result of a for statement", Location);
    }
}