namespace SymbolicExecution;

public class SymbolicExecutionState : IAnalysisState
{
	private SymbolicExecutionState(
		IExceptionThrownState? currentException,
		IImmutableSet<(string name, ITypeSymbol type, IObjectInstance? instance)> localVariables
		)
	{
		CurrentException = currentException;
		LocalVariables = localVariables;
	}

	public IExceptionThrownState? CurrentException { get; }

	public IImmutableSet<(string name, ITypeSymbol type, IObjectInstance? instance)> LocalVariables { get; }
	public bool IsReachable => true;

	public IAnalysisState ThrowException(IObjectInstance exception, Location location)
	{
		var exceptionThrownState = new ExceptionThrownState(exception, location);
		return new SymbolicExecutionState(exceptionThrownState, LocalVariables);
	}

	public IAnalysisState AddLocalVariable(string variableName, ITypeSymbol variableType)
	{
		var newLocalVariables = LocalVariables.Add((variableName, variableType, null));
		return new SymbolicExecutionState(CurrentException, newLocalVariables);
	}

	public static IAnalysisState CreateInitialState()
	{
		return new SymbolicExecutionState(
			currentException: null,
			localVariables: ImmutableHashSet<(string name, ITypeSymbol type, IObjectInstance? instance)>.Empty
			);
	}
}