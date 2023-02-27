namespace SymbolicExecution;

public interface IAnalysisState
{
	IExceptionThrownState? CurrentException { get; }
	IImmutableDictionary<int, IObjectInstance> References { get; }
	IImmutableDictionary<ILocalSymbol, int?> LocalVariables { get; }
	IImmutableDictionary<IParameterSymbol, int?> ParameterVariables { get; }
	TaggedUnion<bool, AnalysisFailure> GetIsReachable(Location location);
	IAnalysisState ThrowException(int exceptionReference, Location location);
	IAnalysisState AddLocalVariable(ILocalSymbol symbol);
	IAnalysisState AddParameterVariable(IParameterSymbol symbol);
	TaggedUnion<int, AnalysisFailure> GetSymbolReferenceOrFailure(ISymbol symbol, Location location);
	IAnalysisState AddReference(int reference, IObjectInstance objectInstance);
	TaggedUnion<IAnalysisState, AnalysisFailure> SetSymbolReference(ISymbol symbol, int reference);
	TaggedUnion<IAnalysisState, AnalysisFailure> AddConstraint(int reference, IConstraint constraint, Location location);
}