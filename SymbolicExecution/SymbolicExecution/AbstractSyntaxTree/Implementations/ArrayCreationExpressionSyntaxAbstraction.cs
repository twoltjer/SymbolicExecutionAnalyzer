using System.Numerics;

namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public class ArrayCreationExpressionSyntaxAbstraction : ExpressionSyntaxAbstraction
{
    public ArrayCreationExpressionSyntaxAbstraction(ImmutableArray<ISyntaxNodeAbstraction> children, ISymbol? symbol, Location location, ITypeSymbol? actualTypeSymbol, ITypeSymbol? convertedTypeSymbol) : base(children, symbol, location, actualTypeSymbol, convertedTypeSymbol)
    {
    }

    public override TaggedUnion<IEnumerable<IAnalysisState>, AnalysisFailure> AnalyzeNode(IAnalysisState previous)
    {
        return new AnalysisFailure("Array creation expressions are not supported", Location);
    }

    public override TaggedUnion<ImmutableArray<(IObjectInstance, IAnalysisState)>, AnalysisFailure> GetExpressionResults(IAnalysisState state)
    {
        if (Children.Length != 1)
            return new AnalysisFailure("Array creation expressions must have exactly one child", Location);
        
        var child = Children[0];
        
        if (child is not ArrayTypeSyntaxAbstraction arrayTypeSyntax)
            return new AnalysisFailure("Array creation expressions must have an array type as their child", Location);
        
        var arrayTypeSyntaxChildren = arrayTypeSyntax.Children;
        if (arrayTypeSyntaxChildren.Length != 2)
            return new AnalysisFailure("Array type must have exactly two children", Location);
        
        if (arrayTypeSyntaxChildren[0] is not IPredefinedTypeSyntaxAbstraction predefinedTypeSyntax)
            return new AnalysisFailure("Array type must have a predefined type as its first child", Location);
        
        if (arrayTypeSyntaxChildren[1] is not ArrayRankSpecifierSyntaxAbstraction arrayRankSpecifierSyntax)
            return new AnalysisFailure("Array type must have an array rank specifier as its second child", Location);
        
        var arrayRankSpecifierSyntaxChildren = arrayRankSpecifierSyntax.Children;
        if (arrayRankSpecifierSyntaxChildren.Length != 1)
            return new AnalysisFailure("Array rank specifier must have exactly one child", Location);
        
        var numberOfElements = arrayRankSpecifierSyntaxChildren[0].GetExpressionResults(state);
        if (!numberOfElements.IsT1)
            return numberOfElements.T2Value;
        
        if (predefinedTypeSyntax.ActualTypeSymbol is not ITypeSymbol typeSymbol)
            return new AnalysisFailure("Predefined type must have an associated type symbol", Location);
        
        if (typeSymbol.SpecialType != SpecialType.System_Int32)
            return new AnalysisFailure("Array creation expressions must have an integer type", Location);
        
        var numberOfElementsValue = numberOfElements.T1Value;
        var results = ImmutableArray.CreateBuilder<(IObjectInstance, IAnalysisState)>();
        foreach (var (numberOfElementsObject, derivedState) in numberOfElementsValue)
        {
            var numberOfElementsObjectValue = (numberOfElementsObject.Value as ConstantValueScope)?.Value;
            if (numberOfElementsObjectValue is not int numberOfElementsInt)
                return new AnalysisFailure("Array creation expressions must have a constant number of elements", Location);
            results.Add((new ObjectInstance(typeof(int[]), typeof(int[]), Location, new IntArrayValueScope(new BigInteger[numberOfElementsInt])), derivedState));
        }
        return results.ToImmutable();
    }
}