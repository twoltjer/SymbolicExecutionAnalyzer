namespace SymbolicExecution;

public readonly struct SymbolicExecutionState : IAnalysisState
{
    public SymbolicExecutionState(ExceptionThrownState? currentException)
    {
        CurrentException = currentException;
    }

    public ExceptionThrownState? CurrentException { get; }
    public IAnalysisState ThrowException(ObjectInstance exception, Location location)
    {
        var exceptionThrownState = new ExceptionThrownState(exception, location);
        return new SymbolicExecutionState(exceptionThrownState);
    }
}