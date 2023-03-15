namespace SymbolicExecution;

public interface IAnalysisState
{
	IExceptionThrownState? CurrentException { get; }
	bool IsReachable { get; }
	IImmutableStack<IImmutableDictionary<IParameterSymbol, IObjectInstance?>> ParametersStack { get; }
	IImmutableStack<IImmutableDictionary<ILocalSymbol, IObjectInstance?>> LocalsStack { get; }
	IObjectInstance? ReturningValue { get; }
	bool IsReturning { get; }
	IAnalysisState ThrowException(IObjectInstance exception, Location location);
	IAnalysisState AddLocalVariable(ILocalSymbol symbol);
	TaggedUnion<IAnalysisState, AnalysisFailure> SetSymbolValue(ISymbol symbol, IObjectInstance value, Location location);
	TaggedUnion<IObjectInstance, AnalysisFailure> GetSymbolValueOrFailure(ISymbol symbol, Location location);
	IAnalysisState PushStackFrame(
		ImmutableArray<(IParameterSymbol symbol, IObjectInstance value)> toImmutableArray,
		IMethodSymbol methodSymbol
		);

	(IAnalysisState state, IObjectInstance? returnValue) PopStackFrame();
	TaggedUnion<IAnalysisState, AnalysisFailure> SetArrayElementValue(
		ILocalSymbol symbol,
		IObjectInstance value,
		int index,
		Location location
		);
	TaggedUnion<IAnalysisState, AnalysisFailure> SetReturnValue(IObjectInstance? value, Location location);
	TaggedUnion<IAnalysisState, AnalysisFailure> ReviseException(IExceptionThrownState newException);
	IMethodSymbol CurrentMethod { get; }
}