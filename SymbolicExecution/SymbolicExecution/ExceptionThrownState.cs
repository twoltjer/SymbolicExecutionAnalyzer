namespace SymbolicExecution;

public readonly struct ExceptionThrownState : IEquatable<ExceptionThrownState>, IExceptionThrownState
{
	public ExceptionThrownState(IObjectInstance exception, Location location, IMethodSymbol methodSymbol)
	{
		Exception = exception;
		Location = location;
		MethodSymbol = methodSymbol;
		InvocationLocations = ImmutableArray<(Location, IMethodSymbol)>.Empty;
	}

	private ExceptionThrownState(IObjectInstance exception, Location location, IMethodSymbol methodSymbol, ImmutableArray<(Location, IMethodSymbol)> invocationLocations)
	{
		Exception = exception;
		Location = location;
		MethodSymbol = methodSymbol;
		InvocationLocations = invocationLocations;
	}

	public IObjectInstance Exception { get; }
	public Location Location { get; }
	public ImmutableArray<(Location, IMethodSymbol)> InvocationLocations { get; }
	public IMethodSymbol MethodSymbol { get; }
	public IExceptionThrownState AddInvocationLocation(Location location, IMethodSymbol methodSymbol)
	{
		return new ExceptionThrownState(Exception, Location, MethodSymbol, InvocationLocations.Add((location, methodSymbol)));
	}

	public bool Equals(ExceptionThrownState other)
	{
		return Exception.Equals(other.Exception) && Location.Equals(other.Location);
	}

	public bool Equals(IExceptionThrownState other)
	{
		return other is ExceptionThrownState exceptionThrownState && Equals(exceptionThrownState);
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