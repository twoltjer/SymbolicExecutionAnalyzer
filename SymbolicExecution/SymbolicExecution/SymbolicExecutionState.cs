namespace SymbolicExecution;

public class SymbolicExecutionState : IAnalysisState
{
	private SymbolicExecutionState(
		IExceptionThrownState? currentException,
		IImmutableDictionary<ILocalSymbol, IObjectInstance?> localVariables
		)
	{
		CurrentException = currentException;
		LocalVariables = localVariables;
	}

	public IExceptionThrownState? CurrentException { get; }

	public IImmutableDictionary<ILocalSymbol, IObjectInstance?> LocalVariables { get; }
	public bool IsReachable => true;

	public IAnalysisState ThrowException(IObjectInstance exception, Location location)
	{
		var exceptionThrownState = new ExceptionThrownState(exception, location);
		return new SymbolicExecutionState(exceptionThrownState, LocalVariables);
	}

	public IAnalysisState AddLocalVariable(ILocalSymbol symbol)
	{
		var newLocalVariables = LocalVariables.Add(symbol, null);
		return new SymbolicExecutionState(CurrentException, newLocalVariables);
	}

	public TaggedUnion<IAnalysisState, AnalysisFailure> SetSymbolValue(ISymbol symbol, IObjectInstance value)
	{
		if (symbol is not ILocalSymbol localSymbol)
			return new AnalysisFailure("Cannot set the value of a non-local symbol", Location.None);

		if (LocalVariables.ContainsKey(localSymbol))
		{
			var newLocalVariables = LocalVariables.SetItem((ILocalSymbol)symbol, value);
			return new SymbolicExecutionState(CurrentException, newLocalVariables);
		}
		else
		{
			return new AnalysisFailure("Cannot set the value of a non-local symbol", Location.None);
		}
	}

	public static IAnalysisState CreateInitialState()
	{
		return new SymbolicExecutionState(
			currentException: null,
			localVariables: ImmutableDictionary<ILocalSymbol, IObjectInstance?>.Empty
			);
	}
}