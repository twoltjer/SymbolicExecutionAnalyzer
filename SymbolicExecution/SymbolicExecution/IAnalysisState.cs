namespace SymbolicExecution;

public interface IAnalysisState
{
    ExceptionThrownState? CurrentException { get; }
    IAnalysisState ThrowException(ObjectInstance exception, Location location);
}