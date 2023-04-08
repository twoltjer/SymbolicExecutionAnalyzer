using System.Numerics;
using Microsoft.CodeAnalysis.CSharp;

namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public class BinaryExpressionSyntaxAbstraction : ExpressionSyntaxAbstraction
{
    private readonly SyntaxKind _syntaxKind;

    public BinaryExpressionSyntaxAbstraction(
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
        return new AnalysisFailure("Binary expressions are not supported", Location);
    }

    public override TaggedUnion<ImmutableArray<(IObjectInstance, IAnalysisState)>, AnalysisFailure> GetExpressionResults(IAnalysisState state)
    {
        if (Children.Length != 2)
            return new AnalysisFailure("Binary expressions must have exactly two children", Location);
        
        var left = Children[0];
        var right = Children[1];
        
        var leftValues = left.GetExpressionResults(state); 
        if (!leftValues.IsT1)
            return leftValues.T2Value;
        
        var twoIntToIntSyntaxKinds = new[]
        {
            SyntaxKind.AddExpression, SyntaxKind.SubtractExpression, SyntaxKind.MultiplyExpression,
            SyntaxKind.DivideExpression, SyntaxKind.ModuloExpression
        };
        var twoIntToBoolSyntaxKinds = new[]
            { SyntaxKind.LessThanOrEqualExpression, SyntaxKind.LessThanExpression };

        var results = ImmutableArray.CreateBuilder<(IObjectInstance, IAnalysisState)>();
        foreach (var (leftValueObject, leftState) in leftValues.T1Value)
        {
            if (leftValueObject.Value is not ConstantValueScope constantLeftValue)
                return new AnalysisFailure("Binary expressions must have constant values on the left", Location);
            
            var rightValues = right.GetExpressionResults(leftState);
            if (!rightValues.IsT1)
                return rightValues.T2Value;
            
            foreach (var (rightValueObject, rightState) in rightValues.T1Value)
            {
                if (rightValueObject.Value is not ConstantValueScope constantRightValue)
                    return new AnalysisFailure("Binary expressions must have constant values on the right", Location);
                
                var leftValue = constantLeftValue.Value;
                var rightValue = constantRightValue.Value;
                
                if (leftValue is not int leftValueInt)
                    return new AnalysisFailure("Binary expressions must have integer values on the left", Location);

                if (rightValue is not int rightValueInt)
                    return new AnalysisFailure(
                        "Binary expressions must have integer values on the right",
                        Location
                        );

                var leftBig = new BigInteger(leftValueInt);
                var rightBig = new BigInteger(rightValueInt);

                if (twoIntToIntSyntaxKinds.Contains(_syntaxKind))
                {
                    var resultOrAnalysisFailure = _syntaxKind switch
                    {
                        SyntaxKind.AddExpression => leftBig + rightBig,
                        SyntaxKind.SubtractExpression => leftBig - rightBig,
                        SyntaxKind.MultiplyExpression => leftBig * rightBig,
                        SyntaxKind.DivideExpression => leftBig / rightBig,
                        SyntaxKind.ModuloExpression => leftBig % rightBig,
                        _ => new TaggedUnion<BigInteger, AnalysisFailure>(
                            new AnalysisFailure("Expression kind not a supported binary expression", Location)
                            )
                    };

                    if (!resultOrAnalysisFailure.IsT1)
                        return resultOrAnalysisFailure.T2Value;

                    var result = resultOrAnalysisFailure.T1Value;

                    if (result > int.MaxValue)
                    {
                        // Overflow, create and throw an exception
                        var exception = new ObjectInstance(
                            typeof(OverflowException),
                            typeof(OverflowException),
                            Location,
                            new ReferenceTypeScope(typeof(OverflowException))
                            );
                        var returnState = rightState.ThrowException(exception, Location);
                        results.Add((null, returnState)!);
                    }
                    else if (result < int.MinValue)
                    {
                        // Overflow, create and throw an exception
                        var exception = new ObjectInstance(
                            typeof(OverflowException),
                            typeof(OverflowException),
                            Location,
                            new ReferenceTypeScope(typeof(OverflowException))
                            );
                        var returnState = rightState.ThrowException(exception, Location);
                        results.Add((null, returnState)!);
                    }
                    else
                    {
                        var intValue = (int)result;
                        // Ensure that the result is an integer
                        Debug.Assert(new BigInteger(intValue) == result);
                        var resultObject = new ObjectInstance(
                            typeof(int),
                            typeof(int),
                            Location,
                            new ConstantValueScope(intValue, typeof(int))
                            );

                        results.Add((resultObject, rightState));
                    }
                }
                else if (twoIntToBoolSyntaxKinds.Contains(_syntaxKind))
                {
                    var resultOrAnalysisFailure = _syntaxKind switch
                    {
                        SyntaxKind.LessThanOrEqualExpression => leftBig <= rightBig,
                        SyntaxKind.LessThanExpression => leftBig < rightBig,
                        _ => new TaggedUnion<bool, AnalysisFailure>(
                            new AnalysisFailure("Expression kind not a supported binary expression", Location)
                            )
                    };
                    
                    if (!resultOrAnalysisFailure.IsT1)
                        return resultOrAnalysisFailure.T2Value;
                    
                    var result = resultOrAnalysisFailure.T1Value;
                    var resultObject = new ObjectInstance(
                        typeof(bool),
                        typeof(bool),
                        Location,
                        new ConstantValueScope(result, typeof(bool))
                        );
                    
                    results.Add((resultObject, rightState));
                }
                else
                {
                    return new AnalysisFailure("Expression kind not a supported binary expression", Location);
                }
            }
        }
        
        return results.ToImmutable();
    }
}