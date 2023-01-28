namespace SymbolicExecution;

public readonly struct SymbolicExecutionException : ISymbolicExecutionException
{
	internal SymbolicExecutionException(Location location, ITypeSymbol type)
	{
		Location = location;
		Type = type;
	}

	public Location Location { get; }
	public ITypeSymbol Type { get; }
}