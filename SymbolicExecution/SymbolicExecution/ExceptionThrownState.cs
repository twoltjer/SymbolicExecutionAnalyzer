namespace SymbolicExecution;

public readonly struct ExceptionThrownState : IEquatable<ExceptionThrownState>, IExceptionThrownState
{
	public ExceptionThrownState(IObjectInstance exception, Location location)
	{
		Exception = exception;
		Location = location;
	}

	public IObjectInstance Exception { get; }
	public Location Location { get; }

	public bool Equals(ExceptionThrownState other)
	{
		return Exception.Equals(other.Exception) && Location.Equals(other.Location);
	}

	public override bool Equals(object? obj)
	{
		return obj is ExceptionThrownState other && Equals(other);
	}

	public override int GetHashCode()
	{
		return (Exception, Location).GetHashCode();
	}
}