namespace SymbolicExecution;

public readonly struct SymbolicExecutionException : ISymbolicExecutionException
{
	internal SymbolicExecutionException(Location location, TaggedUnion<ITypeSymbol, Type> type)
	{
		Location = location;
		Type = type;
	}

	public Location Location { get; }
	public TaggedUnion<ITypeSymbol, Type> Type { get; }
}