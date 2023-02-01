namespace SymbolicExecution;

public interface IAnalysisState
{
	IExceptionThrownState? CurrentException { get; }
	IAnalysisState ThrowException(IObjectInstance exception, Location location);
}