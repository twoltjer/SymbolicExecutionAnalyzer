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
			var newLocalVariables = LocalVariables.SetItem(localSymbol, value);
			return new SymbolicExecutionState(CurrentException, newLocalVariables);
		}
		else
		{
			return new AnalysisFailure("Symbol missing from list of local variables", Location.None);
		}
	}

	public TaggedUnion<IObjectInstance, AnalysisFailure> GetSymbolValueOrFailure(ISymbol symbol, Location location)
	{
		if (symbol is not ILocalSymbol localSymbol)
			return new AnalysisFailure("Cannot get the value of a non-local symbol", location);

		if (LocalVariables.TryGetValue(localSymbol, out var value))
		{
			if (value == null)
				return new AnalysisFailure("Cannot get the value of a local variable that has not been initialized", location);
			else
				return new TaggedUnion<IObjectInstance, AnalysisFailure>(value);
		}
		else
		{
			return new AnalysisFailure("Cannot find symbol", location);
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