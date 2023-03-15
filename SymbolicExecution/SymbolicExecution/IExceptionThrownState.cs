namespace SymbolicExecution;

public interface IExceptionThrownState : IEquatable<IExceptionThrownState>
{
	IObjectInstance Exception { get; }
	Location Location { get; }
	IMethodSymbol MethodSymbol { get; }
	IExceptionThrownState AddInvocationLocation(Location location, IMethodSymbol methodSymbol);
	ImmutableArray<(Location location, IMethodSymbol methodSymbol)> InvocationLocations { get; }
}