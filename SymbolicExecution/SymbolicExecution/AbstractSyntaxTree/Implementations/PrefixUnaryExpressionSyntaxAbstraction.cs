using Microsoft.CodeAnalysis.CSharp;

namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public class PrefixUnaryExpressionSyntaxAbstraction : ExpressionSyntaxAbstraction, IPrefixUnaryExpressionSyntaxAbstraction
{
    private readonly SyntaxKind _syntaxKind;

    public PrefixUnaryExpressionSyntaxAbstraction(ImmutableArray<ISyntaxNodeAbstraction> children, ISymbol? symbol,
        Location location, ITypeSymbol? typeSymbol, SyntaxKind syntaxKind) : base(children, symbol, location, typeSymbol)
    {
        _syntaxKind = syntaxKind;
    }

    public override TaggedUnion<IEnumerable<IAnalysisState>, AnalysisFailure> AnalyzeNode(IAnalysisState previous)
    {
        return new AnalysisFailure("Cannot analyze prefix unary expressions", Location);
    }

    public override TaggedUnion<ImmutableArray<(int, IAnalysisState)>, AnalysisFailure> GetResults(IAnalysisState state)
    {
        return new AnalysisFailure("Cannot analyze prefix unary expressions", Location);
    }
}