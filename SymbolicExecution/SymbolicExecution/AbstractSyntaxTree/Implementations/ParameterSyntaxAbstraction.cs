namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public class ParameterSyntaxAbstraction : BaseParameterSyntaxAbstraction, IParameterSyntaxAbstraction
{
    public ParameterSyntaxAbstraction(ImmutableArray<ISyntaxNodeAbstraction> children, ISymbol? symbol, Location location) : base(children, symbol, location)
    {
    }

    public override TaggedUnion<IEnumerable<IAnalysisState>, AnalysisFailure> AnalyzeNode(IAnalysisState previous)
    {
        return new AnalysisFailure("Cannot analyze parameters", Location);
    }
}