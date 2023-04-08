namespace SymbolicExecution;

/// <summary>
/// An exception thrown state, which contains information about an exception that is in the process of being thrown.
///
/// This type is immutable, and all methods that modify the state return a new instance of the state.
/// </summary>
public interface IExceptionThrownState : IEquatable<IExceptionThrownState>
{
	IObjectInstance Exception { get; }
	Location Location { get; }
	/// <summary>
	/// The method in which the exception was thrown
	/// </summary>
	IMethodSymbol MethodSymbol { get; }
	/// <summary>
	/// Adds an invocation location to the exception state, which are methods that are on the call stack and will be
	/// unwound before the exception is handled
	/// </summary>
	/// <param name="location"></param>
	/// <param name="methodSymbol"></param>
	/// <returns>A copy of the exception state with the invocation location added</returns>
	IExceptionThrownState AddInvocationLocation(Location location, IMethodSymbol methodSymbol);
	/// <summary>
	/// The current invocation locations of the exception
	/// </summary>
	ImmutableArray<(Location location, IMethodSymbol methodSymbol)> InvocationLocations { get; }
}