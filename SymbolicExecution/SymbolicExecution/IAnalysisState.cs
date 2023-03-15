namespace SymbolicExecution;

public interface IAnalysisState
{
	IExceptionThrownState? CurrentException { get; }
	bool IsReachable { get; }
	IImmutableStack<IImmutableDictionary<IParameterSymbol, IObjectInstance?>> ParametersStack { get; }
	IImmutableStack<IImmutableDictionary<ILocalSymbol, IObjectInstance?>> LocalsStack { get; }
	IAnalysisState ThrowException(IObjectInstance exception, Location location);
	IAnalysisState AddLocalVariable(ILocalSymbol symbol);
	TaggedUnion<IAnalysisState, AnalysisFailure> SetSymbolValue(ISymbol symbol, IObjectInstance value);
	TaggedUnion<IObjectInstance, AnalysisFailure> GetSymbolValueOrFailure(ISymbol symbol, Location location);
	IAnalysisState PushStackFrame(
		ImmutableArray<(IParameterSymbol symbol, IObjectInstance value)> toImmutableArray,
		IMethodSymbol? methodSymbol
		);
	IAnalysisState PopStackFrame();
}