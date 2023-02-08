namespace SymbolicExecution;

public interface IAnalysisState
{
	IExceptionThrownState? CurrentException { get; }
	bool IsReachable { get; }
	IAnalysisState ThrowException(IObjectInstance exception, Location location);
	IAnalysisState AddLocalVariable(ILocalSymbol symbol);
	TaggedUnion<IAnalysisState, AnalysisFailure> SetSymbolValue(ISymbol symbol, IObjectInstance value);
}