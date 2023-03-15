namespace SymbolicExecution;

public readonly struct SymbolicExecutionException : ISymbolicExecutionException
{
	internal SymbolicExecutionException(Location location, TaggedUnion<ITypeSymbol, Type> type, IMethodSymbol methodSymbol)
	{
		Location = location;
		Type = type;
		MethodSymbol = methodSymbol;
	}

	public Location Location { get; }
	public TaggedUnion<ITypeSymbol, Type> Type { get; }
	public IMethodSymbol MethodSymbol { get; }
}