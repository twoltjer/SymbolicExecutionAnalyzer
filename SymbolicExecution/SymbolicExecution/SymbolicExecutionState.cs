namespace SymbolicExecution;

public class SymbolicExecutionState : IAnalysisState
{
	private SymbolicExecutionState(
		IExceptionThrownState? currentException,
		IImmutableDictionary<int, IObjectInstance> references,
		IImmutableDictionary<ILocalSymbol, int?> localVariables,
		IImmutableDictionary<IParameterSymbol, int?> parameterVariables
		)
	{
		CurrentException = currentException;
		References = references;
		LocalVariables = localVariables;
		ParameterVariables = parameterVariables;
	}

	public IExceptionThrownState? CurrentException { get; }
	public IImmutableDictionary<int, IObjectInstance> References { get; }

	public IImmutableDictionary<ILocalSymbol, int?> LocalVariables { get; }
	public IImmutableDictionary<IParameterSymbol, int?> ParameterVariables { get; }
	public bool IsReachable => true;

	public IAnalysisState ThrowException(int exception, Location location)
	{
		var exceptionThrownState = new ExceptionThrownState(References[exception], location);
		return new SymbolicExecutionState(exceptionThrownState, References, LocalVariables, ParameterVariables);
	}

	public IAnalysisState AddLocalVariable(ILocalSymbol symbol)
	{
		var newLocalVariables = LocalVariables.Add(symbol, null);
		return new SymbolicExecutionState(CurrentException, References, newLocalVariables, ParameterVariables);
	}

	public TaggedUnion<IAnalysisState, AnalysisFailure> SetSymbolReference(ISymbol symbol, int reference)
	{
		if (symbol is ILocalSymbol localSymbol && LocalVariables.ContainsKey(localSymbol))
		{
			var newLocalVariables = LocalVariables.SetItem(localSymbol, reference);
			return new SymbolicExecutionState(CurrentException, References, newLocalVariables, ParameterVariables);
		}
		else if (symbol is IParameterSymbol parameterSymbol && ParameterVariables.ContainsKey(parameterSymbol))
		{
			var newParameterVariables = ParameterVariables.SetItem(parameterSymbol, reference);
			return new SymbolicExecutionState(CurrentException, References, LocalVariables, newParameterVariables);
		}
		else
		{
			return new AnalysisFailure("Symbol missing from list of local variables", Location.None);
		}
	}

	public TaggedUnion<int, AnalysisFailure> GetSymbolReferenceOrFailure(ISymbol symbol, Location location)
	{
		if (symbol is ILocalSymbol localSymbol && LocalVariables.TryGetValue(localSymbol, out var reference))
		{
			if (!reference.HasValue)
				return new AnalysisFailure("Cannot get the value of a local variable that has not been initialized", location);
			else
				return reference.Value;
		}
		else if (symbol is IParameterSymbol parameterSymbol && ParameterVariables.TryGetValue(parameterSymbol, out reference))
		{
			if (!reference.HasValue)
				return new AnalysisFailure("Cannot get the value of a parameter variable that has not been initialized", location);
			else
				return reference.Value;
		}
		else
		{
			return new AnalysisFailure("Cannot find symbol", location);
		}
	}

	public IAnalysisState AddReference(int reference, ObjectInstance objectInstance)
	{
		var newReferences = References.Add(reference, objectInstance);
		return new SymbolicExecutionState(CurrentException, newReferences, LocalVariables, ParameterVariables);
	}

	public static IAnalysisState CreateInitialState()
	{
		return new SymbolicExecutionState(
			currentException: null,
			references: ImmutableDictionary<int, IObjectInstance>.Empty,
			localVariables: ImmutableDictionary<ILocalSymbol, int?>.Empty,
			parameterVariables: ImmutableDictionary<IParameterSymbol, int?>.Empty
			);
	}
}