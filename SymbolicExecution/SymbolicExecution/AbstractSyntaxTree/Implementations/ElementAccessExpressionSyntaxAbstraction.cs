namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public class ElementAccessExpressionSyntaxAbstraction : ExpressionSyntaxAbstraction
{
    public ElementAccessExpressionSyntaxAbstraction(ImmutableArray<ISyntaxNodeAbstraction> children, ISymbol? symbol, Location location, ITypeSymbol? actualTypeSymbol, ITypeSymbol? convertedTypeSymbol) : base(children, symbol, location, actualTypeSymbol, convertedTypeSymbol)
    {
    }

    public override TaggedUnion<IEnumerable<IAnalysisState>, AnalysisFailure> AnalyzeNode(IAnalysisState previous)
    {
        return new AnalysisFailure("Element access expressions are not supported", Location);
    }

    public override TaggedUnion<ImmutableArray<(IObjectInstance, IAnalysisState)>, AnalysisFailure> GetExpressionResults(IAnalysisState state)
    {
        if (Children.Length != 2)
            return new AnalysisFailure("Element access expressions must have exactly two children", Location);

        var right = Children[1];
        
        if (Children[0] is not IdentifierNameSyntaxAbstraction identifierNameSyntaxAbstraction)
            return new AnalysisFailure("Element access expressions must have an identifier on the left", Location);
        
        if (right is not BracketedArgumentListSyntaxAbstraction bracketedArgumentListSyntaxAbstraction)
            return new AnalysisFailure("Element access expressions must have a bracketed argument list on the right", Location);
        
        if (bracketedArgumentListSyntaxAbstraction.Children.Length != 1)
            return new AnalysisFailure("Element access expressions must have exactly one argument in the bracketed argument list", Location);
        
        var argument = bracketedArgumentListSyntaxAbstraction.Children[0];
        if (argument is not ArgumentSyntaxAbstraction argumentSyntaxAbstraction)
            return new AnalysisFailure("Element access expressions must have an argument in the bracketed argument list", Location);
        
        var argumentExpression = argumentSyntaxAbstraction.Children[0];
        var indexObjectsOrFailure = argumentExpression.GetExpressionResults(state);
        if (!indexObjectsOrFailure.IsT1)
            return indexObjectsOrFailure.T2Value;
        
        if (identifierNameSyntaxAbstraction.Symbol is not ILocalSymbol localArraySymbol)
            return new AnalysisFailure("Element access expressions must have a local symbol on the left", Location);
        
        var arrayObjectOrFailure = state.GetSymbolValueOrFailure(localArraySymbol, Location);
        if (!arrayObjectOrFailure.IsT1)
            return arrayObjectOrFailure.T2Value;
        
        var arrayObject = arrayObjectOrFailure.T1Value;
        
        var indexObjects = indexObjectsOrFailure.T1Value;
        var results = ImmutableArray.CreateBuilder<(IObjectInstance, IAnalysisState)>();
        foreach (var (indexObject, indexState) in indexObjects)
        {
            if (indexObject.Value is not ConstantValueScope constantIndexValue)
                return new AnalysisFailure("Element access expressions must have constant values on the right", Location);
            
            var indexValue = constantIndexValue.Value;
            if (indexValue is not int indexIntValue)
                return new AnalysisFailure("Element access expressions must have integer values on the right", Location);
            
            if (arrayObject.Value is not IntArrayValueScope arrayScope)
                return new AnalysisFailure("Element access expressions must have an array on the left", Location);

            if (indexIntValue < 0 || indexIntValue >= arrayScope.Values.Length)
            {
                var exception = new ObjectInstance(
                    typeof(IndexOutOfRangeException),
                    typeof(IndexOutOfRangeException),
                    Location,
                    new ReferenceTypeScope(typeof(IndexOutOfRangeException))
                    );
                var throwState = indexState.ThrowException(exception, Location);
                results.Add((null!, throwState));
            }
            else
            {
                var arrayValue = (int)arrayScope.Values[indexIntValue];
                var arrayValueObject = new ObjectInstance(
                    typeof(int),
                    typeof(int),
                    Location,
                    new ConstantValueScope(arrayValue, typeof(int))
                    );
                results.Add((arrayValueObject, indexState));
            }
        }
        return results.ToImmutable();
    }
}