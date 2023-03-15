using System.Numerics;
using Microsoft.CodeAnalysis.CSharp;

namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public class PostfixUnaryExpressionSyntaxAbstraction : ExpressionSyntaxAbstraction
{
    private readonly SyntaxKind _syntaxKind;

    public PostfixUnaryExpressionSyntaxAbstraction(
        ImmutableArray<ISyntaxNodeAbstraction> children,
        ISymbol? symbol,
        Location location,
        ITypeSymbol? actualTypeSymbol,
        ITypeSymbol? convertedTypeSymbol,
        SyntaxKind syntaxKind
        ) : base(children, symbol, location, actualTypeSymbol, convertedTypeSymbol)
    {
        _syntaxKind = syntaxKind;
    }

    public override TaggedUnion<IEnumerable<IAnalysisState>, AnalysisFailure> AnalyzeNode(IAnalysisState previous)
    {
        var expressionResults = GetExpressionResults(previous);
        if (!expressionResults.IsT1)
            return expressionResults.T2Value;

        var expressionResultStates = expressionResults.T1Value.Select(x => x.Item2).ToImmutableArray();
        return expressionResultStates;
    }

    public override TaggedUnion<ImmutableArray<(IObjectInstance, IAnalysisState)>, AnalysisFailure> GetExpressionResults(IAnalysisState state)
    {
        if (_syntaxKind != SyntaxKind.PostIncrementExpression)
            return new AnalysisFailure("Only postfix increment is supported", Location);
        
        if (Children.Length != 1)
            return new AnalysisFailure("Postfix unary expressions must have exactly one child", Location);
        
        var child = Children[0];
        if (child is not IdentifierNameSyntaxAbstraction)
            return new AnalysisFailure("Postfix unary expressions must have an identifier as a child", Location);
        
        if (child.Symbol is not ILocalSymbol localSymbol)
            return new AnalysisFailure("Postfix unary expressions must have a local variable as a child", Location);

        var childResultOrFailure = state.GetSymbolValueOrFailure(localSymbol, Location);
        if (!childResultOrFailure.IsT1)
            return childResultOrFailure.T2Value;
        
        var childResult = childResultOrFailure.T1Value;
        if (childResult.Value is not ConstantValueScope constantChildValue)
            return new AnalysisFailure("Postfix unary expressions must have constant values", Location);
        
        if (constantChildValue.Value is not int constantChildIntValue)
            return new AnalysisFailure("Postfix unary expressions must have integer values", Location);
        
        var variableBig = new BigInteger(constantChildIntValue);
        var variableBigPlusOne = variableBig + 1;
        if (variableBigPlusOne > int.MaxValue)
        {
            // Overflow
            var exception = new ObjectInstance(
                typeof(OverflowException),
                typeof(OverflowException),
                Location,
                new ReferenceTypeScope(typeof(OverflowException))
                );
            var exceptionState = state.ThrowException(exception, Location);
            return ImmutableArray.Create((default(IObjectInstance)!, exceptionState));
        }
        else
        {
            var variablePlusOne = (int) variableBigPlusOne;
            var variablePlusOneObject = new ObjectInstance(
                typeof(int),
                typeof(int),
                Location,
                new ConstantValueScope(variablePlusOne, typeof(int))
                );
            var variablePlusOneStateOrFailure = state.SetSymbolValue(localSymbol, variablePlusOneObject, Location);
            if (!variablePlusOneStateOrFailure.IsT1)
                return variablePlusOneStateOrFailure.T2Value;
            
            var variablePlusOneState = variablePlusOneStateOrFailure.T1Value;

            return ImmutableArray.Create((variablePlusOneObject as IObjectInstance, variablePlusOneState));
        }

    }
}