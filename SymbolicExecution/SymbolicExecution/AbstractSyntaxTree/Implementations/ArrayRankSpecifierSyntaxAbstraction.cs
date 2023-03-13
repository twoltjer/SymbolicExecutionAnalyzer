namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public class ArrayRankSpecifierSyntaxAbstraction : CSharpSyntaxNodeAbstraction
{
    public ArrayRankSpecifierSyntaxAbstraction(ImmutableArray<ISyntaxNodeAbstraction> children, ISymbol? symbol, Location location) : base(children, symbol, location)
    {
    }

    public override TaggedUnion<IEnumerable<IAnalysisState>, AnalysisFailure> AnalyzeNode(IAnalysisState previous)
    {
        return new AnalysisFailure("Array rank specifiers are not supported", Location);
    }

    public override TaggedUnion<ImmutableArray<(IObjectInstance, IAnalysisState)>, AnalysisFailure> GetExpressionResults(IAnalysisState state)
    {
        return new AnalysisFailure("Cannot get the result of an array rank specifier", Location);
    }
}