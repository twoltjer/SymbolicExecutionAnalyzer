namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public class ParameterSyntaxAbstraction : BaseParameterSyntaxAbstraction
{
    public ParameterSyntaxAbstraction(ImmutableArray<ISyntaxNodeAbstraction> children, ISymbol? symbol, Location location) : base(children, symbol, location)
    {
    }

    public override TaggedUnion<IEnumerable<IAnalysisState>, AnalysisFailure> AnalyzeNode(IAnalysisState previous)
    {
        return new AnalysisFailure("Parameter syntax is not supported", Location);
    }

    public override TaggedUnion<ImmutableArray<(IObjectInstance, IAnalysisState)>, AnalysisFailure> GetExpressionResults(IAnalysisState state)
    {
        return new AnalysisFailure("Cannot get the result of a parameter", Location);
    }
}