namespace SymbolicExecution;

public readonly struct SymbolicExecutionState : IAnalysisState
{
	public SymbolicExecutionState(IExceptionThrownState? currentException)
	{
		CurrentException = currentException;
	}

	public IExceptionThrownState? CurrentException { get; }
	public IAnalysisState ThrowException(ObjectInstance exception, Location location)
	{
		var exceptionThrownState = new ExceptionThrownState(exception, location);
		return new SymbolicExecutionState(exceptionThrownState);
	}
}