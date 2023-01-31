namespace SymbolicExecution;

public interface IAnalysisState
{
	IExceptionThrownState? CurrentException { get; }
	IAnalysisState ThrowException(ObjectInstance exception, Location location);
}