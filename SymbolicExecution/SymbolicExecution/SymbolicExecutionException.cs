namespace SymbolicExecution;

public readonly struct SymbolicExecutionException : ISymbolicExecutionException
{
	internal SymbolicExecutionException(Location location, Type type)
	{
		Location = location;
		Type = type;
	}

	internal Location Location { get; }
	internal Type Type { get; }
}