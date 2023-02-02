namespace SymbolicExecution;

public readonly struct SymbolicExecutionState : IAnalysisState
{
	public SymbolicExecutionState(IExceptionThrownState? currentException)
	{
		CurrentException = currentException;
	}

	public IExceptionThrownState? CurrentException { get; }
	public bool IsReachable => true;

	public IAnalysisState ThrowException(IObjectInstance exception, Location location)
	{
		var exceptionThrownState = new ExceptionThrownState(exception, location);
		return new SymbolicExecutionState(exceptionThrownState);
	}
}