namespace SymbolicExecution;

public struct ObjectInstance : IObjectInstance
{
	public TaggedUnion<ITypeSymbol, Type> ActualTypeSymbol { get; }
	public TaggedUnion<ITypeSymbol, Type> ConvertedTypeSymbol { get; }
	public IValueScope Value { get; }
	public Location Location { get; }
	public bool IsExactType(Type type) => Value.IsExactType(type);

	public ObjectInstance(TaggedUnion<ITypeSymbol, Type> actualTypeSymbol, TaggedUnion<ITypeSymbol, Type> convertedTypeSymbol, Location location, IValueScope value)
	{
		ActualTypeSymbol = actualTypeSymbol;
		ConvertedTypeSymbol = convertedTypeSymbol;
		Location = location;
		Value = value;
	}

	public override string ToString()
	{
		return $"{ActualTypeSymbol.Match(t1 => t1.ToString(), t2 => t2.ToString())} {Value}";
	}

	public bool Equals(ObjectInstance other)
	{
		return ActualTypeSymbol.Match(x => x.Name, x => x.Name) == other.ActualTypeSymbol.Match(x => x.Name, x => x.Name) &&
		       ActualTypeSymbol.Match(x => x.ContainingNamespace.ToString(), x => x.Namespace) == other.ActualTypeSymbol.Match(x => x.ContainingNamespace.ToString(), x => x.Namespace) &&
		       ConvertedTypeSymbol.Match(x => x.Name, x => x.Name) == other.ConvertedTypeSymbol.Match(x => x.Name, x => x.Name) &&
		       ConvertedTypeSymbol.Match(x => x.ContainingNamespace.ToString(), x => x.Namespace) == other.ConvertedTypeSymbol.Match(x => x.ContainingNamespace.ToString(), x => x.Namespace) &&
		       Value.Equals(other.Value) &&
		       Location.Equals(other.Location);
	}

	public bool Equals(IObjectInstance other)
	{
		return other is ObjectInstance objectInstance && Equals(objectInstance);
	}

	public override bool Equals(object? obj)
	{
		return obj is ObjectInstance other && Equals(other);
	}

	public override int GetHashCode()
	{
		var actualName = ActualTypeSymbol.Match(x => x.Name, x => x.Name);
		var actualNamespace = ActualTypeSymbol.Match(x => x.ContainingNamespace.ToString(), x => x.Namespace);
		var convertedName = ConvertedTypeSymbol.Match(x => x.Name, x => x.Name);
		var convertedNamespace = ConvertedTypeSymbol.Match(x => x.ContainingNamespace.ToString(), x => x.Namespace);
		return (actualName, actualNamespace, convertedName, convertedNamespace, Value, Location).GetHashCode();
	}
}