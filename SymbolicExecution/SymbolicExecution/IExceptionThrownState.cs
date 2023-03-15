namespace SymbolicExecution;

public interface IExceptionThrownState : IEquatable<IExceptionThrownState>
{
	IObjectInstance Exception { get; }
	Location Location { get; }
}