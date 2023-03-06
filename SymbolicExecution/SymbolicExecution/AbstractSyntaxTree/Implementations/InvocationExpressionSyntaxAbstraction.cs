namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public class InvocationExpressionSyntaxAbstraction : ExpressionSyntaxAbstraction, IInvocationExpressionSyntaxAbstraction
{
    public InvocationExpressionSyntaxAbstraction(
        ImmutableArray<ISyntaxNodeAbstraction> children,
        ISymbol? symbol,
        Location location,
        ITypeSymbol? typeSymbol,
        SymbolInfo symbolInfo
        ) : base(children, symbol, location, typeSymbol)
    {
    }

    public override TaggedUnion<IEnumerable<IAnalysisState>, AnalysisFailure> AnalyzeNode(IAnalysisState previous)
    {
        return new AnalysisFailure("Cannot analyze invocation expressions", Location);
    }

    public override TaggedUnion<ImmutableArray<(int, IAnalysisState)>, AnalysisFailure> GetResults(IAnalysisState state)
    {
        return new AnalysisFailure("Cannot get results from invocation expressions", Location);
    }
}