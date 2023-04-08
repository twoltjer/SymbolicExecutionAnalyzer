namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public class MemberAccessExpressionSyntaxAbstraction : ExpressionSyntaxAbstraction
{
    public MemberAccessExpressionSyntaxAbstraction(
        ImmutableArray<ISyntaxNodeAbstraction> children,
        ISymbol? symbol,
        Location location,
        ITypeSymbol? actualTypeSymbol,
        ITypeSymbol? convertedTypeSymbol
        ) : base(children, symbol, location, actualTypeSymbol, convertedTypeSymbol)
    {
    }

    public override TaggedUnion<IEnumerable<IAnalysisState>, AnalysisFailure> AnalyzeNode(IAnalysisState previous)
    {
        return new AnalysisFailure("MemberAccessExpressionSyntax is not supported", Location);
    }

    public override TaggedUnion<ImmutableArray<(IObjectInstance, IAnalysisState)>, AnalysisFailure>
        GetExpressionResults(IAnalysisState state)
    {
        return new AnalysisFailure("Cannot get the result of a MemberAccessExpressionSyntax", Location);
    }
}