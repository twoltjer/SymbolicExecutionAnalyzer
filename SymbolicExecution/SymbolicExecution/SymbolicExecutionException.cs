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
	public bool Equals(ISymbolicExecutionException other)
	{
		var typeName = Type.Match(typeSymbol => typeSymbol.Name, type => type.Name);
		var otherTypeName = other.Type.Match(typeSymbol => typeSymbol.Name, type => type.Name);
		var typeNamespace = Type.Match(typeSymbol => typeSymbol.ContainingNamespace.Name, type => type.Namespace);
		var otherTypeNamespace = other.Type.Match(typeSymbol => typeSymbol.ContainingNamespace.Name, type => type.Namespace);
		return Location == other.Location
			&& typeName == otherTypeName
			&& typeNamespace == otherTypeNamespace
			&& SymbolEqualityComparer.Default.Equals(MethodSymbol, other.MethodSymbol);
	}
}