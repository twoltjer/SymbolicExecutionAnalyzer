namespace SymbolicExecution;

public interface IAnalysisState
{
	IExceptionThrownState? CurrentException { get; }
	IImmutableDictionary<int, IObjectInstance> References { get; }
	IImmutableDictionary<ILocalSymbol, int?> LocalVariables { get; }
	IImmutableDictionary<IParameterSymbol, int?> ParameterVariables { get; }
	bool IsReachable { get; }
	IAnalysisState ThrowException(int exceptionReference, Location location);
	IAnalysisState AddLocalVariable(ILocalSymbol symbol);
	TaggedUnion<int, AnalysisFailure> GetSymbolReferenceOrFailure(ISymbol symbol, Location location);
	IAnalysisState AddReference(int reference, ObjectInstance objectInstance);
	TaggedUnion<IAnalysisState, AnalysisFailure> SetSymbolReference(ISymbol symbol, int reference);
}