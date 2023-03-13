namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public class ForStatementSyntaxAbstraction : StatementSyntaxAbstraction
{
    public ForStatementSyntaxAbstraction(ImmutableArray<ISyntaxNodeAbstraction> children, ISymbol? symbol, Location location) : base(children, symbol, location)
    {
    }

    public override TaggedUnion<IEnumerable<IAnalysisState>, AnalysisFailure> AnalyzeNode(IAnalysisState previous)
    {
        return new AnalysisFailure("For statements are not supported", Location);
    }

    public override TaggedUnion<ImmutableArray<(IObjectInstance, IAnalysisState)>, AnalysisFailure> GetExpressionResults(IAnalysisState state)
    {
        return new AnalysisFailure("Cannot get the result of a for statement", Location);
    }
}