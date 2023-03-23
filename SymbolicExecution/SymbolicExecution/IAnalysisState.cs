namespace SymbolicExecution;

/// <summary>
/// Represents the current state of symbolic execution for a single branch of logic
/// </summary>
public interface IAnalysisState
{
	/// <summary>
	/// Set to the exception state if this state is in the midst of throwing an exception
	/// </summary>
	IExceptionThrownState? CurrentException { get; }
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