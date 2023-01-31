namespace SymbolicExecution;

public interface IExceptionThrownState
{
	ObjectInstance Exception { get; }
	Location Location { get; }
}