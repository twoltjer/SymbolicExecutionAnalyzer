namespace SymbolicExecution;

/// <summary>
/// Represents the current state of symbolic execution for a single branch of logic. Immutable, so any methods that
/// modify the state return a new instance of the state.
/// </summary>
public interface IAnalysisState
{
	/// <summary>
	/// Set to the exception state if this state is in the midst of throwing an exception
	/// </summary>
	IExceptionThrownState? CurrentException { get; }
	
	/// <summary>
	/// The stack of parameter dictionaries for methods on the call stack. Each dictionary maps a parameter symbol to
	/// its value.
	/// </summary>
	IImmutableStack<IImmutableDictionary<IParameterSymbol, IObjectInstance?>> ParametersStack { get; }
	
	/// <summary>
	/// The stack of local variable dictionaries for methods on the call stack. Each dictionary maps a local variable
	/// to its value.
	/// </summary>
	IImmutableStack<IImmutableDictionary<ILocalSymbol, IObjectInstance?>> LocalsStack { get; }
	
	/// <summary>
	/// If this state is in the midst of returning a value, this is the value that is being returned. May be null if
	/// not returning or, the return value is null, or returning void.
	/// </summary>
	IObjectInstance? ReturningValue { get; }
	
	/// <summary>
	/// True if this state is in the midst of returning a value
	/// </summary>
	bool IsReturning { get; }
	
	/// <summary>
	/// Invoked when the user code throws an exception. Sets the state of the returned IAnalysisState to be in the
	/// midst of throwing the given exception object at the given location.
	/// </summary>
	/// <param name="exception">The exception object that was thrown</param>
	/// <param name="location">The location at which the exception was thrown</param>
	/// <returns>A new state with the exception thrown</returns>
	IAnalysisState ThrowException(IObjectInstance exception, Location location);
	
	/// <summary>
	/// Adds a local variable to the current scope of the state and returns a new state with the variable added.
	/// </summary>
	/// <param name="symbol">The symbol for the local variable</param>
	/// <returns>A new state with the local variable added</returns>
	IAnalysisState AddLocalVariable(ILocalSymbol symbol);
	
	/// <summary>
	/// Sets the value of a local variable in the current scope of the state and returns a new state with the value set.
	/// </summary>
	/// <param name="symbol">The symbol for the local variable</param>
	/// <param name="value">The value to set</param>
	/// <param name="location">The location at which the value is set</param>
	/// <returns>A new state with the value set</returns>
	TaggedUnion<IAnalysisState, AnalysisFailure> SetSymbolValue(ISymbol symbol, IObjectInstance value, Location location);
	
	/// <summary>
	/// Attempts to get the value of a local or parameter variable in the current scope of the state. If the variable
	/// is not found, returns a failure.
	/// </summary>
	/// <param name="symbol">The symbol for the local or parameter variable</param>
	/// <param name="location">The location at which the value is being retrieved</param>
	/// <returns>The value of the variable, or a failure if the variable is not found</returns>
	TaggedUnion<IObjectInstance, AnalysisFailure> GetSymbolValueOrFailure(ISymbol symbol, Location location);

	/// <summary>
	/// Pushes a new stack frame onto the stack of stack frames, and returns a new state with the stack frame pushed.
	/// </summary>
	/// <param name="parameters">The parameters to the method</param>
	/// <param name="methodSymbol">The method symbol</param>
	/// <returns>A new state with the stack frame pushed</returns>
	IAnalysisState PushStackFrame(
		ImmutableArray<(IParameterSymbol symbol, IObjectInstance value)> parameters,
		IMethodSymbol methodSymbol
		);

	/// <summary>
	/// Pops the current stack frame from the stack of stack frames, and returns a new state with the stack frame
	/// popped.
	/// </summary>
	/// <param name="state">The state to pop the stack frame from</param>
	/// <param name="returnValue">The return value of the method, if any</param>
	/// <returns>A new state with the stack frame popped</returns>
	(IAnalysisState state, IObjectInstance? returnValue) PopStackFrame();
	
	/// <summary>
	/// Sets the value of an array element and returns a new state with the value set.
	/// </summary>
	/// <param name="symbol">The symbol for the array</param>
	/// <param name="value">The value to set</param>
	/// <param name="index">The index of the element to set</param>
	/// <param name="location">The location at which the value is set</param>
	/// <returns>A new state with the value set</returns>
	TaggedUnion<IAnalysisState, AnalysisFailure> SetArrayElementValue(
		ILocalSymbol symbol,
		IObjectInstance value,
		int index,
		Location location
		);
	
	/// <summary>
	/// Creates a duplicate state which is returning with the given value.
	/// </summary>
	/// <param name="value">The value to return</param>
	/// <param name="location">The location at which the value is being returned</param>
	/// <returns>A new state with the value set</returns>
	TaggedUnion<IAnalysisState, AnalysisFailure> SetReturnValue(IObjectInstance? value, Location location);
	
	/// <summary>
	/// Creates a duplicate state which has a revised IExceptionThrownState. Returns an analysis failure if the
	/// IExceptionThrownState is not set in the current state.
	/// </summary>
	/// <param name="newException"></param>
	/// <returns></returns>
	TaggedUnion<IAnalysisState, AnalysisFailure> ReviseException(IExceptionThrownState newException);
	
	/// <summary>
	/// The method symbol for the method currently being executed
	/// </summary>
	IMethodSymbol CurrentMethod { get; }
}