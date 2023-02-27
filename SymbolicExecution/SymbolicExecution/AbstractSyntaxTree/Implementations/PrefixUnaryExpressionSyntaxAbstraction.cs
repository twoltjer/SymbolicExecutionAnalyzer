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
        if (Children.Length != 1)
            return new AnalysisFailure("Prefix unary expressions must have exactly one child", Location);
        
        if (Children[0] is not IExpressionSyntaxAbstraction childExpression)
            return new AnalysisFailure("Prefix unary expressions must have an expression as a child", Location);
        
        var childResultsOrFailure = childExpression.GetResults(state);
        if (!childResultsOrFailure.IsT1)
            return childResultsOrFailure.T2Value;
        
        var childResults = childResultsOrFailure.T1Value;
        foreach (var (childValueRef, childState) in childResults)
        {
            var value = childState.References[childValueRef];
            var previousValueScope = value.ValueScope as BoolValueScope;
            if (previousValueScope == null)
                return new AnalysisFailure("Cannot apply a prefix unary operator to a non-boolean value", Location);

            var newValueScopeOrFailure = _syntaxKind switch
            {
                SyntaxKind.BitwiseNotExpression => new DerivedBoolValueScope(parentRef: childValueRef, DerivedBoolValueScope.BoolDerivation.NotEquals),
            };
        }
    }
}
