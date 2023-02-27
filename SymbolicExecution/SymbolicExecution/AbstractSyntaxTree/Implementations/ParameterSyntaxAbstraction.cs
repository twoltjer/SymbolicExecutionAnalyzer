namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public class ParameterSyntaxAbstraction : BaseParameterSyntaxAbstraction, IParameterSyntaxAbstraction
{
    public ParameterSyntaxAbstraction(ImmutableArray<ISyntaxNodeAbstraction> children, ISymbol? symbol, Location location) : base(children, symbol, location)
    {
    }

    public override TaggedUnion<IEnumerable<IAnalysisState>, AnalysisFailure> AnalyzeNode(IAnalysisState previous)
    {
        if (Symbol is not IParameterSymbol parameterSymbol)
        {
            return new AnalysisFailure(
                "ParameterSyntaxAbstraction.AnalyzeNode: Symbol is not IParameterSymbol",
                Location
                );
        }

        var newState = previous.AddParameterVariable(parameterSymbol);
        var valueOrFailure = parameterSymbol.Type.SpecialType switch
        {
            SpecialType.System_Boolean => new TaggedUnion<IObjectInstance, AnalysisFailure>(
                new BoolInstance(
                    Location,
                    new AnyBoolValueScope(),
                    ObjectInstance.GetNextReferenceId()
                    )
                ),
            _ => new AnalysisFailure("ParameterSyntaxAbstraction.AnalyzeNode: Unknown parameter type", Location)
        };

        if (!valueOrFailure.IsT1)
            return valueOrFailure.T2Value;

        var objectInstance = valueOrFailure.T1Value;
        var newStateWithReferenceOrFailure = newState.AddReference(objectInstance.Reference, objectInstance)
            .SetSymbolReference(parameterSymbol, objectInstance.Reference);
        if (!newStateWithReferenceOrFailure.IsT1)
            return newStateWithReferenceOrFailure.T2Value;
        return new[] { newStateWithReferenceOrFailure.T1Value };
    }
}