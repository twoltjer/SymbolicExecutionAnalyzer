namespace SymbolicExecution;

public interface IExceptionThrownState
{
	IObjectInstance Exception { get; }
	Location Location { get; }
}